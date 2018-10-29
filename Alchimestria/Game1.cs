using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.Tiled;


namespace Alchimestria.Desktop
{

    public class Game1 : Nez.Core
    {


        public Game1() : base(1280, 720)
        {
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
            Core.pauseOnFocusLost = true;
        }


        protected override void Initialize()
        {
            Scene.setDefaultDesignResolution(1280, 720, Scene.SceneResolutionPolicy.ShowAllPixelPerfect);
            scene = new FirstScene();

            base.Initialize();
        }


    }
}
