using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace usb_app_manager
{
    public partial class NewProgram : Form
    {
        private string programPath = "";
        private string programName = "";

        public NewProgram()
        {
            InitializeComponent();
        }

        private void NewProgram_Load(object sender, EventArgs e)
        {

        }

        public string getProgramPath()
        {
            return programPath;
        }

        public string getProgramName()
        {
            return programName;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (programInput.Text != "" && nameInput.Text != "")
            {
                programPath = programInput.Text;
                programName = nameInput.Text;
                DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("Please provide input for all fields.", "Error", MessageBoxButtons.OK);
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string fileName;
                fileName = dlg.FileName;
                programInput.Text = fileName;
            }
        }
    }
}
