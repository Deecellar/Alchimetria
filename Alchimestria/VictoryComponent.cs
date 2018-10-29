using System;
using Microsoft.Xna.Framework;
using Nez;
using Nez.Sprites;
using Nez.TextureAtlases;
using Nez.Tiled;
using Nez.UI;

namespace Alchimestria.Desktop
{
    class VictoryComponent : UICanvas
    {
        PlayerController p;
        Table t;
        protected void PositionElements()
        {
            var targetHeight = Screen.height / 2;
            var targetWidth = Screen.width / 2;
            t.setBounds(targetWidth, targetHeight, targetWidth, targetHeight);
            t.center().pad(30, 30, 30, 30);
        }
        public override void onAddedToEntity()
        {

            // tables are very flexible and make good candidates to use at the root of your UI. They work much like HTML tables but with more flexibility.
            var table = stage.addElement(new Table());

            // tell the table to fill all the available space. In this case that would be the entire screen.
            table.setFillParent(true);

            TextButton button1 = new TextButton("¿Reiniciar?", Skin.createDefaultSkin());
            TextButton button2 = new TextButton("Salir", Skin.createDefaultSkin());
            Label label = new Label("Ganaste!!");
            label.setFontScale(25);
            label.setFontColor(Color.Yellow);
            button1.setColor(Color.Blue);
            button2.setColor(Color.Red);
            button2.setSize(300, 40);
            button2.setPosition(-150, 120);
            button1.setPosition(-150, 60);
            button1.setSize(300, 40);
            button1.onClicked += Button1_onClicked;
            button2.onClicked += Button2_onClicked;
            table.addElement(label).setPosition(-630, -100);
            table.row();
            table.addElement(button1);
            table.row();
            table.addElement(button2);
            // this tells the table to move on to the next row
            table.row();
            t = table;
            PositionElements();
            t.center().pad(30, 30, 30, 30);

            Core.emitter.addObserver(CoreEvents.GraphicsDeviceReset, PositionElements);
        }

        private void Button2_onClicked(Button obj)
        {
            Game1.exit();
        }

        private void Button1_onClicked(Button obj)
        {
            Game1.scene = new FirstScene();
        }
    }
}
