using System;
using Nez;
using Nez.UI;
using Microsoft.Xna.Framework;

namespace Alchimestria.Desktop
{
    public class PlayerUI : UICanvas, IUpdatable
    {
        PlayerController p;
        Label bar;
        
        ProgressBar progress;
        Image image;
        Image image2;
        Label manaR;
        Label tar;
        Table t;
        [Inspectable]
        Vector2 Vector2 = new Vector2(320, 25);
        public PlayerUI(Entity player)
        {
            p = player.getComponent<PlayerController>();
            
        }
        public override void onAddedToEntity()
        {

            // tables are very flexible and make good candidates to use at the root of your UI. They work much like HTML tables but with more flexibility.
            var table = stage.addElement(new Table());

            // tell the table to fill all the available space. In this case that would be the entire screen.
            table.setFillParent(true);



            // add a Slider



            // if creating buttons with just colors (PrimitiveDrawables) it is important to explicitly set the minimum size since the colored textures created
            // are only 1x1 pixels
            var texture = entity.scene.content.Load<Microsoft.Xna.Framework.Graphics.Texture2D>("Assets/Throwables/tile009");
            image = new Image(texture);
            table.add(image);
            progress = new ProgressBar(0, 100, 0.5f, false, Skin.createDefaultSkin());
            progress.setValue(p.mana);
            progress.setWidth(Vector2.X);
            progress.toBack();
            manaR = new Label(String.Format("{0}/{1}",(int)p.mana,100));
            manaR.toFront();
            manaR.setX(-50);
            manaR.setFontScale(1.1f);
            manaR.setFontColor(Color.Blue);
            table.add(progress).setMinWidth(200) ;
            table.add(manaR);
            table.row();

            // add a ProgressBar
            var texture2 = entity.scene.content.Load<Microsoft.Xna.Framework.Graphics.Texture2D>("Assets/Interactuables/Coin 32x32");
            image = new Image(texture2);
            table.add(image);
            bar = new Label(String.Format("{0} monedas", p.coins));
            bar.setFontScale(1.07f);
            bar.setFontColor(Color.Yellow);
            table.add(bar);
            table.row();

            tar = new Label(String.Format("{0} Vidas", 10 - p.deadCounter));
            tar.setY(30f);
            tar.setFontScale(1.08f);
            tar.setFontColor(Color.Red);
            table.add(tar);


            // this tells the table to move on to the next row
            table.row();
            t = table;
            t.top().left().pad(30,30,30,30);
            
            Core.emitter.addObserver(CoreEvents.GraphicsDeviceReset, PositionElements);
        }

        public void coinUpdate()
        {
            if(p.coins != 1)
            bar.setText(String.Format("{0} monedas", p.coins));
            else
            bar.setText(String.Format("{0} moneda", p.coins));

        }

        public void ManaUpdate()
        {
            progress.setValue(p.mana);

            manaR.setText(String.Format("{0}", (int)p.mana)).setDebug(true);
        }
        
        public void update()
        {
            coinUpdate();
            ManaUpdate();
            tar.setText(String.Format("{0} Vidas", 10 - p.deadCounter));

        }
        protected  void PositionElements()
        {
            var targetHeight = Screen.height / 0.99f;
            t.setBounds(0, Screen.height - targetHeight, Screen.width, targetHeight);
            t.top().left().pad(30, 30, 30, 30);
        }
    }
}