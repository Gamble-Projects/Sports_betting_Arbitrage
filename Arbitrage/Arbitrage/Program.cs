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
    class Program
    {
        private readonly static Dictionary<char, char> m_hebWord = new Dictionary<char, char>();
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
                doc.DocumentNode.Descendants()
                                .Where(n => n.Name == "script")
                                .ToList()
                                .ForEach(n => n.Remove());

                //const string classValue = "name ellipsis outcomedescription";
                const string classGame = "event-content sport_FOOT";
                const string classValueTeamName = "title ";
                const string classValueRtaio = "formatted_price";
                const string classStats = "statistics";
                const string classStatsURL = "stats-popup";
                HtmlNodeCollection nodesTeamNames = doc.DocumentNode.SelectNodes($"//*[@class='{"event"}']");
                int counter = 0;
                
                foreach(var node in nodesTeamNames)
                {
                    Console.WriteLine(node.GetType());
                    StringBuilder tempStringForNode = new StringBuilder();
                    var tempNameOfTeams = node.SelectNodes($"td/table/tbody/tr/td/div/div[@class='{classValueTeamName}']");
                
                    Array tempNameOfTeam1 = tempNameOfTeams[0].Attributes["title"].Value.Reverse().ToArray();
                    Array tempNameOfTeam2 = tempNameOfTeams[2].Attributes["title"].Value.Reverse().ToArray();
                   // foreach (char c in tempNameOfTeam)
                    //{
                      //  if (m_hebWord.ContainsKey(c))
                        //{
                          //  tempStringForNode.Append(c);
                            
                        //}
                    //}
                    
                    //byte [] bytes = Encoding.GetEncoding(862).GetBytes(node.InnerText);
                    //Console.WriteLine("אבא שלכם זונה");
                    //Console.WriteLine(Encoding.GetEncoding("Windows-1255").GetString(bytes));
                    //if(tempStringForNode.Length > 0)
                    //{
                        //counter++;
                    Console.Write(tempNameOfTeam1);
                        //var node_temp = node.SelectSingleNode($"//*[@class='{"price price_147248478 priced"}']");
                        //var num = node.SelectSingleNode($"//*[@class='{"formatted_price"}']");
                        
                        //if (counter == 1)
                        //{
                    Console.Write(tempNameOfTeams[0].SelectSingleNode($"*[@class='{classValueRtaio}']").InnerText);
                    Console.Write(" vs ");
                    Console.Write(tempNameOfTeam2);
                    //}
                    //else if (counter == 2)
                    //{
                    Console.Write(tempNameOfTeams[1].SelectSingleNode($"*[@class='{classValueRtaio}']").InnerText);
                    Console.WriteLine("");
                            //counter = 0;
                           
                        //}
                    //}
                  
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

}