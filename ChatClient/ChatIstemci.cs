using System.Net.Sockets;
using System.Text;
using ChatOrtak;

namespace ChatClient
{
    // Sunucuya bağlanıp mesaj gönderen / alan sınıf
    public class ChatIstemci
    {
        private TcpClient tcp;
        private StreamReader okuyucu;
        private StreamWriter yazici;
        private readonly SemaphoreSlim yazmaKilidi = new SemaphoreSlim(1, 1);

        // kullanıcı kendisi mi kesti yoksa bağlantı mı koptu ayırt etmek için
        private bool bilerekKesildi;

        public string KullaniciAdi { get; private set; }
        public bool Bagli { get; private set; }

        public event Action<Mesaj> MesajGeldi;
        public event Action<string> BaglantiKoptu;

        public async Task BaglanAsync(string sunucuAdresi, int port, string kullaniciAdi)
        {
            if (Bagli)
                throw new InvalidOperationException("Zaten bağlısınız.");

            tcp = new TcpClient();

            // sunucu yoksa sonsuza kadar beklemesin, 5 sn sonra vazgeç
            using (var iptal = new CancellationTokenSource(TimeSpan.FromSeconds(5)))
            {
                try
                {
                    await tcp.ConnectAsync(sunucuAdresi, port, iptal.Token);
                }
                catch (OperationCanceledException)
                {
                    tcp.Close();
                    throw new TimeoutException("Sunucuya bağlanma süresi doldu.");
                }
            }

            NetworkStream akis = tcp.GetStream();
            okuyucu = new StreamReader(akis, Encoding.UTF8);
            yazici = new StreamWriter(akis, new UTF8Encoding(false)) { AutoFlush = true };

            KullaniciAdi = kullaniciAdi;
            Bagli = true;
            bilerekKesildi = false;

            await YazAsync(new Mesaj(MesajTipi.Giris, kullaniciAdi, ""));

            _ = DinleAsync();
        }

        private async Task DinleAsync()
        {
            string sebep = "Sunucu bağlantıyı kapattı.";
            try
            {
                while (Bagli)
                {
                    string satir = await okuyucu.ReadLineAsync();
                    if (satir == null) break;

                    Mesaj mesaj = Mesaj.Coz(satir);
                    if (mesaj != null)
                        MesajGeldi?.Invoke(mesaj);
                }
            }
            catch (IOException)
            {
                sebep = "Sunucu ile bağlantı koptu.";
            }
            catch (ObjectDisposedException)
            {
                // kendimiz kapattık
            }

            Temizle();
            if (!bilerekKesildi)
                BaglantiKoptu?.Invoke(sebep);
        }

        public async Task GonderAsync(string metin)
        {
            await YazAsync(new Mesaj(MesajTipi.Sohbet, KullaniciAdi, metin));
        }

        public async Task OzelGonderAsync(string hedef, string metin)
        {
            Mesaj m = new Mesaj(MesajTipi.Ozel, KullaniciAdi, metin);
            m.Hedef = hedef;
            await YazAsync(m);
        }

        public async Task BaglantiyiKesAsync()
        {
            if (!Bagli) return;
            bilerekKesildi = true;
            try
            {
                await YazAsync(new Mesaj(MesajTipi.Cikis, KullaniciAdi, ""));
            }
            catch
            {
                // çıkış mesajı gitmese de olur
            }
            Temizle();
        }

        private async Task YazAsync(Mesaj mesaj)
        {
            if (yazici == null)
                throw new InvalidOperationException("Sunucuya bağlı değilsiniz.");

            await yazmaKilidi.WaitAsync();
            try
            {
                await yazici.WriteLineAsync(mesaj.Serilestir());
            }
            finally
            {
                yazmaKilidi.Release();
            }
        }

        private void Temizle()
        {
            Bagli = false;
            try
            {
                tcp?.Close();
            }
            catch
            {
            }
        }
    }
}
