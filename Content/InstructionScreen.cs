using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CelesteLike.Content
{
    internal class InstructionScreen : Screen
    {
        private Button back;

        public InstructionScreen()
        {
            background = new Sprite("instructions", Vector2.Zero, 480, 270);
            back = new Button("backButton", new Vector2(30, 240), 57, 13, true);
            buttons.Add(back);
        }

        public override void LoadContent(ContentManager theContentManager)
        {
            background.LoadContent(theContentManager);
            base.LoadContent(theContentManager);

        }

        public override void Update(GameTime theGameTime)
        {
            base.Update(theGameTime);
            if (back.clicked == true)
            {
                nextScreen = ScreenManager.menuScreen;
            }
        }

        public void Sleep()
        {
            nextScreen = null;
        }

    }
}
