using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace MathsInvaders.Entities
{
    internal class Bullet : IEntity
    {
        private const int speed = 5;
        private Texture2D sprite;
        public string Name { get; }
        private int _x;
        private int _y;

        public Bullet(Game1 g, Texture2D mySprite, int x, int y)
        {
            Name = "bullet";
            sprite = mySprite;
            _x = x;
            _y = y;
        }

        bool IEntity.Collidable {
            get {
                return true;
            }
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics, SpriteFont font)
        {
            spriteBatch.Draw(sprite, new Rectangle(_x, _y, sprite.Width, sprite.Height), Color.White);
        }

        public void Initialize()
        {
        }

        public void LoadContent()
        {
        }

        public void Move(int x, int y)
        {
            _x += x;
            _y += y;
        }

        public void UnloadContent()
        {
        }

        public void Update(Game1 game, KeyboardState currentState, KeyboardState lastState, GameTime gameTime)
        {
            Move(0, -speed);
            if (_y < -64)
            {
                game.RemoveEntity(this);
            }
        }

        public Vector2 getPosition()
        {
            return new Vector2(_x, _y);
        }

        public int GetEnemyNumber()
        {
            throw new NotImplementedException();
        }

        public int GetXPosition()
        {
            return _x;
        }

        public int GetYPosition()
        {
            return _y;
        }

        public int GetWidth()
        {
            return sprite.Width;
        }

        public int GetHeight()
        {
            return sprite.Height;
        }

        public void CollidedWithSomething(IEntity collider)
        {
            return;
        }
    }
}