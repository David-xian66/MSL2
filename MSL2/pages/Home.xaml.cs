using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MSL2.pages
{
    /// <summary>
    /// Home.xaml 的交互逻辑
    /// </summary>
    public partial class Home : Page
    {
        public static event DeleControl StartServerCmd;
        public static event DeleControl FramePageControl;
        public Home()
        {
            InitializeComponent();
            Cmdoutlog.StartServerControl += Func;
            Cmdoutlog.StopServerControl += Func1;
        }
        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            noticeLab.Text = MainWindow.notice;
            welcomelabel.Content = "欢迎使用我的世界开服器！版本：" + MainWindow.update;
        }
        private void Func()
        {
            startServer.Content = "关闭服务器";
        }
        void Func1()
        {
            startServer.Content = "开启服务器";
        }
        private void startServer_Click(object sender, RoutedEventArgs e)
        {
            if (startServer.Content.ToString() == "开启服务器")
            {
                FramePageControl();
                //tabcontrol1.SelectedIndex = 1;
                //serversettings.IsEnabled = false;
                //setdefault.IsEnabled = false;
                startServer.Content = "关闭服务器";
                StartServerCmd();
            }
            else
            {
                FramePageControl();
                //startServer.Content = "正在关服...";
                Cmdoutlog.SERVERCMD.StandardInput.WriteLine("stop");
            }
        }
        private void startServer_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (startServer.Content.ToString() == "关闭服务器")
            {
                Cmdoutlog.SERVERCMD.Kill();
            }
        }
    }
}
