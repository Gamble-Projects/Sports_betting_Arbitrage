using System;

namespace Arbitrage
{
    public class ScraperWinner: Scraper
    {
        private static readonly string sr_HtmlClassValueForTeamName = "title ";
        private static readonly string sr_HtmlClassValueForRatio = "formatted_price";

        public ScraperWinner(string WebsiteUrl): base(WebsiteUrl){}

        public override void LoadUrl()
        {
            try
            {
                HtmlDocument = HtmlWeb.Load(WebsiteUrl);
            }
            catch
            {

            }
            //throw new NotImplementedException();
        }
    }
}
