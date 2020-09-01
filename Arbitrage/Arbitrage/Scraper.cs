using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;

namespace Arbitrage
{
    abstract public class Scraper
    {
        private readonly string r_WebsiteUrl;
        private readonly HtmlWeb r_HtmlWeb;
        private HtmlDocument m_HtmlDocument;
        public delegate void ScraperConnectionDelegate();
        public event ScraperConnectionDelegate m_ToDoWhenFailConnection;

        public string WebsiteUrl
        {
            get { return r_WebsiteUrl; }
        }

        public HtmlWeb HtmlWeb
        {
            get { return r_HtmlWeb; }
        }

        public HtmlDocument HtmlDocument
        {
            get { return m_HtmlDocument; }
            set { m_HtmlDocument = value; }
        }

        public Scraper(string WebsiteUrl)
        {
            r_WebsiteUrl = WebsiteUrl;
            r_HtmlWeb = new HtmlWeb();
            m_ToDoWhenFailConnection = null;
        }

        public event ScraperConnectionDelegate ToDoWhenFailConnection {
            add { m_ToDoWhenFailConnection += value; }
            remove { m_ToDoWhenFailConnection -= value; }
        }


        abstract public void LoadUrl(); // throw Exception/event in case of connection not good
        abstract public List<FootballMatch> MakeListOfDailyMatchesPlaying();
        abstract internal void OnFailConnection(string i_Url);
        abstract public string StatsCollector(string m_StatsUrl);
        abstract public void AddActionDelegate(ScraperConnectionDelegate ActionDelegate);
    }
}
