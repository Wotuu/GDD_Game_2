using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Research.Kinect;
using Microsoft.Research.Kinect.Nui;
using XNAInputLibrary.KinectInput;
using Microsoft.Xna.Framework;



namespace XNAInputLibrary.KinectInput
{
    public class KinectManager
    {
        #region Singleton Logic
        /// <summary>
        /// Singleton Logic
        /// </summary>
        private static KinectManager instance;
        public static KinectManager GetInstance()
        {
            if (instance == null)
            {
                instance = new KinectManager();
            }
            return instance;
        }
        #endregion

        Runtime nui;

        #region Hands
        Vector3 RightHandPosition;
        Vector3 LeftHandPosition;
        Vector3 PreviousRightHandPosition;
        Vector3 PreviousLeftHandPosition;
        #endregion

        #region delegates
        public delegate void KinectPointerMoved(object sender, KinectPointerEventArgs e);
        #endregion
        /// <summary>
        /// Initialize the NUI
        /// </summary>
        private KinectManager()
        {
            nui = new Runtime();

            try
            {
                nui.Initialize(RuntimeOptions.UseDepthAndPlayerIndex | RuntimeOptions.UseSkeletalTracking | RuntimeOptions.UseColor);
            }
            catch (InvalidOperationException)
            {
                throw new Exception("Runtime initialization failed. Please make sure Kinect device is plugged in.");
            }


            nui.DepthStream.Open(ImageStreamType.Depth, 2, ImageResolution.Resolution320x240, ImageType.DepthAndPlayerIndex);
            nui.VideoStream.Open(ImageStreamType.Video, 2, ImageResolution.Resolution640x480, ImageType.Color);
            nui.SkeletonEngine.TransformSmooth = true;
            nui.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(nui_SkeletonFrameReady);
        }

        void nui_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            SkeletonFrame skeletonframe = e.SkeletonFrame;
            Joint right;
            Joint left;
            int playerID = 0;
            foreach (SkeletonData data in skeletonframe.Skeletons)
            {
                if (SkeletonTrackingState.Tracked == data.TrackingState)
                {
                   
                    //HANDS
                    right = data.Joints[JointID.HandRight];
                    left = data.Joints[JointID.HandLeft];
                    PreviousRightHandPosition = RightHandPosition;
                    PreviousLeftHandPosition = LeftHandPosition;

                    RightHandPosition = new Vector3(right.Position.X, right.Position.Y, right.Position.Z);
                    LeftHandPosition = new Vector3(left.Position.X, left.Position.Y, left.Position.Z);
                    if (PreviousLeftHandPosition != null && PreviousRightHandPosition != null)
                    {
                        if (!LeftHandPosition.Equals(PreviousLeftHandPosition) && !RightHandPosition.Equals(PreviousRightHandPosition))
                        {
                            RaisePointerMoved(new KinectPointerEventArgs(RightHandPosition, LeftHandPosition,data,playerID));
                        }
                    }
                    else
                    {
                        //Initial always raise
                        RaisePointerMoved(new KinectPointerEventArgs(RightHandPosition, LeftHandPosition,data,playerID));
                    }
                }

                playerID++;
            }
        }

        #region events
        public event KinectPointerMoved PointerMoved;
        protected virtual void RaisePointerMoved(KinectPointerEventArgs EventArgs)
        {
            if (EventArgs != null && PointerMoved != null)
            {
                PointerMoved(this, EventArgs);
            }
        }
        #endregion
    }
}
