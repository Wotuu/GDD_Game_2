using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XNAInputLibrary.KinectInput.Gestures
{

    public enum SwipeDirection
    {
        Up,
        Down,
        Left,
        Right
    }

    
    public class KinectSwipeEventArgs : EventArgs
    {
        public SwipeDirection swipedirection;
        public KinectSwipeEventArgs(SwipeDirection swipedirection)
        {
            this.swipedirection = swipedirection;
        }
    }
}
