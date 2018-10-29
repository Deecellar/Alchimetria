using System;
using Microsoft.Xna.Framework;
using Nez;
using Nez.Sprites;
using Nez.TextureAtlases;
using Nez.Tiled;

namespace Alchimestria.Desktop
{
    public static class SharedClass
    {


		public static Entity CreatePlayer(TiledObject spawn, TiledMap tiledMap, string[] name, Scene sceneP){
			var player = sceneP.createEntity("player");
            var animations = sceneP.content.Load<TextureAtlas>("Assets/Characters/Main");
            var sprite = new Sprite<Animations.Main>(Animations.Main.Walk, animations.getSpriteAnimation("Walk"));
            sprite.addAnimation(Animations.Main.Throw, animations.getSpriteAnimation("Throw"));
            sprite.addAnimation(Animations.Main.Jump, animations.getSpriteAnimation("Jump"));
            sprite.addAnimation(Animations.Main.Hit, animations.getSpriteAnimation("Hit"));
            sprite.addAnimation(Animations.Main.Ladder, animations.getSpriteAnimation("Ladder"));
            player.addComponent(sprite);
            player.transform.setPosition(spawn.x, spawn.y);
            player.addComponent(new PlayerController(tiledMap));
            foreach(string a in name)
            {
                player.addComponent(new TiledMapMover(tiledMap.getLayer<TiledTileLayer>(a)));
            }
            
            player.addComponent(new BoxCollider(-16f, -16f, 32, 32));
            player.addComponent(new FollowCamera(player, FollowCamera.CameraStyle.LockOn));
            
			return player;
		}

        public static void PoblateCoins(TiledMap til, Scene sceneP)
        {
            var texture = sceneP.content.Load<Microsoft.Xna.Framework.Graphics.Texture2D>("Assets/Interactuables/Coin 32x32");
            int iterator = 0;
            var objectLayer = til.getObjectGroup("Eventos");
            foreach(var coin in objectLayer.objectsWithName("Moneda"))
            {
                var Monedas = sceneP.createEntity("Monedas"+iterator);
                Monedas.addComponent(new Sprite(texture));
                Monedas.transform.setPosition(coin.position);
                Monedas.addComponent(new BoxCollider(-16f, -16f, 32f, 32f));
                Monedas.addComponent(new CollectableController(til, sceneP.findEntity("player")));
                iterator++;
            }
        }
        public static void CreateBoss1(TiledMap til, Scene sceneP, string[] name)
        {
            var texture = sceneP.content.Load<TextureAtlas>("Assets/Characters/Boss");
            var objectLayer = til.getObjectGroup("Eventos");
            var sprite = new Sprite<Animations.Boss>(Animations.Boss.Walk, texture.getSpriteAnimation("Walk"));
            sprite.addAnimation(Animations.Boss.Attack1, texture.getSpriteAnimation("Attack1"));
            sprite.addAnimation(Animations.Boss.Attack2, texture.getSpriteAnimation("Attack2"));
            var Enemigo = sceneP.createEntity("Boss");
            Enemigo.addComponent(sprite.setRenderLayer((0)));
            Enemigo.transform.setPosition(objectLayer.objectWithName("Boss").position);
            Enemigo.addComponent(new BoxCollider(-24f, -24f, 48f, 48f));
            Enemigo.addComponent(new Boss1());
            foreach (string a in name)
            {
                Enemigo.addComponent(new TiledMapMover(til.getLayer<TiledTileLayer>(a)));
            }

        }
        public static void poblateEnemies(TiledMap til, Scene sceneP, string[] name)
        {
            var texture = sceneP.content.Load<TextureAtlas>("Assets/Characters/Snake");
            int iterator = 0;
            var objectLayer = til.getObjectGroup("Eventos");

            foreach (var enemy in objectLayer.objectsWithName("Enemigo"))
            {
                var snakeSprite = new Sprite<Animations.Snake>(Animations.Snake.Walk, texture.getSpriteAnimation("Walk"));
                snakeSprite.addAnimation(Animations.Snake.Attack, texture.getSpriteAnimation("Attack"));
                var Enemigo = sceneP.createEntity("Enemigo" + iterator);
                Enemigo.addComponent(snakeSprite.setRenderLayer((iterator)));
                Enemigo.transform.setPosition(enemy.position);
                Enemigo.addComponent(new BoxCollider(-16f, -16f, 32f, 32f));
                Enemigo.addComponent(new EnemyController());
                foreach (string a in name)
                {
                    Enemigo.addComponent(new TiledMapMover(til.getLayer<TiledTileLayer>(a)));
                }
                iterator++;
            }

        }
    }
}
