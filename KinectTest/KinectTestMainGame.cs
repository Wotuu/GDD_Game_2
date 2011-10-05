using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XNAInputLibrary.KinectInput;
using Microsoft.Xna.Framework.Graphics;

namespace KinectTest
{
   public class KinectTestMainGame
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
        Texture2D DrawText;
        Boolean Hitting = false;
        public void Initialize(Game game)
        {
            this.game = game;
            KinectManager.GetInstance().Initialize(game.GraphicsDevice.Viewport, .5f, .5f);
            DrawText =  new Texture2D(game.GraphicsDevice, 1, 1);
            uint[] textdata = { Color.White.PackedValue };
            DrawText.SetData(textdata);
            KinectManager.GetInstance().PointerMoved += OnKinectUpdate;
        }

        public void Draw(SpriteBatch sb)
        {
            if (Hitting)
            {
                sb.Draw(DrawText, RightHandrect, Color.Red);
            }
            else
            {
                sb.Draw(DrawText, RightHandrect, Color.Blue);
            }
            
            sb.Draw(DrawText, LeftHandrect, Color.Green);
        }

        public void Update()
        {
            RightHandrect = new Rectangle((int)RightHand.X, (int)RightHand.Y, (int)(50 / RightHand.Z), (int)(50 / RightHand.Z));
            LeftHandrect = new Rectangle((int)LeftHand.X, (int)LeftHand.Y, (int)(50 / LeftHand.Z), (int)(50 / LeftHand.Z));
            
        }


        public void OnKinectUpdate(object sender, KinectPointerEventArgs e)
        {
            RightHand = e.RightHandPosition;
            LeftHand = e.LeftHandPosition;
            Hitting =  IsHacking(e);
        }


        public Boolean IsHacking(KinectPointerEventArgs e)
        {
            if (e.PreviousRightHandPosition.Z - e.RightHandPosition.Z > 0.05f) { return true; }
            return false;
        }


    }
}
