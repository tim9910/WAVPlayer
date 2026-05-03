using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
// 必須引用這兩個命名空間，才能使用播放器和檔案串流
using System.Media;
using System.IO;
using System.Reflection;

namespace WAVPlayer
{
    public partial class frmWAVPlayer : Form
    {
        ToolTip toolTip = null;
        // 建議將播放器宣告在外部，這樣「停止」按鈕才能精確停止同一個播放器
        //private SoundPlayer player = new SoundPlayer();
        private SoundPlayer player = null;

        public frmWAVPlayer()
        {
            InitializeComponent();
            initToolTip();

            btnBrowse.Text = "";
            btnBrowse.Image = ResizeImage(GetImage("add"), new Size(44, 44));
            btnBrowse.MouseEnter += (sender, e) =>
            {
                btnBrowse.Image = ResizeImage(GetImage("add"), new Size(48, 48));
            };

            btnBrowse.MouseLeave += (sender, e) =>
            {
                btnBrowse.Image = ResizeImage(GetImage("add"), new Size(44, 44));
            };

            btnPlay.Text = "";
            btnPlay.Image = ResizeImage(GetImage("play"), new Size(44, 44));
            btnPlay.MouseEnter += (sender, e) =>
            {
                btnPlay.Image = ResizeImage(GetImage("play"), new Size(48, 48));
            };

            btnPlay.MouseLeave += (sender, e) =>
            {
                btnPlay.Image = ResizeImage(GetImage("play"), new Size(44, 44));
            };

            btnLoop.Text = "";
            btnLoop.Image = ResizeImage(GetImage("repeat"), new Size(44, 44));
            btnLoop.MouseEnter += (sender, e) =>
            {
                btnLoop.Image = ResizeImage(GetImage("repeat"), new Size(48, 48));
            };

            btnLoop.MouseLeave += (sender, e) =>
            {
                btnLoop.Image = ResizeImage(GetImage("repeat"), new Size(44, 44));
            };

            btnStop.Text = "";
            btnStop.Image = ResizeImage(GetImage("stop"), new Size(44, 44));
            btnStop.MouseEnter += (sender, e) =>
            {
                btnStop.Image = ResizeImage(GetImage("stop"), new Size(48, 48));
            };

            btnStop.MouseLeave += (sender, e) =>
            {
                btnStop.Image = ResizeImage(GetImage("stop"), new Size(44, 44));
            };

            btnEnd.Text = "";
            btnEnd.Image = ResizeImage(GetImage("logout"), new Size(44, 44));
            btnEnd.MouseEnter += (sender, e) =>
            {
                btnEnd.Image = ResizeImage(GetImage("logout"), new Size(48, 48));
            };

            btnEnd.MouseLeave += (sender, e) =>
            {
                btnEnd.Image = ResizeImage(GetImage("logout"), new Size(44, 44));
            };

            btnPlay.Enabled = false;
            btnLoop.Enabled = false;
            btnStop.Enabled = false;

            //disabled 視窗的最大化和最小化按鈕，讓使用者只能關閉視窗
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            //disbled 視窗的調整大小功能，讓使用者無法改變視窗大小
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

        }

        private Image GetImage(string name)
        {
            return Properties.Resources.ResourceManager.GetObject(name) as Image;
        }
        private Image ResizeImage(Image img, Size size)
        {
            return new Bitmap(img, size);
        }

        private void frmWAVPlayer_Load(object sender, EventArgs e)
        {
            // 可以在此做初始化設定 

        }
        private void initToolTip()
        {
            toolTip = new ToolTip();
            toolTip.SetToolTip(btnBrowse, "瀏覽檔案");
            toolTip.SetToolTip(btnPlay, "播放一次");
            toolTip.SetToolTip(btnLoop, "重複播放");
            toolTip.SetToolTip(btnStop, "停止播放");
            toolTip.SetToolTip(btnEnd, "結束程式");
        }

        // 1. 瀏覽檔案
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            // 如果你是在設計介面拉的 OpenFileDialog，名稱應為 ofdWAVFile
            // 如果沒拉元件，程式碼會報錯，記得確認 ID
            ofdWAVFile.Filter = "WAV Files(*.wav)|*.wav";
            if (ofdWAVFile.ShowDialog() == DialogResult.OK)
            {
                txtPath.Text = ofdWAVFile.FileName;
                btnPlay.Enabled = true;
                btnLoop.Enabled = true;
                //btnStop.Enabled = true;
            }
        }

        // 2. 播放一次
        private void btnPlay_Click(object sender, EventArgs e)
        {

            try
            {
                //player = new SoundPlayer();
                player = new SoundPlayer(txtPath.Text);
                player.Load();

                player.Play();
                btnStop.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("無法播放音效檔，請確認檔案路徑是否正確!\n" + ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 3. 重複播放
        private void btnLoop_Click(object sender, EventArgs e)
        {
            try
            {
                player = new SoundPlayer(txtPath.Text);
                player.PlayLooping();
                btnStop.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("無法播放音效檔，請確認檔案路徑是否正確!\n" + ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        // 4. 停止播放
        private void btnStop_Click(object sender, EventArgs e)
        {
            // 這裡修正了你原本的寫法，直接用共用的 player 物件停止即可
            // 原本用 FileStream 建立新 player 來停止舊 player 有時會無效            
            try
            {
                if (player != null)
                {
                    player.Stop();
                    btnStop.Enabled = false;
                }
                else
                {
                    MessageBox.Show("播放器未初始化", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("無法播放音效檔，請確認檔案路徑是否正確!\n" + ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 5. 結束程式
        private void btnEnd_Click(object sender, EventArgs e)
        {
            // 這會觸發下面那個 FormClosing 事件
            Application.Exit();
        }

        // 6. 表單關閉前的確認
        private void frmWAVPlayer_FormClosing(object sender, FormClosingEventArgs e)
        {
            var result = MessageBox.Show("確定要關閉應用程式嗎？", "關閉確認",
                         MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.No)
            {
                e.Cancel = true; // 取消關閉
            }
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            string currentText = textBox.Text;
            string controlName = textBox.Name;
            if (controlName == "txtPath")
            {
                if (currentText == "")
                {
                    btnPlay.Enabled = false;
                    btnLoop.Enabled = false;
                    btnStop.Enabled = false;
                }
                else
                {
                    btnPlay.Enabled = true;
                    btnLoop.Enabled = true;
                    //btnStop.Enabled = true;
                }
            }
        }
    }
}