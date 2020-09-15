using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
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
            PrintAllScrapers,
            PrintAllNotConnectedUrl,
            StartJobExecutionOnDailyGetAllFootballMatchFromScrapersAndCalculateArbitrage
        }

        private IScheduler m_schedueler;
        private readonly List<string> m_NotConnectedUrl = new List<string>();
        private readonly Dictionary<string, Scraper> r_Scrapers = new Dictionary<string, Scraper>(); // key = url, value = scraper
        private static bool v_StartedSceduleForScraper = false;

        public SystemManager()
        {
            m_schedueler = this.SchedulerConfig();
        }

        public void OpenSystemManagerForArbitrageFounder()
        {
            string userChoise;
            StringBuilder menu = buildMainMenuFromeMemu();

            userChoise = earaseSpaceInString(UI.printMenuToUserToGetNextAction(menu));

            while (userChoise != eMenu.Quit.ToString())
            {
                if (userChoise == eMenu.AddScraperToDict.ToString()) {
                    getScraperFromUserAndAdd();
                }
                else if (userChoise == eMenu.RemoveScraperFromDict.ToString()) {
                    getUrlToOfScraper();
                }
                else if (userChoise == eMenu.PrintAllScrapers.ToString())
                {
                    printAllScapers();
                }
                else if (userChoise == eMenu.PrintAllNotConnectedUrl.ToString()) {
                    printNotConnectedScapers();
                }
                else if (userChoise == eMenu.StartJobExecutionOnDailyGetAllFootballMatchFromScrapersAndCalculateArbitrage.ToString()) {
                    if (v_StartedSceduleForScraper == false) {
                        this.StartJobExecutionOnDailyGetAllFootballMatchFromScrapersAndCalculateArbitrage();
                        menu.Remove(menu.Length - eMenu.StartJobExecutionOnDailyGetAllFootballMatchFromScrapersAndCalculateArbitrage.ToString().Length - 3, eMenu.StartJobExecutionOnDailyGetAllFootballMatchFromScrapersAndCalculateArbitrage.ToString().Length + 3);
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

        private bool AddScraperToDict(Scraper i_NewScraper)
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

        private bool RemoveScraperFromDict(string i_Url)
        {
            bool v_ScraperBeenRemoved = false;

            // find i_Url
            if (r_Scrapers.ContainsKey(i_Url) == true)
            {
                v_ScraperBeenRemoved = true;
                r_Scrapers.Remove(i_Url);
            }
            else
            {
                v_ScraperBeenRemoved = false;
                Console.WriteLine("couldnt find " + i_Url);
            }
            // remove from dictionary
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
        private async void StartJobExecutionOnDailyGetAllFootballMatchFromScrapersAndCalculateArbitrage() {
            v_StartedSceduleForScraper = true;

            IJobDetail jobDetail = JobBuilder.Create<JobExecuter>()
                .WithIdentity("Arbitrager", "DailyGetAllFootballMatchFromScrapersAndCalculateArbitrage")
                .Build();

            jobDetail.JobDataMap.Put("DictOfScraper", this.r_Scrapers);

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("Arbitrager", "DailyBasis")
                .StartNow()
                .WithDailyTimeIntervalSchedule(x => x.StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(00, 21)).EndingDailyAt(TimeOfDay.HourAndMinuteOfDay(23, 59)).WithIntervalInMinutes(1))
                .Build();

            await m_schedueler.ScheduleJob(jobDetail, trigger);

            //return RedirectToAction("index");
        }

        private Quartz.IScheduler SchedulerConfig()
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

        private void OnFailConnection(object sender, string i_Url)
        {
            Console.WriteLine(i_Url + " Scraper couldn't connect and have been removed."+ Environment.NewLine +
                "you can find the unable to connect url on NotConnectedUrl List");
            m_NotConnectedUrl.Add(i_Url);
            this.RemoveScraperFromDict(i_Url);
        }

        private void getScraperFromUserAndAdd()
        {
            Scraper userScraper = UI.CreateNewScraperWithUser();

            if (userScraper != null)
            {
                this.AddScraperToDict(userScraper);
            }
        }

        private void getUrlToOfScraper()
        {
            System.Text.StringBuilder menuOfUrlToRempove = new System.Text.StringBuilder();

            buildMenuOfUrlToRemove(menuOfUrlToRempove);

            string urlToRemove = UI.printMenuToUserToGetNextAction(menuOfUrlToRempove);
            bool v_ScraperRemoved = this.RemoveScraperFromDict(urlToRemove);

            if (v_ScraperRemoved == false)
            {
                UI.PrintInvalidInput();
            }
        }

        private void buildMenuOfUrlToRemove(StringBuilder menuOfUrlToRempove)
        {
            foreach(string url in this.r_Scrapers.Keys)
            {
                menuOfUrlToRempove.Append("Remove" + url);
                menuOfUrlToRempove.Append('\n');
            }
        }

        private void printAllScapers()
        {
            System.Text.StringBuilder urlOfScrapers = new System.Text.StringBuilder();

            foreach(string url in r_Scrapers.Keys)
            {
                urlOfScrapers.AppendLine(url);
            }

            UI.PrintString(urlOfScrapers.ToString());
        }

        private void printNotConnectedScapers()
        {
            System.Text.StringBuilder urlOfNotConnectedScrapers = new System.Text.StringBuilder();

            foreach (string url in this.m_NotConnectedUrl)
            {
                urlOfNotConnectedScrapers.AppendLine(url);
            }

            UI.PrintString(urlOfNotConnectedScrapers.ToString());
        }

        private StringBuilder buildMainMenuFromeMemu()
        {
            StringBuilder newMenu = new StringBuilder();

            newMenu.Append(seperateLineBySpaceBeforeCapitalLetter(eMenu.Quit.ToString()));
            newMenu.Append('\n');
            newMenu.Append(seperateLineBySpaceBeforeCapitalLetter(eMenu.AddScraperToDict.ToString()));
            newMenu.Append('\n');
            newMenu.Append(seperateLineBySpaceBeforeCapitalLetter(eMenu.RemoveScraperFromDict.ToString()));
            newMenu.Append('\n');
            newMenu.Append(seperateLineBySpaceBeforeCapitalLetter(eMenu.PrintAllScrapers.ToString()));
            newMenu.Append('\n');
            newMenu.Append(seperateLineBySpaceBeforeCapitalLetter(eMenu.PrintAllNotConnectedUrl.ToString()));
            newMenu.Append('\n');
            newMenu.Append(seperateLineBySpaceBeforeCapitalLetter(eMenu.StartJobExecutionOnDailyGetAllFootballMatchFromScrapersAndCalculateArbitrage.ToString()));

            return newMenu;
        }

        private static string seperateLineBySpaceBeforeCapitalLetter(string i_Line)
        {
            StringBuilder newSeperatedLine = new StringBuilder();

            foreach (char c in i_Line)
            {
                if (c >= 'A' && c <= 'Z' && newSeperatedLine.Length != 0)
                {
                    newSeperatedLine.Append(' ');
                }

                newSeperatedLine.Append(c);
            }

            return newSeperatedLine.ToString();
        }

        private static string earaseSpaceInString(string i_Line)
        {
            StringBuilder newLineWithoutSpaces = new StringBuilder();

            foreach (char c in i_Line)
            {
                if (c != ' ')
                {
                    newLineWithoutSpaces.Append(c);
                }
            }

            return newLineWithoutSpaces.ToString();
        }
    }
}
