using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Principal;
using System.Windows;
using System.Windows.Forms;

namespace JavaEnvironmentSetting
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {

        private string _ClassPath= @".;%JAVA_HOME%\lib\dt.jar;%JAVA_HOME%\lib\tools.jar;";

        private string _PathAdded= @"%JAVA_HOME%\bin;%JAVA_HOME%\jre\bin;";

        private string _JavaHome = string.Empty;

        private string _JdkPath = string.Empty;

        public MainWindow()
        {
            InitializeComponent();
            if (!IsAdministrator())
            {
                System.Windows.Forms.MessageBox.Show("请以管理员身份运行");
                System.Windows.Application.Current.Shutdown(-1);
            }          
        }

        private void BtnSetClick_Click(object sender, RoutedEventArgs e)
        {
            RtbResult.AppendText("正在写入\r");
            try
            {
                List<string> ChildPath = new List<string>(Directory.GetDirectories(_JdkPath));
                string JrePath = System.IO.Path.Combine(_JdkPath,"jre");
                string BinPath = System.IO.Path.Combine(_JdkPath,"bin");
                string LibPath = System.IO.Path.Combine(_JdkPath, "lib");
                if (ChildPath.Contains(JrePath)&&ChildPath.Contains(BinPath)&&ChildPath.Contains(LibPath))
                {
                    _JavaHome = _JdkPath;
                    Environment.SetEnvironmentVariable("JAVA_HOME", _JavaHome, EnvironmentVariableTarget.Machine);
                    RtbResult.AppendText("JAVA_HOME写入成功\r");
                    Environment.SetEnvironmentVariable("CLASSPATH",_ClassPath,EnvironmentVariableTarget.Machine);
                    RtbResult.AppendText("CLASSPATH写入成功\r");
                    string PathValue = Environment.GetEnvironmentVariable("Path",EnvironmentVariableTarget.Machine);
                    _PathAdded+= PathValue;
                    Environment.SetEnvironmentVariable("Path",_PathAdded,EnvironmentVariableTarget.Machine);
                }
                else
                {
                    RtbResult.AppendText("写入失败\r");
                }
            }
            catch(Exception ex)
            {
                RtbResult.AppendText(string.Format("写入失败\r+{0}",ex.ToString()));
            }
        }

        private void BtnSearchFolder_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog FolderDialog = new FolderBrowserDialog();
            if (FolderDialog.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }                        
            _JdkPath =FolderDialog.SelectedPath.Trim();
            FolderPathName.Text = _JdkPath;
        }

        private bool IsAdministrator()
        {
            WindowsIdentity current = WindowsIdentity.GetCurrent();
            WindowsPrincipal windowsPricipal = new WindowsPrincipal(current);
            return windowsPricipal.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
}
