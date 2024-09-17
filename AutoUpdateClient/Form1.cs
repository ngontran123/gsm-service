using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
namespace AutoUpdateClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
       
        public void extractFile()
        {
            string file_name = Application.StartupPath + "\\tool.zip";
            if(!File.Exists(file_name))
            {
                MessageBox.Show("Không tìm thấy tập tin.");
                return;
            }
            this.extractZipFileContent(file_name,Application.StartupPath);
            System.Diagnostics.Process.Start("GSMSERVICES.exe");
        }
        public void extractZipFileContent(string file_path,string output_file_path)
        {
            ZipFile zipFile = null;
            try
            {
                zipFile= new ZipFile(File.OpenRead(file_path));
                List<string> selected_file = new List<string>();
                selected_file.Add("AutoUpdateClient.exe");
                selected_file.Add("ICSharpCode.SharpZipLib.dll");
                foreach(ZipEntry zip in zipFile)
                {
                    if(zip.IsFile)
                    {
                        string file_name = zip.Name;
                    
                            byte[] buffer=new byte[4096];
                            Stream inputStream = zipFile.GetInputStream(zip);
                            string[] files_name = file_name.Split('/');
                            //string path = Path.Combine(output_file_path, file_name);
                            string directoryName = output_file_path;
                            if (directoryName == Application.StartupPath)
                            {
                              /*  if (directoryName.Length > 0)
                                {    
                                    Directory.CreateDirectory(directoryName);
                                }*/
                                if (!selected_file.Contains(files_name[1]))
                                {
                                    using (FileStream file_des = File.Create(Path.Combine(directoryName, files_name[1])))
                                    {
                                        StreamUtils.Copy(inputStream, file_des, buffer);
                                    }
                                }
                            }   
                        
                    }                
                }
            }
            catch(Exception er)
            {
                //MessageBox.Show(er.Message);
            }
            finally
            {
             if(zipFile != null)
                {
                    zipFile.IsStreamOwner = true;
                    zipFile.Close();
                }
            }
            
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            new Task(async() =>
        {

            this.update_label.Invoke(new Action(() =>
            {
                this.update_label.Text = "Đã có phiên bản mới.";
            }));
            await Task.Delay(1000);
            this.extractFile();
            File.Delete(Path.Combine(Application.StartupPath,"tool.zip"));
           Application.Exit();
        }).Start();
        }     
    }
}
