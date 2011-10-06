using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using BalloonPaintBucketGame.Managers;

namespace BalloonPaintBucketGame.Audio
{
    public class AudioManager
    {
        SoundEffect BalloonPop;
        Song BackGroundMusic;
        int SongDuration;

        float MaxVolume = 1f;

        int FadeInDuration = 3000;
        int FadeOutDuration = 3000;
        public AudioManager()
        {
           BalloonPop = BalloonPaintBucketMainGame.GetInstance().game.Content.Load<SoundEffect>("Media/Audio/BalloonGame/BalloonPopping");
           BackGroundMusic = BalloonPaintBucketMainGame.GetInstance().game.Content.Load<Song>("Media/Audio/BalloonGame/BalloonAmbient");
           MediaPlayer.MediaStateChanged += new EventHandler<EventArgs>(MediaStateChanged);
        }


        public void PlayBalloonPop()
        {
            BalloonPop.Play();
        }

        public void PlayAmbientBackGroundMusic()
        {
            MediaPlayer.Volume = .01f;
            MediaPlayer.Play(BackGroundMusic);
        }

        public void MediaStateChanged(object sender, EventArgs e)
        {
            if (MediaPlayer.State == MediaState.Playing)
            {
                SongDuration = (int)BackGroundMusic.Duration.TotalMilliseconds;
            }
        }

        public void Update()
        {
            //FADE IN
            if(MediaPlayer.PlayPosition.TotalMilliseconds < FadeInDuration){
                //Scale volume 
                MediaPlayer.Volume = (MaxVolume / FadeInDuration) * (float)MediaPlayer.PlayPosition.TotalMilliseconds;
                Console.WriteLine(MediaPlayer.Volume + " - " + MediaPlayer.PlayPosition.TotalMilliseconds);
            }

            //FADE OUT
            if (SongDuration - MediaPlayer.PlayPosition.TotalMilliseconds < FadeOutDuration)
            {
                MediaPlayer.Volume = (MaxVolume / FadeInDuration) * (SongDuration - (float)MediaPlayer.PlayPosition.TotalMilliseconds);
                Console.WriteLine(MediaPlayer.Volume + " - " + MediaPlayer.PlayPosition.TotalMilliseconds);
            }

        }



    }
}
