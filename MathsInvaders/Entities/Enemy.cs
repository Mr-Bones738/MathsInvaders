using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace MathsInvaders.Entities
{
    internal class Enemy : IEntity
    {
        public string Name { get; }
        private int _x;
        private int _y;
        private Texture2D sprite;
        private Game1 game;
        private int number;
        private Color enemyColor;
        private const int moveCDBase = 800;
        private int moveCDMax;
        private int moveCDCurrent;
        private const int movesBeforeDown = 10;
        private int currentMoves = 0;
        private bool moveRight = true;
        private bool dead = false;

        bool IEntity.Collidable {
            get {
                return true;
            }
        }

        public Enemy(Game1 g, int initx, int inity, Texture2D mySprite, Random rnd)
        {
            Name = "enemy";
            game = g;
            moveCDCurrent = moveCDBase;
            moveCDMax = moveCDBase;
            _x = initx;
            _y = inity;
            sprite = mySprite;
            number = rnd.Next(1, g.maxEnemyNumber);
            if (inity < 100)
                enemyColor = Color.White;
            if (inity >= 100 && inity < 220)
                enemyColor = Color.Yellow;
            if (inity >= 220)
                enemyColor = Color.Orange;
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics, SpriteFont font)
        {
            spriteBatch.Draw(sprite, new Rectangle(_x, _y, 64, 64), enemyColor);
            spriteBatch.DrawString(font, number.ToString(), new Vector2(_x + 25, _y + 27), Color.Black);
        }

        public void Move(int x, int y)
        {
            _x += x;
            _y += y;
        }

        public void Initialize()
        {
        }

        public void Update(Game1 game, KeyboardState currentState, KeyboardState lastState, GameTime gameTime)
        {
            moveCDMax = moveCDBase;
            int timeElapsed = gameTime.ElapsedGameTime.Milliseconds;
            if (moveCDCurrent <= 0)
            {
                CalculateMove();
            }
            if (moveCDCurrent > 0)
            {
                moveCDCurrent -= timeElapsed;
            }
        }

        public void CalculateMove()
        {
            if (currentMoves == movesBeforeDown)
            {
                Move(0, 32);
                currentMoves = 0;
                moveCDCurrent = moveCDMax;
                moveRight = !moveRight;
            }
            else
            {
                if (moveRight)
                    Move(16, 0);
                else
                    Move(-16, 0);
                currentMoves++;
                moveCDCurrent = moveCDMax;
            }
        }

        public int GetEnemyNumber()
        {
            return number;
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
            if (collider.Name == "bullet" && !dead)
            {
                dead = true;
                game.RemoveEntity(this);
                game.RemoveEntity(collider);
                game.MathAddNumber(number);
            }
        }
    }
}