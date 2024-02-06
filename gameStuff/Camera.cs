using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CelesteLike
{
    internal class Camera
    {
        public Matrix Transform { get; private set; }
        Vector2 centre;
        int offsetX = 16;
        int offsetY = 64;

        public void Follow(Sprite target)
        {
            var position = Matrix.CreateTranslation(-target.position.X - target.sourceRectangle.Width / 2,
                -target.position.Y - target.sourceRectangle.Height / 2,
                0);

            var offset = Matrix.CreateTranslation(GameManager.ScreenWidth / 2, GameManager.ScreenHeight / 2, 0);

            Transform = position * offset;
        }

        public void Follow2(Sprite target)
        {
            centre.X = Math.Min(Math.Max(centre.X, target.position.X + target.sourceRectangle.Width / 2 - 240 - offsetX),
                target.position.X + target.sourceRectangle.Width / 2 - 180 + offsetX);
            centre.Y = Math.Min(Math.Max(centre.Y, target.position.Y + target.sourceRectangle.Height / 2 - 135 - offsetY),
                target.position.Y + target.sourceRectangle.Height / 2 - 225 + offsetY);

            Transform = Matrix.CreateScale(new Vector3(1, 1, 0)) * Matrix.CreateTranslation(new Vector3(-centre.X, -centre.Y, 0));
        }
    }
}