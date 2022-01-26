using Microsoft.Win32;
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
    public delegate void DelReadStdOutput(string result);
    public delegate void DelReadStdOutput1(string result);
    /// <summary>
    /// xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string update = "BETA1.0";
        public static string servername;
        public static string serverjava = "Java";
        public static string serverserver;
        public static string serverJVM;
        public static string serverJVMcmd = "";
        public static string serverbase;
        public static string line;
        string DownjavaName;
        public static string frpc;
        public static long PhisicalMemory;
        public static bool notifyIcon;
        public static bool autoserver = false;
        public static Process SERVERCMD = new Process();
        public static Process FRPCMD = new Process();
        public event DelReadStdOutput ReadStdOutput;
        public event DelReadStdOutput1 ReadStdOutput1;
        DispatcherTimer timer1 = new DispatcherTimer();
        DispatcherTimer timer3 = new DispatcherTimer();
        DispatcherTimer timer2 = new DispatcherTimer();
        public MainWindow()
        {
            InitializeComponent();
            ReadStdOutput += new DelReadStdOutput(ReadStdOutputAction);
            ReadStdOutput1 += new DelReadStdOutput1(ReadStdOutputAction1);
        }
        [DllImport("kernel32.dll")]
        public static extern uint WinExec(string lpCmdLine, uint uCmdShow);
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            welcomelabel.Content = "欢迎使用我的世界开服器！版本：" + update;
            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + @"MSL2"))
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"MSL2");
            }
            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"MSL2\frpc.exe"))
            {
                DownloadWindow.downloadurl = "https://oceansky12337-my.sharepoint.com/personal/makabaka_oceansky12337_onmicrosoft_com/_layouts/52/download.aspx?share=Eah8aW19JhdOod5gjdvfgKsB93CGOCI1U8N9fNrPeG5CAA";
                DownloadWindow.filename = AppDomain.CurrentDomain.BaseDirectory + @"MSL2/frpc.exe";
                DownloadWindow.downloadinfo = "下载内网映射中...";
                Window window = new DownloadWindow();
                window.ShowDialog();
            }
            //MessageBox.Show("wqqq");
            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"MSL2\config.json"))
            {
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"MSL2\config.json", String.Format("{{{0}}}", "\n\"frpc\": \"\",\n\"notifyIcon\": \"True\",\n\"notice\": \"0\",\n\"frpcversion\": \"1\"\n"));
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
                        File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"MSL2\config.json", String.Format("{{{0}}}", "\n\"frpc\": \"\",\n\"notifyIcon\": \"True\",\n\"notice\": \"0\",\n\"frpcversion\": \"1\"\n"));
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
                //内网映射版本检测
                try
                {
                    StreamReader reader = File.OpenText(AppDomain.CurrentDomain.BaseDirectory + @"MSL2\config.json");
                    JsonTextReader jsonTextReader = new JsonTextReader(reader);
                    JObject jsonObject = (JObject)JToken.ReadFrom(jsonTextReader);
                    if (jsonObject["frpcversion"] == null)
                    {
                        MessageBox.Show("配置文件错误，即将修复");
                        DownloadWindow.downloadurl = "https://oceansky12337-my.sharepoint.com/personal/makabaka_oceansky12337_onmicrosoft_com/_layouts/52/download.aspx?share=Eah8aW19JhdOod5gjdvfgKsB93CGOCI1U8N9fNrPeG5CAA";
                        DownloadWindow.filename = AppDomain.CurrentDomain.BaseDirectory + @"MSL2/frpc.exe";
                        DownloadWindow.downloadinfo = "下载内网映射中...";
                        Window window = new DownloadWindow();
                        window.ShowDialog();
                        File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"MSL2\config.json", String.Format("{{{0}}}", "\n\"frpc\": \"\",\n\"notifyIcon\": \"True\",\n\"notice\": \"0\",\n\"frpcversion\": \"1\"\n"));
                        Process.Start(Application.ResourceAssembly.Location);
                        Process.GetCurrentProcess().Kill();
                    }
                    if (jsonObject["frpcversion"].ToString() != "1")
                    {
                        DownloadWindow.downloadurl = "https://oceansky12337-my.sharepoint.com/personal/makabaka_oceansky12337_onmicrosoft_com/_layouts/52/download.aspx?share=Eah8aW19JhdOod5gjdvfgKsB93CGOCI1U8N9fNrPeG5CAA";
                        DownloadWindow.filename = AppDomain.CurrentDomain.BaseDirectory + @"MSL2/frpc.exe";
                        DownloadWindow.downloadinfo = "下载内网映射中...";
                        Window window = new DownloadWindow();
                        window.ShowDialog();
                    }
                    reader.Close();
                }
                catch
                { }
                try
                {
                    WebClient MyWebClient = new WebClient();
                    MyWebClient.Credentials = CredentialCache.DefaultCredentials;
                    byte[] pageData = MyWebClient.DownloadData("http://115.220.5.81:8081/web/update2.txt");
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
                        DownloadWindow.filename = AppDomain.CurrentDomain.BaseDirectory + "MSL2" + aaa + ".exe";
                        DownloadWindow.downloadinfo = "下载新版本中……";
                        Window window = new DownloadWindow();
                        window.ShowDialog();
                        /*
                        pbar.Maximum = (int)md.FileSize;
                        Thread bar = new Thread(() =>
                        {
                            while (!md.IsComplete)
                            {
                                if (md.IsFailed)
                                {
                                    infolabel.Text = "下载失败";
                                    break;
                                }
                                Thread.Sleep(50);
                                this.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                                { 
                                    pbar.Value = md.DownloadSize;
                                });
                            }
                            MessageBox.Show("恭喜！文件已下载完成", "提示");

                        });
                        bar.Start();
                        latch.Wait();*/
                        string vBatFile = System.IO.Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + @"\DEL.bat";
                        using (StreamWriter vStreamWriter = new StreamWriter(vBatFile, false, Encoding.Default))
                        {
                            vStreamWriter.Write(string.Format(":del\r\n del \"" + System.Windows.Forms.Application.ExecutablePath + "\"\r\n " + "if exist \"" + System.Windows.Forms.Application.ExecutablePath + "\" goto del\r\n " + "start /d \"" + AppDomain.CurrentDomain.BaseDirectory + "\" MSL2" + aaa + ".exe" + "\r\n" + " del %0\r\n", AppDomain.CurrentDomain.BaseDirectory));
                        }
                        WinExec(vBatFile, 0);
                        Process.GetCurrentProcess().Kill();
                        /*
                        pbar.Maximum = (int)md.FileSize;
                        while (!md.IsComplete)
                        {
                            if (md.IsFailed)
                            {
                                infolabel.Text = "下载失败";
                                break;
                            }
                            pbar.Value = md.DownloadSize;
                        }*/
                    }
                }
                catch
                {
                }
                if (notifyIcon == true)
                {
                    //Form2 fw = new Form2();
                    //fw.Show();
                    //fw.form1ShowEvent += form1Show;
                }
            }


            try
            {
                WebClient MyWebClient = new WebClient();
                MyWebClient.Credentials = CredentialCache.DefaultCredentials;
                byte[] pageData = MyWebClient.DownloadData("http://115.220.5.81:8081/web/notice.txt");
                string pageHtml = Encoding.UTF8.GetString(pageData);
                string strtempa = "*";
                int IndexofA = pageHtml.IndexOf(strtempa);
                string Ru = pageHtml.Substring(IndexofA + 1);
                string noticeversion = Ru.Substring(0, Ru.IndexOf("*"));
                //MessageBox.Show(noticeversion);
                string strtempa2 = "#";
                int IndexofA2 = pageHtml.IndexOf(strtempa2);
                string Ru2 = pageHtml.Substring(IndexofA2 + 1);
                string notice = Ru2.Substring(0, Ru2.IndexOf("#"));
                try
                {
                    noticeLab.Text = "公告：\n" + notice;
                    StreamReader reader = File.OpenText(AppDomain.CurrentDomain.BaseDirectory + @"MSL2\config.json");
                    JsonTextReader jsonTextReader = new JsonTextReader(reader);
                    JObject jsonObject = (JObject)JToken.ReadFrom(jsonTextReader);
                    if (jsonObject["notice"] == null)
                    {
                        MessageBox.Show("配置文件错误，即将修复");
                        File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"MSL2\config.json", String.Format("{{{0}}}", "\n\"frpc\": \"\",\n\"notifyIcon\": \"True\",\n\"notice\": \"0\",\n\"frpcversion\": \"1\"\n"));
                        Process.Start(Application.ResourceAssembly.Location);
                        Process.GetCurrentProcess().Kill();
                    }
                    string noticeversion1 = jsonObject["notice"].ToString();
                    //MessageBox.Show(noticeversion1);
                    reader.Close();
                    if (noticeversion1 != noticeversion)
                    {
                        MessageBox.Show(notice, "Notice");
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
                        { MessageBox.Show(a.Message.ToString(), "ERROR"); }
                    }
                }
                catch
                { }
            }
            catch
            { }
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
                    File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"MSL2\config.json", String.Format("{{{0}}}", "\n\"frpc\": \"\",\n\"notifyIcon\": \"True\",\n\"notice\": \"0\",\n\"frpcversion\": \"1\"\n"));
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
                notifyIcon = true;
            }
            //检测是否配置过服务器
            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"MSL2\server.ini"))
            {
                Window wn = new CreateServer();
                wn.ShowDialog();
            }
            else
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
                }
                catch
                {
                    var mb=MessageBox.Show("开服器检测到您可能没有创建服务器或配置文件出现了错误，是否重新配置服务器？", "ERROR", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (mb == MessageBoxResult.Yes)
                    {
                        Window wn = new CreateServer();
                        wn.ShowDialog();
                    }
                }
            }
            PhisicalMemory = GetPhisicalMemory();
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
        private void closewindow_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void minbtn_Click(object sender, RoutedEventArgs e)
        {
            var story = (Storyboard)this.Resources["HideWindow"];
            if (story != null)
            {
                story.Completed += delegate
                {
                    WindowState = WindowState.Minimized;
                    var story1 = (Storyboard)this.Resources["ShowWindow"];
                    if (story1 != null)
                    {
                        story1.Begin(this);
                    }
                };
                story.Begin(this);
            }
        }

        private void closebtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SERVERCMD.HasExited == false)
                {

                    System.Windows.Forms.MessageBox.Show("您的服务器或内网映射正在运行中，请确保完全关闭后再关闭软件！", "警告", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
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
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void startServer_Click(object sender, RoutedEventArgs e)
        {
            if (startServer.Content.ToString() == "开启服务器")
            {
                tabcontrol1.SelectedIndex = 1;
                serversettings.IsEnabled = false;
                //setdefault.IsEnabled = false;
                startServer.Content = "关闭服务器";
                startServer.Background = new SolidColorBrush(Color.FromRgb(139, 0, 0));
                //serverState.Foreground = new SolidColorBrush(Color.FromRgb(0, 139, 68));
                cmdtext.IsEnabled = true;
                sendcmd.IsEnabled = true;
                fastCMD.IsEnabled = true;
                outlog.Document.Blocks.Clear();
                ShowLog("正在开启服务器，请稍等...", Brushes.Black);
                //serverState.Content = "开服中";
                StartServer(serverJVM + " -jar " + serverserver + " nogui " + serverJVMcmd);
            }
            else
            {
                startServer.Content = "正在关服...";
                SERVERCMD.StandardInput.WriteLine("stop");
            }
        }
        private void startServer_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }
        private void StartServer(string StartFileArg)
        {
            try
            {
                //cmdtext.IsEnabled = true;
                //sendcmd.IsEnabled = true;
                SERVERCMD.StartInfo.FileName = serverjava;
                //SERVERCMD.StartInfo.FileName = StartFileName;
                SERVERCMD.StartInfo.Arguments = StartFileArg;
                Directory.SetCurrentDirectory(serverbase);
                SERVERCMD.StartInfo.CreateNoWindow = true;
                SERVERCMD.StartInfo.UseShellExecute = false;
                SERVERCMD.StartInfo.RedirectStandardInput = true;
                SERVERCMD.StartInfo.RedirectStandardOutput = true;
                SERVERCMD.StartInfo.RedirectStandardError = true;
                SERVERCMD.OutputDataReceived += new DataReceivedEventHandler(p_OutputDataReceived);
                SERVERCMD.ErrorDataReceived += new DataReceivedEventHandler(p_OutputDataReceived);
                SERVERCMD.Start();
                SERVERCMD.BeginOutputReadLine();
                SERVERCMD.BeginErrorReadLine();
                cmdtext.Text = "";
                timer1.Tick += new EventHandler(timer1_Tick);
                timer1.Interval = TimeSpan.FromSeconds(1);
                timer1.Start();
                //SERVERCMD.StandardInput.WriteLine(StartFileArg + "&exit");
                //serverTime = 0;
                //timer2.Tick += new EventHandler(timer2_Tick);
                //timer2.Interval = TimeSpan.FromSeconds(1);
                //timer2.Start();
            }
            catch (Exception e)
            {
                ShowLog("出现错误，正在检查问题...", Brushes.Black);
                string a = serverjava.Replace("\"", "");
                if (File.Exists(a))
                {
                    ShowLog("Java路径正常", Brushes.Green);
                }
                else
                {
                    ShowLog("Java路径有误", Brushes.Red);
                }
                string b = serverserver.Replace("\"", "");
                if (File.Exists(b))
                {
                    ShowLog("服务端路径正常", Brushes.Green);
                }
                else
                {
                    ShowLog("服务端路径有误", Brushes.Red);
                }
                //string c = serverbase.Replace("\"", "");
                if (Directory.Exists(serverbase))
                {
                    ShowLog("服务器目录正常", Brushes.Green);
                }
                else
                {
                    ShowLog("服务器目录有误", Brushes.Red);
                }
                MessageBox.Show("出现错误，开服器已检测完毕，请根据检测信息对服务器设置进行更改！\n错误代码:" + e.Message.ToString(), "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                startServer.Content = "开启服务器";
                //serverState.Foreground = new SolidColorBrush(Color.FromRgb(139, 0, 0));
                startServer.Background = new SolidColorBrush(Color.FromRgb(0, 139, 68));
                try
                {
                    SERVERCMD.CancelOutputRead();
                    SERVERCMD.CancelErrorRead();
                    //ReadStdOutput = null;

                    SERVERCMD.OutputDataReceived -= new DataReceivedEventHandler(p_OutputDataReceived);
                    SERVERCMD.ErrorDataReceived -= new DataReceivedEventHandler(p_OutputDataReceived);

                    //ReadStdOutput += new DelReadStdOutput(ReadStdOutputAction);
                    //outlog.AppendText("\n服务器已关闭！输入start来开启服务器.");
                }
                catch { }
                serversettings.IsEnabled = true;
                //setdefault.IsEnabled = true;
                cmdtext.IsEnabled = false;
                sendcmd.IsEnabled = false;
                fastCMD.IsEnabled = false;

            }
        }
        private void p_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                Dispatcher.Invoke(ReadStdOutput, new object[] { e.Data });
            }
        }
        void ShowLog(string msg, Brush color)
        {
            this.Dispatcher.BeginInvoke(DispatcherPriority.SystemIdle, (ThreadStart)delegate ()
            {
                string s = string.Format("{1}", DateTime.Now, msg);
                Paragraph p = new Paragraph(new Run(s));
                p.Foreground = color;
                outlog.Document.Blocks.Add(p);
                outlog.ScrollToEnd();
            });
        }
        private delegate void AddMessageHandler(string msg);
        private void ReadStdOutputAction(string msg)
        {
            if (msg.IndexOf("EULA") + 1 != 0)
            {
                //outlog.AppendText("[信息]" + msg);
                ShowLog("[信息]" + msg, Brushes.Green);
                MessageBoxResult msgr = MessageBox.Show("检测到您没有接受Mojang的EULA条款！是否阅读并接受EULA条款并继续开服？", "警告", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (msgr == MessageBoxResult.Yes)
                {
                    try
                    {
                        string path1 = serverbase + @"\eula.txt";
                        FileStream fs = new FileStream(path1, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                        StreamReader sr = new StreamReader(fs, System.Text.Encoding.Default);
                        string line;
                        line = sr.ReadToEnd();
                        line = line.Replace("eula=false", "eula=true");
                        string path = serverbase + @"\eula.txt";
                        StreamWriter streamWriter = new StreamWriter(path);
                        streamWriter.WriteLine(line);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }
                    catch
                    {
                        MessageBox.Show("出现错误，请手动修改eula文件或重试:" + "r0x2", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    Process.Start("https://account.mojang.com/documents/minecraft_eula");
                    /*
                    outlog.Text = "";
                    efflabel.Content = "已开启";
                    timelabel.Content = string.Format("{0:00}:{1:00}:{2:00}", 0, 0, 0);
                    sw = new Stopwatch();
                    timer4.Tick += new EventHandler(timer4_Tick);
                    timer4.Interval = TimeSpan.FromSeconds(1);
                    sw.Start();
                    timer4.Start();
                    */
                    MessageBox.Show("阅读完毕后请点击“开启服务器”按钮以开服！");
                }

            }
            else
            {
                if (msg.IndexOf("Unable to access jarfile") + 1 != 0)
                {
                    ShowLog("[错误]" + msg + "\r\n警告：无法访问JAR文件！请检查服务器设置是否正确或更换服务端再试！", Brushes.Red);
                }
                else
                {
                    if (msg.IndexOf("Done") + 1 != 0)
                    {
                        //outlog.AppendText("[信息]" + msg + "已成功开启服务器！在没有改动服务器IP和端口的情况下，请使用127.0.0.1:25565进入服务器；要让他人进服，需要进行内网映射或使用公网IP。");
                        ShowLog("[信息]" + msg + "\r\n已成功开启服务器！\r\n你可以输入stop来关闭服务器！", Brushes.Black);
                        //serverState.Content = "已开服";
                        try
                        {
                            string path1 = serverbase + @"\server.properties";
                            FileStream fs = new FileStream(path1, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                            StreamReader sr = new StreamReader(fs, System.Text.Encoding.Default);
                            string line;
                            line = sr.ReadToEnd();
                            if (line.IndexOf("online-mode=true") + 1 != 0)
                            {
                                onlineMode.IsEnabled = true;
                                ShowLog("\r\n检测到您没有关闭正版验证，如果您的游戏登录方式为离线登录的话，请点击“更多功能”里“关闭正版验证”按钮以关闭正版验证。否则离线账户将无法进入服务器！\r\n", Brushes.Black);
                            }
                        }
                        catch
                        {
                            MessageBox.Show("出现错误" + "r0x4", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                        }

                    }
                    else
                    {
                        if (msg.IndexOf("Stopping server") + 1 != 0)
                        {
                            //outlog.AppendText("[信息]" + msg + "正在关闭服务器！");
                            ShowLog("[信息]" + msg + "\r\n正在关闭服务器！", Brushes.Black);
                            //serverState.Content = "关服中";
                        }
                        else
                        {
                            if (msg.IndexOf("FAILED") + 1 != 0)
                            {
                                if (msg.IndexOf("PORT") + 1 != 0)
                                {
                                    ShowLog("[错误]" + msg + "\r\n警告：由于端口占用，服务器已自动关闭！请检查您的服务器是否多开或者有其他软件占用端口！", Brushes.Red);
                                    //outlog.AppendText("[错误]" + msg + "\r\n警告：由于端口占用，服务器已自动关闭！请检查您的服务器是否多开或者有其他软件占用端口！\r\n");

                                }
                            }
                            else
                            {
                                if (msg.IndexOf("INFO") + 1 != 0)
                                {
                                    //outlog.AppendText("[信息]" + msg);
                                    ShowLog("[信息]" + msg, Brushes.Green);

                                }
                                else
                                {
                                    if (msg.IndexOf("WARN") + 1 != 0)
                                    {
                                        //outlog.AppendText("[警告]" + msg);
                                        ShowLog("[警告]" + msg, Brushes.Orange);
                                    }
                                    else
                                    {
                                        if (msg.IndexOf("ERROR") + 1 != 0)
                                        {
                                            //outlog.AppendText("[错误]" + msg);
                                            ShowLog("[错误]" + msg, Brushes.Red);

                                        }
                                        else
                                        {
                                            //outlog.AppendText(msg);
                                            ShowLog(msg, Brushes.Green);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        void timer1_Tick(object sender, EventArgs e)
        {
            if (SERVERCMD.HasExited == true)
            {
                timer1.Stop();
                if (autoserver == true)
                {
                    ShowLog("正在重启服务器...", Brushes.Black);
                    //serverState.Content = "重启中";
                    SERVERCMD.CancelOutputRead();
                    SERVERCMD.CancelErrorRead();
                    //ReadStdOutput = null;
                    SERVERCMD.OutputDataReceived -= new DataReceivedEventHandler(p_OutputDataReceived);
                    SERVERCMD.ErrorDataReceived -= new DataReceivedEventHandler(p_OutputDataReceived);
                    //ReadStdOutput += new DelReadStdOutput(ReadStdOutputAction);
                    outlog.Document.Blocks.Clear();
                    //outlog.AppendText("正在重启服务器...");
                    StartServer(serverJVM + " -jar " + serverserver + " nogui");
                }
                else
                {
                    cmdtext.Text = "服务器已关闭";
                    //serverState.Content = "已关服";
                    startServer.Content = "开启服务器";
                    //serverState.Foreground = new SolidColorBrush(Color.FromRgb(139, 0, 0));
                    startServer.Background = new SolidColorBrush(Color.FromRgb(0, 139, 68));
                    //tabbtn3.IsEnabled = true;
                    serversettings.IsEnabled = true;
                    //setdefault.IsEnabled = true;
                    cmdtext.IsEnabled = false;
                    sendcmd.IsEnabled = false;
                    fastCMD.IsEnabled = false;
                    SERVERCMD.CancelOutputRead();
                    SERVERCMD.CancelErrorRead();
                    //ReadStdOutput = null;
                    SERVERCMD.OutputDataReceived -= new DataReceivedEventHandler(p_OutputDataReceived);
                    SERVERCMD.ErrorDataReceived -= new DataReceivedEventHandler(p_OutputDataReceived);
                    //ReadStdOutput += new DelReadStdOutput(ReadStdOutputAction);
                    //outlog.AppendText("\n服务器已关闭！输入start来开启服务器.");
                }
            }
        }
        private void sendcmd_Click(object sender, RoutedEventArgs e)
        {
            if (fastCMD.SelectedIndex == 1)
            {
                SERVERCMD.StandardInput.WriteLine("op " + cmdtext.Text);
                cmdtext.Text = "";
                fastCMD.SelectedIndex = 0;
            }
            if (fastCMD.SelectedIndex == 2)
            {
                SERVERCMD.StandardInput.WriteLine("deop " + cmdtext.Text);
                cmdtext.Text = "";
                fastCMD.SelectedIndex = 0;
            }
            if (fastCMD.SelectedIndex == 3)
            {
                SERVERCMD.StandardInput.WriteLine("ban " + cmdtext.Text);
                cmdtext.Text = "";
                fastCMD.SelectedIndex = 0;
            }
            if (fastCMD.SelectedIndex == 4)
            {
                SERVERCMD.StandardInput.WriteLine("say " + cmdtext.Text);
                cmdtext.Text = "";
                fastCMD.SelectedIndex = 0;
            }
            if (fastCMD.SelectedIndex == 5)
            {
                SERVERCMD.StandardInput.WriteLine("pardon " + cmdtext.Text);
                cmdtext.Text = "";
                fastCMD.SelectedIndex = 0;
            }
            SERVERCMD.StandardInput.WriteLine(cmdtext.Text);
            cmdtext.Text = "";
        }

        private void cmdtext_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (fastCMD.SelectedIndex == 1)
                {
                    SERVERCMD.StandardInput.WriteLine("op " + cmdtext.Text);
                    cmdtext.Text = "";
                    fastCMD.SelectedIndex = 0;
                }
                if (fastCMD.SelectedIndex == 2)
                {
                    SERVERCMD.StandardInput.WriteLine("deop " + cmdtext.Text);
                    cmdtext.Text = "";
                    fastCMD.SelectedIndex = 0;
                }
                if (fastCMD.SelectedIndex == 3)
                {
                    SERVERCMD.StandardInput.WriteLine("ban " + cmdtext.Text);
                    cmdtext.Text = "";
                    fastCMD.SelectedIndex = 0;
                }
                if (fastCMD.SelectedIndex == 4)
                {
                    SERVERCMD.StandardInput.WriteLine("say " + cmdtext.Text);
                    cmdtext.Text = "";
                    fastCMD.SelectedIndex = 0;
                }
                if (fastCMD.SelectedIndex == 5)
                {
                    SERVERCMD.StandardInput.WriteLine("pardon " + cmdtext.Text);
                    cmdtext.Text = "";
                    fastCMD.SelectedIndex = 0;
                }
                SERVERCMD.StandardInput.WriteLine(cmdtext.Text);
                cmdtext.Text = "";
            }
        }

        private void autoStartserver_Click(object sender, RoutedEventArgs e)
        {
            if (autoStartserver.Content.ToString() == "关服自动开服:禁用")
            {
                autoserver = true;
                autoStartserver.Content = "关服自动开服:启用";
            }
            else
            {
                autoserver = false;
                autoStartserver.Content = "关服自动开服:禁用";
            }
        }
        private void onlineMode_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SERVERCMD.HasExited == false)
                {
                    MessageBox.Show("检测到服务器正在运行，正在关闭服务器...");
                    SERVERCMD.StandardInput.WriteLine("stop");
                }
            }
            catch
            {
                try
                {
                    string path1 = serverbase + @"\server.properties";
                    FileStream fs = new FileStream(path1, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    StreamReader sr = new StreamReader(fs, System.Text.Encoding.Default);
                    string line;
                    line = sr.ReadToEnd();
                    line = line.Replace("online-mode=true", "online-mode=false");
                    string path = serverbase + @"\server.properties";
                    StreamWriter streamWriter = new StreamWriter(path);
                    streamWriter.WriteLine(line);
                    streamWriter.Flush();
                    streamWriter.Close();
                    MessageBox.Show("修改完毕！请重新启动服务器！");

                }
                catch
                {
                    MessageBox.Show("出现错误，请手动修改server.properties文件或重试:" + "r0x3", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void moreTools_Click(object sender, RoutedEventArgs e)
        {
            if (moreTools.Content.ToString() != "收起")
            {
                sendcmd.Visibility = Visibility.Hidden;
                cmdtext.Visibility = Visibility.Hidden;
                moreTools.Content = "收起";
            }
            else
            {
                sendcmd.Visibility = Visibility.Visible;
                cmdtext.Visibility = Visibility.Visible;
                moreTools.Content = "更多功能";
            }
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Process.GetCurrentProcess().Kill();
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
        private void doneBtn1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                doneBtn1.IsEnabled = false;
                if (useJv8.IsChecked == true)
                {
                    jAva.Text = AppDomain.CurrentDomain.BaseDirectory + @"MSL2\Java8\bin\java.exe";
                    if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + @"MSL2\Java8"))
                    {
                        if (useJVMauto.IsChecked == true)
                        {
                            line = "*|-j " + "\"" + jAva.Text + "\"" + "|-s " + "\"" + server.Text + "\"" + "|-a |-b " + bAse.Text + "|*";
                            FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"MSL2\server.ini", FileMode.Create);
                            StreamWriter sw = new StreamWriter(fs);
                            sw.Write(line);
                            sw.Flush();
                            sw.Close();
                            fs.Close();

                            serverjava = "\"" + jAva.Text + "\"";
                            serverserver = "\"" + server.Text + "\"";
                            serverJVM = "";
                            serverbase = bAse.Text;
                        }
                        else
                        {
                            line = "*|-j " + "\"" + jAva.Text + "\"" + "|-s " + "\"" + server.Text + "\"" + "|-a " + " -Xmx" + memorySlider.Value.ToString("f0") + "M" + "|-b " + bAse.Text + "|*";
                            FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"MSL2\server.ini", FileMode.Create);
                            StreamWriter sw = new StreamWriter(fs);
                            sw.Write(line);
                            sw.Flush();
                            sw.Close();
                            fs.Close();

                            serverjava = "\"" + jAva.Text + "\"";
                            serverserver = "\"" + server.Text + "\"";
                            serverJVM = " -Xmx" + memorySlider.Value.ToString("f0") + "M";
                            serverbase = bAse.Text;
                        }

                        MessageBox.Show("保存完毕！", "信息", MessageBoxButton.OK, MessageBoxImage.Information);
                        doneBtn1.IsEnabled = true;
                    }
                    else
                    {
                        MessageBox.Show("下载Java即代表您接受Java的服务条款https://www.oracle.com/downloads/licenses/javase-license1.html");
                        if (Environment.Is64BitOperatingSystem)
                        {
                            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"MSL2\Java8\bin\java.exe"))
                            {
                                DownjavaName = "Java8";
                                DownloadWindow.downloadurl = "https://oceansky12337-my.sharepoint.com/personal/makabaka_oceansky12337_onmicrosoft_com/_layouts/52/download.aspx?share=Ecs65caK7blGgZipDS1d76IBKDID3YUy9ak-HUzY_vDQUQ";
                                DownloadWindow.filename = AppDomain.CurrentDomain.BaseDirectory + @"MSL2\Java.exe";
                                DownloadWindow.downloadinfo = "下载Java16中……";
                                Window window = new DownloadWindow();
                                window.ShowDialog();
                                try
                                {
                                    Process SERVERCMD = new Process();
                                    SERVERCMD.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + @"MSL2\Java.exe";
                                    Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory + "MSL2");
                                    SERVERCMD.Start();
                                    timer3.Tick += new EventHandler(timer3_Tick);
                                    timer3.Interval = TimeSpan.FromSeconds(3);
                                    timer3.Start();
                                    downout.Content = "安装中...";
                                    /*
                                    try
                                    {
                                        File.Delete(AppDomain.CurrentDomain.BaseDirectory + @"MSL2\Java.exe");
                                        outlog.Content = "完成";
                                        sJVM.IsSelected = true;
                                        sJVM.IsEnabled = true;
                                        sserver.IsEnabled = false;
                                        MainWindow.serverserver = txb3.Text;
                                        next3.IsEnabled = true;
                                    }
                                    catch
                                    {
                                        return;
                                    }*/
                                }
                                catch
                                {
                                    MessageBox.Show("安装失败，请查看是否有杀毒软件进行拦截！请确保添加信任或关闭杀毒软件后进行重新安装！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                                    downout.Content = "安装失败！";
                                }
                                /*
                                Form4 fw = new Form4();
                                fw.ShowDialog();*/
                            }
                        }
                        else
                        {
                            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"MSL2\Java8\bin\java.exe"))
                            {
                                DownjavaName = "Java8";
                                DownloadWindow.downloadurl = "https://oceansky12337-my.sharepoint.com/personal/makabaka_oceansky12337_onmicrosoft_com/_layouts/52/download.aspx?share=ES74HP6tN6dKuyTPUVOfEaYBJAecYATfZKXahAN_EZDC8Q";
                                DownloadWindow.filename = AppDomain.CurrentDomain.BaseDirectory + @"MSL2\Java.exe";
                                DownloadWindow.downloadinfo = "下载Java16中……";
                                Window window = new DownloadWindow();
                                window.ShowDialog();
                                try
                                {
                                    Process SERVERCMD = new Process();
                                    SERVERCMD.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + @"MSL2\Java.exe";
                                    Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory + "MSL2");
                                    SERVERCMD.Start();
                                    timer3.Tick += new EventHandler(timer3_Tick);
                                    timer3.Interval = TimeSpan.FromSeconds(3);
                                    timer3.Start();
                                    downout.Content = "安装中...";
                                    /*
                                    try
                                    {
                                        File.Delete(AppDomain.CurrentDomain.BaseDirectory + @"MSL2\Java.exe");
                                        outlog.Content = "完成";
                                        sJVM.IsSelected = true;
                                        sJVM.IsEnabled = true;
                                        sserver.IsEnabled = false;
                                        MainWindow.serverserver = txb3.Text;
                                        next3.IsEnabled = true;
                                    }
                                    catch
                                    {
                                        return;
                                    }*/
                                }
                                catch
                                {
                                    MessageBox.Show("安装失败，请查看是否有杀毒软件进行拦截！请确保添加信任或关闭杀毒软件后进行重新安装！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                                    downout.Content = "安装失败！";
                                }
                                /*
                                Form4 fw = new Form4();
                                fw.ShowDialog();*/
                            }
                        }

                    }

                    /*
                    sJVM.IsSelected = true;
                    sJVM.IsEnabled = true;
                    sserver.IsEnabled = false;
                    MainWindow.serverserver = txb3.Text;*/
                }
                if (useJv16.IsChecked == true)
                {
                    jAva.Text = AppDomain.CurrentDomain.BaseDirectory + @"MSL2\Java16\bin\java.exe";
                    if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + @"MSL2\Java16"))
                    {
                        if (useJVMauto.IsChecked == true)
                        {
                            line = "*|-j " + "\"" + jAva.Text + "\"" + "|-s " + "\"" + server.Text + "\"" + "|-a |-b " + bAse.Text + "|*";
                            FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"MSL2\server.ini", FileMode.Create);
                            StreamWriter sw = new StreamWriter(fs);
                            sw.Write(line);
                            sw.Flush();
                            sw.Close();
                            fs.Close();

                            serverjava = "\"" + jAva.Text + "\"";
                            serverserver = "\"" + server.Text + "\"";
                            serverJVM = "";
                            serverbase = bAse.Text;
                        }
                        else
                        {
                            line = "*|-j " + "\"" + jAva.Text + "\"" + "|-s " + "\"" + server.Text + "\"" + "|-a " + " -Xmx" + memorySlider.Value.ToString("f0") + "M" + "|-b " + bAse.Text + "|*";
                            FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"MSL2\server.ini", FileMode.Create);
                            StreamWriter sw = new StreamWriter(fs);
                            sw.Write(line);
                            sw.Flush();
                            sw.Close();
                            fs.Close();

                            serverjava = "\"" + jAva.Text + "\"";
                            serverserver = "\"" + server.Text + "\"";
                            serverJVM = " -Xmx" + memorySlider.Value.ToString("f0") + "M";
                            serverbase = bAse.Text;
                        }

                        MessageBox.Show("保存完毕！", "信息", MessageBoxButton.OK, MessageBoxImage.Information);
                        doneBtn1.IsEnabled = true;
                    }
                    else
                    {
                        MessageBox.Show("下载Java即代表您接受Java的服务条款https://www.oracle.com/downloads/licenses/javase-license1.html");
                        if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"MSL2\Java16\bin\java.exe"))
                        {
                            DownjavaName = "Java16";
                            DownloadWindow.downloadurl = "https://oceansky12337-my.sharepoint.com/personal/makabaka_oceansky12337_onmicrosoft_com/_layouts/52/download.aspx?share=EbapBNLCCwRLoFr2kxeCUdcBYNtGdsQO2h1MlzgFU3VZbQ";
                            DownloadWindow.filename = AppDomain.CurrentDomain.BaseDirectory + @"MSL2\Java.exe";
                            DownloadWindow.downloadinfo = "下载Java16中……";
                            Window window = new DownloadWindow();
                            window.ShowDialog();
                            try
                            {
                                Process SERVERCMD = new Process();
                                SERVERCMD.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + @"MSL2\Java.exe";
                                Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory + "MSL2");
                                SERVERCMD.Start();
                                timer3.Tick += new EventHandler(timer3_Tick);
                                timer3.Interval = TimeSpan.FromSeconds(3);
                                timer3.Start();
                                downout.Content = "安装中...";
                                /*
                                try
                                {
                                    File.Delete(AppDomain.CurrentDomain.BaseDirectory + @"MSL2\Java.exe");
                                    outlog.Content = "完成";
                                    sJVM.IsSelected = true;
                                    sJVM.IsEnabled = true;
                                    sserver.IsEnabled = false;
                                    MainWindow.serverserver = txb3.Text;
                                    next3.IsEnabled = true;
                                }
                                catch
                                {
                                    return;
                                }*/
                            }
                            catch
                            {
                                MessageBox.Show("安装失败，请查看是否有杀毒软件进行拦截！请确保添加信任或关闭杀毒软件后进行重新安装！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                                downout.Content = "安装失败！";
                            }
                            /*
                            Form4 fw = new Form4();
                            fw.ShowDialog();*/
                        }

                    }

                    /*
                    sJVM.IsSelected = true;
                    sJVM.IsEnabled = true;
                    sserver.IsEnabled = false;
                    MainWindow.serverserver = txb3.Text;*/
                }
                if (useJv17.IsChecked == true)
                {
                    jAva.Text = AppDomain.CurrentDomain.BaseDirectory + @"MSL2\Java17\bin\java.exe";
                    if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + @"MSL2\Java17"))
                    {
                        if (useJVMauto.IsChecked == true)
                        {
                            line = "*|-j " + "\"" + jAva.Text + "\"" + "|-s " + "\"" + server.Text + "\"" + "|-a |-b " + bAse.Text + "|*";
                            FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"MSL2\server.ini", FileMode.Create);
                            StreamWriter sw = new StreamWriter(fs);
                            sw.Write(line);
                            sw.Flush();
                            sw.Close();
                            fs.Close();

                            serverjava = "\"" + jAva.Text + "\"";
                            serverserver = "\"" + server.Text + "\"";
                            serverJVM = "";
                            serverbase = bAse.Text;
                        }
                        else
                        {
                            line = "*|-j " + "\"" + jAva.Text + "\"" + "|-s " + "\"" + server.Text + "\"" + "|-a " + " -Xmx" + memorySlider.Value.ToString("f0") + "M" + "|-b " + bAse.Text + "|*";
                            FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"MSL2\server.ini", FileMode.Create);
                            StreamWriter sw = new StreamWriter(fs);
                            sw.Write(line);
                            sw.Flush();
                            sw.Close();
                            fs.Close();

                            serverjava = "\"" + jAva.Text + "\"";
                            serverserver = "\"" + server.Text + "\"";
                            serverJVM = " -Xmx" + memorySlider.Value.ToString("f0") + "M";
                            serverbase = bAse.Text;
                        }

                        MessageBox.Show("保存完毕！", "信息", MessageBoxButton.OK, MessageBoxImage.Information);
                        doneBtn1.IsEnabled = true;
                    }
                    else
                    {
                        MessageBox.Show("下载Java即代表您接受Java的服务条款https://www.oracle.com/downloads/licenses/javase-license1.html");
                        if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"MSL2\Java17\bin\java.exe"))
                        {
                            DownjavaName = "Java17";
                            DownloadWindow.downloadurl = "https://oceansky12337-my.sharepoint.com/personal/makabaka_oceansky12337_onmicrosoft_com/_layouts/52/download.aspx?share=EUxH8cdGAlxOkNiZGunIefEBwrdoMM5wPIb5h9xDpiWd_A";
                            DownloadWindow.filename = AppDomain.CurrentDomain.BaseDirectory + @"MSL2\Java.exe";
                            DownloadWindow.downloadinfo = "下载Java17中……";
                            Window window = new DownloadWindow();
                            window.ShowDialog();
                            try
                            {
                                Process SERVERCMD = new Process();
                                SERVERCMD.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + @"MSL2\Java.exe";
                                Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory + "MSL2");
                                SERVERCMD.Start();
                                timer3.Tick += new EventHandler(timer3_Tick);
                                timer3.Interval = TimeSpan.FromSeconds(3);
                                timer3.Start();
                                downout.Content = "安装中...";
                                /*
                                try
                                {
                                    File.Delete(AppDomain.CurrentDomain.BaseDirectory + @"MSL2\Java.exe");
                                    outlog.Content = "完成";
                                    sJVM.IsSelected = true;
                                    sJVM.IsEnabled = true;
                                    sserver.IsEnabled = false;
                                    MainWindow.serverserver = txb3.Text;
                                    next3.IsEnabled = true;
                                }
                                catch
                                {
                                    return;
                                }*/
                            }
                            catch
                            {
                                MessageBox.Show("安装失败，请查看是否有杀毒软件进行拦截！请确保添加信任或关闭杀毒软件后进行重新安装！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                                downout.Content = "安装失败！";
                            }
                            //RealAction("https://oceansky12337-my.sharepoint.com/personal/makabaka_oceansky12337_onmicrosoft_com/_layouts/52/download.aspx?share=EUxH8cdGAlxOkNiZGunIefEBwrdoMM5wPIb5h9xDpiWd_A");
                            /*
                            Form4 fw = new Form4();
                            fw.ShowDialog();*/
                        }

                    }
                    /*
                sJVM.IsSelected = true;
                sJVM.IsEnabled = true;
                sserver.IsEnabled = false;
                MainWindow.serverserver = txb3.Text;*/
                }
                if (useSelf.IsChecked == true)
                {
                    if (useJVMauto.IsChecked == true)
                    {
                        line = "*|-j " + "\"" + jAva.Text + "\"" + "|-s " + "\"" + server.Text + "\"" + "|-a |-b " + bAse.Text + "|*";
                        FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"MSL2\server.ini", FileMode.Create);
                        StreamWriter sw = new StreamWriter(fs);
                        sw.Write(line);
                        sw.Flush();
                        sw.Close();
                        fs.Close();

                        serverjava = "\"" + jAva.Text + "\"";
                        serverserver = "\"" + server.Text + "\"";
                        serverJVM = "";
                        serverbase = bAse.Text;
                    }
                    else
                    {
                        line = "*|-j " + "\"" + jAva.Text + "\"" + "|-s " + "\"" + server.Text + "\"" + "|-a " + " -Xmx" + memorySlider.Value.ToString("f0") + "M" + "|-b " + bAse.Text + "|*";
                        FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"MSL2\server.ini", FileMode.Create);
                        StreamWriter sw = new StreamWriter(fs);
                        sw.Write(line);
                        sw.Flush();
                        sw.Close();
                        fs.Close();

                        serverjava = "\"" + jAva.Text + "\"";
                        serverserver = "\"" + server.Text + "\"";
                        serverJVM = " -Xmx" + memorySlider.Value.ToString("f0") + "M";
                        serverbase = bAse.Text;
                    }

                    MessageBox.Show("保存完毕！", "信息", MessageBoxButton.OK, MessageBoxImage.Information);
                    doneBtn1.IsEnabled = true;
                }
                if (useJvpath.IsChecked == true)
                {
                    if (useJVMauto.IsChecked == true)
                    {
                        line = "*|-j Java|-s " + "\"" + server.Text + "\"" + "|-a |-b " + bAse.Text + "|*";
                        FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"MSL2\server.ini", FileMode.Create);
                        StreamWriter sw = new StreamWriter(fs);
                        sw.Write(line);
                        sw.Flush();
                        sw.Close();
                        fs.Close();

                        serverjava = "Java";
                        serverserver = "\"" + server.Text + "\"";
                        serverJVM = "";
                        serverbase = bAse.Text;
                    }
                    else
                    {
                        line = "*|-j Java|-s " + "\"" + server.Text + "\"" + "|-a " + " -Xmx" + memorySlider.Value.ToString("f0") + "M" + "|-b " + bAse.Text + "|*";
                        FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"MSL2\server.ini", FileMode.Create);
                        StreamWriter sw = new StreamWriter(fs);
                        sw.Write(line);
                        sw.Flush();
                        sw.Close();
                        fs.Close();

                        serverjava = "Java";
                        serverserver = "\"" + server.Text + "\"";
                        serverJVM = " -Xmx" + memorySlider.Value.ToString("f0") + "M";
                        serverbase = bAse.Text;
                    }

                    MessageBox.Show("保存完毕！", "信息", MessageBoxButton.OK, MessageBoxImage.Information);
                    doneBtn1.IsEnabled = true;
                }
            }
            catch(Exception err)
            {
                MessageBox.Show("出现错误！请重试:\n" + err.Message.ToString(), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                doneBtn1.IsEnabled = true;
            }
        }
        void timer2_Tick(object sender, EventArgs e)
        {
            if (memorySlider.IsEnabled == true)
            {
                memoryInfo.Content = "分配内存：" + memorySlider.Value.ToString("f0") + "M";
            }
        }
        private void timer3_Tick(object sender, EventArgs e)
        {
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"MSL2\" + DownjavaName + @"\bin\java.exe"))
            {
                try
                {
                    if (useJVMauto.IsChecked == true)
                    {
                        line = "*|-j " + "\"" + jAva.Text + "\"" + "|-s " + "\"" + server.Text + "\"" + "|-a |-b " + bAse.Text + "|*";
                        FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"MSL2\server.ini", FileMode.Create);
                        StreamWriter sw = new StreamWriter(fs);
                        sw.Write(line);
                        sw.Flush();
                        sw.Close();
                        fs.Close();

                        serverjava = "\"" + jAva.Text + "\"";
                        serverserver = "\"" + server.Text + "\"";
                        serverJVM = "";
                        serverbase = bAse.Text;
                    }
                    else
                    {
                        line = "*|-j " + "\"" + jAva.Text + "\"" + "|-s " + "\"" + server.Text + "\"" + "|-a " + " -Xmx" + memorySlider.Value.ToString("f0") + "M" + "|-b " + bAse.Text + "|*";
                        FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"MSL2\server.ini", FileMode.Create);
                        StreamWriter sw = new StreamWriter(fs);
                        sw.Write(line);
                        sw.Flush();
                        sw.Close();
                        fs.Close();

                        serverjava = "\"" + jAva.Text + "\"";
                        serverserver = "\"" + server.Text + "\"";
                        serverJVM = " -Xmx" + memorySlider.Value.ToString("f0") + "M";
                        serverbase = bAse.Text;
                    }
                    File.Delete(AppDomain.CurrentDomain.BaseDirectory + @"MSL2\Java.exe");
                    MessageBox.Show("保存完毕！", "信息", MessageBoxButton.OK, MessageBoxImage.Information);
                    downout.Content = "安装成功！";
                    doneBtn1.IsEnabled = true;
                    timer3.Stop();
                }
                catch
                {
                    return;
                }
            }
        }

        private void a01_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog();
            openfile.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            openfile.Title = "请选择文件";
            openfile.Filter = "JAR文件|*.jar|所有文件类型|*.*";
            var res = openfile.ShowDialog();
            if (res == true)
            {
                server.Text = openfile.FileName;
            }
        }

        private void downServer_Click(object sender, RoutedEventArgs e)
        {
            Window wn = new DownloadServer();
            wn.ShowDialog();
            server.Text = serverserver.Replace("\"", "");
        }

        private void a02_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = "请选择文件夹";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                bAse.Text = dialog.SelectedPath;
            }
        }

        private void a03_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog();
            openfile.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            openfile.Title = "请选择文件，通常为java.exe";
            openfile.Filter = "EXE文件|*.exe|所有文件类型|*.*";
            var res = openfile.ShowDialog();
            if (res == true)
            {
                jAva.Text = openfile.FileName;
            }
        }
        private void useJVMauto_Click(object sender, RoutedEventArgs e)
        {
            if (useJVMauto.IsChecked == true)
            {
                memorySlider.IsEnabled = false;
                useJVMself.IsChecked = false;
            }
            else
            {
                useJVMauto.IsChecked = true;
            }
        }

        private void useJVMself_Click(object sender, RoutedEventArgs e)
        {
            if (useJVMself.IsChecked == true)
            {
                memorySlider.IsEnabled = true;
                useJVMauto.IsChecked = false;
            }
            else
            {
                useJVMself.IsChecked = true;
            }
        }
        private void tabcontrol1_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
                try
                {
                    server.Text = serverserver.Replace("\"", "");
                    //jVM.Text = serverJVM;
                    memorySlider.Maximum = PhisicalMemory / 1024.0 / 1024.0;
                    bAse.Text = serverbase;
                    jAva.Text = serverjava.Replace("\"", "");
                    if (jAva.Text == "Java")
                    {
                        useJvpath.IsChecked = true;
                    }
                    if (jAva.Text == AppDomain.CurrentDomain.BaseDirectory + @"MSL2\Java8\bin\java.exe")
                    {
                        useJv8.IsChecked = true;
                    }
                    if (jAva.Text == AppDomain.CurrentDomain.BaseDirectory + @"MSL2\Java16\bin\java.exe")
                    {
                        useJv16.IsChecked = true;
                    }
                    if (jAva.Text == AppDomain.CurrentDomain.BaseDirectory + @"MSL2\Java17\bin\java.exe")
                    {
                        useJv17.IsChecked = true;
                    }
                    if (serverJVM == "")
                    {
                        memorySlider.IsEnabled = false;
                        useJVMself.IsChecked = false;
                        useJVMauto.IsChecked = true;
                        memoryInfo.Content = "现在为自动分配";
                    }
                    else
                    {
                        memorySlider.IsEnabled = true;
                        useJVMauto.IsChecked = false;
                        useJVMself.IsChecked = true;
                        int IndexofA6 = serverJVM.IndexOf("-Xmx");
                        string Ru6 = serverJVM.Substring(IndexofA6 + 4);
                        string a600 = Ru6.Substring(0, Ru6.IndexOf("M"));

                        memorySlider.Value = int.Parse(a600);
                        memoryInfo.Content = "分配内存：" + a600 + "M";
                    }
                    timer2.Tick += new EventHandler(timer2_Tick);
                    timer2.Interval = TimeSpan.FromMilliseconds(100);
                    timer2.Start();
                }
                catch
                {
                    var mb = MessageBox.Show("开服器检测到您可能没有创建服务器或配置文件出现了错误，是否重新配置服务器？", "ERROR", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (mb == MessageBoxResult.Yes)
                    {
                        Window wn = new CreateServer();
                        wn.ShowDialog();
                    }
                }
            }
            if (tabcontrol1.SelectedIndex == 3)
            {
                timer2.Stop();
            }
            if (tabcontrol1.SelectedIndex == 4)
            {
                timer2.Stop();
                /*
                try
                {
                    if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"MSL2\frpc"))
                    {
                        if (frpc != null)
                        {
                            frplab1.Content = "您的内网映射已就绪，请点击“启动内网映射”来开启";
                            frplab3.Content = frpc;
                            copyFrpc.IsEnabled = true;
                            startfrpc.IsEnabled = true;
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("出现错误，请重试:" + "m0x3");
                }*/
            }
            if (tabcontrol1.SelectedIndex == 5)
            {
                timer2.Stop();
            }
        }

        private void setServerconfig_Click(object sender, RoutedEventArgs e)
        {

        }
        private void StartFrpc()
        {
            try
            {
                FRPCMD.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + @"MSL2\frpc.exe";
                //CmdProcess.StartInfo.FileName = StartFileName;
                FRPCMD.StartInfo.Arguments = "-c frpc";
                Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory + "MSL2");
                FRPCMD.StartInfo.CreateNoWindow = true;
                FRPCMD.StartInfo.UseShellExecute = false;
                FRPCMD.StartInfo.RedirectStandardInput = true;
                FRPCMD.StartInfo.RedirectStandardOutput = true;
                FRPCMD.OutputDataReceived += new DataReceivedEventHandler(p_OutputDataReceived1);
                FRPCMD.Start();
                FRPCMD.BeginOutputReadLine();
            }
            catch (Exception e)
            {
                MessageBox.Show("出现错误，请重试:" + "r0x1" + e, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
        }
        private void p_OutputDataReceived1(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                Dispatcher.Invoke(ReadStdOutput1, new object[] { e.Data });
            }
        }
        private void ReadStdOutputAction1(string msg)
        {
            frpcOutlog.Text = frpcOutlog.Text + msg + "\n";
            if (msg.IndexOf("login") + 1 != 0)
            {
                if (msg.IndexOf("failed") + 1 != 0)
                {
                    frpcOutlog.Text = frpcOutlog.Text + "内网映射桥接失败！\n";
                    try
                    {
                        FRPCMD.Kill();
                        FRPCMD.CancelOutputRead();
                        //ReadStdOutput = null;
                        FRPCMD.OutputDataReceived -= new DataReceivedEventHandler(p_OutputDataReceived1);
                        setfrpc.IsEnabled = true;
                        startfrpc.Content = "启动内网映射";
                    }
                    catch
                    {
                        try
                        {
                            FRPCMD.CancelOutputRead();
                            //ReadStdOutput = null;
                            FRPCMD.OutputDataReceived -= new DataReceivedEventHandler(p_OutputDataReceived1);
                            setfrpc.IsEnabled = true;
                            startfrpc.Content = "启动内网映射";
                        }
                        catch
                        {
                            setfrpc.IsEnabled = true;
                            startfrpc.Content = "启动内网映射";
                        }
                    }
                }
                if (msg.IndexOf("success") + 1 != 0)
                {
                    frpcOutlog.Text = frpcOutlog.Text + "登录服务器成功！\n";
                }
                if (msg.IndexOf("match") + 1 != 0)
                {
                    if (msg.IndexOf("token") + 1 != 0)
                    {
                        frpcOutlog.Text = frpcOutlog.Text + "重新连接服务器...\n";
                        string frpcserver = frpc.Substring(0, frpc.IndexOf(".")) + "*";
                        int frpcserver1 = frpc.IndexOf(".") + 1;
                        WebClient MyWebClient = new WebClient();
                        MyWebClient.Credentials = CredentialCache.DefaultCredentials;
                        byte[] pageData = MyWebClient.DownloadData("http://115.220.5.81:8081/web/frpcserver.txt");
                        string pageHtml1 = Encoding.UTF8.GetString(pageData);

                        try
                        {
                            int IndexofA = pageHtml1.IndexOf(frpcserver);
                            string Ru = pageHtml1.Substring(IndexofA + frpcserver1);
                            string a111 = Ru.Substring(0, Ru.IndexOf("*"));
                            //MessageBox.Show(a111);

                            WebClient MyWebClient1 = new WebClient();
                            MyWebClient1.Credentials = CredentialCache.DefaultCredentials;
                            byte[] pageData1 = MyWebClient1.DownloadData(a111);
                            string pageHtml = Encoding.UTF8.GetString(pageData1);
                            string decode = DecodeBase64(pageHtml);
                            string aaa = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"MSL2\frpc");
                            int IndexofA2 = aaa.IndexOf("token=");
                            string Ru2 = aaa.Substring(IndexofA2);
                            string a200 = Ru2.Substring(0, Ru2.IndexOf("\n"));
                            aaa = aaa.Replace(a200, "token=" + decode);
                            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"MSL2\frpc", aaa);
                            FRPCMD.CancelOutputRead();
                            //ReadStdOutput = null;
                            FRPCMD.OutputDataReceived -= new DataReceivedEventHandler(p_OutputDataReceived1);
                            StartFrpc();
                        }
                        catch
                        {
                            MessageBox.Show("内网映射桥接失败！请重试！\n错误代码：" + "Mw000x0001", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            try
                            {
                                FRPCMD.CancelOutputRead();
                                //ReadStdOutput = null;
                                FRPCMD.OutputDataReceived -= new DataReceivedEventHandler(p_OutputDataReceived1);
                                setfrpc.IsEnabled = true;
                                startfrpc.Content = "启动内网映射";
                            }
                            catch
                            {
                                setfrpc.IsEnabled = true;
                                startfrpc.Content = "启动内网映射";
                            }

                        }
                    }
                }
            }
            if (msg.IndexOf("reconnect") + 1 != 0)
            {
                if (msg.IndexOf("error") + 1 != 0)
                {
                    if (msg.IndexOf("token") + 1 != 0)
                    {
                        frpcOutlog.Text = frpcOutlog.Text + "重新连接服务器...\n";
                        string frpcserver = frpc.Substring(0, frpc.IndexOf(".")) + "*";
                        int frpcserver1 = frpc.IndexOf(".") + 1;
                        WebClient MyWebClient = new WebClient();
                        MyWebClient.Credentials = CredentialCache.DefaultCredentials;
                        byte[] pageData = MyWebClient.DownloadData("http://115.220.5.81:8081/web/frpcserver.txt");
                        string pageHtml1 = Encoding.UTF8.GetString(pageData);

                        try
                        {
                            int IndexofA = pageHtml1.IndexOf(frpcserver);
                            string Ru = pageHtml1.Substring(IndexofA + frpcserver1);
                            string a111 = Ru.Substring(0, Ru.IndexOf("*"));
                            //MessageBox.Show(a111);

                            WebClient MyWebClient1 = new WebClient();
                            MyWebClient1.Credentials = CredentialCache.DefaultCredentials;
                            byte[] pageData1 = MyWebClient1.DownloadData(a111);
                            string pageHtml = Encoding.UTF8.GetString(pageData1);
                            string decode = DecodeBase64(pageHtml);
                            string aaa = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"MSL2\frpc");
                            int IndexofA2 = aaa.IndexOf("token=");
                            string Ru2 = aaa.Substring(IndexofA2);
                            string a200 = Ru2.Substring(0, Ru2.IndexOf("\n"));
                            aaa = aaa.Replace(a200, "token=" + decode);
                            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"MSL2\frpc", aaa);
                            FRPCMD.CancelOutputRead();
                            //ReadStdOutput = null;
                            FRPCMD.OutputDataReceived -= new DataReceivedEventHandler(p_OutputDataReceived1);
                            StartFrpc();
                        }
                        catch
                        {
                            MessageBox.Show("内网映射桥接失败！请重试！\n错误代码：" + "Mw000x0001", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            try
                            {
                                FRPCMD.CancelOutputRead();
                                //ReadStdOutput = null;
                                FRPCMD.OutputDataReceived -= new DataReceivedEventHandler(p_OutputDataReceived1);
                                setfrpc.IsEnabled = true;
                                startfrpc.Content = "启动内网映射";
                            }
                            catch
                            {
                                setfrpc.IsEnabled = true;
                                startfrpc.Content = "启动内网映射";
                            }

                        }

                    }
                }
            }
            if (msg.IndexOf("start") + 1 != 0)
            {
                if (msg.IndexOf("success") + 1 != 0)
                {
                    frpcOutlog.Text = frpcOutlog.Text + "内网映射桥接成功！\n";
                }
                if (msg.IndexOf("error") + 1 != 0)
                {
                    frpcOutlog.Text = frpcOutlog.Text + "内网映射桥接失败！\n";
                    try
                    {
                        FRPCMD.Kill();
                        FRPCMD.CancelOutputRead();
                        //ReadStdOutput = null;
                        FRPCMD.OutputDataReceived -= new DataReceivedEventHandler(p_OutputDataReceived1);
                        setfrpc.IsEnabled = true;
                        startfrpc.Content = "启动内网映射";
                    }
                    catch
                    {
                        try
                        {
                            FRPCMD.CancelOutputRead();
                            //ReadStdOutput = null;
                            FRPCMD.OutputDataReceived -= new DataReceivedEventHandler(p_OutputDataReceived1);
                            setfrpc.IsEnabled = true;
                            startfrpc.Content = "启动内网映射";
                        }
                        catch
                        {
                            setfrpc.IsEnabled = true;
                            startfrpc.Content = "启动内网映射";
                        }
                    }
                    if (msg.IndexOf("port already used") + 1 != 0)
                    {
                        frpcOutlog.Text = frpcOutlog.Text + "远程端口被占用，请重新配置！\n";
                    }
                    if (msg.IndexOf("port not allowed") + 1 != 0)
                    {
                        frpcOutlog.Text = frpcOutlog.Text + "端口被占用，请重新配置！\n";
                    }
                    if (msg.IndexOf("proxy name") + 1 != 0)
                    {
                        if (msg.IndexOf("already in use") + 1 != 0)
                        {
                            frpcOutlog.Text = frpcOutlog.Text + "此QQ号已被占用！请重启电脑再试或联系作者！\n";
                        }
                    }
                    //frpcOutlog.Text = frpcOutlog.Text + "\n端口被占用！请检查是否有进程占用端口或重新配置内网映射！\n";
                }
            }
            frpcOutlog.ScrollToEnd();
        }

        private void setfrpc_Click(object sender, RoutedEventArgs e)
        {
            Window fw = new SetFrpc();
            fw.ShowDialog();
            try
            {
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"MSL2\frpc"))
                {
                    if (frpc != null)
                    {
                        frplab1.Content = "您的内网映射已就绪，请点击“启动内网映射”来开启";
                        frplab3.Content = frpc;
                        copyFrpc.IsEnabled = true;
                        startfrpc.IsEnabled = true;
                    }
                }
            }
            catch
            {
                MessageBox.Show("出现错误，请重试:" + "m0x3");
            }
        }

        private void startfrpc_Click(object sender, RoutedEventArgs e)
        {
            if (startfrpc.Content.ToString() == "启动内网映射")
            {
                StartFrpc();
                setfrpc.IsEnabled = false;
                startfrpc.Content = "关闭内网映射";
                frpcOutlog.Text = "";
            }
            else
            {
                try
                {
                    FRPCMD.Kill();
                    FRPCMD.CancelOutputRead();
                    //ReadStdOutput = null;
                    FRPCMD.OutputDataReceived -= new DataReceivedEventHandler(p_OutputDataReceived1);
                    setfrpc.IsEnabled = true;
                    startfrpc.Content = "启动内网映射";
                }
                catch
                {
                    try
                    {
                        FRPCMD.CancelOutputRead();
                        //ReadStdOutput = null;
                        FRPCMD.OutputDataReceived -= new DataReceivedEventHandler(p_OutputDataReceived1);
                        setfrpc.IsEnabled = true;
                        startfrpc.Content = "启动内网映射";
                    }
                    catch
                    {
                        setfrpc.IsEnabled = true;
                        startfrpc.Content = "启动内网映射";
                    }
                }
            }
        }

        private void copyFrpc_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetDataObject(frpc);
        }
        private void setdefault_Click(object sender, RoutedEventArgs e)
        {
            //File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"MSL2\config.json", String.Format("{{{0}}}", "\n\"server\": \"\",\n\"java\": \"\",\n\"b\": \"\",\n\"frpc\": \"\",\n\"bcolor\": \"\",\n\"fcolor\": \"\",\n\"notifyIcon\": \"true\"\n"));
            try
            {
                Directory.Delete(AppDomain.CurrentDomain.BaseDirectory + @"MSL2", true);
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"MSL2");
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"MSL2\config.json", String.Format("{{{0}}}", "\n\"frpc\": \"\",\n\"notifyIcon\": \"True\",\n\"notice\": \"0\",\n\"frpcversion\": \"1\"\n"));
            }
            catch
            {
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"MSL2\config.json", String.Format("{{{0}}}", "\n\"frpc\": \"\",\n\"notifyIcon\": \"True\",\n\"notice\": \"0\",\n\"frpcversion\": \"1\"\n"));
            }
            Process.Start(Application.ResourceAssembly.Location);
            Process.GetCurrentProcess().Kill();
        }
    }
}
