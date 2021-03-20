using GameNote.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameNote.WinConfirm
{
    public partial class CheckDialog : Form
    {
        private readonly string _exeFileName;
        private readonly CLIHandler cli;

        public bool SelectedYes { get; set; } = false;

        public CheckDialog(string question)
        {
            /*_exeFileName = fileName;
            cli = new CLIHandler(cliPath);

            if (cli.IsValidPath() == false)
                throw new Exception("Path to CLI not valid");*/
            lblQuestion.Text = question;
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            this.SelectedYes = false;
            this.Close();
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            //cli.GameRun(_exeFileName, force: true);
            this.SelectedYes = true;
            this.Close();
        }

        private void CheckDialog_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnYes_Click(sender, e);

            if (e.KeyCode == Keys.Escape)
                btnNo_Click(sender, e);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            /*lblQuestion.Text = string.Format(
                Properties.Resources.Question,
                _exeFileName
            );*/
        }
    }
}