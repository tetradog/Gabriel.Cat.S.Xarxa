﻿using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gabriel.Cat.S.Extension
{
    public static class HtmlDocumentExtension
    {
        public static HtmlDocument LoadUrl(this HtmlDocument document, string url)
        {
            return document.LoadUrl(new Uri(url));
        }
        public static HtmlDocument LoadUrl(this HtmlDocument document, Uri url)
        {
            return  document.LoadString(url.DownloadString());
        }

        public static HtmlDocument LoadString(this HtmlDocument document, string htmlDoc)
        {

            document.LoadHtml(htmlDoc);
            return document;
        }
        public static IEnumerable<HtmlNode> GetByClass(this HtmlDocument document, string clase)
        {
            foreach (HtmlNode node in document.DocumentNode.GetByClass(clase))
            {
                yield return node;
            }

        }
        public static IEnumerable<HtmlNode> GetByClass(this HtmlNode parentNode, string clase)
        {

            if (parentNode.GetClasses().Any(c => c.Equals(clase)))
                yield return parentNode;
            for (int i = 0; i < parentNode.ChildNodes.Count; i++)
            {
                foreach (HtmlNode node in parentNode.ChildNodes[i].GetByClass(clase))
                {
                    yield return node;
                }

            }
        }

        public static IEnumerable<HtmlNode> GetByTagName(this HtmlDocument document, string tagName)
        {


            foreach (HtmlNode node in document.DocumentNode.GetByTagName(tagName))
            {
                yield return node;
            }



        }
        public static IEnumerable<HtmlNode> GetByTagName(this HtmlNode parentNode, string tagName)
        {

            if (parentNode.Name.Equals(tagName))
                yield return parentNode;

            for (int i = 0; i < parentNode.ChildNodes.Count; i++)
            {
                foreach (HtmlNode node in parentNode.ChildNodes[i].GetByTagName(tagName))
                {
                    yield return node;
                }

            }
        }
    }
}
