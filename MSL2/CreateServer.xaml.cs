using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MSL2
{
    /// <summary>
    /// CreateServer.xaml 的交互逻辑
    /// </summary>
    public partial class CreateServer : Window
    {
        string DownjavaName;
        bool safeClose = false;
        //public static Process CmdProcess = new Process();
        DispatcherTimer timer1 = new DispatcherTimer();
        public CreateServer()
        {
            InitializeComponent();
        }
        private void next3_Click(object sender, RoutedEventArgs e)
        {
            next3.IsEnabled = false;
            return1.IsEnabled = false;
            if (usejv8.IsChecked == true)
            {
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"MSL2\Java8\bin\java.exe"))
                {
                    MainWindow.serverjava = "\"" + AppDomain.CurrentDomain.BaseDirectory + @"MSL2\Java8\bin\java.exe" + "\"";
                    MainWindow.serverserver = "\"" + txb3.Text + "\"";
                    next3.IsEnabled = true;
                    return1.IsEnabled = true;
                    javagrid.Visibility = Visibility.Hidden;
                    servergrid.Visibility = Visibility.Visible;
                    label3.Visibility = Visibility.Visible;
                    downloadjava.Visibility = Visibility.Visible;
                    selectjava.Visibility = Visibility.Visible;
                    return2.Visibility = Visibility.Visible;
                }
                else
                {
                    MessageBox.Show("下载Java即代表您接受Java的服务条款https://www.oracle.com/downloads/licenses/javase-license1.html", "INFO", MessageBoxButton.OK, MessageBoxImage.Information);
                    if (Environment.Is64BitOperatingSystem)
                    {
                        DownjavaName = "Java8";
                        DownloadWindow.downloadurl = "https://oceansky12337-my.sharepoint.com/personal/makabaka_oceansky12337_onmicrosoft_com/_layouts/52/download.aspx?share=Ecs65caK7blGgZipDS1d76IBKDID3YUy9ak-HUzY_vDQUQ";
                        DownloadWindow.downloadPath = AppDomain.CurrentDomain.BaseDirectory + @"MSL2";
                        DownloadWindow.filename = "Java.exe";
                        DownloadWindow.downloadinfo = "下载Java8中……";
                        Window window = new DownloadWindow();
                        window.ShowDialog();
                        try
                        {
                            Process CmdProcess = new Process();
                            CmdProcess.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + @"MSL2\Java.exe";
                            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory + "MSL2");
                            CmdProcess.Start();
                            timer1.Tick += new EventHandler(timer1_Tick);
                            timer1.Interval = TimeSpan.FromSeconds(3);
                            timer1.Start();
                            outlog.Content = "安装中...";
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
                            next3.IsEnabled = true;
                            return1.IsEnabled = true;
                            outlog.Content = "安装失败！";
                        }
                        /*
                        Form4 fw = new Form4();
                        fw.ShowDialog();*/
                    }
                    else
                    {
                        MessageBox.Show("下载Java即代表您接受Java的服务条款https://www.oracle.com/downloads/licenses/javase-license1.html", "INFO", MessageBoxButton.OK, MessageBoxImage.Information);
                        DownjavaName = "Java8";
                        DownloadWindow.downloadurl = "https://oceansky12337-my.sharepoint.com/personal/makabaka_oceansky12337_onmicrosoft_com/_layouts/52/download.aspx?share=ES74HP6tN6dKuyTPUVOfEaYBJAecYATfZKXahAN_EZDC8Q";
                        DownloadWindow.downloadPath = AppDomain.CurrentDomain.BaseDirectory + @"MSL2";
                        DownloadWindow.filename = "Java.exe";
                        DownloadWindow.downloadinfo = "下载Java8中……";
                        Window window = new DownloadWindow();
                        window.ShowDialog();
                        try
                        {
                            Process CmdProcess = new Process();
                            CmdProcess.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + @"MSL2\Java.exe";
                            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory + "MSL2");
                            CmdProcess.Start();
                            timer1.Tick += new EventHandler(timer1_Tick);
                            timer1.Interval = TimeSpan.FromSeconds(3);
                            timer1.Start();
                            outlog.Content = "安装中...";
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
                            next3.IsEnabled = true;
                            return1.IsEnabled = true;
                            outlog.Content = "安装失败！";
                        }
                        /*
                        Form4 fw = new Form4();
                        fw.ShowDialog();*/
                    }
                    //MainWindow.serverjava = "\"" + AppDomain.CurrentDomain.BaseDirectory + @"MSL2\Java8\bin\java.exe" + "\"";
                    //MainWindow.serverserver = "\"" + txb3.Text + "\"";
                }

                /*
                sJVM.IsSelected = true;
                sJVM.IsEnabled = true;
                sserver.IsEnabled = false;
                MainWindow.serverserver = txb3.Text;*/
            }
            if (usejv16.IsChecked == true)
            {
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"MSL2\Java16\bin\java.exe"))
                {
                    MainWindow.serverjava = "\"" + AppDomain.CurrentDomain.BaseDirectory + @"MSL2\Java16\bin\java.exe" + "\"";
                    MainWindow.serverserver = "\"" + txb3.Text + "\"";
                    next3.IsEnabled = true;
                    return1.IsEnabled = true;
                    javagrid.Visibility = Visibility.Hidden;
                    servergrid.Visibility = Visibility.Visible;
                    label3.Visibility = Visibility.Visible;
                    downloadjava.Visibility = Visibility.Visible;
                    selectjava.Visibility = Visibility.Visible;
                    return2.Visibility = Visibility.Visible;
                }
                else
                {
                    MessageBox.Show("下载Java即代表您接受Java的服务条款https://www.oracle.com/downloads/licenses/javase-license1.html", "INFO", MessageBoxButton.OK, MessageBoxImage.Information);
                    DownjavaName = "Java16";
                    DownloadWindow.downloadurl = "https://oceansky12337-my.sharepoint.com/personal/makabaka_oceansky12337_onmicrosoft_com/_layouts/52/download.aspx?share=EbapBNLCCwRLoFr2kxeCUdcBYNtGdsQO2h1MlzgFU3VZbQ";
                    DownloadWindow.downloadPath = AppDomain.CurrentDomain.BaseDirectory + @"MSL2";
                    DownloadWindow.filename = "Java.exe";
                    DownloadWindow.downloadinfo = "下载Java16中……";
                    Window window = new DownloadWindow();
                    window.ShowDialog();
                    try
                    {
                        Process CmdProcess = new Process();
                        CmdProcess.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + @"MSL2\Java.exe";
                        Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory + "MSL2");
                        CmdProcess.Start();
                        timer1.Tick += new EventHandler(timer1_Tick);
                        timer1.Interval = TimeSpan.FromSeconds(3);
                        timer1.Start();
                        outlog.Content = "安装中...";
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
                        next3.IsEnabled = true;
                        return1.IsEnabled = true;
                        outlog.Content = "安装失败！";
                    }
                    /*
                    Form4 fw = new Form4();
                    fw.ShowDialog();*/
                    //MainWindow.serverjava = "\"" + AppDomain.CurrentDomain.BaseDirectory + @"MSL2\Java16\bin\java.exe" + "\"";
                }/*
                sJVM.IsSelected = true;
                sJVM.IsEnabled = true;
                sserver.IsEnabled = false;
                MainWindow.serverserver = txb3.Text;*/
            }
            if (usejv17.IsChecked == true)
            {
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"MSL2\Java17\bin\java.exe"))
                {
                    MainWindow.serverjava = "\"" + AppDomain.CurrentDomain.BaseDirectory + @"MSL2\Java17\bin\java.exe" + "\"";
                    MainWindow.serverserver = "\"" + txb3.Text + "\"";
                    next3.IsEnabled = true;
                    return1.IsEnabled = true;
                    javagrid.Visibility = Visibility.Hidden;
                    servergrid.Visibility = Visibility.Visible;
                    label3.Visibility = Visibility.Visible;
                    downloadjava.Visibility = Visibility.Visible;
                    selectjava.Visibility = Visibility.Visible;
                    return2.Visibility = Visibility.Visible;
                }
                else
                {
                    MessageBox.Show("下载Java即代表您接受Java的服务条款https://www.oracle.com/downloads/licenses/javase-license1.html", "INFO", MessageBoxButton.OK, MessageBoxImage.Information);
                    DownjavaName = "Java17";
                    DownloadWindow.downloadurl = "https://oceansky12337-my.sharepoint.com/personal/makabaka_oceansky12337_onmicrosoft_com/_layouts/52/download.aspx?share=EUxH8cdGAlxOkNiZGunIefEBwrdoMM5wPIb5h9xDpiWd_A";
                    DownloadWindow.downloadPath = AppDomain.CurrentDomain.BaseDirectory + @"MSL2";
                    DownloadWindow.filename = "Java.exe";
                    DownloadWindow.downloadinfo = "下载Java17中……";
                    Window window = new DownloadWindow();
                    window.ShowDialog();
                    try
                    {
                        Process CmdProcess = new Process();
                        CmdProcess.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + @"MSL2\Java.exe";
                        Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory + "MSL2");
                        CmdProcess.Start();
                        timer1.Tick += new EventHandler(timer1_Tick);
                        timer1.Interval = TimeSpan.FromSeconds(3);
                        timer1.Start();
                        outlog.Content = "安装中...";
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
                    catch (Exception aaa)
                    {
                        MessageBox.Show("安装失败，请查看是否有杀毒软件进行拦截！请确保添加信任或关闭杀毒软件后进行重新安装！\n" + aaa, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                        next3.IsEnabled = true;
                        return1.IsEnabled = true;
                        outlog.Content = "安装失败！";
                    }
                    /*
                    Form4 fw = new Form4();
                    fw.ShowDialog();*/
                    //MainWindow.serverjava = "\"" + AppDomain.CurrentDomain.BaseDirectory + @"MSL2\Java17\bin\java.exe" + "\"";
                }
                /*
            sJVM.IsSelected = true;
            sJVM.IsEnabled = true;
            sserver.IsEnabled = false;
            MainWindow.serverserver = txb3.Text;*/
            }
            if (useJVself.IsChecked == true)
            {
                MainWindow.serverjava = "\"" + txjava.Text + "\"";
                MainWindow.serverserver = "\"" + txb3.Text + "\"";
                next3.IsEnabled = true;
                return1.IsEnabled = true;
                javagrid.Visibility = Visibility.Hidden;
                servergrid.Visibility = Visibility.Visible;
                label3.Visibility = Visibility.Visible;
                downloadjava.Visibility = Visibility.Visible;
                selectjava.Visibility = Visibility.Visible;
                return2.Visibility = Visibility.Visible;
            }
            if (usejvPath.IsChecked == true)
            {
                MainWindow.serverjava = "Java";
                MainWindow.serverserver = "\"" + txb3.Text + "\"";
                next3.IsEnabled = true;
                return1.IsEnabled = true;
                javagrid.Visibility = Visibility.Hidden;
                servergrid.Visibility = Visibility.Visible;
                label3.Visibility = Visibility.Visible;
                downloadjava.Visibility = Visibility.Visible;
                selectjava.Visibility = Visibility.Visible;
                return2.Visibility = Visibility.Visible;
            }
        }

        private void next4_Click(object sender, RoutedEventArgs e)
        {
            sserverbase.IsSelected = true;
            sserverbase.IsEnabled = true;
            sJVM.IsEnabled = false;
            if (usedefault.IsChecked == true)
            {
                MainWindow.serverJVM = "";
            }
            else
            {
                MainWindow.serverJVM = "-Xms" + txb4.Text + "M -Xmx" + txb5.Text + "M";
            }
            if (usebasicfastJvm.IsChecked == true)
            {
                MainWindow.serverJVMcmd = "-XX:+AggressiveOpts";
            }
            if (usefastJvm.IsChecked == true)
            {
                MainWindow.serverJVMcmd = "-XX:+UseG1GC -XX:+UnlockExperimentalVMOptions -XX:+ParallelRefProcEnabled -XX:MaxGCPauseMillis=200 -XX:+UnlockExperimentalVMOptions -XX:+DisableExplicitGC -XX:+AlwaysPreTouch -XX:G1NewSizePercent=30 -XX:G1MaxNewSizePercent=40 -XX:G1HeapRegionSize=8M -XX:G1ReservePercent=20 -XX:G1HeapWastePercent=5 -XX:G1MixedGCCountTarget=4 -XX:InitiatingHeapOccupancyPercent=15 -XX:G1MixedGCLiveThresholdPercent=90 -XX:G1RSetUpdatingPauseTimePercent=5 -XX:SurvivorRatio=32 -XX:+PerfDisableSharedMem -XX:MaxTenuringThreshold=1 -Dusing.aikars.flags=https://mcflags.emc.gs -Daikars.new.flags=true";
            }
        }

        private void usedefault_Checked(object sender, RoutedEventArgs e)
        {
            if (this.IsLoaded)
            {
                txb4.IsEnabled = false;
                txb5.IsEnabled = false;
            }
        }

        private void useJVM_Checked(object sender, RoutedEventArgs e)
        {
            txb4.IsEnabled = true;
            txb5.IsEnabled = true;
        }

        private void done_Click(object sender, RoutedEventArgs e)
        {
            safeClose = true;
            MainWindow.serverbase = txb6.Text;
            try
            {
                Directory.CreateDirectory(MainWindow.serverbase);
                StreamWriter sw = File.AppendText(AppDomain.CurrentDomain.BaseDirectory + @"MSL2\server.ini");
                sw.Write("*|-j " + "\"" + MainWindow.serverjava + "\"" + "|-s " + "\"" + MainWindow.serverserver + "\"" + "|-a " + MainWindow.serverJVM + "|-b " + MainWindow.serverbase + "|-c " + MainWindow.serverJVMcmd + "|*");
                sw.Flush();
                sw.Close();
                sw.Dispose();
                MessageBox.Show("创建完毕，请点击“开启服务器”按钮以开服", "信息", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            catch
            {
                MessageBox.Show("出现错误，请重试" + "c0x1", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void return3_Click(object sender, RoutedEventArgs e)
        {
            sserver.IsSelected = true;
            sserver.IsEnabled = true;
            sJVM.IsEnabled = false;
            label3.Visibility = Visibility.Visible;
            downloadjava.Visibility = Visibility.Visible;
            selectjava.Visibility = Visibility.Visible;
            label5.Visibility = Visibility.Hidden;
            usejv8.Visibility = Visibility.Hidden;
            usejv16.Visibility = Visibility.Hidden;
            usejv17.Visibility = Visibility.Hidden;
            outlog.Visibility = Visibility.Hidden;
            jvhelp.Visibility = Visibility.Hidden;
            label4.Visibility = Visibility.Hidden;
            usejvPath.Visibility = Visibility.Hidden;
            useJVself.Visibility = Visibility.Hidden;
            txjava.Visibility = Visibility.Hidden;
            a0002_Copy.Visibility = Visibility.Hidden;
            next3.Visibility = Visibility.Hidden;
        }

        private void a0002_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog();
            openfile.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            openfile.Title = "请选择文件";
            openfile.Filter = "JAR文件|*.jar|所有文件类型|*.*";
            var res = openfile.ShowDialog();
            if (res == true)
            {
                txb3.Text = openfile.FileName;
            }
        }

        private void a0003_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = "请选择文件夹";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txb6.Text = dialog.SelectedPath;
            }
        }

        private void return4_Click(object sender, RoutedEventArgs e)
        {
            sJVM.IsSelected = true;
            sJVM.IsEnabled = true;
            sserverbase.IsEnabled = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!Environment.Is64BitOperatingSystem)
            {
                jvhelp.Content = "您的电脑为32为系统，暂时只能使用Java8，感谢您的理解！";
                usejv16.IsEnabled = false;
                usejv17.IsEnabled = false;
            }
        }

        private void a0002_Copy_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog();
            openfile.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            openfile.Title = "请选择文件，通常为java.exe";
            openfile.Filter = "EXE文件|*.exe|所有文件类型|*.*";
            var res = openfile.ShowDialog();
            if (res == true)
            {
                txjava.Text = openfile.FileName;
            }
        }

        private void useJVself_Checked(object sender, RoutedEventArgs e)
        {
            txjava.IsEnabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"MSL2\" + DownjavaName + @"\bin\java.exe"))
            {
                //CmdProcess.CancelOutputRead();
                //ReadStdOutput = null;
                //CmdProcess.OutputDataReceived -= new DataReceivedEventHandler(p_OutputDataReceived);
                try
                {
                    File.Delete(AppDomain.CurrentDomain.BaseDirectory + @"MSL2\Java.exe");
                    outlog.Content = "完成";
                    MainWindow.serverjava = AppDomain.CurrentDomain.BaseDirectory + @"MSL2\" + DownjavaName + @"\bin\java.exe";
                    MainWindow.serverserver = "\"" + txb3.Text + "\"";
                    next3.IsEnabled = true;
                    return1.IsEnabled = true;
                    javagrid.Visibility = Visibility.Hidden;
                    servergrid.Visibility = Visibility.Visible;
                    label3.Visibility = Visibility.Visible;
                    downloadjava.Visibility = Visibility.Visible;
                    selectjava.Visibility = Visibility.Visible;
                    return2.Visibility = Visibility.Visible;
                    timer1.Stop();
                }
                catch
                {
                    return;
                }
            }
        }

        private void next_Click(object sender, RoutedEventArgs e)
        {
            sserver.IsSelected = true;
            sserver.IsEnabled = true;
            welcome.IsEnabled = false;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (safeClose == false)
            {
                Process.GetCurrentProcess().Kill();
            }
        }

        private void usejvPath_Checked(object sender, RoutedEventArgs e)
        {
            if (this.IsLoaded)
            {
                txjava.IsEnabled = false;
            }
        }

        private void usejv8_Checked(object sender, RoutedEventArgs e)
        {
            if (this.IsLoaded)
            {
                next3.Visibility = Visibility.Visible;
                txjava.IsEnabled = false;
            }
        }

        private void usejv16_Checked(object sender, RoutedEventArgs e)
        {
            if (this.IsLoaded)
            {
                next3.Visibility = Visibility.Visible;
                txjava.IsEnabled = false;
            }
        }

        private void usejv17_Checked(object sender, RoutedEventArgs e)
        {
            if (this.IsLoaded)
            {
                next3.Visibility = Visibility.Visible;
                txjava.IsEnabled = false;
            }
        }

        private void downloadserver_Click(object sender, RoutedEventArgs e)
        {
            Window wn = new DownloadServer();
            wn.ShowDialog();
            txb3.Text = MainWindow.serverserver.Replace("\"", "");
            sJVM.IsSelected = true;
            sJVM.IsEnabled = true;
            sserver.IsEnabled = false;
            next3.IsEnabled = true;
            return1.IsEnabled = true;
        }

        private void selectserver_Click(object sender, RoutedEventArgs e)
        {
            downloadserver.Visibility = Visibility.Hidden;
            selectserver.Visibility = Visibility.Hidden;
            label1.Visibility = Visibility.Hidden;
            return2.Visibility = Visibility.Visible;
            txb3.Visibility = Visibility.Visible;
            next2.Visibility = Visibility.Visible;
            a0002.Visibility = Visibility.Visible;
            label2.Visibility = Visibility.Visible;
        }

        private void next2_Click(object sender, RoutedEventArgs e)
        {
            sJVM.IsSelected = true;
            sJVM.IsEnabled = true;
            sserver.IsEnabled = false;
            next3.IsEnabled = true;
            return1.IsEnabled = true;
        }

        private void selectjava_Click(object sender, RoutedEventArgs e)
        {
            label4.Visibility = Visibility.Visible;
            usejvPath.Visibility = Visibility.Visible;
            useJVself.Visibility = Visibility.Visible;
            txjava.Visibility = Visibility.Visible;
            a0002_Copy.Visibility = Visibility.Visible;
            next3.Visibility = Visibility.Visible;
            return1.Visibility = Visibility.Visible;
            label3.Visibility = Visibility.Hidden;
            downloadjava.Visibility = Visibility.Hidden;
            selectjava.Visibility = Visibility.Hidden;
        }

        private void downloadjava_Click(object sender, RoutedEventArgs e)
        {
            label5.Visibility = Visibility.Visible;
            usejv8.Visibility = Visibility.Visible;
            usejv16.Visibility = Visibility.Visible;
            usejv17.Visibility = Visibility.Visible;
            outlog.Visibility = Visibility.Visible;
            jvhelp.Visibility = Visibility.Visible;
            next3.Visibility = Visibility.Visible;
            return1.Visibility = Visibility.Visible;
            label3.Visibility = Visibility.Hidden;
            downloadjava.Visibility = Visibility.Hidden;
            selectjava.Visibility = Visibility.Hidden;
        }

        private void return1_Click(object sender, RoutedEventArgs e)
        {
            downloadserver.Visibility = Visibility.Visible;
            selectserver.Visibility = Visibility.Visible;
            label1.Visibility = Visibility.Visible;
            javagrid.Visibility = Visibility.Visible;
            label3.Visibility = Visibility.Visible;
            downloadjava.Visibility = Visibility.Visible;
            selectjava.Visibility = Visibility.Visible;
            return1.Visibility = Visibility.Visible;
            servergrid.Visibility = Visibility.Hidden;
            label5.Visibility = Visibility.Hidden;
            usejv8.Visibility = Visibility.Hidden;
            usejv16.Visibility = Visibility.Hidden;
            usejv17.Visibility = Visibility.Hidden;
            outlog.Visibility = Visibility.Hidden;
            jvhelp.Visibility = Visibility.Hidden;
            label4.Visibility = Visibility.Hidden;
            return1.Visibility = Visibility.Hidden;
            return2.Visibility = Visibility.Hidden;
            usejvPath.Visibility = Visibility.Hidden;
            useJVself.Visibility = Visibility.Hidden;
            txjava.Visibility = Visibility.Hidden;
            a0002_Copy.Visibility = Visibility.Hidden;
            next3.Visibility = Visibility.Hidden;
            next2.Visibility = Visibility.Hidden;
            txb3.Visibility = Visibility.Hidden;
            next2.Visibility = Visibility.Hidden;
            a0002.Visibility = Visibility.Hidden;
            label2.Visibility = Visibility.Hidden;
        }

        private void skip_Click(object sender, RoutedEventArgs e)
        {
            safeClose = true;
            try
            {
                File.Create(AppDomain.CurrentDomain.BaseDirectory + @"MSL2\server.ini");
                Close();
            }
            catch
            {
                MessageBox.Show("出现错误，请重试" + "c0x1", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void usebasicfastJvm_Checked(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("使用优化参数需要手动设置大小相同的内存，请对上面的内存进行更改！", "警告", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            useJVM.IsChecked = true;
            txb4.Text = "1024";
            txb5.Text = "1024";
        }
        private void usefastJvm_Checked(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("使用优化参数需要手动设置大小相同的内存，请对上面的内存进行更改！", "警告", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            useJVM.IsChecked = true;
            txb4.Text = "2048";
            txb5.Text = "2048";
        }

        private void usefastJvmPro_Checked(object sender, RoutedEventArgs e)
        {
            useJVM.IsChecked = true;
            txb4.Text = "10240";
            txb5.Text = "10240";
        }
    }
}
