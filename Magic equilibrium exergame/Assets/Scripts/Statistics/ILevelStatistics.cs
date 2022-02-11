﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Statistics
{
    public interface ILevelStatistics
    {
        int DeathsCount { get; set; }
        int SecondsCount { get; set; }
        int ExercizesSkipped { get; }
    }
}
