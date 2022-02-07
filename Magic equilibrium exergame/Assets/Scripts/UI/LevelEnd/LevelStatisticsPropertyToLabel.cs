using Assets.Scripts.Path.BuildingStrategies;
using Assets.Scripts.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.UI.LevelEnd
{
    public class LevelStatisticsPropertyToLabel : PropertyToLabelBinding
    {
        private void Start()
        {
            Object = this.GetInstance<ILevelStatistics>();
        }
    }
}
