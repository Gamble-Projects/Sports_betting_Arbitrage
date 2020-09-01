using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Arbitrage
{
    public class ScraperWinner : Scraper
    {
        private static readonly string sr_HtmlClassValueForTeamName = "title ";
        private static readonly string sr_HtmlClassValueForRatio = "formatted_price";

        public ScraperWinner(string WebsiteUrl) : base(WebsiteUrl)
        {
        }

        override public void AddActionDelegate(ScraperConnectionDelegate ActionDelegate)
        {
            ToDoWhenFailConnection += ActionDelegate;
        }

        // To-Do : throw Exception/event in case of connection not good
        public override void LoadUrl()
        {
            try
            {
                HtmlDocument = HtmlWeb.Load(WebsiteUrl);
            }
            catch
            {
                //ToDoWhenFailConnection.Invoke();
            }
            //throw new NotImplementedException();
        }

        public override List<FootballMatch> MakeListOfDailyMatchesPlaying()
        {
            List<FootballMatch> listOfDayMatches = new List<FootballMatch>();


            // To-Do : add exception handler
            try
            {
                //Filter the content
                HtmlDocument.DocumentNode.Descendants().Where(n => n.Name == "script").ToList().ForEach(n => n.Remove());

                HtmlNodeCollection collectionOfStatsUrl = HtmlDocument.DocumentNode.SelectNodes($"//*[@class='{"statistics"}']");
                HtmlNodeCollection collectionOfTeamsNames = HtmlDocument.DocumentNode.SelectNodes($"//*[@class='{sr_HtmlClassValueForTeamName}']");
                HtmlNodeCollection collectionOfTeamsRatio = HtmlDocument.DocumentNode.SelectNodes($"//*[@class='{sr_HtmlClassValueForRatio}']");

                for (int i = 0; i < collectionOfTeamsNames.Count; i += 3)
                {
                    string tempNameOfFirstTeam = UI.ArrangeHebStringToBeHebUICustomize(collectionOfTeamsNames[i].Attributes["title"].Value);
                    string tempNameOfSecondTeam = UI.ArrangeHebStringToBeHebUICustomize(collectionOfTeamsNames[i + 2].Attributes["title"].Value);
                    string StatsUrl = collectionOfStatsUrl[i / 3].SelectSingleNode($"*[@class='{"stats-popup"}']").Attributes["href"].Value;

                    float tempRatioOfFirstTeam = float.Parse(collectionOfTeamsRatio[i].InnerText);
                    float tempRatioOfSecondTeam = float.Parse(collectionOfTeamsRatio[i + 2].InnerText);

                    listOfDayMatches.Add(new FootballMatch(tempNameOfFirstTeam, tempRatioOfFirstTeam, tempNameOfSecondTeam, tempRatioOfSecondTeam, StatsUrl));
                }
            }
            catch
            {

            }

            return listOfDayMatches;
        }

        override public string StatsCollector(string m_StatsUrl)
        {
            HtmlWeb web1 = new HtmlWeb();
            HtmlDocument doc1 = web1.Load(m_StatsUrl);
            doc1.DocumentNode.Descendants().Where(n => n.Name == "script").ToList().ForEach(n => n.Remove());

            try
            {
                HtmlNodeCollection nodesTeamNames1 = doc1.DocumentNode.SelectNodes($"//p[@class='{"text-center"}']");
                HtmlNodeCollection nodesTeamNames2 = doc1.DocumentNode.SelectNodes($"//div[@class='{"size-s graphics-text-primary-color"}']");

                if (nodesTeamNames1[2].InnerText != "0")
                {
                    Console.WriteLine(UI.ArrangeHebStringToBeHebUICustomize("נגמרו בתיקו. ") + nodesTeamNames2[0].InnerText[0] + UI.ArrangeHebStringToBeHebUICustomize( "משחקים,מתוכם " )+ nodesTeamNames1[2].InnerText + UI.ArrangeHebStringToBeHebUICustomize("הקבוצות שיחקו"));

                    return (UI.ArrangeHebStringToBeHebUICustomize("נגמרו בתיקו. " + nodesTeamNames2[0].InnerText[0] + "משחקים,מתוכם " + nodesTeamNames1[2].InnerText + "הקבוצות שיחקו"));
                }
                else
                {
                    return (" ");
                }
            }
            catch (NullReferenceException e)
            {
                return " ";
            }
        }

        internal override void OnFailConnection(string i_Url)
        {
            /*
            // lets tell the form that I was clicked:
            if (ToDoWhenFailConnection != null)
            {
                ToDoWhenFailConnection.Invoke();
            }
            else
            {
                Console.WriteLine(i_Url + "Has No Action On Fail Connection");
            }
            */
        }
    }
}
