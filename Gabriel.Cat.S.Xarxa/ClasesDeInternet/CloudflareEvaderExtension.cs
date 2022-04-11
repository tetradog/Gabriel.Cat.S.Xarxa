using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Gabriel.Cat.S.Extension
{
    public static class CloudflareEvaderExtension
    {
        public static Bitmap GetBitmapCloudflare([NotNull] this Uri url, bool getNewClient = false, bool waitIfExist = true) => new Bitmap(new MemoryStream(url.GetDataCloudflare(getNewClient, waitIfExist)));
        public static byte[] GetDataCloudflare([NotNull] this Uri url, bool getNewClient = false, bool waitIfExist = true) => url.GetCloudflareWebClient(getNewClient, waitIfExist).DownloadData(url.AbsoluteUri);
        public static WebClient GetCloudflareWebClient([NotNull] this Uri url, bool getNewClient = false, bool waitIfExist = true) => Utilitats.ClasesDeInternet.CloudflareEvader.CloudflareWebClient(url, getNewClient, waitIfExist);
        public static string DownloadCloudflare([NotNull] this Uri url, bool getNewClient = false, bool waitIfExist = true) => url.GetCloudflareWebClient(getNewClient, waitIfExist).DownloadString(url.AbsoluteUri);
        public static HtmlDocument LoadUrlCloudflare([NotNull] this HtmlDocument document, [NotNull] Uri url, bool getNewClient = false, bool waitIfExist = true) => document.LoadString(url.DownloadCloudflare(getNewClient, waitIfExist));
        public static HtmlDocument LoadUrlCloudflare([NotNull] this HtmlDocument document, [NotNull] string url, bool getNewClient = false, bool waitIfExist = true) => document.LoadUrlCloudflare(new Uri(url),getNewClient,waitIfExist);
    }
}
