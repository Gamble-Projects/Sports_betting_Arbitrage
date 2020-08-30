using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Arbitrage
{
    public class gambles_ratio
    {
        
        int first_gamble;
        int second_gamble;
        public gambles_ratio(int gamble_1, int gamble_2)
        {
            this.first_gamble = gamble_1;
            this.second_gamble = gamble_2;
        }
        
    }

    public static class Arbaitrager
    {
        //private static bool arbitrage_exist;
        private static int IAP_1;
        private static int IAP_2;

        private static int IAP(int odd)
        {
            return ((1 / odd) * 100);
        }

        private static bool is_arbitrage(int iap_1, int iap_2)
        {
            return ((iap_1 + iap_2) > 100);
        }

        public static gambles_ratio gambling_ratio(int odd_1, int odd_2)
        {
            IAP_1 = IAP(odd_1);
            IAP_1 = IAP(odd_2);


            return new gambles_ratio(IAP_1 / (IAP_1 + IAP_2), IAP_2 / (IAP_1 + IAP_2));
        }


    }
}
    
