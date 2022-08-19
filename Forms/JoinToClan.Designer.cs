

namespace WindowsFormsApp1
{
    partial class JoinToClan
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(JoinToClan));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges borderEdges1 = new Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges();
            this.bunifuPanel1 = new Bunifu.UI.WinForms.BunifuPanel();
            this.bunifuVScrollBar1 = new Bunifu.UI.WinForms.BunifuVScrollBar();
            this.playersTable = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lClanName = new Bunifu.UI.WinForms.BunifuLabel();
            this.pbIconClan = new WindowsFormsApp1.CustomPictureBox();
            this.btnJoin = new Bunifu.UI.WinForms.BunifuButton.BunifuButton();
            this.blClose = new System.Windows.Forms.Label();
            this.bunifuDragControl1 = new Bunifu.Framework.UI.BunifuDragControl(this.components);
            this.bunifuPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.playersTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbIconClan)).BeginInit();
            this.SuspendLayout();
            // 
            // bunifuPanel1
            // 
            this.bunifuPanel1.BackgroundColor = System.Drawing.Color.Transparent;
            this.bunifuPanel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("bunifuPanel1.BackgroundImage")));
            this.bunifuPanel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bunifuPanel1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(167)))), ((int)(((byte)(16)))), ((int)(((byte)(221)))));
            this.bunifuPanel1.BorderRadius = 2;
            this.bunifuPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.bunifuPanel1.BorderThickness = 2;
            this.bunifuPanel1.Controls.Add(this.bunifuVScrollBar1);
            this.bunifuPanel1.Controls.Add(this.playersTable);
            this.bunifuPanel1.Controls.Add(this.lClanName);
            this.bunifuPanel1.Controls.Add(this.pbIconClan);
            this.bunifuPanel1.Controls.Add(this.btnJoin);
            this.bunifuPanel1.Controls.Add(this.blClose);
            this.bunifuPanel1.Location = new System.Drawing.Point(-3, -3);
            this.bunifuPanel1.Name = "bunifuPanel1";
            this.bunifuPanel1.ShowBorders = true;
            this.bunifuPanel1.Size = new System.Drawing.Size(413, 414);
            this.bunifuPanel1.TabIndex = 54;
            // 
            // bunifuVScrollBar1
            // 
            this.bunifuVScrollBar1.AllowCursorChanges = true;
            this.bunifuVScrollBar1.AllowHomeEndKeysDetection = false;
            this.bunifuVScrollBar1.AllowIncrementalClickMoves = true;
            this.bunifuVScrollBar1.AllowMouseDownEffects = true;
            this.bunifuVScrollBar1.AllowMouseHoverEffects = true;
            this.bunifuVScrollBar1.AllowScrollingAnimations = true;
            this.bunifuVScrollBar1.AllowScrollKeysDetection = true;
            this.bunifuVScrollBar1.AllowScrollOptionsMenu = true;
            this.bunifuVScrollBar1.AllowShrinkingOnFocusLost = false;
            this.bunifuVScrollBar1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(36)))), ((int)(((byte)(50)))));
            this.bunifuVScrollBar1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("bunifuVScrollBar1.BackgroundImage")));
            this.bunifuVScrollBar1.BindingContainer = this.playersTable;
            this.bunifuVScrollBar1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(36)))), ((int)(((byte)(50)))));
            this.bunifuVScrollBar1.BorderRadius = 14;
            this.bunifuVScrollBar1.BorderThickness = 1;
            this.bunifuVScrollBar1.DurationBeforeShrink = 2000;
            this.bunifuVScrollBar1.LargeChange = 10;
            this.bunifuVScrollBar1.Location = new System.Drawing.Point(344, 166);
            this.bunifuVScrollBar1.Maximum = 100;
            this.bunifuVScrollBar1.Minimum = 0;
            this.bunifuVScrollBar1.MinimumThumbLength = 18;
            this.bunifuVScrollBar1.Name = "bunifuVScrollBar1";
            this.bunifuVScrollBar1.OnDisable.ScrollBarBorderColor = System.Drawing.Color.Silver;
            this.bunifuVScrollBar1.OnDisable.ScrollBarColor = System.Drawing.Color.Transparent;
            this.bunifuVScrollBar1.OnDisable.ThumbColor = System.Drawing.Color.Silver;
            this.bunifuVScrollBar1.ScrollBarBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(36)))), ((int)(((byte)(50)))));
            this.bunifuVScrollBar1.ScrollBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(36)))), ((int)(((byte)(50)))));
            this.bunifuVScrollBar1.ShrinkSizeLimit = 3;
            this.bunifuVScrollBar1.Size = new System.Drawing.Size(15, 162);
            this.bunifuVScrollBar1.SmallChange = 1;
            this.bunifuVScrollBar1.TabIndex = 69;
            this.bunifuVScrollBar1.ThumbColor = System.Drawing.Color.Gray;
            this.bunifuVScrollBar1.ThumbLength = 18;
            this.bunifuVScrollBar1.ThumbMargin = 1;
            this.bunifuVScrollBar1.ThumbStyle = Bunifu.UI.WinForms.BunifuVScrollBar.ThumbStyles.Inset;
            this.bunifuVScrollBar1.Value = 0;
            // 
            // playersTable
            // 
            this.playersTable.AllowUserToAddRows = false;
            this.playersTable.AllowUserToDeleteRows = false;
            this.playersTable.AllowUserToOrderColumns = true;
            this.playersTable.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(21)))), ((int)(((byte)(35)))));
            this.playersTable.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.playersTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.playersTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.playersTable.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(31)))), ((int)(((byte)(46)))));
            this.playersTable.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.playersTable.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.playersTable.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(157)))), ((int)(((byte)(16)))), ((int)(((byte)(221)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Leelawadee UI", 10F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(157)))), ((int)(((byte)(16)))), ((int)(((byte)(221)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.playersTable.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.playersTable.ColumnHeadersHeight = 25;
            this.playersTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.playersTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column3,
            this.Column5,
            this.Column7});
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(31)))), ((int)(((byte)(46)))));
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Leelawadee UI", 10.75F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(43)))), ((int)(((byte)(63)))));
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.playersTable.DefaultCellStyle = dataGridViewCellStyle6;
            this.playersTable.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.playersTable.EnableHeadersVisualStyles = false;
            this.playersTable.Location = new System.Drawing.Point(63, 138);
            this.playersTable.Name = "playersTable";
            this.playersTable.ReadOnly = true;
            this.playersTable.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.playersTable.RowHeadersVisible = false;
            this.playersTable.RowTemplate.Height = 50;
            this.playersTable.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.playersTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.playersTable.Size = new System.Drawing.Size(275, 190);
            this.playersTable.TabIndex = 68;
            // 
            // Column1
            // 
            this.Column1.FillWeight = 40F;
            this.Column1.HeaderText = "№";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column3
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column3.DefaultCellStyle = dataGridViewCellStyle3;
            this.Column3.HeaderText = "Nick";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            // 
            // Column5
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Leelawadee UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.Column5.DefaultCellStyle = dataGridViewCellStyle4;
            this.Column5.FillWeight = 75F;
            this.Column5.HeaderText = "Trophies";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            // 
            // Column7
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column7.DefaultCellStyle = dataGridViewCellStyle5;
            this.Column7.HeaderText = "Position";
            this.Column7.Name = "Column7";
            this.Column7.ReadOnly = true;
            this.Column7.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // lClanName
            // 
            this.lClanName.AllowParentOverrides = false;
            this.lClanName.AutoEllipsis = false;
            this.lClanName.AutoSize = false;
            this.lClanName.Cursor = System.Windows.Forms.Cursors.Default;
            this.lClanName.CursorType = System.Windows.Forms.Cursors.Default;
            this.lClanName.Font = new System.Drawing.Font("Leelawadee UI", 14F);
            this.lClanName.ForeColor = System.Drawing.Color.MediumOrchid;
            this.lClanName.Location = new System.Drawing.Point(92, 105);
            this.lClanName.Name = "lClanName";
            this.lClanName.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lClanName.Size = new System.Drawing.Size(223, 24);
            this.lClanName.TabIndex = 67;
            this.lClanName.Text = "SandStorm";
            this.lClanName.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.lClanName.TextFormat = Bunifu.UI.WinForms.BunifuLabel.TextFormattingOptions.Default;
            // 
            // pbIconClan
            // 
            this.pbIconClan.BorderCapStyle = System.Drawing.Drawing2D.DashCap.Flat;
            this.pbIconClan.BorderColor = System.Drawing.Color.RoyalBlue;
            this.pbIconClan.BorderColor2 = System.Drawing.Color.Magenta;
            this.pbIconClan.BorderLineStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            this.pbIconClan.BorderSize = 0;
            this.pbIconClan.GradientAngle = 50F;
            this.pbIconClan.Image = global::WindowsFormsApp1.Properties.Resources.clash_of_clans_96px;
            this.pbIconClan.Location = new System.Drawing.Point(163, 22);
            this.pbIconClan.Name = "pbIconClan";
            this.pbIconClan.Size = new System.Drawing.Size(80, 80);
            this.pbIconClan.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbIconClan.TabIndex = 66;
            this.pbIconClan.TabStop = false;
            // 
            // btnJoin
            // 
            this.btnJoin.AllowAnimations = true;
            this.btnJoin.AllowMouseEffects = true;
            this.btnJoin.AllowToggling = true;
            this.btnJoin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnJoin.AnimationSpeed = 200;
            this.btnJoin.AutoGenerateColors = false;
            this.btnJoin.AutoRoundBorders = false;
            this.btnJoin.AutoSizeLeftIcon = true;
            this.btnJoin.AutoSizeRightIcon = true;
            this.btnJoin.BackColor = System.Drawing.Color.Transparent;
            this.btnJoin.BackColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(122)))), ((int)(((byte)(183)))));
            this.btnJoin.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnJoin.BackgroundImage")));
            this.btnJoin.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnJoin.ButtonText = "Join";
            this.btnJoin.ButtonTextMarginLeft = 0;
            this.btnJoin.ColorContrastOnClick = 45;
            this.btnJoin.ColorContrastOnHover = 45;
            this.btnJoin.Cursor = System.Windows.Forms.Cursors.Default;
            borderEdges1.BottomLeft = true;
            borderEdges1.BottomRight = true;
            borderEdges1.TopLeft = true;
            borderEdges1.TopRight = true;
            this.btnJoin.CustomizableEdges = borderEdges1;
            this.btnJoin.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnJoin.DisabledBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.btnJoin.DisabledFillColor = System.Drawing.Color.Empty;
            this.btnJoin.DisabledForecolor = System.Drawing.Color.Empty;
            this.btnJoin.FocusState = Bunifu.UI.WinForms.BunifuButton.BunifuButton.ButtonStates.Pressed;
            this.btnJoin.Font = new System.Drawing.Font("Leelawadee UI", 11F);
            this.btnJoin.ForeColor = System.Drawing.Color.White;
            this.btnJoin.IconLeft = null;
            this.btnJoin.IconLeftAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnJoin.IconLeftCursor = System.Windows.Forms.Cursors.Default;
            this.btnJoin.IconLeftPadding = new System.Windows.Forms.Padding(11, 3, 3, 3);
            this.btnJoin.IconMarginLeft = 11;
            this.btnJoin.IconPadding = 10;
            this.btnJoin.IconRight = null;
            this.btnJoin.IconRightAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnJoin.IconRightCursor = System.Windows.Forms.Cursors.Default;
            this.btnJoin.IconRightPadding = new System.Windows.Forms.Padding(3, 3, 7, 3);
            this.btnJoin.IconSize = 25;
            this.btnJoin.IdleBorderColor = System.Drawing.Color.Empty;
            this.btnJoin.IdleBorderRadius = 0;
            this.btnJoin.IdleBorderThickness = 0;
            this.btnJoin.IdleFillColor = System.Drawing.Color.Empty;
            this.btnJoin.IdleIconLeftImage = null;
            this.btnJoin.IdleIconRightImage = null;
            this.btnJoin.IndicateFocus = true;
            this.btnJoin.Location = new System.Drawing.Point(127, 334);
            this.btnJoin.Name = "btnJoin";
            this.btnJoin.OnDisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.btnJoin.OnDisabledState.BorderRadius = 34;
            this.btnJoin.OnDisabledState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnJoin.OnDisabledState.BorderThickness = 2;
            this.btnJoin.OnDisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.btnJoin.OnDisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(160)))), ((int)(((byte)(168)))));
            this.btnJoin.OnDisabledState.IconLeftImage = null;
            this.btnJoin.OnDisabledState.IconRightImage = null;
            this.btnJoin.onHoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this.btnJoin.onHoverState.BorderRadius = 34;
            this.btnJoin.onHoverState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnJoin.onHoverState.BorderThickness = 2;
            this.btnJoin.onHoverState.FillColor = System.Drawing.Color.Transparent;
            this.btnJoin.onHoverState.ForeColor = System.Drawing.Color.White;
            this.btnJoin.onHoverState.IconLeftImage = null;
            this.btnJoin.onHoverState.IconRightImage = null;
            this.btnJoin.OnIdleState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(167)))), ((int)(((byte)(16)))), ((int)(((byte)(221)))));
            this.btnJoin.OnIdleState.BorderRadius = 34;
            this.btnJoin.OnIdleState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnJoin.OnIdleState.BorderThickness = 2;
            this.btnJoin.OnIdleState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(31)))), ((int)(((byte)(46)))));
            this.btnJoin.OnIdleState.ForeColor = System.Drawing.Color.White;
            this.btnJoin.OnIdleState.IconLeftImage = null;
            this.btnJoin.OnIdleState.IconRightImage = null;
            this.btnJoin.OnPressedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(167)))), ((int)(((byte)(16)))), ((int)(((byte)(221)))));
            this.btnJoin.OnPressedState.BorderRadius = 34;
            this.btnJoin.OnPressedState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnJoin.OnPressedState.BorderThickness = 2;
            this.btnJoin.OnPressedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(31)))), ((int)(((byte)(46)))));
            this.btnJoin.OnPressedState.ForeColor = System.Drawing.Color.White;
            this.btnJoin.OnPressedState.IconLeftImage = null;
            this.btnJoin.OnPressedState.IconRightImage = null;
            this.btnJoin.Size = new System.Drawing.Size(153, 44);
            this.btnJoin.TabIndex = 65;
            this.btnJoin.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnJoin.TextAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.btnJoin.TextMarginLeft = 0;
            this.btnJoin.TextPadding = new System.Windows.Forms.Padding(0);
            this.btnJoin.UseDefaultRadiusAndThickness = true;
            // 
            // blClose
            // 
            this.blClose.Image = global::WindowsFormsApp1.Properties.Resources.icons8_delete_25px;
            this.blClose.Location = new System.Drawing.Point(375, 6);
            this.blClose.Name = "blClose";
            this.blClose.Size = new System.Drawing.Size(30, 20);
            this.blClose.TabIndex = 0;
            // 
            // bunifuDragControl1
            // 
            this.bunifuDragControl1.Fixed = true;
            this.bunifuDragControl1.Horizontal = true;
            this.bunifuDragControl1.TargetControl = this.bunifuPanel1;
            this.bunifuDragControl1.Vertical = true;
            // 
            // JoinToClan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(31)))), ((int)(((byte)(46)))));
            this.ClientSize = new System.Drawing.Size(407, 409);
            this.Controls.Add(this.bunifuPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "JoinToClan";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "JoinToClan";
            this.Load += new System.EventHandler(this.JoinToClan_Load);
            this.bunifuPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.playersTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbIconClan)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Bunifu.UI.WinForms.BunifuPanel bunifuPanel1;
        private CustomPictureBox pbIconClan;
        private Bunifu.UI.WinForms.BunifuButton.BunifuButton btnJoin;
        private System.Windows.Forms.Label blClose;
        private Bunifu.UI.WinForms.BunifuLabel lClanName;
        private Bunifu.Framework.UI.BunifuDragControl bunifuDragControl1;
        private Bunifu.UI.WinForms.BunifuVScrollBar bunifuVScrollBar1;
        private System.Windows.Forms.DataGridView playersTable;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
    }
}