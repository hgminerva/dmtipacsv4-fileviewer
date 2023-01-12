using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace dmtipacs_viewer_openner
{
    public partial class FrmMain : Form
    {
        bool _openViewer = true;

        private void updateLogs(string message)
        {
            this.Invoke(new MethodInvoker(delegate
            {
                if (txtLogs.Lines.Count() > 20) txtLogs.Text = txtLogs.Text.Substring(txtLogs.Text.IndexOf(Environment.NewLine));

                txtLogs.AppendText(message + Environment.NewLine);
                txtLogs.SelectionStart = txtLogs.Text.Length;
                txtLogs.ScrollToCaret();
                txtLogs.Refresh();
            }));
        }

        public FrmMain()
        {
            InitializeComponent();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            string json;
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"config.json");

            using (StreamReader sr = new StreamReader(path))
            {
                json = sr.ReadToEnd();
                sr.Close();
            };
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            Config c = javaScriptSerializer.Deserialize<Config>(json);

            txtJSONPath.Text = c.JsonPath;
            txtViewerProgramPath.Text = c.ViewerProgramPath;
            txtDropboxDrive.Text = c.DropboxDrive;
            txtDirectoryLevel.Text = c.DropboxDirectoryLevel.ToString();
            fswViewerOpener.Path = c.JsonPath;

            updateLogs("Started monitoring directory:" + txtJSONPath.Text);
        }

        private bool IsFileLocked(FileInfo file)
        {
            try
            {
                using (FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    stream.Close();
                }
            }
            catch (IOException)
            {
                return true;
            }
            return false;
        }

        private void open_dicom_now(Object e)
        {
            
        }

        private void fswViewerOpener_Changed(object sender, FileSystemEventArgs e)
        {
            updateLogs("A file was changed.");

            String json;
            String dirPath = "";

            try
            {
                FileInfo fileInfo = new FileInfo(e.FullPath);
                if (_openViewer == true && !IsFileLocked(fileInfo))
                {
                    _openViewer = false;

                    updateLogs("File changed: " + e.FullPath);

                    using (StreamReader sr = new StreamReader(e.FullPath))
                    {
                        json = sr.ReadToEnd();
                        sr.Close();
                    };
                    JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                    List<DicomFile> d = javaScriptSerializer.Deserialize<List<DicomFile>>(json);

                    string[] dicomFiles = d[0].DICOMFileName.Split(';');
                    string[] dicomPaths = dicomFiles[0].Split('\\');
                    for (int i = 0; i < Int32.Parse(txtDirectoryLevel.Text) * 2; i++)
                    {
                        if (dicomPaths[i].Length > 0)
                        {
                            if (i == 0)
                            {
                                dirPath += txtDropboxDrive.Text;
                            }
                            else
                            {
                                dirPath += "\\" + dicomPaths[i];
                            }
                        }
                    }
                    updateLogs(dirPath);

                    System.Diagnostics.Process.Start(txtViewerProgramPath.Text, " -d " + dirPath);

                    File.Delete(e.FullPath);

                    _openViewer = true;
                }

            }
            catch (Exception errorMsg)
            {
                updateLogs("Error openning directory: " + errorMsg.ToString());
            }
        }

        private void fswViewerOpener_Created(object sender, FileSystemEventArgs e)
        {
            updateLogs("A file was created.");
        }

        private void fswViewerOpener_Renamed(object sender, RenamedEventArgs e)
        {
            updateLogs("A file was renamed.");

            String json;
            String dirPath = "";

            try
            {
                FileInfo fileInfo = new FileInfo(e.FullPath);
                if (_openViewer == true && !IsFileLocked(fileInfo))
                {
                    _openViewer = false;

                    updateLogs("File changed: " + e.FullPath);

                    using (StreamReader sr = new StreamReader(e.FullPath))
                    {
                        json = sr.ReadToEnd();
                        sr.Close();
                    };
                    JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                    List<DicomFile> d = javaScriptSerializer.Deserialize<List<DicomFile>>(json);

                    string[] dicomFiles = d[0].DICOMFileName.Split(';');
                    string[] dicomPaths = dicomFiles[0].Split('\\');
                    for (int i = 0; i < Int32.Parse(txtDirectoryLevel.Text) * 2; i++)
                    {
                        if (dicomPaths[i].Length > 0)
                        {
                            if (i == 0)
                            {
                                dirPath += txtDropboxDrive.Text;
                            }
                            else
                            {
                                dirPath += "\\" + dicomPaths[i];
                            }
                        }
                    }
                    updateLogs(dirPath);

                    System.Diagnostics.Process.Start(txtViewerProgramPath.Text, " -d " + dirPath);

                    File.Delete(e.FullPath);

                    _openViewer = true;
                }

            }
            catch (Exception errorMsg)
            {
                updateLogs("Error openning directory: " + errorMsg.ToString());
            }
        }
    }
}
