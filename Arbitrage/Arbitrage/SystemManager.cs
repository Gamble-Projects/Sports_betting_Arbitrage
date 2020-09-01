using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arbitrage
{
    public class SystemManager
    {
        private readonly Dictionary<string, Scraper> r_Scrapers = new Dictionary<string, Scraper>();

        public bool AddScraperToDict(Scraper i_NewScraper, string i_Url) {
            bool v_ScraperAdded = false;

            // get connection
            // add to dictionary

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
                        footballMatchesToBetOn.Add(tempMatch);
                    }
                }

                // bet/send message / dont know on arbitrage game (footballMatchesToBetOn)
            }
        }
    }
}
