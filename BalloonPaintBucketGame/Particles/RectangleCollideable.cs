using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BalloonPaintBucketGame.Particles
{
    public interface RectangleCollideable
    {
        Rectangle GetCollisionRectangle();
        Boolean CheckCollision(RectangleCollideable collideable);
        void OnCollision(RectangleCollideable collidedWith);
    }
}
