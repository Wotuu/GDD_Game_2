using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Research.Kinect;
using Microsoft.Research.Kinect.Nui;
using XNAInputLibrary.KinectInput;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XNAInputLibrary.KinectInput.Gestures;



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
        SwipeDetector SwipeDetector;
        bool haslock = false;
        bool PreviousLockState = false;
        #region Arms
        Vector3 RightHandPosition;
        Vector3 LeftHandPosition;
        Vector3 PreviousRightHandPosition;
        Vector3 PreviousLeftHandPosition;


        
        #endregion


        #region scaling
        Viewport viewport;
        float MaxY;
        float MaxX;
        #endregion

        #region delegates
        public delegate void KinectPointerMoved(object sender, KinectPointerEventArgs e);
        public delegate void KinectSwipeDetected(object sender, KinectSwipeEventArgs e);
        #endregion
        /// <summary>
        /// Initialize the NUI
        /// </summary>
        /// 

        private KinectManager()
        {
           SwipeDetector  = new SwipeDetector();
        }

        public void Initialize(Viewport viewport,float maxx,float maxy)
        {
            nui = new Runtime();
            this.viewport = viewport;
            this.MaxX = maxx;
            this.MaxY = maxy;
            try
            {
                nui.Initialize(RuntimeOptions.UseDepthAndPlayerIndex | RuntimeOptions.UseSkeletalTracking | RuntimeOptions.UseColor);
            }
            catch (InvalidOperationException)
            {
                //throw new Exception("Runtime initialization failed. Please make sure Kinect device is plugged in.");
                return;
            }


            nui.DepthStream.Open(ImageStreamType.Depth, 2, ImageResolution.Resolution320x240, ImageType.DepthAndPlayerIndex);
            nui.VideoStream.Open(ImageStreamType.Video, 2, ImageResolution.Resolution640x480, ImageType.Color);

            TransformSmoothParameters parameters = new TransformSmoothParameters
            {
                Smoothing = .5f,
                Correction = 0.5f,
                Prediction = 0.5f,
                JitterRadius = 0.05f,
                MaxDeviationRadius = 0.04f
            };




            //nui.SkeletonEngine.SmoothParameters = parameters;
            nui.SkeletonEngine.TransformSmooth = true;
            nui.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(nui_SkeletonFrameReady);
        }

        void nui_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            SkeletonFrame skeletonframe = e.SkeletonFrame;
            
            int playerID = 0;
            haslock = false;
            foreach (SkeletonData data in skeletonframe.Skeletons)
            {
               
                if (SkeletonTrackingState.Tracked == data.TrackingState)
                {
                    haslock = true;
                    CheckHandVectors(data, playerID);
                    CalculateElbowRotation(data, playerID);
                    //Check for a Hit

                }
                
               

                

                playerID++;
            }

            if (!PreviousLockState && haslock)
            {
                Console.WriteLine("Got a Lock");
                PreviousLockState = true;
            }
            else if (PreviousLockState && !haslock)
            {
                Console.WriteLine("Lost lock");
                PreviousLockState = false;
            }
        }

        public void CheckHandVectors(SkeletonData data,int playerid)
        {
            Joint right;
            Joint left;

            //HANDS
            right = data.Joints[JointID.HandRight];
            left = data.Joints[JointID.HandLeft];
            PreviousRightHandPosition = RightHandPosition;
            PreviousLeftHandPosition = LeftHandPosition;

            RightHandPosition = new Vector3(right.Position.X, right.Position.Y, right.Position.Z);
            LeftHandPosition = new Vector3(left.Position.X, left.Position.Y, left.Position.Z);
            SwipeDetector.Add(RightHandPosition);
            if (PreviousLeftHandPosition != null && PreviousRightHandPosition != null)
            {
                if (!LeftHandPosition.Equals(PreviousLeftHandPosition) && !RightHandPosition.Equals(PreviousRightHandPosition))
                {
                    RaisePointerMoved(CreateEventArgs(data, playerid));
                }
            }
            else
            {
                //Initial always raise
                RaisePointerMoved(CreateEventArgs(data, playerid));

            }
        }
        public void CalculateElbowRotation(SkeletonData data, int playerid)
        {
            Joint elbowright = data.Joints[JointID.ElbowRight];
            Vector3 elbow = new Vector3(elbowright.Position.X,elbowright.Position.Y,elbowright.Position.Z);

            Joint shoulderright = data.Joints[JointID.ShoulderRight];
            Vector3 shoulder = new Vector3(shoulderright.Position.X, shoulderright.Position.Y, shoulderright.Position.Z);
            //Dot Product
            Vector3 b1 = elbow - RightHandPosition;
            Vector3 b2 = elbow - shoulder;

           // Console.Write(AngleBetweenTwoVectors(b1, b2));


        }

        public float AngleBetweenTwoVectors(Vector3 a, Vector3 b)
        {

            float dotproduct = 0.0f;
            dotproduct = Vector3.Dot(Vector3.Normalize(a), Vector3.Normalize(b));
            return (float)Math.Acos(dotproduct);
        }

        public KinectPointerEventArgs CreateEventArgs(SkeletonData data,int playerid)
        {
            KinectPointerEventArgs eventargs;
            eventargs = new KinectPointerEventArgs(RightHandPosition.ScaleTo(viewport.Width, viewport.Height, MaxX, MaxY),
                LeftHandPosition.ScaleTo(viewport.Width, viewport.Height, MaxX, MaxY),
                PreviousRightHandPosition.ScaleTo(viewport.Width,viewport.Height,MaxX,MaxY),
                PreviousLeftHandPosition.ScaleTo(viewport.Width,viewport.Height,MaxX,MaxY),
                data,
                playerid);
            return eventargs;
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

        public event KinectSwipeDetected SwipeDetected;
        public virtual void RaiseSwipeDetected(KinectSwipeEventArgs EventArgs)
        {
            if (EventArgs != null && SwipeDetected != null)
            {
                SwipeDetected(this, EventArgs);
            }
        }
        #endregion
    }
}
