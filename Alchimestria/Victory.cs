using System;
using Microsoft.Xna.Framework;
using Nez;
using Nez.Sprites;
using Nez.Tiled;



namespace Alchimestria.Desktop
{
    class Victory :Scene
    {
        public Victory()
        {
            this.addRenderer(new DefaultRenderer());
            this.addRenderer(new ScreenSpaceRenderer(0, 0));
            this.clearColor = Color.Black;
            this.createEntity("Victory").addComponent<VictoryComponent>().setRenderLayer(0);
            this.createEntity("Music").addComponent(new SongManager("Intro"));
        }

    }
}
