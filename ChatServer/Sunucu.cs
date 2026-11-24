using System.Net;
using System.Net.Sockets;
using ChatOrtak;

namespace ChatServer
{
    // TcpListener ile istemcileri kabul eden ve mesajları dağıtan sınıf.
    // Not: Metotlar form (UI) thread'inden başlatıldığı için await sonrası kod da
    // UI thread'inde devam ediyor. Bu yüzden listeye erişirken ayrıca lock kullanmadım.
    public class Sunucu
    {
        private TcpListener dinleyici;
        private readonly List<BagliIstemci> istemciler = new List<BagliIstemci>();

        public bool Calisiyor { get; private set; }
        public int Port { get; private set; }

        // form bu olaylara abone olup arayüzü güncelliyor
        public event Action<string> LogEklendi;
        public event Action IstemciListesiDegisti;

        public IReadOnlyList<BagliIstemci> Istemciler
        {
            get { return istemciler; }
        }

        public void Baslat(int port)
        {
            if (Calisiyor) return;

            dinleyici = new TcpListener(IPAddress.Any, port);
            dinleyici.Start();   // port kullanılıyorsa SocketException fırlatır, formda yakalanıyor
            Port = port;
            Calisiyor = true;
            Log($"Sunucu {port} portunda başlatıldı.");

            _ = BaglantilariKabulEtAsync();
        }

        public void Durdur()
        {
            if (!Calisiyor) return;
            Calisiyor = false;

            foreach (BagliIstemci istemci in istemciler.ToList())
                istemci.Kapat();
            istemciler.Clear();

            dinleyici.Stop();
            IstemciListesiDegisti?.Invoke();
            Log("Sunucu durduruldu.");
        }

        private async Task BaglantilariKabulEtAsync()
        {
            while (Calisiyor)
            {
                try
                {
                    TcpClient tcp = await dinleyici.AcceptTcpClientAsync();
                    _ = IstemciyiDinleAsync(tcp);
                }
                catch (ObjectDisposedException)
                {
                    break; // sunucu durduruldu
                }
                catch (SocketException)
                {
                    break;
                }
            }
        }

        private async Task IstemciyiDinleAsync(TcpClient tcp)
        {
            BagliIstemci istemci = new BagliIstemci(tcp);
            bool listeyeEklendi = false;

            try
            {
                // ilk mesaj Giris olmalı
                string ilkSatir = await istemci.Okuyucu.ReadLineAsync();
                Mesaj giris = Mesaj.Coz(ilkSatir);

                if (giris == null || giris.Tip != MesajTipi.Giris || string.IsNullOrWhiteSpace(giris.Gonderen))
                {
                    await istemci.GonderAsync(new Mesaj(MesajTipi.Hata, "Sunucu", "Geçersiz giriş isteği."));
                    istemci.Kapat();
                    return;
                }

                string ad = giris.Gonderen.Trim();
                if (istemciler.Any(i => i.KullaniciAdi.Equals(ad, StringComparison.OrdinalIgnoreCase)))
                {
                    await istemci.GonderAsync(new Mesaj(MesajTipi.Hata, "Sunucu", $"'{ad}' kullanıcı adı zaten kullanılıyor."));
                    istemci.Kapat();
                    return;
                }

                istemci.KullaniciAdi = ad;
                istemciler.Add(istemci);
                listeyeEklendi = true;
                IstemciListesiDegisti?.Invoke();
                Log($"{ad} bağlandı ({istemci.IpAdresi})");

                await HerkeseGonderAsync(new Mesaj(MesajTipi.Sistem, "Sunucu", $"{ad} sohbete katıldı"));
                await KullaniciListesiniGonderAsync();

                // mesaj döngüsü
                while (Calisiyor)
                {
                    string satir = await istemci.Okuyucu.ReadLineAsync();
                    if (satir == null) break; // istemci bağlantıyı kapattı

                    Mesaj mesaj = Mesaj.Coz(satir);
                    if (mesaj == null) continue;

                    // gönderen adını istemciye güvenmeden biz yazıyoruz
                    mesaj.Gonderen = istemci.KullaniciAdi;
                    mesaj.Zaman = DateTime.Now;

                    if (mesaj.Tip == MesajTipi.Cikis)
                        break;

                    if (mesaj.Tip == MesajTipi.Sohbet)
                    {
                        Log($"{mesaj.Gonderen}: {mesaj.Icerik}");
                        await HerkeseGonderAsync(mesaj);
                    }
                    else if (mesaj.Tip == MesajTipi.Ozel)
                    {
                        await OzelMesajGonderAsync(istemci, mesaj);
                    }
                }
            }
            catch (IOException)
            {
                // bağlantı aniden koptu
            }
            catch (ObjectDisposedException)
            {
                // sunucu durdurulurken akış kapandı
            }
            finally
            {
                istemci.Kapat();
                if (listeyeEklendi && istemciler.Remove(istemci))
                {
                    IstemciListesiDegisti?.Invoke();
                    Log($"{istemci.KullaniciAdi} ayrıldı.");
                    if (Calisiyor)
                    {
                        await HerkeseGonderAsync(new Mesaj(MesajTipi.Sistem, "Sunucu", $"{istemci.KullaniciAdi} sohbetten ayrıldı"));
                        await KullaniciListesiniGonderAsync();
                    }
                }
            }
        }

        private async Task OzelMesajGonderAsync(BagliIstemci gonderen, Mesaj mesaj)
        {
            BagliIstemci hedef = istemciler.FirstOrDefault(i => i.KullaniciAdi == mesaj.Hedef);
            if (hedef == null)
            {
                await GuvenliGonderAsync(gonderen, new Mesaj(MesajTipi.Hata, "Sunucu", $"'{mesaj.Hedef}' adlı kullanıcı bulunamadı."));
                return;
            }

            Log($"(Özel) {mesaj.Gonderen} -> {mesaj.Hedef}");
            await GuvenliGonderAsync(hedef, mesaj);
            if (hedef != gonderen)
                await GuvenliGonderAsync(gonderen, mesaj); // gönderen de kendi mesajını görsün
        }

        public async Task HerkeseGonderAsync(Mesaj mesaj)
        {
            // gönderim sırasında liste değişebilir, kopyası üzerinde dönüyoruz
            foreach (BagliIstemci istemci in istemciler.ToList())
            {
                await GuvenliGonderAsync(istemci, mesaj);
            }
        }

        private async Task KullaniciListesiniGonderAsync()
        {
            string adlar = string.Join(",", istemciler.Select(i => i.KullaniciAdi));
            await HerkeseGonderAsync(new Mesaj(MesajTipi.Liste, "Sunucu", adlar));
        }

        private async Task GuvenliGonderAsync(BagliIstemci istemci, Mesaj mesaj)
        {
            try
            {
                await istemci.GonderAsync(mesaj);
            }
            catch (Exception)
            {
                // bu istemciye yazılamadı, okuma döngüsü kopmayı fark edip listeden çıkaracak
                istemci.Kapat();
            }
        }

        private void Log(string metin)
        {
            LogEklendi?.Invoke($"[{DateTime.Now:HH:mm:ss}] {metin}");
        }
    }
}
