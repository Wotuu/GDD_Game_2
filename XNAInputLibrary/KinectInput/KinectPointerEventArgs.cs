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
        public Vector3 PreviousRightHandPosition;
        public Vector3 PreviousLeftHandPosition;
        public int PlayerID;
        public SkeletonData SkeletonData;

        public KinectPointerEventArgs(Vector3 righthand, Vector3 lefthand,Vector3 previousrighthand,Vector3 previouslefthand,SkeletonData data,int player)
        {
            this.RightHandPosition = righthand;
            this.LeftHandPosition = lefthand;
            this.PreviousLeftHandPosition = previouslefthand;
            this.PreviousRightHandPosition = previousrighthand;
            this.SkeletonData = data;
            this.PlayerID = player;
        }


    }
}
