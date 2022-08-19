using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using WindowsFormsApp1.DataModel;
using WindowsFormsApp1.Properties;

namespace WindowsFormsApp1
{
    public partial class JoinToClan : Form
    {
        private Clan clan;
        public Clan Clan
        {
            set { clan = value; }
        }
        private MyDelegat d;
        public Point p;
        public JoinToClan(MyDelegat sender)
        {
            d = sender;
            InitializeComponent();
        }

        private void JoinToClan_Load(object sender, EventArgs e)
        {
            blClose.Click += (s, a) => Hide();
            btnJoin.Click += (s, a) =>
            {
                d(clan.Id);
                Hide();
            };
            VisibleChanged += (s, a) =>
            {
                if (Visible)
                {
                    CreateTable();
                    bunifuVScrollBar1.BindTo(playersTable);
                    Location = p;
                    pbIconClan.Image = (Image)Resources.ResourceManager.GetObject(clan.IconName);
                    lClanName.Text = clan.Name;
                }
            };
        }

        private void CreateTable()
        {
            bunifuVScrollBar1.Minimum = -1;
            playersTable.Rows.Clear();
            bunifuVScrollBar1.Maximum = 1;
            using (var context = new GameContext())
            {
                var players = context.Players.Where(x => x.ClanId == clan.Id).ToList();
                for (int i = 0; i < players.Count; i++)
                {
                    var id = players[i].Id;
                    playersTable.Rows.Add(new object[] { i + 1, players[i].Nickname, context.PlrsInfo.First(x => x.PlrId == id).Trophies, (ClanPosition)players[i].clanPosition });
                }
            }
            bunifuVScrollBar1.Minimum = 0;
            bunifuVScrollBar1.Maximum = playersTable.Rows.Count == 0 ? 1 : playersTable.Rows.Count;
            bunifuVScrollBar1.Value = 0;

        }
    }
}
