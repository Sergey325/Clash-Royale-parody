using Bunifu.UI.WinForms;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using WindowsFormsApp1.DataModel;
using WindowsFormsApp1.Properties;

namespace WindowsFormsApp1.Forms
{
    public partial class WinLose : Form
    {

        private Log log;
        public Log Log
        {
            set { log = value; }
        }
        public Point p;

        public WinLose()
        {
            InitializeComponent();
        }

        private void WinLose_Load(object sender, EventArgs e)
        {
            VisibleChanged += (s, a) =>
            {
                if (Visible)
                {
                    Location = p;
                    FillData();
                }
            };
            blClose.Click += (s, a) => Hide();
            btnOk.Click += (s, a) => Hide();
        }
        private void FillData()
        {
            var win = log.ScoresPlayer > log.ScoresOpponent;
            lWinLose.BackgroundImage = (Image)Resources.ResourceManager.GetObject(win ? "win" : "lose");
            lTrophies.Text = (win ? "+" : "-") + log.Trophies.ToString();
            for (int i = 0; i < 3; i++)
            {
                (pnlRedCrowns.Controls[i] as BunifuLabel).BackgroundImage = (Image)Resources.ResourceManager.GetObject(i < log.ScoresOpponent ? "red_crown" : "transparent");
                (pnlBlueCrowns.Controls[i] as BunifuLabel).BackgroundImage = (Image)Resources.ResourceManager.GetObject(i < log.ScoresPlayer ? "blue_crown" : "transparent");
            }
            using (var context = new GameContext())
            {
                lOpponent.Text = context.Players.First(x => x.Id == log.OpponentId).Nickname;
                lPlayer.Text = context.Players.First(x => x.Id == log.PlayerId).Nickname;
            }
        }
    }
}
