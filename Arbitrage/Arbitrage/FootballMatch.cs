using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Arbitrage
{
    public class FootballMatch
    {
        private string m_FirstTeam;
        private string m_SecondTeam;
        private int m_FirstTeamRatio;
        private int m_SecondTeamRatio;
        private int m_FirstTeamIAP = 0;
        private int m_SecondTeamIAP = 0;
        private int m_FirstTeamGamble = 0;
        private int m_SecondTeamGamble = 0;

        public string FirstTeam
        {
            get { return m_FirstTeam; }
        }
        public string SecondTeam
        {
            get { return m_SecondTeam; }
        }
        public int FirstTeamRatio
        {
            get { return m_FirstTeamRatio; }
        }
        public int SecondTeamRatio
        {
            get { return m_SecondTeamRatio; }
        }
        public int FirstTeamGamble
        {
            get { return m_FirstTeamGamble; }
            set { m_FirstTeamGamble = value; }
        }
        public int SecondTeamGamble
        {
            get { return m_SecondTeamGamble; }
            set { m_SecondTeamGamble = value; }
        }
        public int FirstTeamIAP
        {
            get { return m_FirstTeamIAP; }
            set { m_FirstTeamIAP = value; }
        }
        public int SecondTeamIAP
        {
            get { return m_SecondTeamIAP; }
            set { m_SecondTeamIAP = value; }
        }
        public FootballMatch(string FirstTeam, int FirstTeamRatio, string SecondTeam, int SecondTeamRatio)
        {
            m_FirstTeam = FirstTeam;
            m_FirstTeamRatio = FirstTeamRatio;
            m_SecondTeam = SecondTeam;
            m_SecondTeamRatio = SecondTeamRatio;
        }
    }
}
