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
        public FormMain()
        {
            InitializeComponent();

            MiniMessageBox.Colors.CaptionFont = this.Font;
            MiniMessageBox.Colors.MessageFont = this.Font;
            m_btnExtract.Enabled = false;
            m_chkSeparateFolders.Checked = true;

#if DEBUG
            var dir = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            m_txtAssembly.Text = Path.Combine(dir,"TestSubject.dll");
            m_txtDestination.Text = Path.Combine(dir, "Extracted");
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
                MiniMessageBox.Show(this, "Not a valid Assembly filename or name with wildcards ( ?, * ).", "Invalid Assembly Filename", MessageBoxButtons.OK, MessageBoxIcon.Error);
                m_txtAssembly.Focus();
                return;
            }

            m_txtAssembly.Text = file;
            if (!string.IsNullOrWhiteSpace(m_txtDestination.Text)) m_btnExtract.Enabled = true;
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
                MiniMessageBox.Show(this, "Not a valid destination folder. It does not need to exist.", "Invalid Folder", MessageBoxButtons.OK, MessageBoxIcon.Error);
                m_txtDestination.Focus();
                return;
            }

            m_txtDestination.Text = file;
            if (!string.IsNullOrWhiteSpace(m_txtAssembly.Text)) m_btnExtract.Enabled = true;
        }

        private void m_btnSelectDestination_Click(object sender, EventArgs e)
        {
            var dir = FolderSelectDialog.Show(this, "Select TV Series Root Folder", m_txtDestination.Text);
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
                MiniMessageBox.ShowDialog(this, "Source filename is no longer valid.", "Error", MiniMessageBox.Buttons.OK, MiniMessageBox.Symbol.Error);
                m_txtAssembly.Focus();
                return;
            }

            if (!Directory.Exists(m_txtDestination.Text))
            {
                MiniMessageBox.ShowDialog(this, "Destination folder is no longer valid.", "Error", MiniMessageBox.Buttons.OK, MiniMessageBox.Symbol.Error);
                m_txtDestination.Focus();
                return;
            }
            m_txtAssembly.Text = file;

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
            this.Close();
        }

        private static string ValidateFileName(string file)
        {
            try
            {
                bool multiList = file.Contains('|');
                bool hasWildcards = file.Any(c => c == '*' || c == '?');

                if (!hasWildcards && multiList)
                {
                    var files = string.Join("|", file.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).Select(f => f.Trim()).Where(f => FileEx.Exists(f)));
                    return string.IsNullOrEmpty(files) ? null : files;
                }

                if (multiList) return null;

                if (!hasWildcards && Directory.Exists(file)) return Path.Combine(Path.GetFullPath(file), "*");
                var dir = Path.GetDirectoryName(file);
                var name = Path.GetFileName(file);
                var dirExists = Directory.Exists(dir);
                if (hasWildcards && dirExists) return Path.Combine(Path.GetFullPath(dir),name);
                if (!dirExists) return null;
                if (FileEx.Exists(file)) return Path.GetFullPath(file);
            }
            catch { }

            return null;
        }
    }
}
