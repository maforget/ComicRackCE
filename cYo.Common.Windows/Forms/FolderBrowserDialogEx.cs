using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using cYo.Common.Localize;

namespace cYo.Common.Windows.Forms
{
    public partial class FolderBrowserDialogEx : FormEx
    {
        private string checkboxText = string.Empty;

        public string SelectedPath { get; private set; }
        public string InitialDirectory { get; set; }

        public string Title
        {
            get => this.Text;
            set => this.Text = value;
        }
        public bool ShowNewFolderButton
        {
            get => btNewFolder.Visible;
            set => btNewFolder.Visible = value;
        }
        public string Description
        {
            get => lblDescription.Text;
            set => lblDescription.Text = value;
        }
        public bool CheckboxChecked
        {
            get => chkOption1.Checked;
            set => chkOption1.Checked = value;
        }

        /// <summary>
        /// <para>If set to a non-empty value, displays a checkbox with the specified text.</para>
        /// <para>If set to empty or null, hides the checkbox and adjusts the layout accordingly.</para>
        /// </summary>
        public string CheckboxText
        {
            get => checkboxText;
            set
            {
                checkboxText = value;
                chkOption1.Text = checkboxText;
                HideCheckbox();
            }
        }

        /// <summary>
        /// Hides the checkbox and adjusts the layout if <see cref="CheckboxText"/> is empty or null.
        /// </summary>
        private void HideCheckbox()
        {
            if (string.IsNullOrEmpty(CheckboxText))
            {
                chkOption1.Visible = false;
                Rectangle rect = panel1.Bounds;
                rect.Inflate(0, chkOption1.Bottom - panel1.Bottom);
                panel1.SetBounds(rect.X, rect.Y, rect.Width, rect.Height);
            }
        }

        public FolderBrowserDialogEx()
        {
            InitializeComponent();
            string selectFolderString = TR.Load(base.Name)["SelectFolder", "Select Folder"];
            this.Text = selectFolderString;
            this.Description = selectFolderString;
            LocalizeUtility.Localize(this, components);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            folderTreeView.Init(); // must be called after the constructor to ensure the handle is created
            HideCheckbox(); // hides the checkbox on load if CheckboxText is initally empty or null
            DrillTolInitialDirectory();
        }

        private void DrillTolInitialDirectory()
        {
            if (!string.IsNullOrEmpty(InitialDirectory) && Directory.Exists(InitialDirectory))
                folderTreeView.DrillToFolder(InitialDirectory);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            SelectedPath = folderTreeView.SelectedNode != null ? folderTreeView.GetSelectedNodePath() : string.Empty;
            CheckboxChecked = chkOption1.Checked;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SelectedPath = string.Empty;
            CheckboxChecked = chkOption1.Checked;
        }

        private void btNewFolder_Click(object sender, EventArgs e)
        {
            if (folderTreeView.SelectedNode != null)
            {
                string path = folderTreeView.GetSelectedNodePath();
                if (!string.IsNullOrEmpty(path) && Directory.Exists(path))
                {
                    string newFolderPath = CreateNewFolder(path);
                    folderTreeView.Init();
                    folderTreeView.DrillToFolder(newFolderPath);
                }
            }
        }

        private string CreateNewFolder(string path)
        {
            try
            {
                string subfolderName = TR.Load(base.Name)["NewFolder", "New Folder"];
                string newFolderPath = Path.Combine(path, subfolderName);
                if (Directory.Exists(newFolderPath))
                {
                    int i = 1;
                    do
                    {
                        newFolderPath = Path.Combine(path, $"{subfolderName} ({i})");
                        i++;
                    } while (Directory.Exists(newFolderPath));
                }
                Directory.CreateDirectory(newFolderPath);
                return newFolderPath;
            }
            catch (Exception)
            {
                return path;
            }
        }
    }
}

