using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.DataModel;

namespace WindowsFormsApp1
{
    public partial class createClan : Form
    {
        public Point p;
        bool icon = false;
        bool name = false;
        public int? clanID = null;
        string iconName;
        MyDelegat d;
        List<Clan> clans;
        public createClan(MyDelegat sender)
        {
            InitializeComponent();
            using (var context = new GameContext()) clans = context.Clans.ToList();
            d = sender;
        }

        private void createClan_Load(object sender, EventArgs e)
        {
            foreach (var cPB in bunifuPanel1.Controls.OfType<CustomPictureBox>())
                cPB.Click += (s, a) => ShowBorder(s as CustomPictureBox);
            blClose.Click += (s, a) => Hide();
            tbClanName.TextChanged += (s, a) =>
            {
                var clanName = clanID is null ? "" : clans.FirstOrDefault(x => x.Id == clanID).Name;
                label3.Visible = (tbClanName.Text.Length < 1 || clans.Any(x =>
                    x.Name == tbClanName.Text && x.Name != clanName));
                name = !label3.Visible;
                btnCreate.Visible = (name && icon);
            };
            tbClanName.TextChange += (s, a) =>
            {
                tbClanName.MaxLength = tbClanName.Text.Count(x => x == 'W') > 8 ? 10 : 13;
            };
            btnCreate.Click += async (s, a) =>
            {
                int? newClanID = null;
                await Task.Run(() =>
                {
                    using (var context = new GameContext())
                    {
                        if (clanID is null)
                        {
                            context.Clans.Add(new Clan { Name = tbClanName.Text.Trim(), IconName = iconName });
                            context.SaveChanges();
                            newClanID = context.Clans.ToList().Last().Id;
                        }
                        else
                        {
                            var c = context.Clans.First(x => x.Id == clanID);
                            c.Name = tbClanName.Text.Trim();
                            c.IconName = iconName;
                            context.SaveChanges();
                        }
                    }
                });
                d(newClanID);
                clanID = null;

                Hide();
            };
            VisibleChanged += (s, a) =>
            {
                using (var context = new GameContext()) clans = context.Clans.ToList();
                Location = p;
                tbClanName.Text = "";
                btnCreate.Text = clanID is null ? "Create" : "Save";
                label3.Visible = false;
                if (icon)
                {
                    bunifuPanel1.Controls.OfType<CustomPictureBox>().First(x => x.BorderSize == 1).BorderSize = 0;
                    icon = false;
                }
                if (clanID != null)
                {
                    bunifuPanel1.Controls.OfType<CustomPictureBox>().First(x =>
                       x.Name == clans.First(y => y.Id == clanID).IconName).BorderSize = 1;
                    iconName = bunifuPanel1.Controls.OfType<CustomPictureBox>().First(x => x.BorderSize == 1).Name;
                    icon = true;
                    tbClanName.Text = clans.First(x => x.Id == clanID).Name;
                    name = true;
                }
                ActiveControl = null;
            };
        }
        private void ShowBorder(CustomPictureBox pb)
        {
            ActiveControl = null;
            if (icon)
            {
                bunifuPanel1.Controls.OfType<CustomPictureBox>().First(x => x.BorderSize == 1).BorderSize = 0;
            }
            pb.BorderSize = 1;
            icon = true;
            iconName = pb.Name;
            btnCreate.Visible = (name && icon);
        }
    }
}
