using System;
using Microsoft.Xna.Framework;
using Nez;
using Nez.Sprites;
using Nez.Tiled;

namespace Alchimestria.Desktop
{
	public class FirstScene : Scene
    {
        public TiledMap tiledMap;
        public FirstScene()
        {
			clearColor = Color.LightGray;
            addRenderer(new DefaultRenderer(0, camera));
            addRenderer(new ScreenSpaceRenderer(0, 3));

			tiledMap = content.Load<TiledMap>("Assets/Maps/Castillo/castillo1");
			var objectLayer = tiledMap.getObjectGroup("Eventos");
			var tiledEntity = createEntity("Tiled Map");
			tiledEntity.addComponent(new TiledMapComponent(tiledMap,"piso").setRenderLayer(10));
            tiledEntity.addComponent(new TiledMapComponent(tiledMap, "piso2").setRenderLayer(10));
            var spawn = objectLayer.objectWithName("Spawn");
            camera.zoomIn(10f);
            camera.setPosition(new Vector2(20, 20));

            

			var player =  SharedClass.CreatePlayer(spawn, tiledMap, new string[]{ "piso","piso2"}, this);
            SharedClass.PoblateCoins(tiledMap, this);
             SharedClass.poblateEnemies(tiledMap, this, new string[] { "piso", "piso2" });
            SharedClass.CreateBoss1(tiledMap, this, new string[] { "piso", "piso2" });
            this.createEntity("UI").addComponent(new PlayerUI(player)).setRenderLayer(3);
            this.createEntity("Music").addComponent(new SongManager("Fun"));

        }
    }
}
