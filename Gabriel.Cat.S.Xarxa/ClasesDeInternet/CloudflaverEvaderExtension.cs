using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Gabriel.Cat.S.Extension
{
    public static class CloudflaverEvaderExtension
    {
        public static Bitmap GetBitmapBypassed([NotNull] this Uri url)
        {
            return new Bitmap(new MemoryStream(url.GetDataBypassed()));
        }
        public static byte[] GetDataBypassed([NotNull] this Uri url)
        {
            return url.GetBypassedWebClient().DownloadData(url.AbsoluteUri);
        }
        public static WebClient GetBypassedWebClient(this Uri url) => Utilitats.ClasesDeInternet.CloudflareEvader.CreateBypassedWebClient(url);
        public static string DownloadBypassed(this Uri url) => url.GetBypassedWebClient().DownloadString(url.AbsoluteUri);
        public static HtmlDocument LoadUrlByPassed(this HtmlDocument document, Uri url)
        {
            return document.LoadString(url.DownloadBypassed());

        }
        public static HtmlDocument LoadUrlByPassed(this HtmlDocument document, string url)
        {
            return document.LoadUrlByPassed(new Uri(url));

        }
    }
}
