using System.Net.Sockets;
using ChatOrtak;

namespace ChatClient
{
    public partial class Form1 : Form
    {
        private readonly ChatIstemci istemci = new ChatIstemci();

        public Form1()
        {
            InitializeComponent();

            istemci.MesajGeldi += Istemci_MesajGeldi;
            istemci.BaglantiKoptu += Istemci_BaglantiKoptu;
        }

        private async void btnBaglan_Click(object sender, EventArgs e)
        {
            string ad = txtKullanici.Text.Trim();
            if (ad == "")
            {
                MessageBox.Show("Lütfen bir kullanıcı adı girin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtKullanici.Focus();
                return;
            }
            if (ad.Contains('|') || ad.Contains(','))
            {
                MessageBox.Show("Kullanıcı adında | veya , karakteri olamaz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!int.TryParse(txtPort.Text, out int port) || port < 1 || port > 65535)
            {
                MessageBox.Show("Geçerli bir port girin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnBaglan.Enabled = false;
            lblDurum.Text = "Durum: Bağlanıyor...";
            lblDurum.ForeColor = Color.DarkOrange;

            try
            {
                await istemci.BaglanAsync(txtSunucu.Text.Trim(), port, ad);
                ArayuzDurumu(true);
                MesajEkle($"[{DateTime.Now:HH:mm:ss}] Sunucuya bağlanıldı ({txtSunucu.Text}:{port})");
                txtMesaj.Focus();
            }
            catch (SocketException)
            {
                HataGoster("Sunucuya bağlanılamadı. Sunucunun açık olduğundan ve adres/portun doğru olduğundan emin olun.");
                ArayuzDurumu(false);
            }
            catch (TimeoutException ex)
            {
                HataGoster(ex.Message);
                ArayuzDurumu(false);
            }
            catch (Exception ex)
            {
                HataGoster("Beklenmeyen bir hata oluştu: " + ex.Message);
                ArayuzDurumu(false);
            }
        }

        private async void btnKes_Click(object sender, EventArgs e)
        {
            await istemci.BaglantiyiKesAsync();
            MesajEkle($"[{DateTime.Now:HH:mm:ss}] Bağlantı kesildi.");
            ArayuzDurumu(false);
        }

        private async void btnGonder_Click(object sender, EventArgs e)
        {
            string metin = txtMesaj.Text.Trim();
            if (metin == "" || !istemci.Bagli) return;

            try
            {
                if (chkOzel.Checked)
                {
                    if (lstKullanicilar.SelectedItem == null)
                    {
                        MessageBox.Show("Özel mesaj için listeden bir kullanıcı seçin.", "Uyarı",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    await istemci.OzelGonderAsync(lstKullanicilar.SelectedItem.ToString(), metin);
                }
                else
                {
                    await istemci.GonderAsync(metin);
                }
                txtMesaj.Clear();
            }
            catch (Exception ex) when (ex is IOException || ex is ObjectDisposedException || ex is InvalidOperationException)
            {
                HataGoster("Mesaj gönderilemedi: " + ex.Message);
            }
            txtMesaj.Focus();
        }

        private void txtMesaj_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // "ding" sesini engelle
                btnGonder.PerformClick();
            }
        }

        private void Istemci_MesajGeldi(Mesaj mesaj)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<Mesaj>(Istemci_MesajGeldi), mesaj);
                return;
            }

            switch (mesaj.Tip)
            {
                case MesajTipi.Liste:
                    KullaniciListesiniGuncelle(mesaj.Icerik);
                    break;
                case MesajTipi.Hata:
                    MesajEkle(mesaj.ToString());
                    HataGoster(mesaj.Icerik);
                    break;
                default:
                    MesajEkle(mesaj.ToString());
                    break;
            }
        }

        private void Istemci_BaglantiKoptu(string sebep)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<string>(Istemci_BaglantiKoptu), sebep);
                return;
            }
            MesajEkle($"[{DateTime.Now:HH:mm:ss}] {sebep}");
            ArayuzDurumu(false);
        }

        private void KullaniciListesiniGuncelle(string virgulluListe)
        {
            object seciliOlan = lstKullanicilar.SelectedItem;
            lstKullanicilar.Items.Clear();

            foreach (string ad in virgulluListe.Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                if (ad != istemci.KullaniciAdi) // kendimizi listede göstermeyelim
                    lstKullanicilar.Items.Add(ad);
            }

            if (seciliOlan != null && lstKullanicilar.Items.Contains(seciliOlan))
                lstKullanicilar.SelectedItem = seciliOlan;

            lblCevrimici.Text = $"Çevrimiçi kullanıcılar ({lstKullanicilar.Items.Count}):";
        }

        private void btnGecmisKaydet_Click(object sender, EventArgs e)
        {
            if (lstMesajlar.Items.Count == 0)
            {
                MessageBox.Show("Kaydedilecek mesaj yok.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Metin dosyası (*.txt)|*.txt";
                sfd.FileName = "sohbet_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".txt";
                if (sfd.ShowDialog() != DialogResult.OK) return;

                try
                {
                    using (StreamWriter sw = new StreamWriter(sfd.FileName))
                    {
                        foreach (object satir in lstMesajlar.Items)
                            sw.WriteLine(satir.ToString());
                    }
                    MessageBox.Show("Sohbet geçmişi kaydedildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    HataGoster("Dosya kaydedilemedi: " + ex.Message);
                }
            }
        }

        private void MesajEkle(string metin)
        {
            lstMesajlar.Items.Add(metin);
            lstMesajlar.TopIndex = lstMesajlar.Items.Count - 1;
        }

        private void ArayuzDurumu(bool bagli)
        {
            btnBaglan.Enabled = !bagli;
            btnKes.Enabled = bagli;
            txtMesaj.Enabled = bagli;
            btnGonder.Enabled = bagli;
            txtSunucu.Enabled = !bagli;
            txtPort.Enabled = !bagli;
            txtKullanici.Enabled = !bagli;
            AcceptButton = bagli ? null : btnBaglan;

            if (bagli)
            {
                lblDurum.Text = $"Durum: Bağlı ({istemci.KullaniciAdi})";
                lblDurum.ForeColor = Color.SeaGreen;
                Text = $"Chat İstemci - {istemci.KullaniciAdi}";
            }
            else
            {
                lblDurum.Text = "Durum: Bağlı değil";
                lblDurum.ForeColor = Color.IndianRed;
                Text = "Chat İstemci - YMH219";
                lstKullanicilar.Items.Clear();
            }
        }

        private void HataGoster(string mesaj)
        {
            MessageBox.Show(mesaj, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private async void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (istemci.Bagli)
                await istemci.BaglantiyiKesAsync();
        }
    }
}
