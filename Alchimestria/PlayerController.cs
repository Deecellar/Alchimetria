using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.Tiled;
using Nez.Sprites;

namespace Alchimestria.Desktop
{
    public class PlayerController : Component, IUpdatable
    {
        
        public float movespeed = 150f;
        
        public float gravity = 1000f;
        
        Vector2 velocity = new Vector2();
        
        public float height;
        
        public float friccion = 3.3f;
        TiledMap til;
        Entity barr = null;
        Component component;
        bool onLadders = false;
        TiledMapMover mover;
        BoxCollider box;
        TiledObjectGroup objectLayer;
        TiledMapMover.CollisionState CollisionState = new TiledMapMover.CollisionState();
        bool onIce = false;
        List<RectangleF> boxCollider = new List<RectangleF>();
        List<RectangleF> pinchos = new List<RectangleF>();
        int inverted = 1;
        List<RectangleF> palancas = new List<RectangleF>();
        bool right = false;
        bool left = false;
        bool[] pos = new bool[6];
        bool canUseSwitch = false;
        bool isGravityInverted = false;
        bool onAir = false;
       public  bool isBottleSpawned = false;
        List<RectangleF> puertas = new List<RectangleF>();
        List<string> ubicaciones = new List<string>();
        public string irA = "Spawn";
        bool canEnterOnDoor = false;
        public int coins = 0;
        public float mana = 100f;
        public int deadCounter = 0;
        Sprite<Animations.Main> sprite;

        public PlayerController(TiledMap arg)
        {
            til = arg;
            objectLayer = til.getObjectGroup("Eventos");
            foreach (TiledObject hielo in objectLayer.objectsWithName("Hielo"))
            {
                boxCollider.Add(new RectangleF(hielo.x, hielo.y, hielo.width, hielo.height));
            }
            foreach (TiledObject t in objectLayer.objectsWithName("Pinchos"))
            {
                pinchos.Add(new RectangleF(t.x, t.y, t.width, t.height));
            }
            foreach (TiledObject t in objectLayer.objectsWithName("Gravedad"))
            {
                palancas.Add(new RectangleF(t.x, t.y, t.width, t.height));
            }
            foreach (TiledObject t in objectLayer.objectsWithName("Puerta"))
            {
                puertas.Add(new RectangleF(t.x, t.y, t.width, t.height));
                ubicaciones.Add(t.properties["puntoDeTransporte"]);
            }
        }

        public override void onAddedToEntity()
        {
            mover = this.getComponent<TiledMapMover>();
            box = this.getComponent<BoxCollider>();
            sprite = this.getComponent<Sprite<Animations.Main>>();
        }

        public void update()
        {
            #region Control
            if (!onLadders)
            {
                if (velocity.X != 0)
                {
                    height = 32 * 3.6f * (Mathf.sqrt(Math.Abs(velocity.X) + 1) - Mathf.sqrt(Math.Abs(gravity) - 800));
                }
                else
                {
                    height = 32 * 3.6f * Mathf.sqrt(Math.Abs(velocity.X) + 1) - Mathf.sqrt(Math.Abs(gravity) - 800);
                }

                onIce = false;
                foreach (RectangleF hie in boxCollider)
                {
                    onIce |= hie.contains(this.entity.position + new Vector2(0f, 16f));
                    onIce |= hie.contains(this.entity.position - new Vector2(0f, 16f));
                    onIce |= hie.contains(this.entity.position + new Vector2(16f, 0));
                    onIce |= hie.contains(this.entity.position - new Vector2(16f, 0f));
                    onIce |= hie.contains(this.entity.position - new Vector2(16f, 16f));
                    onIce |= hie.contains(this.entity.position + new Vector2(16f, 16f));
                    onIce |= hie.contains(this.entity.position);

                }

                //if (!isGravityInverted)
                //{
                //    if (barr == null)
                //    {
                //        entity.addComponent(new TiledMapMover(til.getLayer<TiledTileLayer>("barreras")));
                //    }

                //}
                //else
                //{
                //    entity.removeComponent((new TiledMapMover(til.getLayer<TiledTileLayer>("barreras"))));

                //}

                if (Input.isKeyPressed(Keys.V))
                {
                    inverted = -inverted;
                    velocity.Y -= 1f;
                    isGravityInverted = !isGravityInverted;


                }
                else if (Input.isKeyPressed(Keys.E) && canEnterOnDoor)
                {
                    this.entity.position = objectLayer.objectWithName(irA).position;
                    canEnterOnDoor = false;
                }

                if( Input.isKeyPressed(Keys.J)  && !isBottleSpawned && mana >= 7.5f)
                {
                    isBottleSpawned =  createBottle(sprite.flipX);
                    mana -= 7.5f;
                    if(mana < 0)
                    {
                        mana = 0;
                        
                    }
                    
                }
                if (isGravityInverted)
                {
                    if (sprite.transform.localRotationDegrees < 180)
                    {
                        sprite.transform.localRotationDegrees += 10;
                    }

                }
                else
                {
                    if (sprite.transform.localRotationDegrees != 0)
                    {
                        sprite.transform.localRotationDegrees -= 10;
                    }

                }
                if (Input.isKeyDown(Keys.A))
                {
                    velocity.X = -movespeed;
                    left = true;
                    right = false;
                    if (!sprite.flipX)
                    {
                        sprite.flipX = true;
                    }
                    if (sprite.flipX && isGravityInverted)
                    {
                        sprite.flipX = false;
                    }
                    if (!sprite.isPlaying)
                    {
                        sprite.play(Animations.Main.Walk);
                    }


                }

                else if (Input.isKeyDown(Keys.D))
                {
                    velocity.X = movespeed;
                    left = !true;
                    right = !false;
                    if (sprite.flipX)
                    {
                        sprite.flipX = false;
                    }
                    if (!sprite.flipX && isGravityInverted)
                    {
                        sprite.flipX = true;

                    }
                    if (!sprite.isPlaying)
                    {
                        sprite.play(Animations.Main.Walk);
                    }
                }
                else if (left && onIce)
                {
                    velocity.X += friccion;
                    if (velocity.X >= 0)
                    {
                        velocity.X = 0;
                        left = false;
                        right = false;
                    }
                }
                else if (right && onIce)
                {
                    velocity.X -= friccion;
                    if (velocity.X <= 0)
                    {
                        velocity.X = 0;
                        left = false;
                        right = false;
                    }
                }
                else
                {
                    velocity.X = 0;
                    sprite.stop();
                }


                if (CollisionState.below && Input.isKeyPressed(Keys.W))
                {
                    velocity.Y = -Mathf.sqrt(Math.Abs(2 * height * gravity));
                    Console.WriteLine("He sido presionado OwO " + velocity.Y);
                    onAir = true;
                    isGravityInverted = false;
                    inverted = 1;
                    if (!sprite.isPlaying)
                    {
                        sprite.play(Animations.Main.Jump);
                    }

                }
                else if (!CollisionState.below && Input.isKeyPressed(Keys.W) && isGravityInverted && !onAir)
                {
                    velocity.Y = Mathf.sqrt(Math.Abs(2 * height * gravity));
                    onAir = true;
                    if (!sprite.isPlaying)
                    {
                        sprite.play(Animations.Main.Jump);
                    }
                }




                velocity.Y += gravity * Time.deltaTime * inverted;

                mover.move(velocity * Time.deltaTime, box, CollisionState);




                if (CollisionState.right || CollisionState.left)
                {
                    velocity.X = 0;

                }
                if (CollisionState.above || CollisionState.below)
                {
                    velocity.Y = 0;
                    onAir = false;
                }

                if (mana < 100)
                {
                    mana += 5 * Time.deltaTime;
                    
                }
                if (mana > 100)
                {
                    mana = 100;
                }
            }
            else
            {

            }
            #endregion

            #region colisiones
            var spawn = objectLayer.objectWithName(irA);

            foreach (RectangleF rectangle in pinchos)
            {
                pos[0] = rectangle.contains(this.entity.position + new Vector2(0f, 16f));
                pos[1] = rectangle.contains(this.entity.position - new Vector2(0f, 16f));
                pos[2] = rectangle.contains(this.entity.position + new Vector2(16f, 0));
                pos[3] = rectangle.contains(this.entity.position - new Vector2(16f, 0f));
                pos[4] = rectangle.contains(this.entity.position - new Vector2(16f, 16f));
                pos[5] = rectangle.contains(this.entity.position + new Vector2(16f, 16f));
                bool pose = false;

                foreach (bool p in pos)
                {
                    pose |= p;
                }
                if (pose)
                {
                    kill(irA);
                    break;
                }
            }

            foreach (RectangleF rectangleF in palancas)
            {
                canUseSwitch = rectangleF.contains(this.entity.position);
            }

            foreach (RectangleF rectangleP in puertas)
            {
                if (rectangleP.contains(new Vector2(this.entity.position.X, this.entity.position.Y)))
                {
                    canEnterOnDoor = true;
                    irA = ubicaciones[puertas.IndexOf(rectangleP)];
                }
            }

            if (deadCounter == 10)
            {
                Core.scene = new GameOverScreen();
            }
            #endregion

           
        }
        public void kill(string irA)
        {
            this.entity.setPosition(objectLayer.objectWithName(irA).position);
            isGravityInverted = false;
            inverted = 1;
            velocity.Y = 0;
            onIce = false;
            deadCounter++;
        }
        public bool createBottle(bool left)
        {
            var texture = entity.scene.content.Load<Microsoft.Xna.Framework.Graphics.Texture2D>("Assets/Throwables/tile009");
            Sprite sprite = new Sprite(texture);
            Entity bottle = entity.scene.findEntity("Bottle");
            if (bottle != null)
            {
                bottle.destroy();
            }
            bottle = entity.scene.createEntity("Bottle");
            if (left)
            {
                bottle.transform.position = entity.position + new Vector2(-32, 0);

            }
            else
            {
                bottle.transform.position = entity.position + new Vector2(32, 0);
            }
            bottle.addComponent(sprite);
            bottle.addComponent(new BoxCollider(-8f, -8f, 16f, 16f));
            bottle.addComponent(new TiledMapMover(til.getLayer<TiledTileLayer>("piso")));
            bottle.addComponent(new ThrowController(left));
            return true;
        }
    }
}
