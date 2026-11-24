using System.Net;
using System.Net.Sockets;
using System.Text;
using ChatOrtak;

namespace ChatServer
{
    // Sunucuya bağlanan her bir istemciyi temsil eder
    public class BagliIstemci
    {
        public TcpClient Tcp { get; }
        public StreamReader Okuyucu { get; }
        public StreamWriter Yazici { get; }

        public string KullaniciAdi { get; set; } = "";
        public string IpAdresi { get; }
        public DateTime BaglanmaZamani { get; }

        public BagliIstemci(TcpClient tcp)
        {
            Tcp = tcp;
            NetworkStream akis = tcp.GetStream();
            Okuyucu = new StreamReader(akis, Encoding.UTF8);
            Yazici = new StreamWriter(akis, new UTF8Encoding(false));
            Yazici.AutoFlush = true;

            IPEndPoint uc = tcp.Client.RemoteEndPoint as IPEndPoint;
            IpAdresi = uc != null ? uc.ToString() : "bilinmiyor";
            BaglanmaZamani = DateTime.Now;
        }

        // aynı anda iki yazma işlemi olmasın diye (yoksa StreamWriter hata veriyor)
        private readonly SemaphoreSlim yazmaKilidi = new SemaphoreSlim(1, 1);

        public async Task GonderAsync(Mesaj mesaj)
        {
            await yazmaKilidi.WaitAsync();
            try
            {
                await Yazici.WriteLineAsync(mesaj.Serilestir());
            }
            finally
            {
                yazmaKilidi.Release();
            }
        }

        public void Kapat()
        {
            try
            {
                Tcp.Close();
            }
            catch
            {
                // zaten kapalıysa sorun değil
            }
        }

        // sunucu formundaki listede böyle görünecek
        public override string ToString()
        {
            return $"{KullaniciAdi}  |  {IpAdresi}  |  {BaglanmaZamani:HH:mm:ss}";
        }
    }
}
