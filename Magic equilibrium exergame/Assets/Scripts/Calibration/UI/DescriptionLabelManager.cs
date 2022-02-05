using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Calibration.UI
{
    public class DescriptionLabelManager : LabelManager
    {
        protected override Dictionary<CalibrationPhase, string> CreateDescriptions() => new Dictionary<CalibrationPhase, string>
        {
            { CalibrationPhase.Start, "Before start, we need to calibrate the board" },
            { CalibrationPhase.ForwardAngle, "Incline the board forward, at the max of your possibility" },
            { CalibrationPhase.BackwardAngle, "Incline the board backward, at the max of your possibility" },
            { CalibrationPhase.HorizontalAngle, "Incline the board right, at the max of your possibility" },
            { CalibrationPhase.End, "We can start now" },
        };
    }
}
