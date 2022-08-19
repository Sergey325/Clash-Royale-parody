using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.DataModel;
using WindowsFormsApp1.Properties;

namespace WindowsFormsApp1
{
    public partial class SignInForm : Form
    {
        private Player player;
        public SignInForm()
        {
            InitializeComponent();
        }
        private void SignInForm_Load(object sender, System.EventArgs e)
        {
            readSavedLogPass();
            foreach (var cPB in pnlSignUp.Controls.OfType<CustomPictureBox>())
                cPB.Click += (s, a) => ShowBorder(s as CustomPictureBox);
            lbuttonResize.Click += (s, a) =>
            {
                if (WindowState == FormWindowState.Normal)
                {
                    this.WindowState = FormWindowState.Maximized;
                    lbuttonResize.Text = "❐";
                }
                else
                {
                    this.WindowState = FormWindowState.Normal;
                    lbuttonResize.Text = "❒";
                }
            };
            lbuttonExit.Click += (s, a) => Application.Exit();
            lbuttonRollUp.Click += (s, a) => { WindowState = FormWindowState.Minimized; };
            toggleSwitch1.Click += (s, a) =>
            {
                labelRemembMe.ForeColor = toggleSwitch1.Value ? Color.FromArgb(137, 26, 241) : Color.FromArgb(191, 191, 191);
                if (toggleSwitch1.Value && inputLogin.Text.Trim() != "" && inputPassword.Text.Trim() != "")
                    File.WriteAllText("ini.txt", $"{inputLogin.Text} {inputPassword.Text}");
                else if (File.Exists("ini.txt")) File.Delete("ini.txt");
            };
            buttonLogIn.Click += async (s, a) =>
            {
                if (toggleSwitch1.Value && inputLogin.Text.Trim() != "" && inputPassword.Text.Trim() != "")
                    File.WriteAllText("ini.txt", $"{inputLogin.Text} {inputPassword.Text}");
                else if (File.Exists("ini.txt")) File.Delete("ini.txt");
                loaderSignIn.Visible = true;
                await Task.Run(() =>
                {
                    player = new GameContext().Players.FirstOrDefault(x => x.Password == inputPassword.Text
                            && x.Nickname == inputLogin.Text);
                    loaderSignIn.Visible = false;
                });
                if (player != null)
                {
                    unlock();
                    MyContext.ShowForm2(player, Location);
                    Focus();
                }
                else showMessage(1, "Account not found", true);
            };
            inputPassword.TextChanged += (s, a) =>
            {
                inputPassword.PasswordChar = '*';
                if (inputPassword.Text.Length == 0) inputPassword.PasswordChar = (char)0;
            };
            tbNickname.TextChanged += (s, a) => CheckInput();
            tbPassword.TextChanged += (s, a) =>
            {
                tbPassword.PasswordChar = '*';
                if (tbPassword.Text.Length == 0) tbPassword.PasswordChar = (char)0;
                CheckInput();
            };
            tbConfirm.TextChanged += (s, a) =>
            {
                tbConfirm.PasswordChar = '*';
                if (tbConfirm.Text.Length == 0) tbConfirm.PasswordChar = (char)0;
                CheckInput();
            };
            lRegister.MouseEnter += (s, a) =>
           {
               lRegister.ForeColor = Color.FromArgb(137, 26, 241);
           };
            lRegister.Click += (s, a) =>
            {
                WindowState = FormWindowState.Normal;
                lRegister.ForeColor = Color.FromArgb(157, 16, 221);
                pnlSignUp.Controls.AddRange(new Control[] { pnlSignIn.Controls[9], pnlSignIn.Controls[10], pnlSignIn.Controls[11] });
                pages.SelectedIndex = 1;
            };
            lRegister.MouseLeave += (s, a) =>
            {
                lRegister.ForeColor = Color.FromArgb(157, 16, 221);
            };
            lIconBack.MouseEnter += (s, a) => RecolorBack(true);
            lIconBack.Click += (s, a) => BackToSignIn();
            lIconBack.MouseLeave += (s, a) => RecolorBack(false);
            lBack.MouseLeave += (s, a) => RecolorBack(false);
            lBack.MouseEnter += (s, a) => RecolorBack(true);
            lBack.Click += (s, a) => BackToSignIn();
            btnCreate.Click += async (s, a) =>
            {
                loaderSIgnUp.Visible = true;
                await Task.Run(() =>
                {
                    using (var context = new GameContext())
                    {
                        if (context.Players.Any(x => x.Nickname == tbNickname.Text.Trim()))
                        {
                            showMessage(2, "Provided username already in use", false);
                        }
                        else
                        {
                            var logs = new List<Log>() { new Log(), new Log(), new Log(), new Log(), new Log(), new Log(),
                                                         new Log(), new Log(), new Log(), new Log() };
                            var coll = new CardCollection();
                            var player = new Player { Nickname = tbNickname.Text.Trim(), Password = tbConfirm.Text.Trim(), Collection = coll, Position = "Common" };
                            var playerInfo = new PlrInfo { Player = player, IconName = pnlSignUp.Controls.OfType<CustomPictureBox>().First(x => x.BorderSize == 1).Name };
                            context.CardCollections.Add(coll);
                            context.Logs.AddRange(logs);
                            context.Players.Add(player);
                            context.PlrsInfo.Add(playerInfo);
                            context.SaveChanges();
                            showMessage(2, "User has been created", false);
                        }
                    }
                });
                loaderSIgnUp.Visible = false;
            };
        }
        private void readSavedLogPass()
        {
            if (File.Exists("ini.txt"))
            {
                toggleSwitch1.Value = true;
                labelRemembMe.ForeColor = Color.FromArgb(137, 26, 241);
                var str = File.ReadAllText("ini.txt").Split(' ');
                inputLogin.Text = str[0];
                inputPassword.Text = str[1];
                inputPassword.PasswordChar = '*';
                ActiveControl = null;
            }
        }
        private void RecolorBack(bool purple)
        {
            if (purple)
            {
                lIconBack.BackgroundImage = (Image)Resources.ResourceManager.GetObject("back_50px_purple");
                lBack.ForeColor = Color.FromArgb(137, 16, 221);
            }
            else
            {
                lIconBack.BackgroundImage = (Image)Resources.ResourceManager.GetObject("back_50px_white");
                lBack.ForeColor = Color.FromArgb(224, 224, 224);
            }

        }
        private void ShowBorder(CustomPictureBox pb)
        {
            ActiveControl = null;
            if (pnlSignUp.Controls.OfType<CustomPictureBox>().Any(x => x.BorderSize == 1))
            {
                pnlSignUp.Controls.OfType<CustomPictureBox>().First(x => x.BorderSize == 1).BorderSize = 0;
            }
            pb.BorderSize = 1;
            CheckInput();
        }
        private void CheckInput()
        {
            btnCreate.Visible = tbNickname.Text.Length > 0 && tbPassword.Text.Length > 0 && tbConfirm.Text == tbPassword.Text
                                && pnlSignUp.Controls.OfType<CustomPictureBox>().Any(x => x.BorderSize == 1);
        }
        private void BackToSignIn()
        {
            WindowState = FormWindowState.Normal;
            pnlSignIn.Controls.AddRange(new Control[] { pnlSignUp.Controls[22], pnlSignUp.Controls[23], pnlSignUp.Controls[24] });
            pages.SelectedIndex = 0;
            RecolorBack(false);
        }
        private async void showMessage(int sec, string msg, bool SignIn)
        {
            if (SignIn)
            {
                lSignInInfo.Text = msg;
                await Task.Delay(sec * 1000);
                lSignInInfo.Text = "";
            }
            else
            {
                lSignUpInfo.Text = msg;
                await Task.Delay(sec * 1000);
                lSignUpInfo.Text = "";
            }
        }
        private async void unlock()
        {
            await Task.Run(() =>
             {
                 animation1.Hide(pnlSignIn);
             });
            for (Opacity = 1; Opacity > 0; Opacity -= 0.015) await Task.Delay(1);
            Close();
        }
    }
}
