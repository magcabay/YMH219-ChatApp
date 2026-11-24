using System.Net.Sockets;
using ChatOrtak;

namespace ChatServer
{
    public partial class Form1 : Form
    {
        private readonly Sunucu sunucu = new Sunucu();

        public Form1()
        {
            InitializeComponent();

            sunucu.LogEklendi += Sunucu_LogEklendi;
            sunucu.IstemciListesiDegisti += Sunucu_IstemciListesiDegisti;
        }

        private void btnBaslat_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtPort.Text, out int port) || port < 1024 || port > 65535)
            {
                MessageBox.Show("Port 1024 ile 65535 arasında bir sayı olmalı.", "Geçersiz Port",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                sunucu.Baslat(port);
                ArayuzDurumu(true);
            }
            catch (SocketException ex)
            {
                MessageBox.Show("Sunucu başlatılamadı. Port kullanımda olabilir.\n\n" + ex.Message, "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDurdur_Click(object sender, EventArgs e)
        {
            sunucu.Durdur();
            ArayuzDurumu(false);
        }

        private async void btnDuyuru_Click(object sender, EventArgs e)
        {
            string metin = txtDuyuru.Text.Trim();
            if (metin == "" || !sunucu.Calisiyor) return;

            await sunucu.HerkeseGonderAsync(new Mesaj(MesajTipi.Sistem, "Sunucu", "Duyuru: " + metin));
            Sunucu_LogEklendi($"[{DateTime.Now:HH:mm:ss}] Duyuru gönderildi: {metin}");
            txtDuyuru.Clear();
        }

        private void txtDuyuru_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                btnDuyuru.PerformClick();
            }
        }

        private void Sunucu_LogEklendi(string metin)
        {
            // olaylar UI thread'inden geliyor ama yine de garanti olsun
            if (InvokeRequired)
            {
                BeginInvoke(new Action<string>(Sunucu_LogEklendi), metin);
                return;
            }
            lstLog.Items.Add(metin);
            lstLog.TopIndex = lstLog.Items.Count - 1;
        }

        private void Sunucu_IstemciListesiDegisti()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(Sunucu_IstemciListesiDegisti));
                return;
            }
            lstIstemciler.Items.Clear();
            foreach (BagliIstemci istemci in sunucu.Istemciler)
                lstIstemciler.Items.Add(istemci.ToString());

            lblIstemciler.Text = $"Bağlı istemciler ({sunucu.Istemciler.Count}) - ad | IP:port | bağlanma:";
        }

        private void ArayuzDurumu(bool calisiyor)
        {
            btnBaslat.Enabled = !calisiyor;
            btnDurdur.Enabled = calisiyor;
            btnDuyuru.Enabled = calisiyor;
            txtPort.Enabled = !calisiyor;

            if (calisiyor)
            {
                lblDurum.Text = $"Durum: Çalışıyor (port {sunucu.Port})";
                lblDurum.ForeColor = Color.SeaGreen;
            }
            else
            {
                lblDurum.Text = "Durum: Kapalı";
                lblDurum.ForeColor = Color.IndianRed;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            sunucu.Durdur();
        }
    }
}
