using Assets.Scripts.Path.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Environment.ObjectSpawner
{
    public class PathSpawner : CurveSpawner
    {
        private void Awake()
        {
            var pathGenerator = FindObjectOfType<PathGenerator>();
            pathGenerator.PathGenerated += GenerateObjects;
        }
    }
}
