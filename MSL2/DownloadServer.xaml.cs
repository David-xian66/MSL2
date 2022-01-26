using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
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
using static MSL2.DownloadWindow;

namespace MSL2
{
    /// <summary>
    /// DownloadServer.xaml 的交互逻辑
    /// </summary>
    public partial class DownloadServer : Window
    {
        string autoupdate;
        //public static string autoupdateserver="&";
        string domain;
        public DownloadServer()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //timer7.Tick += new EventHandler(timer7_Tick);
            //timer7.Interval = TimeSpan.FromSeconds(2);
            // timer7.Start();
            Thread thread = new Thread(GetServer);
            thread.Start();
            //isgetserverOpen = true;
        }
        //服务端下载
        private void serverlist_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (serverlist1.SelectedIndex.ToString() != "-1")
            {
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"MSL2/" + serverlist.SelectedItem.ToString() + ".json"))
                {
                    int url = serverlist1.SelectedIndex;
                    //string filename = serverlist.SelectedItem.ToString();
                    //MessageBox.Show(Url);
                    try
                    {
                        filename = AppDomain.CurrentDomain.BaseDirectory + @"MSL2\" + autoupdate.Substring(0, autoupdate.IndexOf("（")) + "-" + serverlist1.SelectedItem.ToString() + ".jar";
                    }
                    catch
                    {
                        filename = AppDomain.CurrentDomain.BaseDirectory + @"MSL2\" + autoupdate + serverlist1.SelectedItem.ToString() + ".jar";
                    }
                    downloadurl = domain + serverdownlist.Items[url].ToString();
                    downloadinfo = "下载服务端中……";
                    Window window = new DownloadWindow();
                    window.ShowDialog();
                    //MessageBox.Show(Url,Filename);
                    downmsg1.Content = "下载成功，已自动为您选择该服务端（默认下载目录为软件运行目录的MSL2文件夹）";
                    MainWindow.serverserver = "\"" + filename + "\"";
                    serverlist.IsEnabled = true;
                    serverlist1.IsEnabled = true;
                }
                else
                {
                    int url = serverlist1.SelectedIndex;
                    //string filename = serverlist.SelectedItem.ToString();
                    downloadurl = serverdownlist.Items[url].ToString();
                    filename = AppDomain.CurrentDomain.BaseDirectory + @"MSL2\" + serverlist.SelectedItem.ToString() + "-" + serverlist1.SelectedItem.ToString() + ".jar";
                    downloadinfo = "下载服务端中……";
                    Window window = new DownloadWindow();
                    window.ShowDialog();
                    //MessageBox.Show(Url,Filename);
                    downmsg1.Content = "下载成功，已自动为您选择该服务端（默认下载目录为软件运行目录的MSL2文件夹）";
                    MainWindow.serverserver = "\"" + filename + "\"";
                    serverlist.IsEnabled = true;
                    serverlist1.IsEnabled = true;
                }
            }
        }
        void GetServer()
        {
            try
            {
                WebClient MyWebClient1 = new WebClient();
                MyWebClient1.Credentials = CredentialCache.DefaultCredentials;
                byte[] pageData1 = MyWebClient1.DownloadData("http://115.220.5.81:8081/web/getserver.txt");
                string pageHtml1 = Encoding.UTF8.GetString(pageData1);

                int IndexofA0 = pageHtml1.IndexOf("*");
                string Ru0 = pageHtml1.Substring(IndexofA0 + 1);
                string pageHtml = Ru0.Substring(0, Ru0.IndexOf("*"));

                if (pageHtml1.IndexOf("#") + 1 != 0)
                {
                    while (pageHtml1.IndexOf("#") != -1)
                    {
                        int IndexofA = pageHtml1.IndexOf("#");
                        string Ru = pageHtml1.Substring(IndexofA + 1);
                        autoupdate = Ru.Substring(0, Ru.IndexOf("|"));

                        int IndexofA1 = pageHtml1.IndexOf("|");
                        string Ru1 = pageHtml1.Substring(IndexofA1 + 1);
                        string autoupdate1 = Ru1.Substring(0, Ru1.IndexOf("\n"));

                        this.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                        {
                            serverlist.Items.Add(autoupdate);
                        });
                        //MessageBox.Show(autoupdate);
                        //MessageBox.Show(autoupdate1);
                        //MessageBox.Show(pageHtml1);
                        HttpWebRequest Myrq1 = (HttpWebRequest)HttpWebRequest.Create(autoupdate1);
                        HttpWebResponse myrp1;
                        myrp1 = (HttpWebResponse)Myrq1.GetResponse();
                        long totalBytes1 = myrp1.ContentLength;
                        Stream st1 = myrp1.GetResponseStream();
                        FileStream so1 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"MSL2/" + autoupdate + ".json", FileMode.Create);
                        long totalDownloadedByte1 = 0;
                        byte[] by1 = new byte[1024];
                        int osize1 = st1.Read(by1, 0, (int)by1.Length);
                        while (osize1 > 0)
                        {
                            totalDownloadedByte1 = osize1 + totalDownloadedByte1;
                            DispatcherHelper.DoEvents();
                            so1.Write(by1, 0, osize1);
                            osize1 = st1.Read(by1, 0, (int)by1.Length);
                            float percent = 0;
                            percent = (float)totalDownloadedByte1 / (float)totalBytes1 * 100;
                            DispatcherHelper.DoEvents();
                        }
                        so1.Close();
                        st1.Close();
                        int IndexofA3 = pageHtml1.IndexOf("|");
                        string Ru3 = pageHtml1.Substring(IndexofA3 + 1);
                        pageHtml1 = Ru3;
                        //autoupdateserver = autoupdateserver +",&"+ autoupdate;
                        //MessageBox.Show(autoupdate);
                        //MessageBox.Show(autoupdate1);
                        //MessageBox.Show(pageHtml1);
                    }

                    /*
                    //分类服务端
                    StreamReader reader1 = File.OpenText(AppDomain.CurrentDomain.BaseDirectory + @"MSL2/paperlist.json");
                    JsonTextReader jsonTextReader1 = new JsonTextReader(reader1);
                    JObject jsonObject1 = (JObject)JToken.ReadFrom(jsonTextReader1);

                    foreach (var x in jsonObject1)
                    {
                        this.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                        {
                            serverlist.Items.Add(x.Key);
                        });
                        //MessageBox.Show( x.Value.ToString(), x.Key);
                    }
                    reader1.Close();*/
                }

                try
                {
                    HttpWebRequest Myrq = (HttpWebRequest)HttpWebRequest.Create(pageHtml);
                    HttpWebResponse myrp;
                    myrp = (HttpWebResponse)Myrq.GetResponse();
                    long totalBytes = myrp.ContentLength;
                    Stream st = myrp.GetResponseStream();
                    FileStream so = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"MSL2/serverlist.json", FileMode.Create);
                    long totalDownloadedByte = 0;
                    byte[] by = new byte[1024];
                    int osize = st.Read(by, 0, (int)by.Length);
                    while (osize > 0)
                    {
                        totalDownloadedByte = osize + totalDownloadedByte;
                        DispatcherHelper.DoEvents();
                        so.Write(by, 0, osize);
                        osize = st.Read(by, 0, (int)by.Length);
                        float percent = 0;
                        percent = (float)totalDownloadedByte / (float)totalBytes * 100;
                        DispatcherHelper.DoEvents();
                    }
                    so.Close();
                    st.Close();
                    //分类服务端
                    StreamReader reader = File.OpenText(AppDomain.CurrentDomain.BaseDirectory + @"MSL2/serverlist.json");
                    JsonTextReader jsonTextReader = new JsonTextReader(reader);
                    JObject jsonObject = (JObject)JToken.ReadFrom(jsonTextReader);

                    foreach (var x in jsonObject)
                    {
                        this.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                        {
                            serverlist.Items.Add(x.Key);
                        });
                        //MessageBox.Show( x.Value.ToString(), x.Key);
                    }
                    reader.Close();

                    this.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                    {
                        serverlist.SelectedIndex = 0;
                        getservermsg.Visibility = Visibility.Hidden;
                    });
                }
                catch
                {
                }
            }
            catch (Exception a)
            {
                this.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                {
                    getservermsg.Text = "获取服务端失败！请重试" + "w4x2" + a;
                    //File.Delete(AppDomain.CurrentDomain.BaseDirectory + @"MSL2/serverlist.json");
                });
                //timer7.Stop();
            }
        }

        private void updatehistory_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://msdoc.nstarmc.cn/docs/Download_source/history.html");
        }

        private void serverlist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"MSL2/" + serverlist.SelectedItem.ToString() + ".json"))
            {
                autoupdate = serverlist.SelectedItem.ToString();
                StreamReader reader1 = File.OpenText(AppDomain.CurrentDomain.BaseDirectory + @"MSL2/" + serverlist.SelectedItem.ToString() + ".json");
                JsonTextReader jsonTextReader1 = new JsonTextReader(reader1);
                JObject jsonObject11 = (JObject)JToken.ReadFrom(jsonTextReader1);
                domain = jsonObject11["domain"].ToString();
                updatetime.Content = "最新下载源更新时间：\n" + jsonObject11["updatetime"].ToString();
                //MessageBox.Show(serverlist.SelectedItem.ToString());
                //string abc1 = serverlist.SelectedItem.ToString();
                JObject jsonObject111 = (JObject)jsonObject11["versions"];
                serverlist1.Items.Clear();
                serverdownlist.Items.Clear();
                foreach (var x in jsonObject111)
                {
                    serverlist1.Items.Add(x.Key);
                    serverdownlist.Items.Add(x.Value.ToString());
                    //MessageBox.Show(x.Value.ToString(), x.Key);
                }
                reader1.Close();
            }
            else
            {
                StreamReader reader = File.OpenText(AppDomain.CurrentDomain.BaseDirectory + @"MSL2/serverlist.json");
                JsonTextReader jsonTextReader = new JsonTextReader(reader);
                JObject jsonObject = (JObject)JToken.ReadFrom(jsonTextReader);
                //MessageBox.Show(serverlist.SelectedItem.ToString());
                string abc = serverlist.SelectedItem.ToString();
                JObject jsonObject1 = (JObject)jsonObject[abc];
                updatetime.Content = "最新下载源更新时间";
                serverlist1.Items.Clear();
                serverdownlist.Items.Clear();
                foreach (var x in jsonObject1)
                {

                    serverlist1.Items.Add(x.Key);
                    serverdownlist.Items.Add(x.Value.ToString());
                    //MessageBox.Show(x.Value.ToString(), x.Key);
                }
                reader.Close();
            }
        }

        private void openSpigot_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://www.spigotmc.org/");
        }

        private void openPaper_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://papermc.io/");
        }

        private void openMojang_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://www.minecraft.net/zh-hans/download/server");
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"MSL2/serverlist.json"))
            {
                File.Delete(AppDomain.CurrentDomain.BaseDirectory + @"MSL2/serverlist.json");
            }
            int i = serverlist.Items.Count;
            int x = 0;
            while (x != i)
            {
                string aaa = serverlist.Items[x].ToString();
                x++;
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"MSL2/" + aaa + ".json"))
                {
                    File.Delete(AppDomain.CurrentDomain.BaseDirectory + @"MSL2/" + aaa + ".json");
                }
            }
        }
    }
}
