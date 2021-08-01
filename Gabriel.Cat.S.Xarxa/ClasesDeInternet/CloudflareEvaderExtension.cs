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
        public static Bitmap GetBitmapCloudflare([NotNull] this Uri url) => new Bitmap(new MemoryStream(url.GetDataCloudflare()));
        public static byte[] GetDataCloudflare([NotNull] this Uri url) => url.GetCloudflareWebClient().DownloadData(url.AbsoluteUri);
        public static WebClient GetCloudflareWebClient([NotNull] this Uri url) => Utilitats.ClasesDeInternet.CloudflareEvader.CloudflareWebClient(url);
        public static string DownloadCloudflare([NotNull] this Uri url) => url.GetCloudflareWebClient().DownloadString(url.AbsoluteUri);
        public static HtmlDocument LoadUrlCloudflare([NotNull] this HtmlDocument document, [NotNull] Uri url) => document.LoadString(url.DownloadCloudflare());
        public static HtmlDocument LoadUrlCloudflare([NotNull] this HtmlDocument document, [NotNull] string url) => document.LoadUrlCloudflare(new Uri(url));
    }
}
