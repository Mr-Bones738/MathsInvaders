using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MathsInvaders
{
    public interface IEntity
    {
        void Initialize();

        void Update(Game1 game, KeyboardState currentState, KeyboardState lastState, GameTime gameTime);

        void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics, SpriteFont font);

        void Move(int x, int y);

        string Name { get; }

        int GetEnemyNumber();

        int GetXPosition();

        int GetYPosition();

        int GetWidth();

        int GetHeight();

        bool Collidable { get; }

        void CollidedWithSomething(IEntity collider);
    }
}