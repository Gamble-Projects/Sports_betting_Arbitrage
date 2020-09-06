using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Quartz;
using Quartz.Impl;

namespace Arbitrage
{
    public class SystemManager
    {
        public enum eMenu { 
            Quit,
            AddScraperToDict,
            RemoveScraperFromDict,
            PrintAllNotConnectedUrl,
            StartJobExecutionOnDailyGetAllFootballMatchFromScrapersAndCalculateArbitrage
        }

        private readonly List<string> m_NotConnectedUrl = new List<string>();
        // key = url, value = scraper
        private readonly Dictionary<string, Scraper> r_Scrapers = new Dictionary<string, Scraper>();
        private IScheduler m_schedueler;
        private static bool v_StartedSceduleForScraper = false;

        public SystemManager()
        {
            m_schedueler = this.SchedulerConfig();
        }

        public void OpenSystemManagerForArbitrageFounder()
        {
            string[] menu;
            int userChoise;

            userChoise = UI.printMenuToUserToGetNextAction(menu);

            while (userChoise != (int)eMenu.Quit)
            {
                if (userChoise == (int)eMenu.AddScraperToDict) {
                    UI.GetScraperFromUser();
                }
                else if (userChoise == (int)eMenu.RemoveScraperFromDict) {
                    UI.GetUrlToOfScraper();
                }
                else if (userChoise == (int)eMenu.PrintAllNotConnectedUrl) {
                    UI.PrintListOfString();
                }
                else if (userChoise == (int)eMenu.StartJobExecutionOnDailyGetAllFootballMatchFromScrapersAndCalculateArbitrage) {
                    if (v_StartedSceduleForScraper == false) {
                        this.StartJobExecutionOnDailyGetAllFootballMatchFromScrapersAndCalculateArbitrage();
                    }
                    else {
                        UI.PrintInvalidInput();
                    }
                }
                else{
                    UI.PrintInvalidInput();
                }

                userChoise = UI.printMenuToUserToGetNextAction(menu);
            }
        }

        public bool AddScraperToDict(Scraper i_NewScraper)
        {
            bool v_ScraperAdded = false;

            // get connection
            //i_NewScraper.LoadUrl();
            //add event
            i_NewScraper.AddActionDelegate(this.OnFailConnection);
            // add to dict
            r_Scrapers.Add(i_NewScraper.WebsiteUrl, i_NewScraper);
            Console.WriteLine("at add scraper");
            return v_ScraperAdded;
        }

        public bool RemoveScraperFromDict(string i_Url)
        {
            bool v_ScraperBeenRemoved = false;

            // find i_Url
            if (r_Scrapers.ContainsKey(i_Url) == true)
            {
                r_Scrapers.Remove(i_Url);
            }
            else
            {
                Console.WriteLine("couldnt find " + i_Url);
            }
            // remove from dictionary
            Console.WriteLine("at remove scraper");
            return v_ScraperBeenRemoved;
        }
        /*
        // Do This Daily Basis ????
        public void DailyGetAllFootballMatchFromScrapersAndCalculateArbitrage()
        {
            List<FootballMatch> footballMatchesToBetOn = new List<FootballMatch>();
            Console.WriteLine("at list matches");
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
        */
        // in video youtube return Task<IActionResult> (https://www.youtube.com/watch?v=4HPY3Mk5TsA&list=PLSi1BNmQ61ZohCcl43UdAksg7X3_TSmly&index=9)
        public async void StartJobExecutionOnDailyGetAllFootballMatchFromScrapersAndCalculateArbitrage() {
            v_StartedSceduleForScraper = true;

            IJobDetail jobDetail = JobBuilder.Create<JobExecuter>()
                .WithIdentity("Arbitrager", "DailyGetAllFootballMatchFromScrapersAndCalculateArbitrage")
                .Build();

            jobDetail.JobDataMap.Put("DictOfScraper", this.r_Scrapers);

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("Arbitrager", "DailyBasis")
                .StartNow()
                .WithDailyTimeIntervalSchedule(x => x.StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(18, 21)).EndingDailyAt(TimeOfDay.HourAndMinuteOfDay(23, 59)).WithIntervalInMinutes(1))
                .Build();

            await m_schedueler.ScheduleJob(jobDetail, trigger);

            //return RedirectToAction("index");
        }

        public Quartz.IScheduler SchedulerConfig()
        {
            NameValueCollection props = new NameValueCollection {
            { "quartz.serializer.type","binary"}
        };

            StdSchedulerFactory factory = new StdSchedulerFactory(props);

            var scheduler = factory.GetScheduler().Result;

            scheduler.Start().Wait();

            return scheduler;
        }

        private void OnShutdown()
        {
            if (m_schedueler.IsShutdown == false)
            {
                m_schedueler.Shutdown();
            }
        }

        public void OnFailConnection(object sender, string i_Url)
        {
            Console.WriteLine(i_Url + " Scraper couldn't connect and have been removed."+ Environment.NewLine +
                "you can find the unable to connect url on NotConnectedUrl List");
            m_NotConnectedUrl.Add(i_Url);
            this.RemoveScraperFromDict(i_Url);
        }
    }
}
