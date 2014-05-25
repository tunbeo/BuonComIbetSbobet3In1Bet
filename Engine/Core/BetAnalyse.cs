using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iBet.DTO
{
    class BetAnalyse
    {
        public int Count
        {
            get;
            set;
        }
        public int Won
        {
            get;
            set;
        }
        public int Lose
        {
            get;
            set;
        }
        public int Draw
        {
            get;
            set;
        }
        public int Diff
        {
            get { return Won - Lose; }
        }
        public String OddType
        {
            get;
            set;
        }
        public bool Allow
        {
            get;
            set;
        }
        public bool isGoodOddToBet
        {
            get;
            set;
        }
        public float WinPercent
        {
            get { return (float)(Won + Draw / 2) / Count; }
        }
    }
}
