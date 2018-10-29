using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.Sprites;
using Nez.Tiled;
namespace Alchimestria.Desktop
{
    class ThrowController : Component, IUpdatable
    {
        [Inspectable]
        Vector2 velocity = new Vector2(250, -200);
        BoxCollider box;
        bool isDestroyed = false;
        bool isLeft = false;
        float rotation = 0;
        TiledMapMover.CollisionState collision = new TiledMapMover.CollisionState();

        public override void onAddedToEntity()
        {
            base.onAddedToEntity();
            box = entity.getComponent<BoxCollider>();

        }

        public ThrowController(bool left)
        {
            isLeft = left;


        }

        [InspectorCallable]


        public void update()
        {
            {

                    rotation += 10f;
                if (isLeft)
                {
                    velocity.X = -250;
                }
                velocity.Y += 1000 * Time.deltaTime;    
                    if (!isDestroyed)
                    {
                        isDestroyed = killBottle();
                        entity.getComponent<Sprite>().transform.setRotationDegrees(rotation);
                        entity.getComponent<TiledMapMover>().move(velocity * Time.deltaTime, box, collision);
                    }



            }
        }
        [InspectorCallable]
        public bool killBottle()
        {
            CollisionResult result;
            if (box.collidesWithAny(out  result))
            {
                entity.scene.findEntity("player").getComponent<PlayerController>().isBottleSpawned = false;
                var enemy = result.collider.entity.getComponent<EnemyController>();
                var enemy2 = result.collider.entity.getComponent<Boss1>();
                if (enemy != null)
                {
                    enemy.kill();
                    entity.destroy();
                    return true;
                }
                 if (enemy2 != null)
                {
                    enemy2.kill();
                    entity.destroy();
                    return true;
                }
                else
                {
                    entity.destroy();
                    return true;
                }
            }
            return false;
        }
    }
}
