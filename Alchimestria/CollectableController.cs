using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez.Sprites;
using Nez;
using System.Collections;
using Nez.Tiled;

namespace Alchimestria.Desktop
{
    class CollectableController : Component, IUpdatable
    {
        TiledMap t;
        Entity p;
        double angle = 0;
        public CollectableController(TiledMap map, Entity player) 
        {
            t = map;
            p = player;
        }

        public void update()
        {
            if(p.getComponent<BoxCollider>().overlaps(entity.getComponent<BoxCollider>()))
            {
                entity.destroy();
                p.getComponent<PlayerController>().coins++;
               
            }
            entity.setScale(new Vector2((float)Math.Abs(Math.Cos(angle)),1));
            Core.startCoroutine(Angle());
        }

        IEnumerator Angle()
        {
            angle += 0.1f;
            yield return Coroutine.waitForSeconds(1.5f);
            
        }
    }
}
