using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace MathsInvaders.Entities
{
    internal class Player : IEntity
    {
        private Texture2D sprite;
        private Texture2D bulletSprite;
        public string Name { get; } = "player";

        bool IEntity.Collidable {
            get {
                return true;
            }
        }

        private int _x;
        private int _y;
        private Game1 game;
        private int maxX;
        private const int fireCooldown = 100;
        private int currentCooldown = 0;

        public Player(Game1 g, Texture2D mySprite, Texture2D BulletSprite)
        {
            game = g;
            sprite = mySprite;
            bulletSprite = BulletSprite;
            maxX = game.GraphicsDevice.PresentationParameters.BackBufferWidth - sprite.Width;
            _x = 400 - sprite.Width;
            _y = 600 - sprite.Height;
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics, SpriteFont font)
        {
            spriteBatch.Draw(sprite, new Rectangle(_x, _y, 64, 64), Color.White);
        }

        public void Initialize()
        {
        }

        public void Update(Game1 game, KeyboardState currentState, KeyboardState lastState, GameTime gameTime)
        {
            if (currentState.IsKeyDown(Keys.Left))
            {
                Move(-5, 0);
            }
            if (currentState.IsKeyDown(Keys.Right))
            {
                Move(5, 0);
            }
            if (currentState.IsKeyDown(Keys.Space) && !lastState.IsKeyDown(Keys.Space))
            {
                if (game.GetEntites(x => x.Name == "bullet") == null)

                {
                    game.AddEntity(new Bullet(game, bulletSprite, _x + 25, _y - 30));
                }
            }

            if (_x > maxX)
            {
                _x = maxX;
            }
            if (_x < 0)
            {
                _x = 0;
            }

            if (currentCooldown > 0)
            {
                currentCooldown--;
            }
        }

        public void Move(int x, int y)
        {
            _x += x;
            _y += y;
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
            if (collider.Name == "enemy")
            {
                game.changeGameState(Game1.GameStates.GAMEOVER);
            }
        }
    }
}