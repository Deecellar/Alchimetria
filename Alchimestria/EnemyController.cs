﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.AI.FSM;
using Nez.Tiled;
using Nez.Sprites;
using System.Collections;

namespace Alchimestria.Desktop
{
    class EnemyController : SimpleStateMachine<Animations.Snake>
    {
        BoxCollider box;
        TiledMapMover mover;
        TiledMapMover.CollisionState CollisionState = new TiledMapMover.CollisionState();
        Vector2 velocity = new Vector2(130f,1000f);
        Sprite<Animations.Snake> sprite;
        float rotation = 0;
        public override void onAddedToEntity()
        {
            base.onAddedToEntity();
            
            mover = this.getComponent<TiledMapMover>();
            box = this.getComponent<BoxCollider>();
            sprite = this.getComponent<Sprite<Animations.Snake>>();
            initialState = Animations.Snake.Walk;
        }

        void Walk_Enter() {
            currentState = Animations.Snake.Walk;
            sprite.play(Animations.Snake.Walk);
        }
        void Walk_Tick() {
            if (CollisionState.right)
            {
                velocity.X = -130f;
            }
            else if (CollisionState.left)
            {
                velocity.X = 130f;
                
            }

            if(velocity.X > 0)
            {
                sprite.flipX = false;
            }
            else
            {
                sprite.flipX = true;

            }

            mover.move(velocity * Time.deltaTime, box, CollisionState);
            if (!sprite.isPlaying)
            {
                sprite.play(Animations.Snake.Walk);
            }
            if (entity.getComponent<BoxCollider>().overlaps(entity.scene.findEntity("player").getComponent<BoxCollider>()))
            {
                entity.scene.findEntity("player").getComponent<PlayerController>().kill(entity.scene.findEntity("player").getComponent<PlayerController>().irA);
            }
            if (Math.Abs(this.entity.transform.position.Y - entity.scene.findEntity("player").transform.position.Y) <  30)
            {
                if (Math.Abs(this.entity.transform.position.X - entity.scene.findEntity("player").transform.position.X) <= 100)
                {
                    currentState = Animations.Snake.Attack;
                }
            }
        }
        void Walk_Exit() {
            velocity.X = 180f;
            sprite.stop();

        }

        void Attack_Enter() {
            sprite.play(Animations.Snake.Attack);
        }
        void Attack_Tick() {
            if (entity.getComponent<BoxCollider>().overlaps(entity.scene.findEntity("player").getComponent<BoxCollider>()))
            {
                entity.scene.findEntity("player").getComponent<PlayerController>().kill(entity.scene.findEntity("player").getComponent<PlayerController>().irA);
            }
            if ((-this.entity.transform.position.X + entity.scene.findEntity("player").transform.position.X) > 0)
            {
                velocity.X = 180f;
                sprite.flipX = false;
            }
            else if ((-this.entity.transform.position.X + entity.scene.findEntity("player").transform.position.X) < 0)
            {
                velocity.X = -180f;
                sprite.flipX = true;
            }
            mover.move(velocity * Time.deltaTime, box, CollisionState);
            if(Math.Abs(this.entity.transform.position.Y - entity.scene.findEntity("player").transform.position.Y) >= 30 ) {
                currentState = Animations.Snake.Walk;

            }
            if (Math.Abs(this.entity.transform.position.X - entity.scene.findEntity("player").transform.position.X) > 100)
            {
                currentState = Animations.Snake.Walk;

            }
        }
        void Attack_Exit() {
            velocity.X = 130f;
            sprite.stop();

        }

        void Die_Enter()
        {
            sprite.stop();
        }
        void Die_Tick()
        {

            if (rotation == 180)
            {
                string s = entity.name;
                Vector2 position = entity.position;
                Scene e = entity.scene;
                entity.destroy();
                Entity Monedas = e.createEntity(s).setPosition(position);
                var texture = e.content.Load<Microsoft.Xna.Framework.Graphics.Texture2D>("Assets/Interactuables/Coin 32x32");
                Monedas.addComponent(new Sprite(texture));
                Monedas.addComponent(new BoxCollider(-16f, -16f, 32f, 32f));
                Monedas.addComponent(new CollectableController(e.findEntity("Tiled Map").getComponent<TiledMapComponent>().tiledMap, e.findEntity("player")));


            }
            else
            {
                            rotation += 10;
            sprite.transform.setRotationDegrees(rotation);
            }
        }
        void Die_Exit()
        {
            

        }
        public void kill()
        {
            currentState = Animations.Snake.Die;
        }
    }
}
