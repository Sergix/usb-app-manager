using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace usb_app_manager
{
    public partial class ProgramManager : Form
    {
        private ImageList imageList = new ImageList();
        private string tempLabelStore = "";

        public FileStream fileStream = null;
        public string dataFilePath = "pmdata.dat";
        public Dictionary<string, string> programData = new Dictionary<string, string>();

        public ProgramManager()
        {
            InitializeComponent();

            AppDomain.CurrentDomain.ProcessExit += OnApplicationExit;
            Application.ApplicationExit += OnApplicationExit;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            programList.LargeImageList = imageList;
            
            if (!System.IO.File.Exists(dataFilePath))
            {
                fileStream = System.IO.File.Create(dataFilePath);
                fileStream.Close();
                DataFile.LoadLocalPrograms(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));
            }
            else
            {
                DataFile.LoadStoredPrograms();
            }

        }

        void OnApplicationExit(object sender, EventArgs eventArgs)
        {
            
        }

        public int GetIndex(Dictionary<string, string> dictionary, string key)
        {
            for (int index = 0; index < dictionary.Count; index++)
            {
                if (dictionary.ElementAt(index).Key == dictionary[key])
                    return index;
            }

            return -1;
        }

        public void AddNewItem(string name, string path)
        {
            string[] arr = new string[2];
            ListViewItem itm;

            arr[0] = name;
            arr[1] = path;

            Icon programIcon = Icon.ExtractAssociatedIcon(arr[1]);
            imageList.Images.Add(arr[0], programIcon);

            itm = new ListViewItem(arr);
            itm.ImageKey = arr[0];

            programList.Items.Add(itm);
            DataFile.CacheProgram(name, path);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewProgram add = new NewProgram();
            add.ShowDialog(this);

            if (add.DialogResult == DialogResult.OK)
            {
                AddNewItem(add.getProgramName(), add.getProgramPath());
            }
        }

        private void programList_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (programList.FocusedItem.Bounds.Contains(e.Location))
                {
                    itemMenuStrip.Show(Cursor.Position);
                }
            }
        }

        private void programList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (programList.FocusedItem.Bounds.Contains(e.Location))
                {
                    string item = programList.FocusedItem.Text;

                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.UseShellExecute = true;
                    startInfo.FileName = programData[item];
                    startInfo.Arguments = "";

                    /// TODO : ADD ARGUMENT OPTIONS
                    /// 

                    Process.Start(startInfo);
                }
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tempLabelStore = programList.FocusedItem.Text;
            programList.FocusedItem.BeginEdit();
        }

        private void programList_AfterLabelEdit(object sender, System.Windows.Forms.LabelEditEventArgs e)
        {
            if (e.Label == null)
                return;

            DataFile.EditProgram(tempLabelStore, e.Label);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataFile.DeleteProgram(programList.FocusedItem.Text);
            programList.FocusedItem.Remove();
        }
    }
}
