using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ChuckHill2;
using ChuckHill2.Forms;

namespace DotNetResourceExtractor
{
    public partial class FormMain : Form
    {
        bool AppClosing = false; //flag to notify m_txtXxxxx_Leave() that we no longer care about textbox validation.

        public FormMain()
        {
            InitializeComponent();

            MiniMessageBox.Colors.CaptionFont = this.Font;
            MiniMessageBox.Colors.MessageFont = this.Font;
            m_btnExtract.Enabled = false;
            m_chkSeparateFolders.Checked = true;
            this.FormClosing += (s, e) => AppClosing = true;

#if DEBUG
            m_txtAssembly.Text = Path.GetFullPath(@"..\..\..\TestSubject\bin\Debug\TestSubject.dll");
            m_txtDestination.Text = Path.GetFullPath("Extracted");
            m_btnExtract.Enabled = true;
#endif
        }

        private void m_txtAssembly_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;
            var file = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];

            file = ValidateFileName(file);
            if (file == null) return;
            e.Effect = DragDropEffects.Link;
        }

        private void m_txtAssembly_DragDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;
            var file = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];

            file = ValidateFileName(file);
            if (file == null) return;
            m_txtAssembly.Text = file;
            if (!string.IsNullOrWhiteSpace(m_txtDestination.Text)) m_btnExtract.Enabled = true;
        }

        private void m_txtAssembly_Leave(object sender, EventArgs e)
        {
            if (AppClosing) return;
            if (m_btnExit.Focused) return;
            if (m_btnSelectAssembly.Focused) return;

            var file = m_txtAssembly.Text;
            if (string.IsNullOrWhiteSpace(file))
            {
                m_btnExtract.Enabled = false;
                m_txtAssembly.Text = string.Empty;
                return;
            }

            file = ValidateFileName(file);
            if (file == null)
            {
                m_btnExtract.Enabled = false;
                MiniMessageBox.ShowDialog(this, "Not a valid/existing Assembly filename or name with wildcards ( ?, * ).", "Invalid Assembly Filename", MessageBoxButtons.OK, MessageBoxIcon.Error);
                m_txtAssembly.Focus();
                return;
            }

            m_txtAssembly.Text = file;
            if (!string.IsNullOrWhiteSpace(m_txtDestination.Text)) m_btnExtract.Enabled = true;
        }

        private void m_txtAssembly_DoubleClick(object sender, EventArgs e)
        {
            m_txtAssembly.Select(m_txtAssembly.SelectionStart, 0);
            if (!FileEx.GetPathParts(m_txtAssembly.Text, out var dir, out var name, out var ext)) return;
            Process.Start(dir);
        }

        private void m_btnSelectAssembly_Click(object sender, EventArgs e)
        {
            string filename;
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "All .NET Assembly Formats(*.exe,*.dll)|*.exe;*.dll|All Files(*.*)|*.*";
                ofd.AddExtension = false;
                ofd.CheckFileExists = true;
                ofd.DereferenceLinks = true;
                ofd.Multiselect = false;
                //ofd.InitialDirectory =  "";
                ofd.RestoreDirectory = true;
                ofd.Multiselect = false;
                ofd.Title = "Select Assembly File";
                ofd.ValidateNames = false;
                ofd.AutoUpgradeEnabled = false;
                ofd.FileName = m_txtAssembly.Text;

                if (ofd.ShowDialog(this) != DialogResult.OK) return;
                filename = ofd.FileName;
            }

            filename = ValidateFileName(filename);
            if (filename == null) return;
            m_txtAssembly.Text = filename;
            if (!string.IsNullOrWhiteSpace(m_txtDestination.Text)) m_btnExtract.Enabled = true;
        }

        private void m_txtDestination_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;
            var file = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];

            e.Effect = DragDropEffects.Link;
        }

        private void m_txtDestination_DragDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;
            var file = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];

            if (!Directory.Exists(file)) file = Path.GetDirectoryName(file);
            m_txtDestination.Text = file;
            if (!string.IsNullOrWhiteSpace(m_txtAssembly.Text)) m_btnExtract.Enabled = true;
        }

        private void m_txtDestination_Leave(object sender, EventArgs e)
        {
            if (AppClosing) return;
            if (m_btnExit.Focused) return;
            if (m_btnSelectDestination.Focused) return;

            var file = m_txtDestination.Text;
            if (string.IsNullOrWhiteSpace(file))
            {
                m_btnExtract.Enabled = false;
                m_txtDestination.Text = string.Empty;
                return;
            }

            file = FileEx.GetFullPath(file);
            if (file == null)
            {
                m_btnExtract.Enabled = false;
                MiniMessageBox.ShowDialog(this, "Not a valid destination folder. It does not need to exist.", "Invalid Folder", MessageBoxButtons.OK, MessageBoxIcon.Error);
                m_txtDestination.Focus();
                return;
            }

            m_txtDestination.Text = file;
            if (!string.IsNullOrWhiteSpace(m_txtAssembly.Text)) m_btnExtract.Enabled = true;
        }

        private void m_txtDestination_DoubleClick(object sender, EventArgs e)
        {
            m_txtDestination.Select(m_txtDestination.SelectionStart, 0);
            var dir = FileEx.GetFullPath(m_txtDestination.Text);
            if (dir == null) return;
            if (!FileEx.DirectoryExists(dir)) return;
            Process.Start(dir);
        }

        private void m_btnSelectDestination_Click(object sender, EventArgs e)
        {
            var dir = FolderSelectDialog.Show(this, "Select Extracted Results Folder", m_txtDestination.Text);
            if (dir == null) return;
            m_txtDestination.Text = dir;
            if (!string.IsNullOrWhiteSpace(m_txtAssembly.Text)) m_btnExtract.Enabled = true;
        }

        private void m_btnAbout_Click(object sender, EventArgs e)
        {
            AboutBox.Show(this);
        }

        private void m_btnExtract_Click(object sender, EventArgs e)
        {
            var file = ValidateFileName(m_txtAssembly.Text);
            if (file == null)
            {
                m_btnExtract.Enabled = false;
                MiniMessageBox.ShowDialog(this, "Source filename is no longer valid.", "Error", MiniMessageBox.Buttons.OK, MiniMessageBox.Symbol.Error);
                m_txtAssembly.Focus();
                return;
            }
            m_txtAssembly.Text = file;

            var dir = FileEx.GetFullPath(m_txtDestination.Text);
            if (dir==null)
            {
                m_btnExtract.Enabled = false;
                MiniMessageBox.ShowDialog(this, "Destination folder is no longer valid.", "Error", MiniMessageBox.Buttons.OK, MiniMessageBox.Symbol.Error);
                m_txtDestination.Focus();
                return;
            }
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            m_txtDestination.Text = dir;

            m_btnExtract.Enabled = false;
            CancellationTokenSource cancelSource = new CancellationTokenSource();
            CancellationToken cancelToken = cancelSource.Token;
            MiniMessageBox.Show(this, $"Extracting resources....", "Please Wait", MiniMessageBox.Buttons.Abort, MiniMessageBox.Symbol.Wait);
            MiniMessageBox.Clicked += (dlgResult) =>
            {
                MiniMessageBox.Hide();
                MiniMessageBox.Show(this, $"Aborting. Allowing outstanding extractions to complete. To terminate immediately, click the Exit button.", "Please Wait", MiniMessageBox.Buttons.None, MiniMessageBox.Symbol.Wait);
                cancelSource.Cancel();
            };

            var task = Task.Run(() => { using (var ex = new Extractor()) { return ex.DoWork(m_txtAssembly.Text, m_txtDestination.Text, m_chkSeparateFolders.Checked, cancelToken); } });
            task.ContinueWith((t) =>
            {
                this.Invoke((Action)(() =>
                {
                    MiniMessageBox.Hide();
                    MiniMessageBox.ShowDialog(this, $"Extracted resources from {t.Result} assemblies.", "Extraction Status", MiniMessageBox.Buttons.OK, MiniMessageBox.Symbol.Information);
                    m_btnExtract.Enabled = true;
                }));
            });
        }

        private void m_btnExit_Click(object sender, EventArgs e)
        {
            AppClosing = true;
            this.Close();
        }

        private static string ValidateFileName(string file)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(file)) return null;

                bool multiList = file.Contains('|');
                bool hasWildcards = file.Any(c => c == '*' || c == '?');

                if (!hasWildcards && multiList)
                {
                    var files = string.Join("|", file.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).Select(f => f.Trim()).Where(f => FileEx.Exists(f)));
                    return string.IsNullOrEmpty(files) ? null : files;
                }

                if (multiList) return null;

                if (!FileEx.GetPathParts(file, out var dir, out var name, out var ext)) return null;

                if (name.Length == 0 && ext.Length == 0)
                {
                    name = "*";
                    return string.Concat(dir, "\\", name, ext);
                }
                if (!FileEx.DirectoryExists(dir)) return null;
                file = string.Concat(dir, "\\", name, ext);
                if (!hasWildcards && !FileEx.IsAssembly(file)) return null;
                return file;
            }
            catch { }

            return null;
        }
    }
}
