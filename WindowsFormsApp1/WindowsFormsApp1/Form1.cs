using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private async void button2_Click(object sender, EventArgs e)
        {
            //Async Part
            var watch = System.Diagnostics.Stopwatch.StartNew();

            //await RundownloadASync();
            await RundownloadParallelASync();

            watch.Stop();
            var elapseMs = watch.ElapsedMilliseconds;
            txt1.Text += $"Total execution time: {elapseMs}";
        }

        private async Task RundownloadASync()
        {
            List<String> websites = PrepData();
            foreach (string site in websites)
            {
                WebsiteDataModel results =await Task.Run(() => DownlaodWebSite(site));
                ReportWebsiteInfo(results);
            }
        }


        private async Task RundownloadParallelASync()
        {
            List<String> websites = PrepData();
            List<Task<WebsiteDataModel>> tasks = new List<Task<WebsiteDataModel>>();
            foreach (string site in websites)
            {
                //tasks.Add(Task.Run(() => DownlaodWebSite(site)));
                //tasks.Add(Task.Run(() => DownlaodWebSiteAsync(site)));
                tasks.Add(DownlaodWebSiteAsync(site));
            }

            WebsiteDataModel[] results = await Task.WhenAll(tasks);

            results.ToList< WebsiteDataModel>().ForEach(p => ReportWebsiteInfo(p));

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            RundownloadSync();
            watch.Stop();
            var elapseMs = watch.ElapsedMilliseconds;
            txt1.Text += $"Total execution time: {elapseMs}";

        }

        private void RundownloadSync()
        {
            List<String> websites = PrepData();
            foreach(string site in websites)
            {
                WebsiteDataModel results = DownlaodWebSite(site);
                ReportWebsiteInfo(results);

            }
        }

        private void ReportWebsiteInfo(WebsiteDataModel data)
        {
            txt1.Text += $"{data.WebsiteUrl}  downloaded: {data.WebsiteData.Length} characters long. {Environment.NewLine}";
        }


        private async Task<WebsiteDataModel> DownlaodWebSiteAsync(string websiteURL)
        {
            WebsiteDataModel output = new WebsiteDataModel();
            WebClient client = new WebClient();
            output.WebsiteUrl = websiteURL;
            output.WebsiteData = await client.DownloadStringTaskAsync(websiteURL);
            return output;
        }

        private WebsiteDataModel DownlaodWebSite(string websiteURL)
        {
            WebsiteDataModel output = new WebsiteDataModel();
            WebClient client = new WebClient();
            output.WebsiteUrl = websiteURL;
            output.WebsiteData = client.DownloadString(websiteURL);
            return output;
        }

        private List<string> PrepData()
        {
            List<String> output = new List<string>();
            txt1.Text = "";
            output.Add("https://www.yahoo.com");
            output.Add("https://www.google.com");
            output.Add("https://www.cnn.com");
            output.Add("https://www.codeproject.com");
            output.Add("https://www.stackoverflow.com");
            output.Add("https://www.linkedin.com/feed/");
            return output;
        }
    }
}

