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
                const string url = "https://www.winner.co.il/mainbook/sport-%D7%9B%D7%93%D7%95%D7%A8%D7%92%D7%9C";
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

                const string classValue = "name ellipsis outcomedescription";
                var nodes = doc.DocumentNode.SelectNodes($"//*[@class='{classValue}']") ?? Enumerable.Empty<HtmlNode>();
                int i = 0;

                foreach (var node in nodes)
                {
                    StringBuilder tempStringForNode = new StringBuilder();
                    Array tempNameOfTeam = node.InnerText.Reverse().ToArray();

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
                        i++;
                        Console.Write(tempStringForNode);
                        const string classValuetemp = "formatted_price";
                        //var tempnode = doc.DocumentNode.SelectNodes($"//*[@class='{classValuetemp}']") ?? Enumerable.Empty<HtmlNode>();
                        Console.WriteLine(node.SelectNodes($"//*[@class='{classValue}']") ?? Enumerable.Empty<HtmlNode>());
                        if (i==1)
                        {
                            Console.Write(" vs ");
                        }
                        else if (i == 2)
                        {
                            Console.WriteLine("");
                            i = 0;
                        }
                    }

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