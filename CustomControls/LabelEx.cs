using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WindowsFormsApp1
{

    public partial class LabelEx : Label
    {
        public enum GradientMode
        {
            vertical = 0,
            horizontal = 1,
        }

        private Color color = Color.RoyalBlue;
        private Color color2 = Color.HotPink;
        private GradientMode gradientmode = GradientMode.vertical;
        public LabelEx()
        {
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            Font font = new Font(Font.Name, Font.Size, Font.Style);
            LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, Width, Height), Color, Color2,
                Gradientmode == 0 ? LinearGradientMode.Vertical : LinearGradientMode.Horizontal);
            e.Graphics.DrawString(Text, font, brush, 0, 0);

        }

        [Category("Style")]
        [DefaultValue(true)]
        public GradientMode Gradientmode
        {
            get { return gradientmode; }
            set
            {
                gradientmode = value;
                this.Invalidate();
            }
        }

        [Category("Style")]
        public Color Color
        {
            get { return color; }
            set
            {
                color = value;
                this.Invalidate();
            }
        }
        [Category("Style")]
        public Color Color2
        {
            get { return color2; }
            set
            {
                color2 = value;
                this.Invalidate();
            }
        }
        private void InitializeComponent()
        {
            ((ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            ((ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
