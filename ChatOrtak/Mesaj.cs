namespace ChatOrtak
{
    public enum MesajTipi
    {
        Giris,      // istemci -> sunucu : kullanıcı adı ile bağlanma
        Sohbet,     // herkese mesaj
        Ozel,       // özel mesaj (Hedef alanı dolu)
        Sistem,     // sunucunun bilgi mesajı (katıldı/ayrıldı vs.)
        Liste,      // çevrimiçi kullanıcı listesi (İçerik = ad1,ad2,...)
        Hata,       // sunucunun hata bildirimi
        Cikis       // istemci çıkıyor
    }

    // Sunucu ve istemci arasında gidip gelen mesaj.
    // Ağ üzerinden tek satır olarak gönderiliyor:  Tip|Gonderen|Hedef|Zaman|Icerik
    public class Mesaj
    {
        private const char Ayirici = '|';

        public MesajTipi Tip { get; set; }
        public string Gonderen { get; set; } = "";
        public string Hedef { get; set; } = "";
        public DateTime Zaman { get; set; } = DateTime.Now;
        public string Icerik { get; set; } = "";

        public Mesaj() { }

        public Mesaj(MesajTipi tip, string gonderen, string icerik)
        {
            Tip = tip;
            Gonderen = gonderen;
            Icerik = icerik;
            Zaman = DateTime.Now;
        }

        public string Serilestir()
        {
            // içerikteki alt satırlar protokolü bozmasın
            string temizIcerik = (Icerik ?? "").Replace("\r", " ").Replace("\n", " ");
            return string.Join(Ayirici.ToString(),
                Tip.ToString(),
                Temizle(Gonderen),
                Temizle(Hedef),
                Zaman.Ticks.ToString(),
                temizIcerik);
        }

        // gelen satırı Mesaj nesnesine çevirir, bozuksa null döner
        public static Mesaj Coz(string satir)
        {
            if (string.IsNullOrWhiteSpace(satir))
                return null;

            // içerikte | olabilir, o yüzden en fazla 5 parçaya böl
            string[] parcalar = satir.Split(Ayirici, 5);
            if (parcalar.Length < 5)
                return null;

            if (!Enum.TryParse(parcalar[0], out MesajTipi tip))
                return null;

            if (!long.TryParse(parcalar[3], out long ticks))
                ticks = DateTime.Now.Ticks;

            return new Mesaj
            {
                Tip = tip,
                Gonderen = parcalar[1],
                Hedef = parcalar[2],
                Zaman = new DateTime(ticks),
                Icerik = parcalar[4]
            };
        }

        private static string Temizle(string s)
        {
            return (s ?? "").Replace(Ayirici, '_').Replace("\r", "").Replace("\n", "");
        }

        // arayüzde göstermek için
        public override string ToString()
        {
            string saat = Zaman.ToString("HH:mm:ss");
            switch (Tip)
            {
                case MesajTipi.Sohbet:
                    return $"[{saat}] {Gonderen}: {Icerik}";
                case MesajTipi.Ozel:
                    return $"[{saat}] (Özel) {Gonderen} -> {Hedef}: {Icerik}";
                case MesajTipi.Sistem:
                    return $"[{saat}] *** {Icerik} ***";
                case MesajTipi.Hata:
                    return $"[{saat}] HATA: {Icerik}";
                default:
                    return $"[{saat}] {Tip} {Gonderen} {Icerik}";
            }
        }
    }
}
