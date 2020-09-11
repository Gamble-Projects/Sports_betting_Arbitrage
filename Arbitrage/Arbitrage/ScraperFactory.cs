using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arbitrage
{
    public static class ScraperFactory
    {
        private static readonly StringBuilder m_Menu = new StringBuilder();

        static ScraperFactory()
        {
            buildMenu();
        }

        private static void buildMenu()
        {
            m_Menu.AppendLine(eTypeOfScrapers.Winner.ToString());
        }

        public enum eTypeOfScrapers
        {
            Winner
        }

        public static Scraper MakeNewScpraper(string i_Url)
        {
            Scraper newScraper = null;
            eTypeOfScrapers userChoice;

            userChoice = (eTypeOfScrapers)UI.printMenuToUserToGetNextAction(m_Menu);

            if(userChoice == eTypeOfScrapers.Winner)
            {
                newScraper = new ScraperWinner(i_Url);
            }
            else
            {
                UI.PrintInvalidInput();
            }

            return newScraper;
        }
    }
}
