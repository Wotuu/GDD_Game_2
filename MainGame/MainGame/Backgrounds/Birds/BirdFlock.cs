using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MainGame.Backgrounds.Birds
{
    public class BirdFlock
    {
        public LinkedList<Bird> birds = new LinkedList<Bird>();
        public FlyDirection direction { get; set; }

        public enum FlyDirection
        {
            LeftToRight,
            RightToLeft,
        }

        public BirdFlock(FlyDirection direction, int count)
        {
            this.direction = direction;

            for (int i = 0; i < count; i++)
            {
                birds.AddLast(new Bird(this, i));
            }
        }
    }
}
