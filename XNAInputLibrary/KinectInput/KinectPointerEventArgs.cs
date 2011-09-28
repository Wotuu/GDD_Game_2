using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Research.Kinect.Nui;

namespace XNAInputLibrary.KinectInput
{
    public class KinectPointerEventArgs : EventArgs
    {
        public Vector3 RightHandPosition;
        public Vector3 LeftHandPosition;
        public int PlayerID;
        public SkeletonData SkeletonData;

        public KinectPointerEventArgs(Vector3 righthand, Vector3 lefthand,SkeletonData data,int player)
        {
            this.RightHandPosition = righthand;
            this.LeftHandPosition = lefthand;
            this.SkeletonData = data;
            this.PlayerID = player;
        }
    }
}
