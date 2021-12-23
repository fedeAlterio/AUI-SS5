using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Models.Path.Generation.Line
{
    public static class TextureUtils
    {
        public static IEnumerable<(int x, int y)> GetBorderPoints(Texture2D texture)
        {
            foreach(var (i,j) in GetPoints(texture))
                if (IsBorderPixel(texture, i, j))
                    yield return (i, j);
        }


        public static IEnumerable<(int x, int y)> GetPoints(Texture2D texture)
        {
            for (var i = 0; i < texture.width; i++)
                for (var j = 0; j < texture.height; j++)                    
                    yield return (i, j);
        }


        public static Texture2D GetBorderAsTexture(Texture2D texture)
        {
            var ret = new Texture2D(texture.width, texture.height);
            foreach (var (x, y) in GetPoints(texture))
                ret.SetPixel(x, y, Color.white);
            foreach (var (x, y) in GetBorderPoints(texture))
                ret.SetPixel(x, y, Color.black);
            ret.Apply();
            return ret;
        }



        // Utils
        private static bool IsBlack(Color color)
        {
            var epsilon = 0.3f;
            return color.r < epsilon && color.g < epsilon && color.b < epsilon;
        }

        private static bool IsBorderPixel(Texture2D texture, int x, int y)
        {
            var pixel = texture.GetPixel(x, y);
            if (!IsBlack(pixel))
                return false;

            if (x == 0 || x == texture.width - 1)
                return true;

            if (y == 0 || y == texture.height - 1)
                return true;

            var pixels = new[]
            {
                texture.GetPixel(x+1, y), texture.GetPixel(x-1, y),
                texture.GetPixel(x, y+1), texture.GetPixel(x, y-1),
                texture.GetPixel(x+1, y+1), texture.GetPixel(x+1, y-1),
                texture.GetPixel(x-1, y+1), texture.GetPixel(x-1, y-1),
            };
            return pixels.Where(p => !IsBlack(p)).Count() >= 2;
        }
    }
}
