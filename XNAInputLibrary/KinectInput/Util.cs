using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XNAInputLibrary
{
    internal static class Util
    {
        public static Vector3 ScaleTo(this Vector3 joint, int width, int height, float skeletonMaxX, float skeletonMaxY)
        {
            Vector3 pos = new Vector3()
            {
                X = Scale(width, skeletonMaxX, joint.X),
                Y = Scale(height, skeletonMaxY, -joint.Y),
                Z = joint.Z,
            };



            return pos;
        }


        private static float Scale(int maxPixel, float maxSkeleton, float position)
        {
            float value = ((((maxPixel / maxSkeleton) / 2) * position) + (maxPixel / 2));
            if (value > maxPixel)
                return maxPixel;
            if (value < 0)
                return 0;
            return value;
        }
    }
}
