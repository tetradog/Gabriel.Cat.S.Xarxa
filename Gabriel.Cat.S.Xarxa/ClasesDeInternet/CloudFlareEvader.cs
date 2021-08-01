using System;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Collections;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;
using Gabriel.Cat.S.Extension;

using Jint;
using System.Collections.Specialized;
using System.Collections.Generic;

namespace Gabriel.Cat.S.Utilitats.ClasesDeInternet
{

    //source:https://stackoverflow.com/questions/32425973/how-can-i-get-html-from-page-with-cloudflare-ddos-portection
    public static class CloudflareEvader
    {
        static SortedList<string, WebClient> DicClients { get; set; } = new SortedList<string, WebClient>();
        /// <summary>
        /// Tries to return a webclient with the neccessary cookies installed to do requests for a cloudflare protected website.
        /// </summary>
        /// <param name="url">The page which is behind cloudflare's anti-dDoS protection</param>
        /// <returns>A WebClient object or null on failure</returns>
        public static WebClient CloudflareWebClient(Uri uri,bool getNewClient=false,bool waitIfExist=true)
        {
            const string USERAGENT = "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:40.0) Gecko/20100101 Firefox/40.0";

            WebResponse res;
            WebClient response;
            CookieContainer coockieContainer;
            CookieCollection initialCookies;
            string challenge;
            string challengePass;
            string builder;
            long solved;
            string cookieUrl;
            UriBuilder uriBuilder;
            NameValueCollection query;
            HttpWebRequest cookieReq;
            HttpWebResponse cookieResp;
            CookieCollection cookiesParsed;
            WebClient modedWebClient;
            HttpWebRequest req;
            Engine JSEngine;
            string html;

            if(getNewClient && DicClients.ContainsKey(uri.Host))
            {
                DicClients.Remove(uri.Host);
            }

            if (!DicClients.ContainsKey(uri.Host))
            {
                html = string.Empty;
                JSEngine = new Engine(); //Use this JavaScript engine to compute the result.
                                         //Download the original page
                req = (HttpWebRequest)WebRequest.Create(uri);
                req.UserAgent = USERAGENT;
                //Try to make the usual request first. If this fails with a 503, the page is behind cloudflare.
                try
                {
                    res = req.GetResponse();

                    using (var reader = new StreamReader(res.GetResponseStream()))
                        html = reader.ReadToEnd();
                    response = new WebClient();
                }
                catch (WebException ex) //We usually get this because of a 503 service not available.
                {
                    using (StreamReader reader = new StreamReader(ex.Response.GetResponseStream()))
                        html = reader.ReadToEnd();
                    //If we get on the landing page, Cloudflare gives us a User-ID token with the cookie. We need to save that and use it in the next request.
                    coockieContainer = new CookieContainer();
                    //using a custom function because ex.Response.Cookies returns an empty set ALTHOUGH cookies were sent back.
                    initialCookies = GetAllCookiesFromHeader(ex.Response.Headers["Set-Cookie"], uri.Host);
                    foreach (Cookie initCookie in initialCookies)
                        coockieContainer.Add(initCookie);

                    /* solve the actual challenge with a bunch of RegEx's. Copy-Pasted from the python scrapper version.*/
                    challenge = Regex.Match(html, "name=\"jschl_vc\" value=\"(\\w+)\"").Groups[1].Value;
                    challengePass = Regex.Match(html, "name=\"pass\" value=\"(.+?)\"").Groups[1].Value;

                    builder = Regex.Match(html, @"setTimeout\(function\(\){\s+(var t,r,a,f.+?\r?\n[\s\S]+?a\.value =.+?)\r?\n").Groups[1].Value;
                    builder = Regex.Replace(builder, @"a\.value =(.+?) \+ .+?;", "$1");
                    builder = Regex.Replace(builder, @"\s{3,}[a-z](?: = |\.).+", "");

                    //Format the javascript..
                    builder = Regex.Replace(builder, @"[\n\\']", "");

                    //Execute it. 
                    solved = long.Parse(JSEngine.Execute(builder).GetCompletionValue().ToObject().ToString());
                    solved += uri.Host.Length; //add the length of the domain to it.

                    Debug.WriteLine("***** SOLVED CHALLENGE ******: " + solved);
                    WaitCloudflare(); //This sleeping IS requiered or cloudflare will not give you the token!!

                    //Retreive the cookies. Prepare the URL for cookie exfiltration.
                    cookieUrl = string.Format("{0}://{1}/cdn-cgi/l/chk_jschl", uri.Scheme, uri.Host);
                    uriBuilder = new UriBuilder(cookieUrl);
                    query = HttpUtility.ParseQueryString(uriBuilder.Query);
                    //Add our answers to the GET query
                    query["jschl_vc"] = challenge;
                    query["jschl_answer"] = solved.ToString();
                    query["pass"] = challengePass;
                    uriBuilder.Query = query.ToString();

                    //Create the actual request to get the security clearance cookie
                    cookieReq = (HttpWebRequest)WebRequest.Create(uriBuilder.Uri);
                    cookieReq.AllowAutoRedirect = false;
                    cookieReq.CookieContainer = coockieContainer;
                    cookieReq.Referer = uri.AbsolutePath;
                    cookieReq.UserAgent = USERAGENT;
                    //We assume that this request goes through well, so no try-catch
                    cookieResp = (HttpWebResponse)cookieReq.GetResponse();
                    //The response *should* contain the security clearance cookie!
                    if (cookieResp.Cookies.Count != 0) //first check if the HttpWebResponse has picked up the cookie.
                        foreach (Cookie cookie in cookieResp.Cookies)
                            coockieContainer.Add(cookie);
                    else //otherwise, use the custom function again
                    {
                        //the cookie we *hopefully* received here is the cloudflare security clearance token.
                        if (!Equals(cookieResp.Headers["Set-Cookie"], default))
                        {
                            cookiesParsed = GetAllCookiesFromHeader(cookieResp.Headers["Set-Cookie"], uri.Host);
                            foreach (Cookie cookie in cookiesParsed)
                                coockieContainer.Add(cookie);
                        }
                        else
                        {
                            //No security clearence? something went wrong...
                            throw new Exception("MASSIVE ERROR: COULDN'T GET CLOUDFLARE CLEARANCE!");

                        }
                    }
                    //Create a custom webclient with the two cookies we already acquired.
                    modedWebClient = new WebClientEx(coockieContainer);
                    modedWebClient.Headers.Add("User-Agent", USERAGENT);
                    modedWebClient.Headers.Add("Referer", uri.AbsolutePath);
                    response = modedWebClient;
                }
                DicClients.Add(uri.Host, response);
            }
            else if (waitIfExist)
            {
                WaitCloudflare();
            }
            return DicClients[uri.Host];
        }

        static void WaitCloudflare()
        {
            Thread.Sleep(3000);
        }

        /* Credit goes to https://stackoverflow.com/questions/15103513/httpwebresponse-cookies-empty-despite-set-cookie-header-no-redirect 
           (user https://stackoverflow.com/users/541404/cameron-tinker) for these functions 
        */
        static CookieCollection GetAllCookiesFromHeader(string strHeader, string strHost)
        {
            ArrayList al;
            CookieCollection cc = new CookieCollection();
            if (!string.IsNullOrEmpty(strHeader))
            {
                al = ConvertCookieHeaderToArrayList(strHeader);
                cc = ConvertCookieArraysToCookieCollection(al, strHost);
            }
            return cc;
        }

        static ArrayList ConvertCookieHeaderToArrayList(string strCookHeader)
        {
            ArrayList al;
            string[] strCookTemp;
            int n;
            int i = 0;

            strCookHeader = strCookHeader.Replace("\r", "");
            strCookHeader = strCookHeader.Replace("\n", "");
            strCookTemp = strCookHeader.Split(',');
            al = new ArrayList();
            n = strCookTemp.Length;
            while (i < n)
            {
                if (strCookTemp[i].IndexOf("expires=", StringComparison.OrdinalIgnoreCase) > 0)
                {
                    al.Add(strCookTemp[i] + "," + strCookTemp[i + 1]);
                    i++;
                }
                else
                    al.Add(strCookTemp[i]);
                i++;
            }
            return al;
        }

        static CookieCollection ConvertCookieArraysToCookieCollection(ArrayList al, string strHost)
        {
            int intEachCookPartsCount;
            string strCNameAndCValue;
            string strPNameAndPValue;
            string[] NameValuePairTemp;
            Cookie cookTemp;
            int firstEqual;
            string firstName;
            string allValue;
            string strEachCook;
            string[] strEachCookParts;
            CookieCollection cc = new CookieCollection();
            int alcount = al.Count;

            for (int i = 0; i < alcount; i++)
            {
                strEachCook = al[i].ToString();
                strEachCookParts = strEachCook.Split(';');
                intEachCookPartsCount = strEachCookParts.Length;
                cookTemp = new Cookie();
                if (intEachCookPartsCount >= 1)
                {

                    strCNameAndCValue = strEachCookParts[0];
                    if (!string.IsNullOrEmpty(strCNameAndCValue))
                    {
                        firstEqual = strCNameAndCValue.IndexOf("=");
                        firstName = strCNameAndCValue.Substring(0, firstEqual);
                        allValue = strCNameAndCValue.Substring(firstEqual + 1, strCNameAndCValue.Length - (firstEqual + 1));
                        cookTemp.Name = firstName;
                        cookTemp.Value = allValue;
                    }


                }
                for (int j = 1; j < intEachCookPartsCount; j++)
                {

                    if (strEachCookParts[j].IndexOf("path", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        strPNameAndPValue = strEachCookParts[j];
                        if (!string.IsNullOrEmpty(strPNameAndPValue))
                        {
                            NameValuePairTemp = strPNameAndPValue.Split('=');
                            if (NameValuePairTemp[1] != string.Empty)
                                cookTemp.Path = NameValuePairTemp[1];
                            else
                                cookTemp.Path = "/";
                        }
                    }

                    else if (strEachCookParts[j].IndexOf("domain", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        strPNameAndPValue = strEachCookParts[j];
                        if (!string.IsNullOrEmpty(strPNameAndPValue))
                        {
                            NameValuePairTemp = strPNameAndPValue.Split('=');

                            if (!string.IsNullOrEmpty(NameValuePairTemp[1]))
                                cookTemp.Domain = NameValuePairTemp[1];
                            else
                                cookTemp.Domain = strHost;
                        }
                      
                    }
                }

                if (string.IsNullOrEmpty(cookTemp.Path))
                    cookTemp.Path = "/";
                if (string.IsNullOrEmpty(cookTemp.Domain))
                    cookTemp.Domain = strHost;
                cc.Add(cookTemp);
            }
            return cc;
        }
    }

    /*Credit goes to  https://stackoverflow.com/questions/1777221/using-cookiecontainer-with-webclient-class
 (user https://stackoverflow.com/users/129124/pavel-savara) */
    public class WebClientEx : WebClient
    {
        public WebClientEx(CookieContainer container)
        {
            CookieContainer = container;
        }

        public CookieContainer CookieContainer { get; set; }

        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest r = base.GetWebRequest(address);
            HttpWebRequest request = r as HttpWebRequest;
            if (request != null)
            {
                request.CookieContainer = CookieContainer;
            }
            return r;
        }

        protected override WebResponse GetWebResponse(WebRequest request, IAsyncResult result)
        {
            WebResponse response = base.GetWebResponse(request, result);
            ReadCookies(response);
            return response;
        }

        protected override WebResponse GetWebResponse(WebRequest request)
        {
            WebResponse response = base.GetWebResponse(request);
            ReadCookies(response);
            return response;
        }

        private void ReadCookies(WebResponse r)
        {
            CookieCollection cookies;
            HttpWebResponse response = r as HttpWebResponse;
            if (!ReferenceEquals(response, null))
            {
                cookies = response.Cookies;
                CookieContainer.Add(cookies);
            }
        }
    }
}