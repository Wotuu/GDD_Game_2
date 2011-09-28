using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XNAInputLibrary.KinectInput;

namespace KinectTest
{
    class KinectTestMainGame
    {
         #region Singleton logic
        private static KinectTestMainGame instance;

        public static KinectTestMainGame GetInstance()
        {
            if (instance == null) instance = new KinectTestMainGame();
            return instance;
        }
        #endregion

        private KinectTestMainGame() { }
        public Game game { get; set; }

        Vector3 RightHand;
        Vector3 LeftHand;
        Rectangle RightHandrect = new Rectangle();
        Rectangle LeftHandrect = new Rectangle();
        public void Initialize(Game game)
        {
            this.game = game;
            KinectManager.GetInstance().PointerMoved += OnKinectUpdate;
        }

        public void Draw()
        {
        }

        public void Update()
        {
            RightHandrect = new Rectangle((int)RightHand.X, (int)RightHand.Y, (int)(50 * RightHand.Z), (int)(50 * RightHand.Z));
            LeftHandrect = new Rectangle((int)LeftHand.X, (int)LeftHand.Y, (int)(50 * LeftHand.Z), (int)(50 * LeftHand.Z));
        }


        public void OnKinectUpdate(object sender, KinectPointerEventArgs e)
        {
            RightHand = e.RightHandPosition.ScaleTo(game.GraphicsDevice.Viewport.Width,game.GraphicsDevice.Viewport.Height,0.5f,0.5f);
            LeftHand = e.LeftHandPosition.ScaleTo(game.GraphicsDevice.Viewport.Width,game.GraphicsDevice.Viewport.Height,0.5f,0.5f);
            
        }



    }
}
