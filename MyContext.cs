using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.DataModel;

namespace WindowsFormsApp1
{
    public class MyContext : ApplicationContext
    {
        private List<Form> forms;
        private static MyContext context;
        private static Player player;
        public MyContext()
        {
            context = this;
            forms = new List<Form>();
            Form form1 = new SignInForm();
            context.AddForm(form1);
            form1.Show();
        }

        public static void ShowForm2(Player _player, Point p)
        {
            player = _player;
            using (var context = new GameContext())
            {
                context.PlrsInfo.Find(player.Id).Online = true;
                context.SaveChanges();
            }
            Form form2 = new Form1(player, p);
            context.AddForm(form2);
            //Task.Delay(1000);
            form2.Show();
        }

        private void AddForm(Form f)
        {
            f.Closed += FormClosed;
            forms.Add(f);
        }

        private void FormClosed(object sender, EventArgs e)
        {
            Form f = sender as Form;
            if (f is Form1)
            {
                using (var context = new GameContext())
                {
                    context.PlrsInfo.Find(player.Id).Online = false;
                    context.SaveChanges();
                }
            }
            if (forms != null)
                forms.Remove(f);
            if (forms.Count == 0)
                Application.Exit();
        }
    }
}
