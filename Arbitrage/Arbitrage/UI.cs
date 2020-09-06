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

        public static int printMenuToUserToGetNextAction(string[] i_Menu)
        {
            int i = 0;
            string inputFromUser;
            int actionUserChose = 0;
            bool v_ValidInput = false;

            while (v_ValidInput == false)
            {
                foreach (string action in i_Menu)
                {
                    Console.WriteLine(string.Format($"To {0} press {1}"), action, i);
                    i++;
                }

                inputFromUser = Console.ReadLine();

                if (int.TryParse(inputFromUser, out actionUserChose) == true)
                {
                    v_ValidInput = true;
                }
                else
                {
                    Console.WriteLine("invalid Input");
                    Console.Clear();
                }
            }

            return actionUserChose;
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
            Array arrayOfUnArrangeHebStringReversed = UnArrangeHebString.Reverse().ToArray();

            foreach (char c in arrayOfUnArrangeHebStringReversed)
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
