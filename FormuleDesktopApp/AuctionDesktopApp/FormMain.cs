using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AuctionDesktopApp
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void myAuctionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Forms.FormAuctionGrid form = new Forms.FormAuctionGrid();
            form.MdiParent = this;
            form.Show();
        }

        private void userToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Forms.FormUserGrid form = new Forms.FormUserGrid();
            form.MdiParent = this;
            form.Show();
        }

        private void categoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Forms.FormCategoryGrid form = new Forms.FormCategoryGrid();
            form.MdiParent = this;
            form.Show();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormAbout form = new FormAbout();
            form.ShowDialog();
        }
    }
}
