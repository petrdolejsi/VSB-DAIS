using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AuctionDesktopApp.Templates
{
    /// <summary>
    /// Derive this class to create a form with a read-only overview of records.
    /// </summary>
    public partial class FormGrid : Form
    {
        public FormGrid()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Override this method to implement a custom logic for refreshing grid data.
        /// </summary>
        protected virtual void RefreshData()
        {

        }

        /// <summary>
        /// Override this method to implement a custom logic of creating new records.
        /// </summary>
        protected virtual void NewRecord()
        {

        }

        /// <summary>
        /// Override this method to implement a custom logic of editing existing records.
        /// </summary>
        protected virtual void EditRecord()
        {

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            NewRecord();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            EditRecord();
        }

        private void FormGrid_Shown(object sender, EventArgs e)
        {
            RefreshData();
        }
    }
}
