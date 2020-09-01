using Arbitrage;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace WebScraper
{
    public class Program
    {
        public static void Main() {
            SystemManager manager = new SystemManager();

            ScraperWinner winner = new ScraperWinner("https://www.winner.co.il/mainbook/sport-%D7%9B%D7%93%D7%95%D7%A8%D7%92%D7%9C?&marketTypePeriod=1%7C100");

            manager.AddScraperToDict(winner);

            manager.DailyGetAllFootballMatchFromScrapersAndCalculateArbitrage();

            Console.ReadLine();
        }
    }
    /*
    class Program
    {
        private static Dictionary<char, char> m_hebWord = new Dictionary<char, char>();

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.GetEncoding("Windows-1255");
            makeDictOfHebWord();
            WebDataScrap();

        }

        public static void WebDataScrap()
        {
            try
            {
                //Get the content of the URL from the Web
                const string url = "https://www.winner.co.il/mainbook/sport-%D7%9B%D7%93%D7%95%D7%A8%D7%92%D7%9C?&marketTypePeriod=1%7C100";
                var web = new HtmlWeb();

                //web.OverrideEncoding = Encoding.GetEncoding(862);
                var doc = web.Load(url);

                //Get the content from a file
                //var path = "countries.html";
                //var doc = new HtmlDocument();
                //doc.Load(path);

                //Filter the content
                doc.DocumentNode.Descendants().Where(n => n.Name == "script").ToList() .ForEach(n => n.Remove());

                //const string classValue = "name ellipsis outcomedescription";
                //const string MainTable = "rollup market_type market_type_id_1 period_id_100 win_draw_win multi_event game_type rollup-down";
                const string classValueTeamName = "title ";
                const string classValueRtaio = "formatted_price";
                HtmlNodeCollection nodesTeamNames = doc.DocumentNode.SelectNodes($"//*[@class='{classValueTeamName}']");
                HtmlNodeCollection nodeRatioss = doc.DocumentNode.SelectNodes($"//*[@class='{classValueRtaio}']") ;
                int counter = 0;
                int j;

                
                for(int i = 0; i < nodesTeamNames.Count; i++)
                {
                    StringBuilder tempStringForNode = new StringBuilder();
                    Array tempNameOfTeam = nodesTeamNames[i].Attributes["title"].Value.Reverse().ToArray();

                    //Array tempNameOfTeam = node.InnerText.Reverse().ToArray();

                    foreach (char c in tempNameOfTeam)
                    {
                        if (m_hebWord.ContainsKey(c))
                        {
                            tempStringForNode.Append(c);
                        }
                    }

                    //byte [] bytes = Encoding.GetEncoding(862).GetBytes(node.InnerText);
                    //Console.WriteLine("אבא שלכם זונה");
                    //Console.WriteLine(Encoding.GetEncoding("Windows-1255").GetString(bytes));
                    if(tempStringForNode.Length > 0)
                    {
                        counter++;
                        Console.Write(tempStringForNode);
                        //var node_temp = node.SelectSingleNode($"//*[@class='{"price price_147248478 priced"}']");
                        //var num = node.SelectSingleNode($"//*[@class='{"formatted_price"}']");

                        if (counter == 1)
                        {
                            Console.Write(nodeRatioss[i].InnerText);
                            Console.Write(" vs ");
                        }
                        else if (counter == 2)
                        {
                            Console.Write(nodeRatioss[i].InnerText);
                            Console.WriteLine("");
                            counter = 0;

                        }
                    }

                }
                

                for (int i = 0; i < nodesTeamNames.Count; i += 3)
                {
                    string tempNameOfFirstTeam = UI.ArrangeHebStringToBeHebUICustomize(nodesTeamNames[i].Attributes["title"].Value);
                    string tempNameOfSecondTeam = UI.ArrangeHebStringToBeHebUICustomize(nodesTeamNames[i + 2].Attributes["title"].Value);

                    float tempRatioOfFirstTeam = float.Parse(nodeRatioss[i].InnerText);
                    float tempRatioOfSecondTeam = float.Parse(nodeRatioss[i + 2].InnerText);

                    Console.WriteLine(tempNameOfFirstTeam + " " + tempRatioOfFirstTeam + " " + tempNameOfSecondTeam + " " + tempRatioOfSecondTeam);
                }


                Console.WriteLine("\r\nPlease press a key...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured:\r\n{ex.Message}");
            }
        }
        private static void makeDictOfHebWord()
        {
            string hebChar = " .ףץםןאבגדהוזחטיכלמנסעפצקרשת";

            foreach (char c in hebChar)
            {
                m_hebWord.Add(c, c);
            }

        }
    }
*/
}