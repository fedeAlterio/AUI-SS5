using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Statistics
{
    public interface ILevelStatistics
    {
        int DeathsCount { get; set; }
        public int SecondsCount { get; set; }
    }
}
