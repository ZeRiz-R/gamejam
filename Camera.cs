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
        int offsetY = 32;

        public void Follow(Sprite target)
        {
            var position = Matrix.CreateTranslation(-target.Position.X - (target.SourceRectangle.Width / 2),
                -target.Position.Y - (target.SourceRectangle.Height / 2),
                0);

            var offset = Matrix.CreateTranslation(GameManager.ScreenWidth / 2, GameManager.ScreenHeight / 2, 0);

            Transform = position * offset;
        }

        public void Follow2(Sprite target)
        {
            centre.X = Math.Min(Math.Max(centre.X, target.Position.X + (target.SourceRectangle.Width / 2) -180 - offsetX),
                target.Position.X + (target.SourceRectangle.Width / 2) -180 + offsetX);
            centre.Y = Math.Min(Math.Max(centre.Y, target.Position.Y + (target.SourceRectangle.Height / 2) - 225 -offsetY),
                target.Position.Y + (target.SourceRectangle.Height / 2) - 225 + offsetX);

            Transform = Matrix.CreateScale(new Vector3(1, 1, 0)) * Matrix.CreateTranslation(new Vector3(-centre.X, -centre.Y, 0));
        }
    }
}