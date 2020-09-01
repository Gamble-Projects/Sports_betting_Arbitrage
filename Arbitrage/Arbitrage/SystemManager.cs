using System;
using System.Collections.Generic;
using Quartz;

namespace Arbitrage
{
    public class SystemManager
    {
        // key = url, value = scraper
        private readonly Dictionary<string, Scraper> r_Scrapers = new Dictionary<string, Scraper>();
        private readonly Quartz.SchedulerBuilder r_Scheduler;

        public bool AddScraperToDict(Scraper i_NewScraper) {
            bool v_ScraperAdded = false;

            // get connection
            i_NewScraper.LoadUrl();
            //add event
            i_NewScraper.AddActionDelegate(new EventsForSystemManager().OnFailConnection);
            // add to dict
            r_Scrapers.Add(i_NewScraper.WebsiteUrl, i_NewScraper);

            return v_ScraperAdded;
        }

        public bool RemoveScraperToDict(string i_Url)
        {
            bool v_ScraperBeenRemoved = false;

            // find i_Url
            // remove from dictionary

            return v_ScraperBeenRemoved;
        }

        // Do This Daily Basis ????
        public void DailyGetAllFootballMatchFromScrapersAndCalculateArbitrage()
        {
            List<FootballMatch> footballMatchesToBetOn = new List<FootballMatch>();

            foreach (Scraper scraper in r_Scrapers.Values)
            {
                List<FootballMatch> FootballMatchesFromScraper = scraper.MakeListOfDailyMatchesPlaying();

                foreach(FootballMatch match in FootballMatchesFromScraper)
                {
                    FootballMatch tempMatch = match;

                    if (Arbitrager.isArbitrage(ref tempMatch) == true)
                    {
                        Arbitrager.GamblingRatio(ref tempMatch);
                        tempMatch.MatchStats = scraper.StatsCollector(tempMatch.StatsUrl);
                        footballMatchesToBetOn.Add(tempMatch);
                    }
                }

                foreach(FootballMatch match in footballMatchesToBetOn)
                {
                    Console.WriteLine(match.FirstTeam + " " + match.FirstTeamGamble + " " + match.SecondTeam +  " " + match.SecondTeamGamble);
                    Console.WriteLine(match.MatchStats);
                }
                // bet/send message/dont know on arbitrage game (footballMatchesToBetOn)
            }
        }
    }

    public class EventsForSystemManager
    {
        public void OnFailConnection()
        {
            //r_Scrapers.Remove(i_Url);
        }
    }
}
