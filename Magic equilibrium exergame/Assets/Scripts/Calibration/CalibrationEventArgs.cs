using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Calibration
{

    public class CalibrationEventArgs
    {
        public CalibrationEventArgs(CalibrationPhase calibrationPhase, int secondsLeft)
        {
            CalibrationPhase = calibrationPhase;
            PhaseLength = secondsLeft;
        }

        public CalibrationPhase CalibrationPhase { get; }
        public int PhaseLength { get; }
    }
}
