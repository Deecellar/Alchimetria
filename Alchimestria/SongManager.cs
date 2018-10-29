using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nez;
using Nez.Audio;
using Microsoft.Xna.Framework.Media;

using Microsoft.Xna.Framework.Audio;
namespace Alchimestria.Desktop
{
    class SongManager : Component
    {


        string assetPathSong = "Assets/Songs/";
        public SongManager(string Song)
        {
            assetPathSong += Song;
        }

        public override void onAddedToEntity()
        {
            play();
        }
    
        void play()
        {
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(this.entity.scene.content.Load<Song>(assetPathSong));
        }
    }
}
