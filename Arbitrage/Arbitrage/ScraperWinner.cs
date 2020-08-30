using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Arbitrage
{
    public class ScraperWinner: Scraper
    {
        private static readonly string sr_HtmlClassValueForTeamName = "title ";
        private static readonly string sr_HtmlClassValueForRatio = "formatted_price";

        public ScraperWinner(string WebsiteUrl): base(WebsiteUrl){}

        // To-Do : throw Exception/event in case of connection not good
        public override void LoadUrl()
        {
            try
            {
                HtmlDocument = HtmlWeb.Load(WebsiteUrl);
            }
            catch
            {

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

                HtmlNodeCollection collectionOfTeamsNames = HtmlDocument.DocumentNode.SelectNodes($"//*[@class='{sr_HtmlClassValueForTeamName}']");
                HtmlNodeCollection collectionOfTeamsRatio = HtmlDocument.DocumentNode.SelectNodes($"//*[@class='{sr_HtmlClassValueForRatio}']");

                for (int i = 0; i < collectionOfTeamsNames.Count; i += 3)
                {
                    string tempNameOfFirstTeam = UI.ArrangeHebStringToBeHebUICustomize(collectionOfTeamsNames[i].Attributes["title"].Value);
                    string tempNameOfSecondTeam = UI.ArrangeHebStringToBeHebUICustomize(collectionOfTeamsNames[i + 2].Attributes["title"].Value);

                    int tempRatioOfFirstTeam = int.Parse(collectionOfTeamsRatio[i].InnerText);
                    int tempRatioOfSecondTeam = int.Parse(collectionOfTeamsRatio[i + 2].InnerText);

                    listOfDayMatches.Add(new FootballMatch(tempNameOfFirstTeam, tempRatioOfFirstTeam, tempNameOfSecondTeam, tempRatioOfSecondTeam));
                }
            }
            catch
            {
                
            }

            return listOfDayMatches;
        }
    }
}
