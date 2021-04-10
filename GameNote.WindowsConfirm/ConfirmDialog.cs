using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameNote.WindowsConfirm
{
    public partial class ConfirmDialog : Form
    {
        public bool SelectedYes { get; set; } = false;

        public ConfirmDialog()
        {
            InitializeComponent();
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            this.SelectedYes = false;
            this.Close();
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            this.SelectedYes = true;
            this.Close();
        }

        private void btnYes_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnYes_Click(sender, e);

            if (e.KeyCode == Keys.Escape)
                btnNo_Click(sender, e);
        }

        private void ConfirmDialog_Load(object sender, EventArgs e)
        {
            StartPosition = FormStartPosition.CenterScreen;
        }
    }
}