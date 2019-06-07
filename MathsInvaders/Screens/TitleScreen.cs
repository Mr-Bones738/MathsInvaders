using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MathsInvaders.Screens
{
    internal class TitleScreen
    {
        public Texture2D Sprite;

        public TitleScreen(Texture2D s)
        {
            Sprite = s;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite, new Rectangle(0, 0, Sprite.Width, Sprite.Height), Color.White);
        }
    }
}