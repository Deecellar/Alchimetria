using System;
using Microsoft.Xna.Framework;
using Nez;
using Nez.Sprites;
using Nez.Tiled;

namespace Alchimestria.Desktop
{
    class GameOverScreen : Scene
    {
        public GameOverScreen()
        {
            this.addRenderer(new DefaultRenderer());
            this.addRenderer(new ScreenSpaceRenderer(0, 0));
            this.clearColor = Color.Black;
            this.createEntity("GameOver").addComponent<GameOverScreenComponent>().setRenderLayer(0);
            this.createEntity("Music").addComponent(new SongManager("Dark Ambient"));

        }
    }
}
