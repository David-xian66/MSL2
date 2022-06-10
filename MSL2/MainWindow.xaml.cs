using Microsoft.Win32;
using ModernWpf.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MSL2
{
    public delegate void DeleControl();
    /// <summary>
    /// xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string update = "v1.9.1";
        public static string notice;
        //public static string mslConfig = string.Format("{{{0}}}", "\n\"frpc\": \"\",\n\"notifyIcon\": \"False\",\n\"notice\": \"0\",\n\"frpcversion\": \"2\",\n\"skin\": \"default\"\n");
        public static string mslConfig = string.Format("{{{0}}}", "\n\"frpc\": \"\",\n\"notifyIcon\": \"False\",\n\"notice\": \"0\",\n\"frpcversion\": \"2\"\n");
        pages.Home _homePage = new pages.Home();
        pages.Cmdoutlog _cmdPage = new pages.Cmdoutlog();
        pages.SettingsPage _setPage = new pages.SettingsPage();
        pages.FrpcPage _frpcPage = new pages.FrpcPage();
        pages.PluginsgsOrMods _pluginsOrmodsPage = new pages.PluginsgsOrMods();
        pages.About _aboutPage = new pages.About();
        public static string serverjava = "Java";
        public static string serverserver="";
        public static string serverJVM;
        public static string serverJVMcmd = "";
        public static string serverbase;
        public static string frpc;
        public static long PhisicalMemory;
        public static bool notifyIcon;
        //public static event DeleControl ServerReadOutputStd;
        public static bool ServerReadOutputStd=false;
        //public static Process SERVERCMD = new Process();
        //public event DelReadStdOutput ReadStdOutput;


        public MainWindow()
        {
            InitializeComponent();
            pages.Home home = new pages.Home();
            frame.Content = home;
            pages.Home.FramePageControl += Func;
            pages.Cmdoutlog.StartServerControl += Func1;
            pages.Cmdoutlog.StopServerControl += Func2;
            pages.SettingsPage.C_NotifyIcon += Func3;
            //pages.Cmdoutlog.FramePageControl += Func;

        }
        [DllImport("kernel32.dll")]
        public static extern uint WinExec(string lpCmdLine, uint uCmdShow);
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //welcomelabel.Content = "欢迎使用我的世界开服器！版本：" + update;
            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + @"MSL2"))
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"MSL2");
            }
            //MessageBox.Show("wqqq");
            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"MSL2\config.json"))
            {
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"MSL2\config.json", mslConfig);
                Process.Start(Application.ResourceAssembly.Location);
                Process.GetCurrentProcess().Kill();
            }
            else
            {
                //检测是否配置了内网映射
                try
                {
                    StreamReader reader = File.OpenText(AppDomain.CurrentDomain.BaseDirectory + @"MSL2\config.json");
                    JsonTextReader jsonTextReader = new JsonTextReader(reader);
                    JObject jsonObject = (JObject)JToken.ReadFrom(jsonTextReader);
                    if (jsonObject["frpc"] == null)
                    {
                        MessageBox.Show("配置文件错误，即将修复");
                        File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"MSL2\config.json", mslConfig);
                        Process.Start(Application.ResourceAssembly.Location);
                        Process.GetCurrentProcess().Kill();
                    }
                    frpc = jsonObject["frpc"].ToString();
                    reader.Close();
                    if (frpc == "")
                    {
                        frpc = null;
                    }
                }
                catch
                {
                    frpc = null;
                }
                //自动更新
                try
                {
                    WebClient MyWebClient = new WebClient();
                    MyWebClient.Credentials = CredentialCache.DefaultCredentials;
                    byte[] pageData = MyWebClient.DownloadData("http://106.12.157.82/web/update.txt");
                    string pageHtml = Encoding.UTF8.GetString(pageData);
                    string strtempa = "#";
                    int IndexofA = pageHtml.IndexOf(strtempa);
                    string Ru = pageHtml.Substring(IndexofA + 1);
                    string aaa = Ru.Substring(0, Ru.IndexOf("#"));
                    if (aaa != update)
                    {
                        string strtempa1 = "*";
                        int IndexofA1 = pageHtml.IndexOf(strtempa1);
                        string Ru1 = pageHtml.Substring(IndexofA1 + 1);
                        string aaa1 = Ru1.Substring(0, Ru1.IndexOf("*"));
                        DownloadWindow.downloadurl = aaa1;
                        DownloadWindow.downloadPath = AppDomain.CurrentDomain.BaseDirectory;
                        DownloadWindow.filename = "MSL2" + aaa + ".exe";
                        DownloadWindow.downloadinfo = "下载新版本中……";
                        Window window = new DownloadWindow();
                        window.ShowDialog();
                        string vBatFile = System.IO.Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + @"\DEL.bat";
                        using (StreamWriter vStreamWriter = new StreamWriter(vBatFile, false, Encoding.Default))
                        {
                            vStreamWriter.Write(string.Format(":del\r\n del \"" + System.Windows.Forms.Application.ExecutablePath + "\"\r\n " + "if exist \"" + System.Windows.Forms.Application.ExecutablePath + "\" goto del\r\n " + "start /d \"" + AppDomain.CurrentDomain.BaseDirectory + "\" MSL2" + aaa + ".exe" + "\r\n" + " del %0\r\n", AppDomain.CurrentDomain.BaseDirectory));
                        }
                        WinExec(vBatFile, 0);
                        Process.GetCurrentProcess().Kill();
                    }
                }
                catch
                {
                }
            }
            //托盘图标检测
            try
            {
                StreamReader reader = File.OpenText(AppDomain.CurrentDomain.BaseDirectory + @"MSL2\config.json");
                JsonTextReader jsonTextReader = new JsonTextReader(reader);
                JObject jsonObject = (JObject)JToken.ReadFrom(jsonTextReader);
                if (jsonObject["notifyIcon"] == null)
                {
                    reader.Close();
                    MessageBox.Show("配置文件错误，即将修复");
                    File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"MSL2\config.json", mslConfig);
                    Process.Start(Application.ResourceAssembly.Location);
                    Process.GetCurrentProcess().Kill();
                }
                if (jsonObject["notifyIcon"].ToString() == "True")
                {
                    notifyIcon = true;
                }
                else
                {
                    notifyIcon = false;
                    //setnotifyIcon.Content = "开启托盘图标";
                }
                reader.Close();
            }
            catch
            {
                notifyIcon = false;
            }
            if (notifyIcon == true)
            {
                NotifyForm fw = new NotifyForm();
                fw.Show();
                fw.NotifyFormShowEvent += NotifyFormShow;
            }

            /*
            try
            {
                StreamReader reader = File.OpenText(AppDomain.CurrentDomain.BaseDirectory + @"MSL2\config.json");
                JsonTextReader jsonTextReader = new JsonTextReader(reader);
                JObject jsonObject = (JObject)JToken.ReadFrom(jsonTextReader);
                if (jsonObject["skin"] == null)
                {
                    reader.Close();
                    MessageBox.Show("配置文件错误，即将修复");
                    File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"MSL2\config.json", mslConfig);
                    Process.Start(Application.ResourceAssembly.Location);
                    Process.GetCurrentProcess().Kill();
                }
                if (jsonObject["skin"].ToString() == "default")
                {
                    Brush aaa = new SolidColorBrush(Color.FromRgb(241, 243, 248));//blue
                    Background = aaa;
                }
                if (jsonObject["skin"].ToString() == "red")
                {
                    Brush aaa = new SolidColorBrush(Color.FromRgb(248, 241, 241));//red
                    Background = aaa;
                }
                if (jsonObject["skin"].ToString() == "yellow")
                {
                    Brush aaa = new SolidColorBrush(Color.FromRgb(248, 248, 241));//yellow
                    Background = aaa;
                }
                if (jsonObject["skin"].ToString() == "green")
                {
                    Brush aaa = new SolidColorBrush(Color.FromRgb(241, 248, 244));//green
                    Background = aaa;
                }
                if (jsonObject["skin"].ToString() == "pink")
                {
                    Brush aaa = new SolidColorBrush(Color.FromRgb(247, 241, 248));//pink
                    Background = aaa;
                }
                if (jsonObject["skin"].ToString() == "purple")
                {
                    Brush aaa = new SolidColorBrush(Color.FromRgb(237, 231, 245));//purple
                    Background = aaa;
                }
                reader.Close();
            }
            catch
            {
                Brush aaa = new SolidColorBrush(Color.FromRgb(241, 243, 248));//blue
                Background = aaa;
            }
            */

            //检测是否配置过服务器
            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"MSL2\server.ini"))
            {
                Window wn = new CreateServer();
                wn.ShowDialog();
            }
            else
            {
                GetServerConfig();
            }
            //获取电脑内存
            PhisicalMemory = GetPhisicalMemory();
            //公告
            try
            {
                WebClient MyWebClient = new WebClient();
                MyWebClient.Credentials = CredentialCache.DefaultCredentials;
                byte[] pageData = MyWebClient.DownloadData("http://106.12.157.82/web/notice.txt");
                string pageHtml = Encoding.UTF8.GetString(pageData);
                string strtempa = "*";
                int IndexofA = pageHtml.IndexOf(strtempa);
                string Ru = pageHtml.Substring(IndexofA + 1);
                string noticeversion = Ru.Substring(0, Ru.IndexOf("*"));
                //MessageBox.Show(noticeversion);
                string strtempa2 = "#";
                int IndexofA2 = pageHtml.IndexOf(strtempa2);
                string Ru2 = pageHtml.Substring(IndexofA2 + 1);
                notice = Ru2.Substring(0, Ru2.IndexOf("#"));
                try
                {
                    //noticeLab.Text = "公告：\n" + notice;
                    StreamReader reader = File.OpenText(AppDomain.CurrentDomain.BaseDirectory + @"MSL2\config.json");
                    JsonTextReader jsonTextReader = new JsonTextReader(reader);
                    JObject jsonObject = (JObject)JToken.ReadFrom(jsonTextReader);
                    if (jsonObject["notice"] == null)
                    {
                        MessageBox.Show("配置文件错误，即将修复");
                        File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"MSL2\config.json", mslConfig);
                        Process.Start(Application.ResourceAssembly.Location);
                        Process.GetCurrentProcess().Kill();
                    }
                    string noticeversion1 = jsonObject["notice"].ToString();
                    //MessageBox.Show(noticeversion1);
                    reader.Close();
                    if (noticeversion1 != noticeversion)
                    {
                        //MessageBox.Show(notice, "Notice");
                        StackPanel panel = new StackPanel()
                        {
                            VerticalAlignment = VerticalAlignment.Stretch,
                            HorizontalAlignment = HorizontalAlignment.Stretch,
                        };
                        panel.Children.Add(new TextBlock() { Text = notice });
                        ContentDialog dialog = new ContentDialog()
                        {
                            Title = "公告",
                            CloseButtonText = "确定",
                            IsPrimaryButtonEnabled = false,
                            DefaultButton = ContentDialogButton.Primary,
                            Content = panel,
                        };
                        dialog.ShowAsync();
                        try
                        {
                            //StreamReader reader = File.OpenText(Application.StartupPath+@"\server\MSL2.json", System.Text.Encoding.UTF8);
                            string jsonString = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"MSL2\config.json", System.Text.Encoding.UTF8);
                            JObject jobject = JObject.Parse(jsonString);
                            jobject["notice"] = noticeversion.ToString();
                            string convertString = Convert.ToString(jobject);
                            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"MSL2\config.json", convertString, System.Text.Encoding.UTF8);
                        }
                        catch (Exception a)
                        {
                            StackPanel panel1 = new StackPanel()
                            {
                                VerticalAlignment = VerticalAlignment.Stretch,
                                HorizontalAlignment = HorizontalAlignment.Stretch,
                            };
                            //MessageBox.Show(a.Message, "ERROR");
                            panel1.Children.Add(new TextBlock() { Text = a.Message });
                            ContentDialog dialog1 = new ContentDialog()
                            {
                                Title = "ERROR",
                                CloseButtonText = "确定",
                                IsPrimaryButtonEnabled = false,
                                DefaultButton = ContentDialogButton.Primary,
                                Content = panel1,
                            };
                            dialog1.ShowAsync();
                        }
                    }
                }
                catch
                { }
            }
            catch
            {
                notice = "公告：获取公告失败！请检查网络连接！";
            }
        }
        private void NotifyFormShow()
        {
            this.Visibility = Visibility.Visible;
        }
        private static long GetPhisicalMemory()
        {
            long amemory = 0;
            //获得物理内存 
            ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                if (mo["TotalPhysicalMemory"] != null)
                {
                    amemory = long.Parse(mo["TotalPhysicalMemory"].ToString());
                }
            }
            return amemory;
        }
        public async void GetServerConfig()
        {
            try
            {
                string line = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"MSL2\server.ini");

                int IndexofA2 = line.IndexOf("-j ");
                string Ru2 = line.Substring(IndexofA2 + 3);
                string a200 = Ru2.Substring(0, Ru2.IndexOf("|"));
                //serverjavalist.Items.Add(a200);
                serverjava = a200;

                int IndexofA3 = line.IndexOf("-s ");
                string Ru3 = line.Substring(IndexofA3 + 3);
                string a300 = Ru3.Substring(0, Ru3.IndexOf("|"));
                //serverserverlist.Items.Add(a300);
                serverserver = a300;

                int IndexofA4 = line.IndexOf("-a ");
                string Ru4 = line.Substring(IndexofA4 + 3);
                string a400 = Ru4.Substring(0, Ru4.IndexOf("|"));
                //serverJVMlist.Items.Add(a400);
                serverJVM = a400;

                int IndexofA5 = line.IndexOf("-b ");
                string Ru5 = line.Substring(IndexofA5 + 3);
                string a500 = Ru5.Substring(0, Ru5.IndexOf("|"));
                //serverbaselist.Items.Add(a500);
                serverbase = a500;

                int IndexofA6 = line.IndexOf("-c ");
                string Ru6 = line.Substring(IndexofA6 + 3);
                string a600 = Ru6.Substring(0, Ru6.IndexOf("|"));
                //serverbaselist.Items.Add(a500);
                serverJVMcmd = a600;
            }
            catch
            {
                StackPanel panel = new StackPanel()
                {
                    VerticalAlignment = VerticalAlignment.Stretch,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                };
                panel.Children.Add(new TextBlock() { Text = "开服器检测到您可能没有创建服务器或配置文件出现了错误，是否重新配置服务器？" });
                ContentDialog dialog = new ContentDialog()
                {
                    Title = "ERROR",
                    PrimaryButtonText = "确定",
                    CloseButtonText = "取消",
                    IsPrimaryButtonEnabled = true,
                    DefaultButton = ContentDialogButton.Primary,
                    Content = panel,
                };
                var result = await dialog.ShowAsync();
                if (result == ContentDialogResult.Primary)
                {
                    Window wn = new CreateServer();
                    wn.ShowDialog();
                }
            }
        }
        private void closewindow_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void minbtn_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void closebtn_Click(object sender, RoutedEventArgs e)
        {
            if (notifyIcon == true)
            {
                this.Visibility = Visibility.Hidden;
            }
            else
            {
                try
                {
                    if (pages.Cmdoutlog.SERVERCMD.HasExited == false || pages.FrpcPage.FRPCMD.HasExited == false)
                    {
                        StackPanel panel = new StackPanel()
                        {
                            VerticalAlignment = VerticalAlignment.Stretch,
                            HorizontalAlignment = HorizontalAlignment.Stretch,
                        };
                        panel.Children.Add(new TextBlock() { Text = "您的服务器或内网映射正在运行中，请确保完全关闭后再关闭软件！" });
                        ContentDialog dialog = new ContentDialog()
                        {
                            Title = "警告⚠",
                            CloseButtonText = "确定",
                            IsPrimaryButtonEnabled = false,
                            DefaultButton = ContentDialogButton.Primary,
                            Content = panel,
                        };
                        dialog.ShowAsync();
                        //System.Windows.Forms.MessageBox.Show("您的服务器或内网映射正在运行中，请确保完全关闭后再关闭软件！", "警告", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    }
                    else
                    {
                        var story = (Storyboard)this.Resources["HideWindow"];
                        if (story != null)
                        {
                            story.Completed += delegate { this.Close(); };
                            story.Begin(this);
                        }
                    }
                }
                catch
                {
                    try
                    {
                        if (pages.FrpcPage.FRPCMD.HasExited == false)
                        {
                            StackPanel panel = new StackPanel()
                            {
                                VerticalAlignment = VerticalAlignment.Stretch,
                                HorizontalAlignment = HorizontalAlignment.Stretch,
                            };
                            panel.Children.Add(new TextBlock() { Text = "您的内网映射正在运行中，请确保完全关闭后再关闭软件！" });
                            ContentDialog dialog = new ContentDialog()
                            {
                                Title = "警告⚠",
                                CloseButtonText = "确定",
                                IsPrimaryButtonEnabled = false,
                                DefaultButton = ContentDialogButton.Primary,
                                Content = panel,
                            };
                            dialog.ShowAsync();
                            //System.Windows.Forms.MessageBox.Show("您的服务器或内网映射正在运行中，请确保完全关闭后再关闭软件！", "警告", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                        }
                        else
                        {
                            var story1 = (Storyboard)this.Resources["HideWindow"];
                            if (story1 != null)
                            {
                                story1.Completed += delegate { this.Close(); };
                                story1.Begin(this);
                            }
                        }
                    }
                    catch
                    {
                        var story = (Storyboard)this.Resources["HideWindow"];
                        if (story != null)
                        {
                            story.Completed += delegate { this.Close(); };
                            story.Begin(this);
                        }
                    }
                }
            }
        }

        
        
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (notifyIcon == true)
            {
                e.Cancel = true;
                this.Visibility = Visibility.Hidden;
            }
            else
            {
                try
                {
                    if (pages.Cmdoutlog.SERVERCMD.HasExited == false || pages.FrpcPage.FRPCMD.HasExited == false)
                    {
                        System.Windows.Forms.MessageBox.Show("您的服务器或内网映射正在运行中，请确保完全关闭后再关闭软件！", "警告", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                        e.Cancel = true;
                    }
                    else
                    {
                        Process.GetCurrentProcess().Kill();
                    }
                }
                catch
                {
                    try
                    {
                        if (pages.FrpcPage.FRPCMD.HasExited == false)
                        {
                            System.Windows.Forms.MessageBox.Show("内网映射正在运行中，请确保完全关闭后再关闭软件！", "警告", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                            e.Cancel = true;
                        }
                        else
                        {
                            Process.GetCurrentProcess().Kill();
                        }
                    }
                    catch
                    {
                        Process.GetCurrentProcess().Kill();
                    }
                }
            }
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                var story1 = (Storyboard)this.Resources["ShowWindow"];
                if (story1 != null)
                {
                    story1.Begin(this);
                }
            }
        }
        /*
        private async void tabcontrol1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tabcontrol1.SelectedIndex == 0)
            {
                timer2.Stop();
            }
            if (tabcontrol1.SelectedIndex == 1)
            {
                timer2.Stop();
            }
            if (tabcontrol1.SelectedIndex == 2)
            {
                
            }
            if (tabcontrol1.SelectedIndex == 3)
            {
                timer2.Stop();
                
            }
            if (tabcontrol1.SelectedIndex == 4)
            {
                timer2.Stop();
                
            }
        }
        
        */
        void Func()
        {
            outlogPage.IsSelected = true;
            frame.Content = _cmdPage;
        }
        void Func1()
        {
            settingsPage.IsEnabled = false;
        }
        void Func2()
        {
            settingsPage.IsEnabled = true;
        }
        void Func3()
        {
            NotifyForm fw = new NotifyForm();
            fw.Show();
            fw.NotifyFormShowEvent += NotifyFormShow;
        }
        private void homePage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            frame.Content = _homePage;
        }

        private void outlogPage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            
            frame.Content = _cmdPage;
        }

        private void settingsPage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            frame.Content = _setPage;
        }

        private void frpcPage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            frame.Content = _frpcPage;
        }
        private void pluginsOrmodsPage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            frame.Content = _pluginsOrmodsPage;
        }

        private void aboutPage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            frame.Content = _aboutPage;
        }
    }
}
