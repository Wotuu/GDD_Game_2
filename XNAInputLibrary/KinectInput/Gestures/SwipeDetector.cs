using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XNAInputLibrary.KinectInput.Gestures
{
    public class SwipeDetector
    {

        float MaxHorizontalYDeviation = 0.3f;
        float MinXHorizontalLength = 0.5f;


        float MaxVerticalXDeviation = 0.3f;
        float MinYVerticalLength = 0.5f;

        int MinimumSwipeDuration = 350;
        int MaximumSwipeDuration = 500;

        int MaxEntrys = 50;


        List<Entry> HandPoints = new List<Entry>();
        public SwipeDetector()
        {
        }

        public void Add(Vector3 position){
            Entry entry = new Entry { Position = position, Time = DateTime.Now };
            HandPoints.Add(entry);
            if (HandPoints.Count > MaxEntrys)
            {
                Entry entrytodelete = HandPoints[0];
                HandPoints.Remove(entrytodelete);
            }
            if (HandPoints.Count > 40)
            {
                CheckSwipeGesture();
            }
            
        }

        public void CheckSwipeGesture()
        {
            bool swipedetected = false;
            int startCheckPoint = 0;
            int startYCheckPoint = 0;
            for(int index = 1; index < HandPoints.Count -1;index++)
            {
                #region left / right detection
                if (Math.Abs(HandPoints[index].Position.Y - HandPoints[0].Position.Y) > MaxHorizontalYDeviation)
                {
                    //Deveation on Y axis is ok;
                    startCheckPoint = index;
                }
                if (Math.Abs(HandPoints[index].Position.X - HandPoints[startCheckPoint].Position.X) >= MinXHorizontalLength)
                {
                    // Now check on X axis is ok
                    //Time to check the time between those points
                    double totalMilliseconds = (HandPoints[index].Time - HandPoints[startCheckPoint].Time).TotalMilliseconds;
                    if (totalMilliseconds >= MinimumSwipeDuration && totalMilliseconds <= MaximumSwipeDuration)
                    {
                        //Hebben we een Swipe
                        //Welke Direction ? 
                        if (HandPoints[index].Position.X - HandPoints[startCheckPoint].Position.X > -0.01f)
                        {
                            //SwipeRight
                            KinectManager.GetInstance().RaiseSwipeDetected(new KinectSwipeEventArgs(SwipeDirection.Right));
                            swipedetected = true;
                            Console.WriteLine("SwipeRight");
                        }
                        else if (HandPoints[index].Position.X - HandPoints[startCheckPoint].Position.X < 0.01f)
                        {
                            //SwipeLeft
                            KinectManager.GetInstance().RaiseSwipeDetected(new KinectSwipeEventArgs(SwipeDirection.Left));
                            swipedetected = true;
                            Console.WriteLine("SwipeLeft");
                        }
                    
                        

                    }
                }
                #endregion 
                #region up / down direction
                if (Math.Abs(HandPoints[index].Position.X - HandPoints[0].Position.X) >  MaxVerticalXDeviation)
                {
                    //Deveation on Y axis is ok;
                    startYCheckPoint = index;
                }
                if (Math.Abs(HandPoints[index].Position.Y - HandPoints[startYCheckPoint].Position.Y) >= MinYVerticalLength)
                {
                    // Now check on X axis is ok
                    //Time to check the time between those points
                    double totalMilliseconds = (HandPoints[index].Time - HandPoints[startYCheckPoint].Time).TotalMilliseconds;
                    if (totalMilliseconds >= MinimumSwipeDuration && totalMilliseconds <= MaximumSwipeDuration)
                    {
                        //Hebben we een Swipe
                        //Welke Direction ? 
                        if (HandPoints[index].Position.Y - HandPoints[startYCheckPoint].Position.Y > -0.01f)
                        {
                            //SwipeRight
                            KinectManager.GetInstance().RaiseSwipeDetected(new KinectSwipeEventArgs(SwipeDirection.Up));
                            Console.WriteLine("SwipeUp");
                            swipedetected = true;
                        }
                        else if (HandPoints[index].Position.Y - HandPoints[startYCheckPoint].Position.Y < 0.01f)
                        {
                            //SwipeLeft
                            KinectManager.GetInstance().RaiseSwipeDetected(new KinectSwipeEventArgs(SwipeDirection.Down));
                            Console.WriteLine("SwipeDown");
                            swipedetected = true;
                        }
                        
                        

                    }
                }
                #endregion
                if (swipedetected)
                {
                    HandPoints.Clear();
                }
            }
            

            
        }
    }
}
