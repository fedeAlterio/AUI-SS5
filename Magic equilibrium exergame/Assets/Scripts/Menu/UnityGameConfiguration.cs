using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class MyConfig : GameConfiguration
{
    // Properties
    [PropertyRange(1, 10)]
    [PropertyRename("Length")]
    public int PathLength { get; set; }


    [PropertyRange(1, 3)]
    [PropertyDefaultValue(2)]
    public int Difficulty { get; set; }



    public bool PathWithHoles { get; set; }


    public override string ToString()
    {
        throw new NotImplementedException();
    }
}
