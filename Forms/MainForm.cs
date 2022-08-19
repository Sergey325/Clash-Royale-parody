using Bunifu.UI.WinForms;
using Bunifu.UI.WinForms.BunifuButton;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.DataModel;
using WindowsFormsApp1.Forms;
using WindowsFormsApp1.Properties;

///
///
namespace WindowsFormsApp1
{
    enum ClanPosition : byte
    {
        Leader,
        CoLeader,
        Elder,
        Member
    }
    enum ArenaName : byte
    {
        Traning_Camp,
        Spooky_Town,
        Royale_Arena,
        Ice_Peak,
        Electro_Valley,
        Legendary_Arena
    }
    public partial class Form1 : Form
    {
        private Player player;
        private PlrInfo info;
        private List<Player> clanPlayers;
        private List<Clan> clans;
        private List<Player> players;
        private createClan createClan;
        private JoinToClan joinToClan;
        private WinLose winLose;
        private MyDelegat func;
        private BunifuPanel panelClans;
        private BunifuVScrollBar scrollBar;
        private DataGridView table;
        private bool[] filters = { false, false, false };
        private ushort pageOfClans = 1;
        public Form1(Player player, Point p)
        {
            this.Location = p;
            InitializeComponent();
            func = new MyDelegat(Callback);
            createClan = new createClan(func);
            joinToClan = new JoinToClan(func);
            winLose = new WinLose();
            this.player = player;
            playersTable.Columns[6].ReadOnly = player.Position != "Admin";
            SetPlayerInfo();
            buildCardPanels();
            BuildPageClans();
            fillBattlePage();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            lbuttonResize.Click += (s, a) =>
            {
                ResizeWindow();
            };
            lbuttonExit.Click += (s, a) =>
            {
                if (!progBar.Visible && !loader1.Visible)
                {
                    using (var context = new GameContext())
                    {
                        context.PlrsInfo.Find(player.Id).Online = false;
                        context.SaveChanges();
                    }
                    Application.Exit();
                }

            };
            lbuttonRollUp.Click += (s, a) => { WindowState = FormWindowState.Minimized; };
            btnProfile.Click += async (s, a) =>
            {
                panel1.BringToFront();
                panel1.Visible = true;
                pages.SelectedTab = pages.TabPages[0];
                await Task.Delay(1);
                panel1.Visible = false;
            };
            btnBattle.Click += async (s, a) =>
            {
                panel2.BringToFront();
                panel2.Visible = true;
                pages.SelectedTab = pages.TabPages[1];
                scrollLogs.BindTo(pnlLogs);
                await Task.Delay(1);
                panel2.Visible = false;
            };
            btnClans.Click += async (s, a) =>
            {
                await Task.Run(() => { using (var context = new GameContext()) clans = context.Clans.ToList(); });
                panel3.BringToFront();
                if (createClan == null) createClan = new createClan(func);
                if (joinToClan == null) joinToClan = new JoinToClan(func);
                pages.SelectedTab = pages.TabPages[2];
                if (player.ClanId != null)
                {
                    PreparingToShowClan();
                }
                else
                {
                    updateClansList(pageOfClans);
                }
            };
            btnPlayers.Click += (s, a) =>
            {
                buttonAll.OnIdleState.FillColor = Color.FromArgb(167, 16, 221);
                buttonAll.onHoverState.FillColor = Color.FromArgb(167, 16, 221);
                buttonAll.Enabled = false;
                buttonAll.Enabled = true;
                scrollTablePlayer.Minimum = -1;
                pages.SelectedTab = pages.TabPages[3];
                resetFilters();
                tbSearch.Text = "";
                scrollTablePlayer.Minimum = 0;
                scrollTablePlayer.Value = 0;
            };
            playersTable.CellEndEdit += async (s, a) =>
            {
                var position = (string)playersTable.CurrentCell.Value;
                await Task.Run(() =>
                {
                    using (var context = new GameContext())
                    {
                        int id = (int)playersTable.CurrentRow.Cells[1].Value;
                        context.Players.First(x => x.Id == id - 1).Position = position;
                        context.SaveChanges();
                    }
                });
                ClearAndBuildTablePlayers();
            };
            scrollTablePlayer.ValueChanged += (object s, BunifuVScrollBar.ValueChangedEventArgs a) =>
            {
                if (scrollTablePlayer.Enabled)
                    playersTable.FirstDisplayedScrollingRowIndex = playersTable.Rows[playersTable.Rows.Count == 1
                        ? 0 : a.Value].Index;
                bunifuSeparator2.Visible = scrollTablePlayer.Value + 1 == playersTable.Rows.Count;
            };
            buttonAll.Click += (s, a) => resetFilters();
            buttonOnline.Click += (s, a) =>
            {
                if (filters[1])
                {
                    buttonModers.OnIdleState.FillColor = Color.FromArgb(167, 16, 221);
                    buttonModers.onHoverState.FillColor = Color.FromArgb(167, 16, 221);
                }
                else
                {
                    buttonAll.OnIdleState.FillColor = Color.FromArgb(34, 31, 46);
                    buttonAll.onHoverState.FillColor = Color.FromArgb(34, 31, 46);
                    buttonAll.Enabled = false;
                    buttonAll.Enabled = true;
                }
                buttonOnline.OnIdleState.FillColor = Color.FromArgb(167, 16, 221);
                buttonOnline.OnPressedState.FillColor = Color.FromArgb(167, 16, 221);
                filters[0] = true;
                ClearAndBuildTablePlayers();
            };
            buttonModers.Click += (s, a) =>
            {
                if (filters[0])
                {
                    buttonOnline.OnIdleState.FillColor = Color.FromArgb(167, 16, 221);
                    buttonOnline.onHoverState.FillColor = Color.FromArgb(167, 16, 221);
                }
                else
                {
                    buttonAll.OnIdleState.FillColor = Color.FromArgb(34, 31, 46);
                    buttonAll.onHoverState.FillColor = Color.FromArgb(34, 31, 46);
                    buttonAll.Enabled = false;
                    buttonAll.Enabled = true;
                }
                buttonModers.OnIdleState.FillColor = Color.FromArgb(167, 16, 221);
                buttonModers.OnPressedState.FillColor = Color.FromArgb(167, 16, 221);
                filters[1] = true;
                ClearAndBuildTablePlayers();
            };
            buttonSearch.Click += (s, a) =>
            {
                filters[2] = true;
                ClearAndBuildTablePlayers();
            };
            btnSearchBattle.Click += async (s, a) =>
            {
                if (pnlChosenCards.Controls.OfType<CustomPanel>().Count(x => x.BackgroundImage == null) > 0)
                {
                    bunifuSnackbar1.Show(this, "Build a deck first", type: BunifuSnackbar.MessageTypes.Information,
                        duration: 2000, position: BunifuSnackbar.Positions.TopRight);
                }
                else
                {
                    winLose.Hide();
                    btnSearchBattle.Visible = false;
                    lWaiting.Visible = true;
                    loader1.Visible = true;
                    lbCancel.Visible = true;
                    using (var context = new GameContext())
                    {
                        var searchings = context.Searchings.OrderBy(x => Math.Abs(x.Trophies - info.Trophies)).ToList();
                        if (searchings.Any(x => Math.Abs(x.Trophies - info.Trophies) < 100 && x.PlayerId != player.Id))
                        {
                            searchings[0].OpponentId = player.Id;
                            progBar.Tag = player.Id;
                            startBattle(players.First(x => x.Id == searchings[0].PlayerId).Nickname);
                        }
                        else
                        {
                            context.Searchings.Add(new Searching { PlayerId = player.Id, Trophies = info.Trophies });
                            timerSearching.Enabled = true;
                        }
                        await context.SaveChangesAsync();
                    }
                }
            };
            lbCancel.Click += async (s, a) =>
            {
                timerSearching.Enabled = false;
                using (var context = new GameContext())
                {
                    context.Searchings.Remove(context.Searchings.First(x => x.PlayerId == player.Id));
                    await context.SaveChangesAsync();
                }
                btnSearchBattle.Visible = true;
                lWaiting.Visible = false;
                loader1.Visible = false;
                lbCancel.Visible = false;
            };
            timerSearching.Tick += (s, a) =>
            {
                using (var context = new GameContext())
                {
                    var searching = context.Searchings.First(x => x.PlayerId == player.Id);
                    if (searching.OpponentId != null)
                    {
                        startBattle(context.Players.First(x => x.Id == searching.OpponentId).Nickname);
                        progBar.Tag = searching.OpponentId;
                    }
                }
            };
            timerProgBar.Tick += (s, a) =>
            {
                if (progBar.Value == 100)
                {
                    battleResult();
                }
                else progBar.Value += 1;
                if (progBar.Value == 50) generateLog();
            };
        }
        private async void SetPlayerInfo()
        {
            nickLabel.Text = player.Nickname;
            lPlayerName.Text = player.Nickname;
            playersTable.Columns[3].Visible = !(player.Position == "Common");
            if (lPlayerName.Size.Width > 163)
            {
                for (int i = lPlayerName.Text.Length - 1; lPlayerName.Size.Width > 163; i--)
                {
                    lPlayerName.Text = lPlayerName.Text.Remove(i, 1);
                }
                lPlayerName.Text += "...";
            }
            await Task.Run(() =>
            {
                using (var context = new GameContext())
                {
                    players = context.Players.ToList();
                    info = context.PlrsInfo.First(x => x.PlrId == player.Id);
                    pbPlayerIcon.Image = (Image)Resources.ResourceManager.GetObject(info.IconName);
                    pbIcon.Image = (Image)Resources.ResourceManager.GetObject(info.IconName);
                    var playersInfo = context.PlrsInfo.ToList();
                    lTop.Text = (playersInfo.OrderBy(x => x.Trophies).Reverse().ToList().IndexOf(info) + 1).ToString();
                    lAvgPrsnt.Text = $"{100 - ((double)info.Losses / (info.Wins == 0 ? 1 : info.Wins) * 100): #.}%";
                }
            });
            lTrophies.Text = info.Trophies.ToString();
        }
        private async void buildCardPanels()
        {
            var unchosenCards = new List<Card>();
            var chosenCards = new List<Card>();
            await Task.Run(() =>
            {
                using (var context = new GameContext())
                {
                    var collection = context.CardCollections.First(x => x.Id == player.CollectionId);
                    unchosenCards = collection.GetType().GetProperties().Where(
                        x => x.PropertyType == typeof(byte) && (byte)x.GetValue(collection) == 1).Select(
                        x => context.Cards.First(y => y.Name == x.Name)).ToList();
                    chosenCards = collection.GetType().GetProperties().Where(
                        x => x.PropertyType == typeof(byte) && (byte)x.GetValue(collection) == 2).Select(
                        x => context.Cards.First(y => y.Name == x.Name)).ToList();
                }
            });
            fillUnchosenCards(unchosenCards);
            lElixirCost.Tag = new List<double>();
            for (int i = 0; i < chosenCards.Count; i++)
            {
                var p = pnlChosenCards.Controls[i] as CustomPanel;
                p.BackgroundImage = (Image)Resources.ResourceManager.GetObject(chosenCards[i].iconName);
                p.Tag = chosenCards[i].iconName;
                p.BackgroundImageLayout = chosenCards[i].Rarity == "Legendary" ? ImageLayout.Stretch : ImageLayout.Zoom;
                p.BorderSize = 0;
                (lElixirCost.Tag as List<double>).Add(chosenCards[i].Elixir);
                p.DoubleClick += async (s, a) =>
                {
                    if (p.BackgroundImage != null)
                    {
                        p.BorderSize = 2;
                        p.Inactive();
                        p.BackgroundImage = null;
                        var cards = new List<Card>();
                        using (var context = new GameContext())
                        {
                            var col = context.CardCollections.First(x => x.Id == player.CollectionId);
                            col.GetType().GetProperty(p.Tag.ToString()).SetValue(col, (byte)1);
                            double elixir = context.Cards.First(x => x.Name == p.Tag.ToString()).Elixir;
                            (lElixirCost.Tag as List<double>).Remove(elixir);
                            await context.SaveChangesAsync();
                            var collection = context.CardCollections.First(x => x.Id == player.CollectionId);
                            cards = collection.GetType().GetProperties().Where(
                                x => x.PropertyType == typeof(byte) && (byte)x.GetValue(collection) == 1).Select(
                                x => context.Cards.First(y => y.Name == x.Name)).ToList();
                        }
                        var list = (lElixirCost.Tag as List<double>);
                        lElixirCost.Text = $"{(list.Count > 0 ? $"{list.Average():#.0}" : "0.0")}";
                        fillUnchosenCards(cards);
                    }
                };
            }
            var l = lElixirCost.Tag as List<double>;
            lElixirCost.Text = $"{(l.Count > 0 ? $"{l.Average():#.0}" : "0.0")}";
        }
        private void fillUnchosenCards(List<Card> unchosenCards)
        {
            pnlUnchosenCards.Visible = false;
            pnlUnchosenCards.Controls.Clear();
            var panels = new List<Panel>();
            scrollBarCards.Visible = unchosenCards.Count > 8;
            for (int i = 0; i < Math.Ceiling((double)unchosenCards.Count() / 8); i++)
            {
                panels.Add(new Panel
                {
                    Location = new Point(0, (i) * 90),
                    Size = new Size(582, 90),
                    BackColor = Color.FromArgb(34, 31, 46)
                });
            }
            int k = 0;
            foreach (Panel panel in panels)
            {
                for (int i = k; i < k + 8 && i != unchosenCards.Count; i++)
                {
                    var g = new MovableLabel
                    {
                        BackColor = Color.FromArgb(34, 31, 46),
                        BackgroundImage = (Image)Resources.ResourceManager.GetObject(unchosenCards[i].iconName),
                        Tag = unchosenCards[i].iconName,
                        BackgroundImageLayout = unchosenCards[i].Rarity == "Legendary" ? ImageLayout.Stretch : ImageLayout.Zoom,
                        Location = new Point(72 * (i % 8) + 6, 0),
                        Size = new Size(66, 87),
                        Client = page1
                    };
                    panel.Controls.Add(g);
                    g.MouseDown += (s, a) =>
                    {
                        if (pnlChosenCards.Controls.OfType<CustomPanel>().Count(x => x.BackgroundImage == null) != 0)
                        {
                            g.Statik = false;
                            panel.Controls.RemoveByKey(g.Name);
                            page1.Controls.Add(g);
                            g.BringToFront();
                            pnlChosenCards.Controls.OfType<CustomPanel>().First(x => x.BackgroundImage == null).Active();
                        }
                        else g.Statik = true;
                    };
                    g.MouseUp += async (s, a) =>
                    {
                        if (pnlChosenCards.ClientRectangle.IntersectsWith(new Rectangle(
                                new Point(g.Location.X + g.Width / 2 - pnlChosenCards.Location.X,
                                g.Location.Y + g.Height / 2 - pnlChosenCards.Location.Y), new Size(1, 1))))
                        {
                            var cards = new List<Card>();
                            double elixirCost;
                            using (var context = new GameContext())
                            {
                                var col = context.CardCollections.First(x => x.Id == player.CollectionId);
                                col.GetType().GetProperty(g.Tag.ToString()).SetValue(col, (byte)2);
                                elixirCost = context.Cards.First(x => x.Name == g.Tag.ToString()).Elixir;
                                await context.SaveChangesAsync();
                                var collection = context.CardCollections.First(x => x.Id == player.CollectionId);
                                cards = collection.GetType().GetProperties().Where(
                                    x => x.PropertyType == typeof(byte) && (byte)x.GetValue(collection) == 1).Select(
                                    x => context.Cards.First(y => y.Name == x.Name)).ToList();
                            }
                            var p = pnlChosenCards.Controls.OfType<CustomPanel>().First(x => x.BackgroundImage == null);
                            p.BackgroundImage = g.BackgroundImage;
                            p.BackgroundImageLayout = g.BackgroundImageLayout;
                            p.BorderSize = 0;
                            p.Tag = g.Tag.ToString();
                            p.DoubleClick += async (obj, e) =>
                            {
                                if (p.BackgroundImage != null)
                                {
                                    p.BorderSize = 2;
                                    p.Inactive();
                                    p.BackgroundImage = null;
                                    var crds = new List<Card>();
                                    using (var context = new GameContext())
                                    {
                                        var col = context.CardCollections.First(x => x.Id == player.CollectionId);
                                        col.GetType().GetProperty(p.Tag.ToString()).SetValue(col, (byte)1);
                                        double elixir = context.Cards.First(x => x.Name == p.Tag.ToString()).Elixir;
                                        (lElixirCost.Tag as List<double>).Remove(elixir);
                                        await context.SaveChangesAsync();
                                        var collection = context.CardCollections.First(x => x.Id == player.CollectionId);
                                        crds = collection.GetType().GetProperties().Where(
                                            x => x.PropertyType == typeof(byte) && (byte)x.GetValue(collection) == 1).Select(
                                            x => context.Cards.First(y => y.Name == x.Name)).ToList();
                                    }
                                    var list = (lElixirCost.Tag as List<double>);
                                    lElixirCost.Text = $"{(list.Count > 0 ? $"{list.Average():#.0}" : "0.0")}";
                                    fillUnchosenCards(crds);
                                }
                            };
                            g.Dispose();
                            page1.Controls.RemoveByKey(g.Name);
                            (lElixirCost.Tag as List<double>).Add(elixirCost);
                            lElixirCost.Text = $"{(lElixirCost.Tag as List<double>).Average():#.0}";
                            fillUnchosenCards(cards);
                        }
                        else if (pnlChosenCards.Controls.OfType<CustomPanel>().Count(x => x.BackgroundImage == null) != 0)
                        {
                            var p = pnlChosenCards.Controls.OfType<CustomPanel>().First(x => x.BackgroundImage == null);
                            p.BorderColor = Color.Gray;
                            p.BorderColor2 = Color.FromArgb(64, 64, 64);
                            page1.Controls.RemoveByKey(g.Name);
                            panel.Controls.Add(g);
                            g.Location = g.InitialPosition;
                        }
                    };
                }
                k += 8;
                pnlUnchosenCards.Controls.Add(panel);
            }
            if (unchosenCards.Count > 0) pnlUnchosenCards.Controls[0].Location = new Point(0, 0);
            pnlUnchosenCards.Visible = true;
            scrollBarCards.BindTo(pnlUnchosenCards);
            scrollBarCards.Value = 0;
        }
        private async void fillBattlePage()
        {
            panel2.Visible = true;
            int arena = 0;
            var logs = new List<Log>();
            await Task.Run(() =>
            {
                using (var context = new GameContext())
                {
                    logs = context.Logs.Where(x => x.Id >= player.Id * 10 - 9 && x.Id <= player.Id * 10).ToList();
                    info = context.PlrsInfo.First(x => x.PlrId == player.Id);
                    player = context.Players.First(x => x.Id == player.Id);
                    players = context.Players.ToList();
                    if (info.Trophies > 2099) arena = 5;
                    else arena = Enumerable.Range(0, 5).FirstOrDefault(x => ((3 + x) * 100 + 100 * (Math.Pow(x, 2) + 3 * x) / 2) > info.Trophies);
                    pbArena.Image = (Image)Resources.ResourceManager.GetObject($"Arena{arena}");
                    lArena.Text = ((ArenaName)arena).ToString().Replace("_", " ");
                    lArena.Tag = arena;
                    lArenaTrophies.Text = $"{(arena == 0 ? 1 : (3 + arena - 1)) * 100 + 100 * (Math.Pow(arena - 1, 2) + 3 * (arena - 1)) / 2}-{(arena == 5 ? "..." : $"{(3 + arena) * 100 + 100 * (Math.Pow(arena, 2) + 3 * arena) / 2}")}";
                    lArena.Location = new Point((pbArena.Location.X + pbArena.Width / 2) - lArena.Width / 2, lArena.Location.Y);
                    var col = context.CardCollections.First(x => x.Id == player.CollectionId);
                    var cards = context.Cards.Where(x => x.Arena == arena + 1).ToArray();
                    for (int i = 0; i < cards.Length; i++)
                    {
                        (pnlNextCards.Controls[i] as PictureBox).BackgroundImage = (Image)Resources.ResourceManager.GetObject(cards[i].iconName);
                        (pnlNextCards.Controls[i] as PictureBox).BackgroundImageLayout = cards[i].Rarity == "Legendary" ? ImageLayout.Stretch : ImageLayout.Zoom;
                    }
                    if (cards.Length == 0 || ((byte)col.GetType().GetProperty(cards[0].Name).GetValue(col) == 1))
                    {
                        lNextCards.Visible = false;
                        pnlNextCards.Visible = false;
                        pnlLogs.Size = new Size(267, 448);
                        scrollLogs.Size = new Size(17, 442);
                    }
                }
            });
            pnlLogs.Controls.Clear();
            for (int i = 9; i > 9 - logs.Count(x => x.PlayerId != null); i--)
            {
                var win = logs[i].ScoresOpponent < logs[i].ScoresPlayer;
                var t = DateTime.Now - DateTime.Parse(logs[i].time.ToString());
                var panel = new BunifuPanel
                {
                    BackgroundColor = Color.Transparent,
                    BorderColor = Color.FromArgb(64, 140, 216),
                    BorderRadius = 8,
                    BorderThickness = 1,
                    Location = new Point(9, 2 + (9 - i) * 59),
                    ShowBorders = true,
                    Size = new Size(250, 53),
                };
                var lDateTime = new Label
                {
                    AutoSize = true,
                    BackColor = Color.Transparent,
                    Font = new Font("Leelawadee UI", 12F),
                    ForeColor = System.Drawing.Color.FromArgb(224, 224, 224),
                    Location = new Point(178, 2),
                    Size = new Size(58, 21),
                    Text = (t.Days != 0 ? $"{t.Days}d" : t.Hours != 0 ? $"{t.Hours}h" : t.Minutes != 0 ? $"{t.Minutes}m" : $"{t.Seconds}s") + " ago",
                };
                var lIconTrophies = new BunifuLabel
                {
                    AutoSize = false,
                    BackgroundImage = global::WindowsFormsApp1.Properties.Resources.trophy_96px,
                    BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch,
                    Location = new Point(46, 0),
                    Size = new Size(25, 25),
                };
                var lTrophies = new LabelEx
                {
                    BackColor = System.Drawing.Color.Transparent,
                    Color = System.Drawing.Color.RoyalBlue,
                    Color2 = System.Drawing.Color.Magenta,
                    Font = new Font("Leelawadee UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0),
                    Gradientmode = WindowsFormsApp1.LabelEx.GradientMode.horizontal,
                    Location = win ? new Point(10, 0) : new Point(13, 0),
                    Size = new Size(45, 25),
                    Text = $"{(win ? "+" : "-")}{logs[i].Trophies}",
                };
                var lRedCrown = new BunifuLabel
                {
                    AutoSize = false,
                    BackgroundImage = (Image)Resources.ResourceManager.GetObject("red_crown"),
                    BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch,
                    Location = new Point(154, 3),
                    Size = new Size(25, 20),
                };
                var lBlueCrown = new BunifuLabel
                {
                    AutoSize = false,
                    BackgroundImage = (Image)Resources.ResourceManager.GetObject("blue_crown"),
                    BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch,
                    Location = new Point(79, 3),
                    Size = new Size(25, 20),
                };
                var lCrossedSwords = new BunifuLabel
                {
                    AutoSize = false,
                    BackgroundImage = (Image)Resources.ResourceManager.GetObject("crossed_swords"),
                    BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom,
                    Location = new Point(117, 25),
                    Size = new Size(25, 20),
                };
                var lOpponentName = new Label
                {
                    BackColor = System.Drawing.Color.Transparent,
                    Font = new Font("Leelawadee UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0),
                    ForeColor = System.Drawing.Color.FromArgb(200, 48, 68),
                    Location = new Point(137, 20),
                    Size = new Size(108, 25),
                    Text = players.First(x => x.Id == logs[i].OpponentId).Nickname,
                    TextAlign = System.Drawing.ContentAlignment.MiddleRight,
                };
                var lPlayerName = new Label
                {
                    BackColor = System.Drawing.Color.Transparent,
                    Font = new Font("Leelawadee UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0),
                    ForeColor = System.Drawing.Color.FromArgb(64, 140, 216),
                    Location = new Point(11, 20),
                    Size = new Size(108, 25),
                    Text = player.Nickname,
                    TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
                };
                var lScores = new Label
                {
                    AutoSize = true,
                    BackColor = System.Drawing.Color.Transparent,
                    Font = new Font("Leelawadee UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0),
                    ForeColor = System.Drawing.Color.FromArgb(224, 224, 224),
                    Location = new Point(109, 0),
                    Size = new Size(40, 25),
                    Text = $"{logs[i].ScoresPlayer}-{logs[i].ScoresOpponent}",
                };
                panel.Controls.AddRange(new Control[] { lDateTime, lIconTrophies, lTrophies, lRedCrown, lBlueCrown, lCrossedSwords, lOpponentName, lPlayerName, lScores });
                panel.BorderColor = win ? Color.FromArgb(64, 140, 216) : Color.FromArgb(200, 48, 68);
                pnlLogs.Controls.Add(panel);
            }
            if (pnlLogs.Controls.Count > 0) pnlLogs.Controls[0].Location = new Point(9, 2);
            scrollLogs.Visible = (pnlLogs.Controls.Count > 7 && arena == 5) || (pnlLogs.Controls.Count > 5 && arena < 5);
            panel2.Visible = false;
        }
        private async void generateLog()
        {
            using (var context = new GameContext())
            {
                if ((int)progBar.Tag != player.Id)
                {
                    var opponent = context.Players.First(x => x.Id == (int)progBar.Tag);
                    player = context.Players.First(x => x.Id == player.Id);
                    info = context.PlrsInfo.First(x => x.PlrId == player.Id);
                    var infoOp = context.PlrsInfo.First(x => x.PlrId == opponent.Id);
                    var logsPl = context.Logs.Where(x => x.Id >= player.Id * 10 - 9 && x.Id <= player.Id * 10).ToList();
                    var logsOp = context.Logs.Where(x => x.Id >= opponent.Id * 10 - 9 && x.Id <= opponent.Id * 10).ToList();
                    int arenaOp;
                    if (infoOp.Trophies > 2099) arenaOp = 5;
                    else arenaOp = Enumerable.Range(0, 5).FirstOrDefault(x => ((3 + x) * 100 + 100 * (Math.Pow(x, 2) + 3 * x) / 2) > infoOp.Trophies);
                    var trophies = 30 + (int)Math.Round((double)(info.Trophies - infoOp.Trophies) / 10);
                    var random = new Random();
                    var scores = new List<byte> { 0, 1, 2, 3 };
                    var op = scores[random.Next(4)];
                    scores.RemoveAt(op);
                    var pl = scores[random.Next(3)];

                    //filling player logs
                    for (int i = 0; i < 9; i++)
                    {
                        if (logsPl[i + 1].PlayerId != null)
                        {
                            logsPl[i].PlayerId = logsPl[i + 1].PlayerId;
                            logsPl[i].OpponentId = logsPl[i + 1].OpponentId;
                            logsPl[i].time = logsPl[i + 1].time;
                            logsPl[i].ScoresOpponent = logsPl[i + 1].ScoresOpponent;
                            logsPl[i].ScoresPlayer = logsPl[i + 1].ScoresPlayer;
                            logsPl[i].Trophies = logsPl[i + 1].Trophies;
                        }
                    }
                    logsPl[9].PlayerId = player.Id;
                    logsPl[9].OpponentId = opponent.Id;
                    logsPl[9].time = DateTime.Now;
                    logsPl[9].ScoresOpponent = op;
                    logsPl[9].ScoresPlayer = pl;
                    logsPl[9].Trophies = trophies;

                    //filling opponent logs
                    for (int i = 0; i < 9; i++)
                    {
                        if (logsOp[i + 1].PlayerId != null)
                        {
                            logsOp[i].PlayerId = logsOp[i + 1].PlayerId;
                            logsOp[i].OpponentId = logsOp[i + 1].OpponentId;
                            logsOp[i].time = logsOp[i + 1].time;
                            logsOp[i].ScoresOpponent = logsOp[i + 1].ScoresOpponent;
                            logsOp[i].ScoresPlayer = logsOp[i + 1].ScoresPlayer;
                            logsOp[i].Trophies = logsOp[i + 1].Trophies;
                        }
                    }
                    logsOp[9].PlayerId = opponent.Id;
                    logsOp[9].OpponentId = player.Id;
                    logsOp[9].time = DateTime.Now;
                    logsOp[9].ScoresOpponent = pl;
                    logsOp[9].ScoresPlayer = op;
                    logsOp[9].Trophies = trophies;
                    if (op > pl)
                    {
                        infoOp.Wins += 1;
                        info.Losses += 1;
                    }
                    else
                    {
                        info.Wins += 1;
                        infoOp.Losses += 1;
                    }
                    infoOp.Trophies += op > pl ? trophies : infoOp.Trophies < 50 ? 0 : -trophies;
                    if (opponent.ClanId != null) context.Clans.First(x => x.Id == opponent.ClanId).TotalTrophies += op > pl ? trophies : infoOp.Trophies < 50 ? 0 : -trophies;
                    info.Trophies += pl > op ? trophies : info.Trophies < 50 ? 0 : -trophies;
                    if (player.ClanId != null) context.Clans.First(x => x.Id == player.ClanId).TotalTrophies += pl > op ? trophies : info.Trophies < 50 ? 0 : -trophies;
                    context.Searchings.Remove(context.Searchings.First(x => x.PlayerId == player.Id));
                    await context.SaveChangesAsync();
                }
            }
        }
        private async void battleResult()
        {
            timerProgBar.Enabled = false;
            progBar.Visible = false;
            lplayer.Visible = false;
            lOpponent.Visible = false;
            lswords.Visible = false;
            progBar.Value = 0;
            pbArena.Visible = true;
            btnSearchBattle.Visible = true;
            pbSwordsGif.Visible = false;
            using (var context = new GameContext()) winLose.Log = context.Logs.First(x => x.Id == player.Id * 10);
            winLose.p = new Point(Location.X + Width / 2 - winLose.Width / 2, Location.Y + Height / 2 - winLose.Height / 2);
            winLose.panel1.BringToFront();
            winLose.panel1.Visible = true;
            winLose.Show();
            await Task.Delay(1);
            winLose.panel1.Visible = false;
            UpdateCards();
            fillBattlePage();
            buildCardPanels();
        }
        private void startBattle(string nameOP)
        {
            pbArena.Visible = false;
            lWaiting.Visible = false;
            loader1.Visible = false;
            lbCancel.Visible = false;
            progBar.Visible = true;
            lplayer.Visible = true;
            lOpponent.Visible = true;
            lswords.Visible = true;
            pbSwordsGif.Visible = true;
            lplayer.Text = player.Nickname;
            lOpponent.Text = nameOP;

            timerProgBar.Enabled = true;
            timerSearching.Enabled = false;
        }
        private async void UpdateCards()
        {
            using (var context = new GameContext())
            {
                var cards = context.Cards;
                int arena;
                info = context.PlrsInfo.First(x => x.PlrId == player.Id);
                var col = context.CardCollections.First(x => x.Id == player.CollectionId);
                if (info.Trophies > 2099) arena = 5;
                else arena = Enumerable.Range(0, 5).FirstOrDefault(x => ((3 + x) * 100 + 100 * (Math.Pow(x, 2) + 3 * x) / 2) > info.Trophies);
                foreach (var prop in col.GetType().GetProperties().Where(x => x.PropertyType == typeof(bool)))
                {
                    prop.SetValue(col, (byte)(cards.First(x => x.Name == prop.Name).Arena == arena ? 1 : (byte)prop.GetValue(col)));
                }
                await context.SaveChangesAsync();
            }
        }
        private void resetFilters()
        {
            filters[0] = false;
            filters[1] = false;
            filters[2] = false;
            ClearAndBuildTablePlayers();
            buttonOnline.OnIdleState.FillColor = Color.FromArgb(34, 31, 46);
            buttonOnline.OnPressedState.FillColor = Color.FromArgb(34, 31, 46);
            buttonOnline.onHoverState.FillColor = Color.FromArgb(34, 31, 46);
            buttonModers.OnIdleState.FillColor = Color.FromArgb(34, 31, 46);
            buttonModers.onHoverState.FillColor = Color.FromArgb(34, 31, 46);
            buttonModers.OnPressedState.FillColor = Color.FromArgb(34, 31, 46);
            buttonOnline.Enabled = false;
            buttonModers.Enabled = false;
            buttonOnline.Enabled = true;
            buttonModers.Enabled = true;
        }
        private void updateClansList(ushort page)
        {
            panel3.SendToBack();
            panelClans.Controls.Clear();
            ShowPageClans(page);
            page3.Controls.Remove(scrollBar);
            var scrollBar1 = new BunifuVScrollBar()
            {
                AllowCursorChanges = false,
                AllowIncrementalClickMoves = false,
                AllowMouseDownEffects = false,
                AllowMouseHoverEffects = false,
                AllowScrollingAnimations = false,
                AllowScrollKeysDetection = false,
                AllowScrollOptionsMenu = false,
                BackgroundColor = System.Drawing.Color.FromArgb(64, 64, 64),
                BindingContainer = panelClans,
                BorderColor = System.Drawing.Color.Purple,
                LargeChange = 40,
                Location = new Point(687, 48),
                Name = "scrollBar1",
                ScrollBarBorderColor = System.Drawing.Color.Purple,
                ScrollBarColor = System.Drawing.Color.FromArgb(64, 64, 64),
                Size = new Size(18, 383),
                SmallChange = 1,
                ThumbColor = System.Drawing.Color.Indigo,
                ThumbStyle = BunifuVScrollBar.ThumbStyles.Inset,
                Value = 0,
                Visible = false,
            };
            scrollBar1.BindTo(panelClans);
            scrollBar = scrollBar1;
            page3.Controls.Add(scrollBar1);
        }
        private void ClearAndBuildTablePlayers()
        {
            scrollTablePlayer.Minimum = -1;
            playersTable.Rows.Clear();
            createTablePlayers();
            scrollTablePlayer.Enabled = playersTable.Rows.Count > 0;
            scrollTablePlayer.Maximum = playersTable.Rows.Count == 0 || playersTable.Rows.Count == 1
                ? 1 : playersTable.Rows.Count - 1;
            scrollTablePlayer.Minimum = 0;
            scrollTablePlayer.Value = 0;
        }
        private void createTablePlayers()
        {
            using (GameContext context = new GameContext())
            {
                players = context.Players.ToList();
                var Info = context.PlrsInfo.ToList();
                lRecords.Text = $"Records: {players.Count}";
                if (filters[0]) players = players.Where(x => Info.First(y => y.PlrId == x.Id).Online).ToList();
                if (filters[1]) players = players.Where(x => x.Position == "Moder").ToList();
                if (filters[2]) players = players.Where(x => x.Nickname.ToLower().Contains(tbSearch.Text.Trim().ToLower())).ToList();
                lFiltered.Text = $"Filtered: {players.Count}";
                foreach (var player in players)
                {
                    var info = Info.First(x => player.Id == x.PlrId);
                    playersTable.Rows.Add(new object[] {
                    Resources.ResourceManager.GetObject(info.IconName.Replace("64","38")),
                    player.Id,
                    player.Nickname,
                    player.Password,
                    info.Trophies,
                    info.Online ? Resources.green_dot : Resources.red_dot,
                    player.Position
                    });
                }
            }

        }
        private void createTableClanPlayers()
        {
            table.Rows.Clear();
            using (GameContext context = new GameContext())
            {
                clanPlayers = context.Players.Where(x => x.ClanId == player.ClanId).ToList();
                foreach (var plr in clanPlayers)
                {
                    var info = context.PlrsInfo.First(x => x.PlrId == plr.Id);
                    table.Rows.Add(new object[] {
                    Resources.ResourceManager.GetObject(info.IconName.Replace("64","38")),
                    plr.Nickname,
                    info.Trophies,
                    info.Online ? Resources.green_dot : Resources.red_dot,
                    (ClanPosition)plr.clanPosition,
                    (byte)plr.clanPosition > player.clanPosition ? Resources.multiply_30px : Resources.transparent
                    });
                }
                scrollBar.BindTo(table);
            }
        }
        private async void ShowPageClans(ushort page)
        {
            panelClans.Visible = false;
            using (var context = new GameContext()) clans = context.Clans.ToList();
            var p = page * 10;
            var sliceclans = clans.Where(x => clans.IndexOf(x) >= p - 10 && clans.IndexOf(x) < p).ToList();
            for (int i = 0; i < sliceclans.Count; i++)
            {
                createClanPanel(sliceclans[i], i);
            }
            panelClans.Controls[0].Location = new Point(15, 0);
            await Task.Delay(1);
            panelClans.Visible = true;
        }
        private void createClanPanel(Clan clan, int n)
        {
            ///create labels
            ///
            // NAME
            var label1 = new BunifuLabel
            {
                AutoSize = true,
                Font = new Font("Leelawadee UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0),
                ForeColor = Color.White,
                Location = new Point(61, 16),
                Text = clan.Name
            };

            // NUMBER OF PLAYERS
            var label2 = new BunifuLabel
            {
                AutoSize = false,
                Font = new Font("Leelawadee UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0),
                ForeColor = Color.White,
                Location = new Point(259, 18),
                Size = new Size(48, 20),
                Text = $"{clan.NumbOfPlayers}/50"
            };

            // NUMBER OF TROPHIES
            var label3 = new BunifuLabel
            {
                AutoSize = true,
                Font = new Font("Leelawadee UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 0),
                ForeColor = Color.White,
                Location = new Point(345, 18),
                Text = clan.TotalTrophies.ToString(),
                TextAlignment = ContentAlignment.MiddleCenter
            };

            // IMAGE CROWN
            var label4 = new BunifuLabel
            {
                AutoSize = false,
                BackgroundImage = (Image)Resources.ResourceManager.GetObject(clan.IconName),
                BackgroundImageLayout = ImageLayout.Stretch,
                Location = new Point(15, 9),
                Size = new Size(40, 40)
            };

            // IMAGE PEOPLE
            var label5 = new BunifuLabel
            {
                AutoSize = false,
                BackgroundImage = Resources.people_96px,
                BackgroundImageLayout = ImageLayout.Stretch,
                Location = new Point(227, 12),
                Size = new Size(30, 30)
            };

            // IMAGE TROPHIES
            var label6 = new BunifuLabel
            {
                AutoSize = false,
                BackgroundImage = Resources.trophy_96px,
                BackgroundImageLayout = ImageLayout.Stretch,
                Location = new Point(317, 12),
                Size = new Size(30, 30)
            };

            ///create panel
            var panel = new BunifuPanel
            {
                BackgroundColor = Color.Black,
                BorderRadius = 50,
                BorderThickness = 2,
                Location = new Point(15, 60 * n),
                Size = new Size(423, 55),
            };
            panel.Click += (s, a) =>
            {
                if (joinToClan.Visible) joinToClan.Hide();
                joinToClan.p = new Point(Location.X + 963 / 2 - 407 / 2, Location.Y + 50);
                joinToClan.Clan = clan;
                joinToClan.Show();
            };
            panel.MouseEnter += (s, a) => { panel.BackgroundColor = Color.FromArgb(65, 61, 84); };
            panel.MouseLeave += (s, a) => { panel.BackgroundColor = Color.Black; };
            ///
            panel.Controls.Add(label1);
            panel.Controls.Add(label2);
            panel.Controls.Add(label3);
            panel.Controls.Add(label4);
            panel.Controls.Add(label5);
            panel.Controls.Add(label6);
            foreach (BunifuLabel control in panel.Controls)
            {
                control.Click += (s, a) =>
                {
                    if (joinToClan.Visible) joinToClan.Hide();
                    joinToClan.p = new Point(Location.X + 963 / 2 - 407 / 2, Location.Y + 50);
                    joinToClan.Clan = clan;
                    joinToClan.Show();
                };
                control.MouseEnter += (s, a) =>
                {
                    panel.BackgroundColor = Color.FromArgb(65, 61, 84);
                };
                control.MouseLeave += (s, a) =>
                {
                    panel.BackgroundColor = Color.Black;
                };
            }
            panelClans.Controls.Add(panel);

        }
        private void PreparingToShowClan()
        {
            panel3.SendToBack();
            scrollBar.Minimum = -1;
            createTableClanPlayers();
            scrollBar.Maximum = table.Rows.Count == 0 || table.Rows.Count == 1
                ? 1 : table.Rows.Count - 1;
            scrollBar.Minimum = 0;
            scrollBar.Value = 0;
        }
        private async void Callback(int? clanID = null)
        {
            if (clanID != null)
            {
                await Task.Run(() =>
                {
                    using (var context = new GameContext())
                    {
                        var plr = context.Players.First(x => x.Id == player.Id);
                        info = context.PlrsInfo.First(x => x.PlrId == player.Id);
                        var cln = context.Clans.First(x => x.Id == clanID);
                        plr.ClanId = clanID;
                        plr.clanPosition = (byte)(cln.NumbOfPlayers == 0 ? 0 : 3);
                        cln.NumbOfPlayers++;
                        cln.TotalTrophies += info.Trophies;
                        player = plr;
                        context.SaveChanges();
                    };
                });

            }
            if (WindowState == FormWindowState.Maximized)
            {
                ResizeWindow();
                BuildPageClans();
                await Task.Delay(1);
                ResizeWindow();
            }
            else BuildPageClans();

        }
        private async void BuildPageClans()
        {
            page3.Controls.Remove(panel3);
            page3.Controls.Clear();
            page3.Controls.Add(panel3);
            List<PlrInfo> clansinfo = new List<PlrInfo>();
            await Task.Run(() =>
            {
                using (var context = new GameContext())
                {
                    player = context.Players.First(x => x.Id == player.Id);
                    players = context.Players.ToList();
                    clans = context.Clans.ToList();
                    foreach (var plr in players.Where(x => x.ClanId == player.ClanId))
                    {
                        clansinfo.Add(context.PlrsInfo.First(x => x.PlrId == plr.Id));
                    }
                }
            });

            if (player.ClanId == null)
            {
                lClanName.Text = "No Clan";
                var btnCreateNew = new BunifuButton
                {
                    AllowMouseEffects = true,
                    Anchor = AnchorStyles.None,
                    Font = new Font("Leelawadee UI", 11F),
                    IndicateFocus = true,
                    Location = new Point(294, 410),
                    Name = "btnCreateNew",
                    Size = new Size(153, 44),
                    Text = "Create new"
                };
                btnCreateNew.onHoverState.BorderColor = Color.FromArgb(117, 26, 241);
                btnCreateNew.onHoverState.BorderRadius = 34;
                btnCreateNew.onHoverState.BorderThickness = 2;
                btnCreateNew.onHoverState.ForeColor = Color.FromArgb(117, 26, 241);
                btnCreateNew.onHoverState.FillColor = Color.Transparent;
                btnCreateNew.OnIdleState.BorderColor = System.Drawing.Color.FromArgb(140, 64, 237);
                btnCreateNew.OnIdleState.ForeColor = System.Drawing.Color.FromArgb(140, 64, 237);
                btnCreateNew.OnIdleState.BorderRadius = 34;
                btnCreateNew.OnIdleState.BorderThickness = 2;
                btnCreateNew.OnIdleState.FillColor = System.Drawing.Color.FromArgb(34, 31, 46);
                btnCreateNew.OnPressedState.BorderColor = System.Drawing.Color.FromArgb(140, 64, 237);
                btnCreateNew.OnPressedState.ForeColor = System.Drawing.Color.FromArgb(140, 64, 237);
                btnCreateNew.OnPressedState.BorderRadius = 34;
                btnCreateNew.OnPressedState.BorderThickness = 2;
                btnCreateNew.OnPressedState.FillColor = System.Drawing.Color.FromArgb(34, 31, 46);
                btnCreateNew.Click += (s, a) =>
                {
                    createClan.p = new Point(Location.X + 963 / 2 - 407 / 2, Location.Y + 50);
                    createClan.clanID = null;
                    createClan.Show();
                };

                var lbuttonForward = new BunifuLabel()
                {
                    AutoSize = false,
                    Anchor = AnchorStyles.Top,
                    BackColor = Color.FromArgb(34, 31, 46),
                    BackgroundImage = Resources.icons8_forward_96px_2__1__1,
                    Location = new Point(596, 191),
                    Name = "lbuttonForward",
                    Size = new Size(35, 45),
                    BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
                };
                lbuttonForward.Click += (s, a) => { if (pageOfClans * 10 < clans.Count) updateClansList(++pageOfClans); };

                var lbuttonBack = new BunifuLabel()
                {
                    AutoSize = false,
                    Anchor = AnchorStyles.Top,
                    BackColor = Color.FromArgb(34, 31, 46),
                    BackgroundImage = global::WindowsFormsApp1.Properties.Resources.icons8_back_96px_4__1__1,
                    Location = new Point(123, 191),
                    Name = "lbuttonBack",
                    Size = new Size(35, 45),
                    BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
                };
                lbuttonBack.Click += (s, a) =>
                {
                    if (pageOfClans != 1) updateClansList(--pageOfClans);
                };

                var bunifuPanel7 = new BunifuPanel()
                {
                    Anchor = AnchorStyles.Bottom | AnchorStyles.Top,
                    BackgroundColor = System.Drawing.Color.FromArgb(34, 31, 46),
                    BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch,
                    Location = new Point(150, 48),
                    Name = "bunifuPanel7",
                    ShowBorders = false,
                    Size = new Size(440, 330),
                    Visible = false
                };

                var scrollBar1 = new BunifuVScrollBar()
                {
                    AllowCursorChanges = false,
                    AllowIncrementalClickMoves = false,
                    AllowMouseDownEffects = false,
                    AllowMouseHoverEffects = false,
                    AllowScrollingAnimations = false,
                    AllowScrollKeysDetection = false,
                    AllowScrollOptionsMenu = false,
                    BackgroundColor = System.Drawing.Color.FromArgb(64, 64, 64),
                    BorderColor = System.Drawing.Color.Purple,
                    LargeChange = 40,
                    Location = new Point(687, 48),
                    Name = "scrollBar1",

                    ScrollBarBorderColor = System.Drawing.Color.Purple,
                    ScrollBarColor = System.Drawing.Color.FromArgb(64, 64, 64),
                    Size = new Size(18, 383),
                    SmallChange = 1,
                    ThumbColor = System.Drawing.Color.Indigo,
                    ThumbStyle = BunifuVScrollBar.ThumbStyles.Inset,
                    Value = 0,
                    Visible = false,
                };
                scrollBar1.OnDisable.ScrollBarBorderColor = System.Drawing.Color.Silver;
                scrollBar1.OnDisable.ScrollBarColor = System.Drawing.Color.Transparent;
                scrollBar1.OnDisable.ThumbColor = System.Drawing.Color.Silver;
                scrollBar = scrollBar1;

                var bunifuSeparator1 = new BunifuSeparator()
                {
                    Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                    BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch,
                    LineColor = System.Drawing.Color.Indigo,
                    LineStyle = Bunifu.UI.WinForms.BunifuSeparator.LineStyles.DoubleEdgeFaded,
                    LineThickness = 4,
                    Location = new Point(194, 385),
                    Margin = new System.Windows.Forms.Padding(4),
                    Name = "bunifuSeparator1",
                    Size = new Size(352, 18),
                    Visible = false
                };

                panelClans = bunifuPanel7;
                page3.Controls.Add(btnCreateNew);
                page3.Controls.Add(lbuttonForward);
                page3.Controls.Add(lbuttonBack);
                page3.Controls.Add(bunifuSeparator1);
                page3.Controls.Add(scrollBar1);
                page3.Controls.Add(bunifuPanel7);
                btnCreateNew.Focus();
                updateClansList(pageOfClans);
            }
            else
            {
                DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle
                {
                    Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter,
                    BackColor = System.Drawing.Color.FromArgb(23, 21, 35)
                };
                DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle
                {
                    Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter,
                    BackColor = System.Drawing.Color.FromArgb(157, 16, 221),
                    Font = new Font("Leelawadee UI", 10F, System.Drawing.FontStyle.Bold),
                    ForeColor = System.Drawing.Color.FromArgb(224, 224, 224),
                    SelectionBackColor = System.Drawing.Color.FromArgb(157, 16, 221),
                    SelectionForeColor = System.Drawing.Color.White,
                    WrapMode = System.Windows.Forms.DataGridViewTriState.True,
                };
                DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle
                {
                    Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter,
                    Padding = new System.Windows.Forms.Padding(0, 0, 15, 0)
                };
                DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle
                {
                    Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter,
                    Font = new Font("Leelawadee UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0),
                    Padding = new System.Windows.Forms.Padding(0, 0, 18, 0),
                };
                DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle
                { Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter };
                DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle
                {
                    Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter,
                    BackColor = System.Drawing.Color.FromArgb(34, 31, 46),
                    Font = new Font("Leelawadee UI", 10.75F, System.Drawing.FontStyle.Bold),
                    ForeColor = System.Drawing.Color.FromArgb(224, 224, 224),
                    SelectionBackColor = System.Drawing.Color.FromArgb(47, 43, 63),
                    SelectionForeColor = System.Drawing.Color.FromArgb(224, 224, 224),
                };
                DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle
                {
                    Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter,
                    BackColor = System.Drawing.SystemColors.Control,
                    Font = new Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 204),
                    ForeColor = System.Drawing.SystemColors.WindowText,
                    SelectionBackColor = System.Drawing.SystemColors.Highlight,
                    SelectionForeColor = System.Drawing.SystemColors.HighlightText,
                    WrapMode = System.Windows.Forms.DataGridViewTriState.True,
                };

                var clan = clans.First(x => x.Id == player.ClanId);
                lClanName.Text = clan.Name;

                var avgwinrate = clansinfo.Select(x => 100 - ((double)x.Losses / (x.Wins == 0 ? 1 : x.Wins) * 100)).Average();

                var lbtnEdit = new BunifuLabel
                {
                    AllowParentOverrides = false,
                    AutoEllipsis = false,
                    AutoSize = false,
                    BackgroundImage = global::WindowsFormsApp1.Properties.Resources.edit_64px,
                    BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom,
                    Cursor = System.Windows.Forms.Cursors.Default,
                    CursorType = System.Windows.Forms.Cursors.Default,
                    Font = new Font("Segoe UI", 9F),
                    Name = "lbtnEdit",
                    RightToLeft = System.Windows.Forms.RightToLeft.No,
                    Size = new Size(23, 39),
                    TextAlignment = System.Drawing.ContentAlignment.TopLeft,
                    TextFormat = Bunifu.UI.WinForms.BunifuLabel.TextFormattingOptions.Default,
                    Visible = player.clanPosition == 0 || player.clanPosition == 1
                };

                var PBClanIcon = new CustomPictureBox
                {
                    BorderCapStyle = System.Drawing.Drawing2D.DashCap.Flat,
                    BorderColor = System.Drawing.Color.RoyalBlue,
                    BorderColor2 = System.Drawing.Color.HotPink,
                    BorderLineStyle = System.Drawing.Drawing2D.DashStyle.Solid,
                    BorderSize = 0,
                    GradientAngle = 50F,
                    Image = (Image)Resources.ResourceManager.GetObject(clan.IconName),
                    Location = new Point(16, 14),
                    Name = "PBClanIcon",
                    Size = new Size(81, 81),
                    SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage,
                    TabStop = false
                };

                var lblClanName = new LabelEx
                {
                    AutoSize = true,
                    Color = System.Drawing.Color.RoyalBlue,
                    Color2 = System.Drawing.Color.Magenta,
                    Font = new Font("Leelawadee UI", 24F),
                    Gradientmode = LabelEx.GradientMode.horizontal,
                    Location = new Point(92, 27),
                    Name = "lblClanName",
                    Text = clan.Name,
                };

                var lblLeadName = new LabelEx
                {
                    AutoSize = true,
                    Anchor = AnchorStyles.Top | AnchorStyles.Right,
                    Color = System.Drawing.Color.RoyalBlue,
                    Color2 = System.Drawing.Color.HotPink,
                    Font = new Font("Leelawadee UI", 14F, System.Drawing.FontStyle.Bold),
                    Gradientmode = LabelEx.GradientMode.horizontal,
                    ImageAlign = System.Drawing.ContentAlignment.MiddleRight,
                    Location = new Point(608, 68),
                    Name = "lblLeadName",
                    Size = new Size(73, 25),
                    Text = players.First(x => x.ClanId == player.ClanId && x.clanPosition == 0).Nickname,
                    TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                };

                var lblLeader = new BunifuLabel
                {
                    AllowParentOverrides = false,
                    AutoEllipsis = false,
                    Anchor = AnchorStyles.Top | AnchorStyles.Right,
                    Cursor = System.Windows.Forms.Cursors.Default,
                    CursorType = System.Windows.Forms.Cursors.Default,
                    Font = new Font("Leelawadee UI", 13F),
                    ForeColor = System.Drawing.Color.FromArgb(140, 64, 237),
                    Location = new Point(552, 69),
                    Name = "lblLeader",
                    RightToLeft = System.Windows.Forms.RightToLeft.No,
                    Size = new Size(55, 23),
                    Text = "Leader: ",
                    TextAlignment = System.Drawing.ContentAlignment.TopLeft,
                    TextFormat = Bunifu.UI.WinForms.BunifuLabel.TextFormattingOptions.Default,
                };

                var lblIconTrophy = new BunifuLabel
                {
                    AllowParentOverrides = false,
                    AutoEllipsis = false,
                    AutoSize = false,
                    Anchor = AnchorStyles.Top,
                    BackgroundImage = global::WindowsFormsApp1.Properties.Resources.trophy_96px,
                    BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch,
                    Cursor = System.Windows.Forms.Cursors.Default,
                    CursorType = System.Windows.Forms.Cursors.Default,
                    Font = new Font("Segoe UI", 9F),
                    Location = new Point(229, 111),
                    Name = "lblIconTrophy",
                    RightToLeft = System.Windows.Forms.RightToLeft.No,
                    Size = new Size(40, 40),
                    TextAlignment = System.Drawing.ContentAlignment.TopLeft,
                    TextFormat = Bunifu.UI.WinForms.BunifuLabel.TextFormattingOptions.Default,
                };

                var lblTrophy = new BunifuLabel
                {
                    AllowParentOverrides = false,
                    AutoEllipsis = false,
                    Anchor = AnchorStyles.Top,
                    Cursor = System.Windows.Forms.Cursors.Default,
                    CursorType = System.Windows.Forms.Cursors.Default,
                    Font = new Font("Leelawadee UI", 15F),
                    ForeColor = System.Drawing.Color.FromArgb(155, 56, 239),
                    Location = new Point(275, 117),
                    Name = "lblTrophy",
                    RightToLeft = System.Windows.Forms.RightToLeft.No,
                    Size = new Size(55, 28),
                    Text = clan.TotalTrophies.ToString(),
                    TextAlignment = System.Drawing.ContentAlignment.TopLeft,
                    TextFormat = Bunifu.UI.WinForms.BunifuLabel.TextFormattingOptions.Default,
                };

                var lblIconPeople = new BunifuLabel
                {
                    AllowParentOverrides = false,
                    AutoEllipsis = false,
                    AutoSize = false,
                    Anchor = AnchorStyles.Top,
                    BackgroundImage = global::WindowsFormsApp1.Properties.Resources.people_64px,
                    BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch,
                    Cursor = System.Windows.Forms.Cursors.Default,
                    CursorType = System.Windows.Forms.Cursors.Default,
                    Font = new Font("Segoe UI", 9F),
                    Location = new Point(410, 109),
                    Name = "lblIconPeople",
                    RightToLeft = System.Windows.Forms.RightToLeft.No,
                    Size = new Size(45, 45),
                    TextAlignment = System.Drawing.ContentAlignment.TopLeft,
                    TextFormat = Bunifu.UI.WinForms.BunifuLabel.TextFormattingOptions.Default,
                };

                var lblPeople = new BunifuLabel
                {
                    AllowParentOverrides = false,
                    AutoEllipsis = false,
                    Anchor = AnchorStyles.Top,
                    Cursor = System.Windows.Forms.Cursors.Default,
                    CursorType = System.Windows.Forms.Cursors.Default,
                    Font = new Font("Leelawadee UI", 15F),
                    ForeColor = System.Drawing.Color.FromArgb(155, 56, 239),
                    Location = new Point(460, 117),
                    Name = "ldlPeople",
                    RightToLeft = System.Windows.Forms.RightToLeft.No,
                    Size = new Size(52, 28),
                    Text = $"{clan.NumbOfPlayers}/50",
                    TextAlignment = System.Drawing.ContentAlignment.TopLeft,
                    TextFormat = Bunifu.UI.WinForms.BunifuLabel.TextFormattingOptions.Default,
                };

                var lblIconTop = new BunifuLabel
                {
                    AllowParentOverrides = false,
                    AutoEllipsis = false,
                    AutoSize = false,
                    Anchor = AnchorStyles.Top,
                    BackgroundImage = global::WindowsFormsApp1.Properties.Resources.leaderboard_64px,
                    BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch,
                    Cursor = System.Windows.Forms.Cursors.Default,
                    CursorType = System.Windows.Forms.Cursors.Default,
                    Font = new Font("Segoe UI", 9F),
                    Location = new Point(581, 102),
                    Name = "lblIconTop",
                    RightToLeft = System.Windows.Forms.RightToLeft.No,
                    Size = new Size(55, 55),
                    TextAlignment = System.Drawing.ContentAlignment.TopLeft,
                    TextFormat = Bunifu.UI.WinForms.BunifuLabel.TextFormattingOptions.Default,
                };

                var lblTop = new BunifuLabel
                {
                    AllowParentOverrides = false,
                    AutoEllipsis = false,
                    Anchor = AnchorStyles.Top,
                    Cursor = System.Windows.Forms.Cursors.Default,
                    CursorType = System.Windows.Forms.Cursors.Default,
                    Font = new Font("Leelawadee UI", 15F),
                    ForeColor = System.Drawing.Color.FromArgb(155, 56, 239),
                    Location = new Point(642, 117),
                    Name = "lblTop",
                    RightToLeft = System.Windows.Forms.RightToLeft.No,
                    Size = new Size(22, 28),
                    Text = (clans.OrderBy(x => x.TotalTrophies).Reverse().ToList().IndexOf(clan) + 1).ToString(),
                    TextAlignment = System.Drawing.ContentAlignment.TopLeft,
                    TextFormat = Bunifu.UI.WinForms.BunifuLabel.TextFormattingOptions.Default,
                };

                var bunifuSeparator1 = new BunifuSeparator
                {
                    AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange,
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                    BackColor = System.Drawing.Color.Transparent,
                    BackgroundImage = ((System.Drawing.Image)(Resources.ResourceManager.GetObject("bunifuSeparator1.BackgroundImage"))),
                    BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch,
                    DashCap = Bunifu.UI.WinForms.BunifuSeparator.CapStyles.Flat,
                    LineColor = System.Drawing.Color.FromArgb(140, 64, 237),
                    LineStyle = Bunifu.UI.WinForms.BunifuSeparator.LineStyles.DoubleEdgeFaded,
                    LineThickness = 2,
                    Location = new Point(20, 92),
                    Name = "bunifuSeparator1",
                    Orientation = Bunifu.UI.WinForms.BunifuSeparator.LineOrientation.Horizontal,
                    Size = new Size(700, 14),
                };

                var lblIconAvgWin = new BunifuLabel
                {
                    AllowParentOverrides = false,
                    AutoEllipsis = false,
                    AutoSize = false,
                    Anchor = AnchorStyles.Top,
                    BackgroundImage = global::WindowsFormsApp1.Properties.Resources.account_64px,
                    BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch,
                    Cursor = System.Windows.Forms.Cursors.Default,
                    CursorType = System.Windows.Forms.Cursors.Default,
                    Font = new Font("Segoe UI", 9F),
                    Location = new Point(61, 111),
                    Name = "lblIconAvgWin",
                    RightToLeft = System.Windows.Forms.RightToLeft.No,
                    Size = new Size(40, 40),
                    TextAlignment = System.Drawing.ContentAlignment.TopLeft,
                    TextFormat = Bunifu.UI.WinForms.BunifuLabel.TextFormattingOptions.Default,
                };

                var lblAvgWin = new BunifuLabel
                {
                    AllowParentOverrides = false,
                    AutoEllipsis = false,
                    Anchor = AnchorStyles.Top,
                    Cursor = System.Windows.Forms.Cursors.Default,
                    CursorType = System.Windows.Forms.Cursors.Default,
                    Font = new Font("Leelawadee UI", 15F),
                    ForeColor = System.Drawing.Color.FromArgb(155, 56, 239),
                    Location = new Point(110, 117),
                    Name = "lblAvgWin",
                    RightToLeft = System.Windows.Forms.RightToLeft.No,
                    Size = new Size(38, 28),
                    Text = $"{avgwinrate: #.}%",
                    TextAlignment = System.Drawing.ContentAlignment.TopLeft,
                    TextFormat = Bunifu.UI.WinForms.BunifuLabel.TextFormattingOptions.Default,
                };

                var kick = new DataGridViewImageColumn
                {
                    FillWeight = 40F,
                    HeaderText = "",
                    Name = "kick",
                    ReadOnly = true,
                };

                var position = new DataGridViewTextBoxColumn
                {
                    DefaultCellStyle = dataGridViewCellStyle5,
                    FillWeight = 70F,
                    HeaderText = "Position",
                    Name = "position",
                    ReadOnly = true,
                    SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable,
                };

                var online = new DataGridViewImageColumn
                {
                    FillWeight = 50F,
                    HeaderText = "Online",
                    Name = "online",
                    ReadOnly = true,
                };

                var trophies = new DataGridViewTextBoxColumn
                {
                    DefaultCellStyle = dataGridViewCellStyle4,
                    FillWeight = 75F,
                    HeaderText = "Trophies",
                    Name = "trophies",
                    ReadOnly = true,
                };

                var nick = new DataGridViewTextBoxColumn
                {
                    DefaultCellStyle = dataGridViewCellStyle3,
                    FillWeight = 80F,
                    HeaderText = "Nick",
                    Name = "nick",
                    ReadOnly = true,
                };

                var avatar = new DataGridViewImageColumn
                {
                    FillWeight = 40F,
                    HeaderText = "",
                    Name = "avatar",
                    ReadOnly = true,
                };

                var tblClanPlayers = new DataGridView
                {
                    AllowUserToAddRows = false,
                    AllowUserToDeleteRows = false,
                    AllowUserToOrderColumns = true,
                    AllowUserToResizeRows = false,
                    AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1,
                    Anchor = (AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right,
                    AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill,
                    BackgroundColor = System.Drawing.Color.FromArgb(34, 31, 46),
                    BorderStyle = System.Windows.Forms.BorderStyle.None,
                    CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None,
                    ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None,
                    ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2,
                    ColumnHeadersHeight = 25,
                    ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
                    DefaultCellStyle = dataGridViewCellStyle6,
                    EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically,
                    EnableHeadersVisualStyles = false,
                    Location = new Point(77, 227),
                    Name = "tblClanPlayers",
                    ReadOnly = true,
                    RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None,
                    RowHeadersDefaultCellStyle = dataGridViewCellStyle7,
                    RowHeadersVisible = false,
                    ScrollBars = System.Windows.Forms.ScrollBars.None,
                    SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect,
                    Size = new Size(586, 225),
                };
                tblClanPlayers.Columns.AddRange(new DataGridViewColumn[] {
                    avatar,
                    nick,
                    trophies,
                    online,
                    position,
                    kick
                });
                tblClanPlayers.RowTemplate.Height = 50;

                var listBox1 = new ListBox
                {
                    BackColor = System.Drawing.Color.FromArgb(24, 21, 26),
                    BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle,
                    DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed,
                    Font = new Font("Leelawadee UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0),
                    ForeColor = System.Drawing.Color.FromArgb(155, 56, 239),
                    FormattingEnabled = true,
                    ItemHeight = 20,
                    Location = new Point(732, 192),
                    Name = "listBox1",
                    Size = new Size(100, 2 + (20 * 4 - (byte)player.clanPosition)),
                    Visible = false,
                };
                listBox1.Items.AddRange(Enumerable.Range(0, 4).Where(x => player.clanPosition <= x)
                    .Select(x => ((ClanPosition)x).ToString()).ToArray());

                var bunifuSeparator3 = new BunifuSeparator
                {
                    AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange,
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                    BackColor = System.Drawing.Color.Transparent,
                    BackgroundImage = ((System.Drawing.Image)(Resources.ResourceManager.GetObject("bunifuSeparator3.BackgroundImage"))),
                    BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch,
                    DashCap = Bunifu.UI.WinForms.BunifuSeparator.CapStyles.Flat,
                    LineColor = System.Drawing.Color.FromArgb(140, 64, 237),
                    LineStyle = Bunifu.UI.WinForms.BunifuSeparator.LineStyles.DoubleEdgeFaded,
                    LineThickness = 2,
                    Location = new Point(20, 159),
                    Name = "bunifuSeparator3",
                    Orientation = Bunifu.UI.WinForms.BunifuSeparator.LineOrientation.Horizontal,
                    Size = new Size(700, 14),
                };

                var sprtClanPlayers = new BunifuSeparator
                {
                    Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                    BackColor = System.Drawing.Color.Transparent,
                    BackgroundImage = ((System.Drawing.Image)(Resources.ResourceManager.GetObject("sprtClanPlayers.BackgroundImage"))),
                    BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch,
                    DashCap = Bunifu.UI.WinForms.BunifuSeparator.CapStyles.Flat,
                    Font = new Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 204),
                    LineColor = System.Drawing.Color.Indigo,
                    LineStyle = Bunifu.UI.WinForms.BunifuSeparator.LineStyles.DoubleEdgeFaded,
                    LineThickness = 4,
                    Location = new Point(77, 455),
                    Margin = new System.Windows.Forms.Padding(4),
                    Name = "sprtClanPlayers",
                    Orientation = Bunifu.UI.WinForms.BunifuSeparator.LineOrientation.Horizontal,
                    Size = new Size(586, 18),
                    Visible = true,
                };

                var lbtnLeaveClan = new BunifuLabel
                {
                    AllowParentOverrides = false,
                    AutoEllipsis = false,
                    AutoSize = false,
                    Anchor = AnchorStyles.Top | AnchorStyles.Right,
                    BackgroundImage = global::WindowsFormsApp1.Properties.Resources.exit_sign_64px,
                    BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch,
                    Cursor = System.Windows.Forms.Cursors.Default,
                    CursorType = System.Windows.Forms.Cursors.Default,
                    Font = new Font("Segoe UI", 9F),
                    Location = new Point(693, 6),
                    Name = "lbtnLeaveClan",
                    RightToLeft = System.Windows.Forms.RightToLeft.No,
                    Size = new Size(40, 40),
                    TextAlignment = System.Drawing.ContentAlignment.TopLeft,
                    TextFormat = Bunifu.UI.WinForms.BunifuLabel.TextFormattingOptions.Default,
                };

                var scrollClanPlayers = new BunifuVScrollBar
                {
                    AllowCursorChanges = false,
                    AllowHomeEndKeysDetection = false,
                    AllowIncrementalClickMoves = true,
                    AllowMouseDownEffects = false,
                    AllowMouseHoverEffects = false,
                    AllowScrollingAnimations = true,
                    AllowScrollKeysDetection = false,
                    AllowScrollOptionsMenu = false,
                    AllowShrinkingOnFocusLost = false,
                    Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                         | System.Windows.Forms.AnchorStyles.Right),
                    BackgroundColor = System.Drawing.Color.FromArgb(40, 36, 50),
                    BackgroundImage = ((System.Drawing.Image)(Resources.ResourceManager.GetObject("scrollClanPlayers.BackgroundImage"))),
                    BindingContainer = tblClanPlayers,
                    BorderColor = System.Drawing.Color.FromArgb(40, 36, 50),
                    BorderRadius = 14,
                    BorderThickness = 1,
                    DurationBeforeShrink = 2000,
                    LargeChange = 40,
                    Location = new Point(667, 252),
                    Maximum = 100,
                    Minimum = 0,
                    MinimumThumbLength = 18,
                    Name = "scrollClanPlayers",
                    ScrollBarBorderColor = System.Drawing.Color.FromArgb(40, 36, 50),
                    ScrollBarColor = System.Drawing.Color.FromArgb(40, 36, 50),
                    ShrinkSizeLimit = 3,
                    Size = new Size(17, 201),
                    SmallChange = 1,
                    ThumbColor = Color.FromArgb(100, 26, 200),
                    ThumbLength = 78,
                    ThumbMargin = 1,
                    ThumbStyle = Bunifu.UI.WinForms.BunifuVScrollBar.ThumbStyles.Inset,
                    Value = 0,
                };
                tblClanPlayers.CellClick += (s, a) =>
                {
                    listBox1.Visible = false;
                    if (a.ColumnIndex == 4 && a.RowIndex > -1)
                    {
                        if (player.clanPosition < (byte)tblClanPlayers.CurrentCell.Value)
                        {
                            var x = tblClanPlayers.GetColumnDisplayRectangle(a.ColumnIndex, false).Location.X + 84;
                            var y = tblClanPlayers.GetRowDisplayRectangle(a.RowIndex, false).Location.Y + 230;
                            listBox1.Location = new Point(WindowState == FormWindowState.Maximized ? x + 100 : x, y);
                            listBox1.SelectedIndex = (byte)tblClanPlayers.CurrentCell.Value - (byte)player.clanPosition;
                            listBox1.BringToFront();
                            bunifuTransition1.Show(listBox1);
                        }
                    }
                    if (a.ColumnIndex == 5 && a.RowIndex > -1 && (byte)player.clanPosition
                        < (byte)tblClanPlayers.CurrentRow.Cells[4].Value)
                    {
                        snackBar.Show(this, duration: 3000, message: $"Do you really want to kick " +
                            $"{tblClanPlayers.CurrentRow.Cells[1].Value}", action: "Kick",
                            position: BunifuSnackbar.Positions.MiddleRight).Then(async (result) =>
                            {
                                if (result == BunifuSnackbar.SnackbarResult.ActionClicked)
                                {
                                    await Task.Run(() =>
                                    {
                                        using (var context = new GameContext())
                                        {
                                            var cln = context.Clans.First(x => x.Id == player.ClanId);
                                            var plrId = clanPlayers[tblClanPlayers.CurrentRow.Index].Id;
                                            var info = context.PlrsInfo.First(x => x.PlrId == plrId);
                                            var plr = context.Players.First(x => x.Id == plrId);
                                            plr.clanPosition = null;
                                            plr.ClanId = null;
                                            cln.NumbOfPlayers--;
                                            cln.TotalTrophies -= info.Trophies;
                                            context.SaveChanges();
                                        }
                                    });
                                    if (WindowState == FormWindowState.Maximized)
                                    {
                                        ResizeWindow();
                                        BuildPageClans();
                                        await Task.Delay(1);
                                        ResizeWindow();
                                    }
                                    else BuildPageClans();
                                }
                            });
                    }
                };
                tblClanPlayers.MouseLeave += (s, a) =>
                {
                    if (player.ClanId != null && !listBox1.ClientRectangle.Contains(
                        listBox1.PointToClient(Cursor.Position)))
                        bunifuTransition1.Hide(page3.Controls.OfType<ListBox>().First());
                };
                scrollClanPlayers.ValueChanged += (s, a) =>
                {
                    bunifuTransition1.Hide(listBox1);
                    tblClanPlayers.FirstDisplayedScrollingRowIndex = tblClanPlayers.Rows[
                        tblClanPlayers.Rows.Count == 1 ? 0 : a.Value].Index;
                    sprtClanPlayers.Visible = a.Value == tblClanPlayers.Rows.Count - 1;
                };
                listBox1.SelectedIndexChanged += async (s, a) =>
                {
                    if (listBox1.SelectedIndex != (byte)tblClanPlayers.CurrentCell.Value - (byte)player.clanPosition)
                    {
                        tblClanPlayers.CurrentCell.Value = (ClanPosition)(listBox1.SelectedIndex + (byte)player.clanPosition);

                        await Task.Run(() =>
                        {
                            using (var context = new GameContext())
                            {
                                var plrId = clanPlayers[tblClanPlayers.CurrentRow.Index].Id;
                                var plr = context.Players.First(x => x.Id == plrId);
                                if ((ClanPosition)tblClanPlayers.CurrentCell.Value == 0)
                                {
                                    context.Players.First(x => x.ClanId == plr.ClanId && x.clanPosition == 0).clanPosition = 1;
                                }
                                context.Players.First(x => x.Id == plrId).clanPosition = (byte)tblClanPlayers.CurrentCell.Value;
                                context.SaveChanges();
                                player = context.Players.First(x => x.Id == player.Id);
                            }
                        });
                        if ((ClanPosition)tblClanPlayers.CurrentCell.Value == 0)
                        {
                            if (WindowState == FormWindowState.Maximized)
                            {
                                ResizeWindow();
                                await Task.Delay(1);
                                BuildPageClans();
                                ResizeWindow();
                            }
                            else BuildPageClans();
                        }
                    }
                    bunifuTransition1.Hide(listBox1);
                };
                listBox1.DrawItem += (s, a) =>
                {
                    var b_color = a.BackColor;
                    var clr = Color.FromArgb(0, b_color);
                    var h_color = Color.FromArgb(155, 56, 239);
                    Brush bb = new LinearGradientBrush(a.Bounds, b_color, clr, 120);
                    if (a.Index >= 0)
                    {
                        SolidBrush sb = new SolidBrush(((a.State & DrawItemState.Selected) == DrawItemState.Selected)
                            ? h_color : b_color);
                        a.Graphics.FillRectangle(sb, a.Bounds);
                        a.Graphics.FillRectangle(bb, a.Bounds);
                        var txt = listBox1.Items[a.Index].ToString();
                        SolidBrush tb = new SolidBrush(a.ForeColor);
                        a.Graphics.DrawString(txt, a.Font, tb, listBox1.GetItemRectangle(a.Index).Location);
                    }
                };
                lbtnEdit.Click += (s, a) =>
                {
                    createClan.p = new Point(Location.X + 963 / 2 - 407 / 2, Location.Y + 50);
                    createClan.clanID = player.ClanId;
                    createClan.Show();
                };
                lbtnLeaveClan.Click += (s, a) =>
                {
                    bunifuSnackbar1.Show(this, "Do you really want to leave?", BunifuSnackbar.MessageTypes.Warning,
                3000, "Leave", BunifuSnackbar.Positions.TopRight).Then(async (result) =>
                 {
                     if (result == BunifuSnackbar.SnackbarResult.ActionClicked)
                     {
                         await Task.Run(() =>
                        {
                            using (var context = new GameContext())
                            {
                                var plr = context.Players.First(x => x.Id == player.Id);
                                info = context.PlrsInfo.First(x => x.PlrId == player.Id);
                                var cln = context.Clans.First(x => x.Id == plr.ClanId);
                                cln.NumbOfPlayers--;
                                cln.TotalTrophies -= info.Trophies;
                                if (cln.NumbOfPlayers == 0)
                                {
                                    context.Clans.Remove(cln);
                                }
                                plr.clanPosition = null;
                                plr.ClanId = null;
                                context.SaveChanges();
                            }
                        });
                         if (WindowState == FormWindowState.Maximized)
                         {
                             ResizeWindow();
                             BuildPageClans();
                             await Task.Delay(1);
                             ResizeWindow();
                         }
                         else BuildPageClans();
                         pages.SelectedTab = pages.TabPages[0];
                     }
                 });
                };
                lblClanName.SizeChanged += (s, a) =>
                {
                    lbtnEdit.Location = new Point(lblClanName.Location.X + lblClanName.Width, lblClanName.Location.Y - 10);
                };

                scrollBar = scrollClanPlayers;
                table = tblClanPlayers;

                page3.Controls.Add(lbtnEdit);
                page3.Controls.Add(lbtnLeaveClan);
                page3.Controls.Add(sprtClanPlayers);
                page3.Controls.Add(bunifuSeparator3);
                page3.Controls.Add(listBox1);
                page3.Controls.Add(scrollClanPlayers);
                page3.Controls.Add(lblAvgWin);
                page3.Controls.Add(lblIconAvgWin);
                page3.Controls.Add(bunifuSeparator1);
                page3.Controls.Add(lblTop);
                page3.Controls.Add(lblIconTop);
                page3.Controls.Add(lblPeople);
                page3.Controls.Add(lblIconPeople);
                page3.Controls.Add(lblTrophy);
                page3.Controls.Add(lblIconTrophy);
                page3.Controls.Add(lblLeader);
                page3.Controls.Add(lblLeadName);
                page3.Controls.Add(tblClanPlayers);
                page3.Controls.Add(lblClanName);
                page3.Controls.Add(PBClanIcon);
                PreparingToShowClan();
            }
        }
        private void ResizeWindow()
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
        }
    }
}
