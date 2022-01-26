using Downloader;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
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

namespace MSL2
{
    /// <summary>
    /// DownloadWindow.xaml 的交互逻辑
    /// </summary>
    public partial class DownloadWindow : Window
    {
        public static string downloadinfo;
        public static string filename;
        public static string downloadurl;
        static Thread thread;
        public DownloadWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            taskinfo.Content = downloadinfo;
            infolabel.Text= downloadinfo;
            thread = new Thread(Downloader);
            thread.Start();
        }
        public static class DispatcherHelper
        {
            [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
            public static void DoEvents()
            {
                DispatcherFrame frame = new DispatcherFrame();
                Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, new DispatcherOperationCallback(ExitFrames), frame);
                try { Dispatcher.PushFrame(frame); }
                catch (InvalidOperationException) { }
            }
            private static object ExitFrames(object frame)
            {
                ((DispatcherFrame)frame).Continue = false;
                return null;
            }
        }
        public void Downloader()
        {
            var downloadOpt = new DownloadConfiguration()
            {
                BufferBlockSize = 10240, // usually, hosts support max to 8000 bytes, default values is 8000
                ChunkCount = 1, // file parts to download, default value is 1
                MaximumBytesPerSecond = 1024 * 1024, // download speed limited to 1MB/s, default values is zero or unlimited
                MaxTryAgainOnFailover = int.MaxValue, // the maximum number of times to fail
                OnTheFlyDownload = false, // caching in-memory or not? default values is true
                ParallelDownload = true, // download parts of file as parallel or not. Default value is false
                TempDirectory = AppDomain.CurrentDomain.BaseDirectory+"MSL2\\temp", // Set the temp path for buffering chunk files, the default path is Path.GetTempPath()
                Timeout = 1000, // timeout (millisecond) per stream block reader, default values is 1000
                RequestConfiguration = // config and customize request headers
    {
        Accept = "*/*",
        AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
        CookieContainer =  new CookieContainer(), // Add your cookies
        Headers = new WebHeaderCollection(), // Add your custom headers
        KeepAlive = false,
        ProtocolVersion = HttpVersion.Version11, // Default value is HTTP 1.1
        UseDefaultCredentials = false,
        UserAgent = $"DownloaderSample/{Assembly.GetExecutingAssembly().GetName().Version.ToString(3)}"
    }
            };
            var downloader = new DownloadService(downloadOpt);
            // Provide `FileName` and `TotalBytesToReceive` at the start of each downloads
            downloader.DownloadStarted += OnDownloadStarted;
            // Provide any information about download progress, like progress percentage of sum of chunks, total speed, average speed, total received bytes and received bytes array to live streaming.
            downloader.DownloadProgressChanged += OnDownloadProgressChanged;
            // Download completed event that can include occurred errors or cancelled or download completed successfully.
            downloader.DownloadFileCompleted += OnDownloadFileCompleted;
            string file = filename;
            string url = downloadurl;
            downloader.DownloadFileTaskAsync(url, file);
        }

        private void OnDownloadStarted(object sender, DownloadStartedEventArgs e)
        {
            this.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
            {
                infolabel.Text = "开始下载";
            });
        }
        private void OnDownloadProgressChanged(object sender, Downloader.DownloadProgressChangedEventArgs e)
        {
            this.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
            {
                infolabel.Text = "开始下载" + (int)e.ProgressPercentage+"%";
                pbar.Value = (int)e.ProgressPercentage;
            });
        }
        private void OnDownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            this.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
            {
                infolabel.Text = "下载完成";
                Close();
            });
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            thread.Abort();
            Close();
        }
    }
}
