using System;
using System.Collections.Generic;
using Quartz;
using Quartz.Jobs;
namespace Arbitrage
{
    public class SystemManager
    {
        // key = url, value = scraper
        private readonly Dictionary<string, Scraper> r_Scrapers = new Dictionary<string, Scraper>();
        private IScheduler m_schedueler;

        public bool AddScraperToDict(Scraper i_NewScraper)
        {
            bool v_ScraperAdded = false;

            // get connection
            //i_NewScraper.LoadUrl();
            //add event
            i_NewScraper.AddActionDelegate(this.OnFailConnection);
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
                try
                {
                    scraper.LoadUrl();
                    List<FootballMatch> FootballMatchesFromScraper = scraper.MakeListOfDailyMatchesPlaying();

                    foreach (FootballMatch match in FootballMatchesFromScraper)
                    {
                        FootballMatch tempMatch = match;

                        if (Arbitrager.isArbitrage(ref tempMatch) == true)
                        {
                            Arbitrager.GamblingRatio(ref tempMatch);
                            tempMatch.MatchStats = scraper.StatsCollector(tempMatch.StatsUrl);
                            footballMatchesToBetOn.Add(tempMatch);
                        }
                    }

                    foreach (FootballMatch match in footballMatchesToBetOn)
                    {
                        Console.WriteLine(match.FirstTeam + " " + match.FirstTeamGamble + " " + match.SecondTeam + " " + match.SecondTeamGamble);
                        Console.WriteLine(match.MatchStats);
                    }
                    // bet/send message/dont know on arbitrage game (footballMatchesToBetOn)
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        public async void StartJobExecutionOnDailyGetAllFootballMatchFromScrapersAndCalculateArbitrage() {
            IJobDetail jobDetail = JobBuilder.Create<JobExecuter>()
                .WithIdentity("Arbitrager", "DailyGetAllFootballMatchFromScrapersAndCalculateArbitrage")
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("Arbitrager", "DailyBasis")
                .StartNow()
                .WithSimpleSchedule(x => x.WithIntervalInSeconds(5).WithRepeatCount(5))
                .Build();

            await m_schedueler.ScheduleJob(jobDetail, trigger);
        }

        public void OnFailConnection(object sender, string i_Url)
        {
            Console.WriteLine(i_Url + " Scraper couldn't connect and have been removed.");
            Console.WriteLine(string.Format($"To Remove The scraper use RemoveScraperToDict({0})", i_Url));
        }
    }
}
