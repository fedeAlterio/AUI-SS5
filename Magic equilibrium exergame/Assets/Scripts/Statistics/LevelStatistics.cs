using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Statistics
{
    public class LevelStatistics : ILevelStatistics
    {
        // Initialization        
        public static LevelStatistics FromOther (ILevelStatistics other)
        {
            var ret = new LevelStatistics ();
            foreach(var property in typeof(ILevelStatistics).GetProperties())
            {
                var value = property.GetValue(other);
                property.SetValue(ret, value);
            }
            return ret;
        }



        // Properties
        public int DeathsCount { get; set; }
    }
}
