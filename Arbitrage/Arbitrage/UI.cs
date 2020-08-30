using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arbitrage
{
    static class UI
    {
        private static Dictionary<char, char> m_hebWord = new Dictionary<char, char>();

        static UI()
        {
            Console.OutputEncoding = Encoding.GetEncoding("Windows-1255");
            makeDictOfHebWord();
        }

        private static void makeDictOfHebWord()
        {
            string hebChar = " .ףץםןאבגדהוזחטיכלמנסעפצקרשת";

            foreach (char c in hebChar)
            {
                m_hebWord.Add(c, c);
            }

        }

        public static string ArrangeHebStringToBeHebUICustomize(string UnArrangeHebString)
        {
            StringBuilder tempString = new StringBuilder();
            Array arrayOFUnArrangeHebStringReversed = UnArrangeHebString.Reverse().ToArray();

            foreach (char c in arrayOFUnArrangeHebStringReversed)
            {
                if (m_hebWord.ContainsKey(c))
                {
                    tempString.Append(c);
                }
            }

            return tempString.ToString();
        }
    }
}
