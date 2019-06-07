using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MathsInvaders
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private List<IEntity> Entities;
        private SpriteFont defaultFont;
        private KeyboardState currentState;
        private KeyboardState lastState;
        private Screens.TitleScreen titleScreen;
        private bool paused = false;
        public int score = 0;
        public int difficulty = 1;
        public int maxEnemyNumber = 9;
        private int maxTarget = 15;
        private Random mathRnd = new Random();
        private int mathTarget = 0;
        private int mathNum1 = 0;
        private int mathNum2 = 0;
        private int mathNum3 = 0;
        private int mathNum4 = 0;
        private Texture2D tint;
        public float gameSpeedMultiplier = 1;
        public Systems.CollisionSystem collisionSystem;

        public enum GameStates
        {
            MENU,
            INGAME,
            GAMEOVER,
            WIN
        };

        private GameStates currentGameState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            currentGameState = GameStates.MENU;
            Entities = new List<IEntity>();
            collisionSystem = new Systems.CollisionSystem();
        }

        private Camera2D _camera;

        protected override void Initialize()
        {
            base.Initialize();
            Window.Title = "Maths Invaders!";
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            graphics.ApplyChanges();
            Window.AllowUserResizing = true;
            Window.AllowAltF4 = true;
            changeGameState(GameStates.MENU);
            _camera = new Camera2D(new BoxingViewportAdapter(Window, GraphicsDevice, 800, 600));
            tint = new Texture2D(GraphicsDevice, 1, 1);
            tint.SetData<Color>(new Color[] { Color.White });
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            defaultFont = Content.Load<SpriteFont>("default");
            titleScreen = new Screens.TitleScreen(Content.Load<Texture2D>("minvaders/title"));
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            currentState = Keyboard.GetState();
            switch (currentGameState)
            {
                case GameStates.INGAME:
                    if (!Entities.FindAll(x => x.Name == "enemy").Any())
                    {
                        changeGameState(Game1.GameStates.WIN);
                    }
                    foreach (IEntity c in Entities.ToList())
                    {
                        if (!paused)
                        {
                            c.Update(this, currentState, lastState, gameTime);
                            collisionSystem.Update(Entities.ToList(), gameTime);
                        }
                    }
                    if (currentState.IsKeyDown(Keys.Escape) && paused)
                    {
                        changeGameState(GameStates.MENU);
                    }
                    if (currentState.IsKeyDown(Keys.P) && !lastState.IsKeyDown(Keys.P))
                    {
                        paused = !paused;
                    }
                    if (mathNum1 > mathTarget || mathNum1 + mathNum2 > mathTarget || mathNum1 + mathNum2 + mathNum3 > mathTarget)
                    {
                        ResetMaths();
                    }
                    if (mathNum1 + mathNum2 + mathNum3 + mathNum4 == mathTarget)
                    {
                        score += (100 * difficulty);
                        ResetMaths();
                    }
                    if (mathNum1 > 0 && mathNum2 > 0 && mathNum3 > 0 && mathNum4 > 0)
                    {
                        ResetMaths();
                    }
                    break;

                case GameStates.MENU:
                    if (currentState.IsKeyDown(Keys.Enter) && currentState.IsKeyUp(Keys.LeftAlt) && currentState.IsKeyUp(Keys.LeftAlt))
                    {
                        changeGameState(GameStates.INGAME);
                    }
                    else if (currentState.IsKeyDown(Keys.Escape) && !lastState.IsKeyDown(Keys.Escape))
                    {
                        Exit();
                    }
                    break;

                case GameStates.GAMEOVER:
                    if (currentState.IsKeyDown(Keys.Escape))
                    {
                        changeGameState(GameStates.MENU);
                    }
                    else if (currentState.IsKeyDown(Keys.Enter) && currentState.IsKeyUp(Keys.LeftAlt) && currentState.IsKeyUp(Keys.LeftAlt))
                    {
                        changeGameState(GameStates.INGAME);
                    }
                    break;

                case GameStates.WIN:
                    if (currentState.IsKeyDown(Keys.Escape))
                    {
                        changeGameState(GameStates.MENU);
                    }
                    else if (currentState.IsKeyDown(Keys.Enter) && currentState.IsKeyUp(Keys.LeftAlt) && currentState.IsKeyUp(Keys.LeftAlt))
                    {
                        changeGameState(GameStates.INGAME);
                    }
                    break;
            }
            if (currentState.IsKeyDown(Keys.LeftAlt) || currentState.IsKeyDown(Keys.RightAlt))
            {
                if (currentState.IsKeyDown(Keys.Enter) && lastState.IsKeyUp(Keys.Enter))
                {
                    graphics.ToggleFullScreen();
                }
            }
            lastState = Keyboard.GetState();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            var transformMatrix = _camera.GetViewMatrix();
            spriteBatch.Begin(transformMatrix: transformMatrix);
            switch (currentGameState)
            {
                case GameStates.INGAME:
                    foreach (IEntity c in Entities.ToList())
                    {
                        c.Draw(spriteBatch, graphics, defaultFont);
                    }
                    spriteBatch.DrawString(defaultFont, "Score: " + score, new Vector2(0, 0), Color.White);
                    spriteBatch.DrawString(defaultFont, "Level: " + difficulty, new Vector2(680, 0), Color.White);

                    string mathNum1display;
                    string mathNum2display;
                    string mathNum3display;
                    string mathNum4display;
                    if (mathNum1 == 0)
                    {
                        mathNum1display = "?";
                    }
                    else
                    {
                        mathNum1display = mathNum1.ToString();
                    }
                    if (mathNum2 == 0)
                    {
                        mathNum2display = "?";
                    }
                    else
                    {
                        mathNum2display = mathNum2.ToString();
                    }
                    if (mathNum3 == 0)
                    {
                        mathNum3display = "?";
                    }
                    else
                    {
                        mathNum3display = mathNum3.ToString();
                    }
                    if (mathNum4 == 0)
                    {
                        mathNum4display = "?";
                    }
                    else
                    {
                        mathNum4display = mathNum4.ToString();
                    }
                    spriteBatch.DrawString(defaultFont, mathNum1display + " + " + mathNum2display + " + " + mathNum3display + " + " + mathNum4display + " = Target Number: " + mathTarget, new Vector2(200, 0), Color.White);
                    if (paused)
                    {
                        spriteBatch.Draw(tint, new Rectangle(0, 0, 800, 600), new Color(new Vector4(0.0f, 0.0f, 0.0f, 0.5f)));
                        spriteBatch.DrawString(defaultFont, "Paused. \nPress Escape to go to menu.\nPress P to resume.", new Vector2(250, 50), Color.White);
                    }
                    break;

                case GameStates.MENU:
                    titleScreen.Draw(spriteBatch);
                    break;

                case GameStates.GAMEOVER:
                    titleScreen.Draw(spriteBatch);
                    spriteBatch.DrawString(defaultFont, "Final Score: " + score, new Vector2(400, 0), Color.White);
                    break;

                case GameStates.WIN:
                    titleScreen.Draw(spriteBatch);
                    spriteBatch.DrawString(defaultFont, "Final Score: " + score, new Vector2(400, 0), Color.White);
                    break;
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public void changeGameState(GameStates g)
        {
            currentGameState = g;
            switch (g)
            {
                case GameStates.GAMEOVER:
                    score = 0;
                    difficulty = 1;
                    Entities = new List<IEntity>();
                    titleScreen.Sprite = Content.Load<Texture2D>("minvaders/gameover");
                    break;

                case GameStates.INGAME:
                    Entities.Add(new Entities.Player(this, Content.Load<Texture2D>("minvaders/player"), Content.Load<Texture2D>("minvaders/bullet")));
                    Util.SpawnEnemies.spawnEnemies(Entities, this, 4, 9);
                    maxEnemyNumber = 9 * difficulty;
                    maxTarget = 15 * difficulty;
                    paused = false;
                    ResetMaths();
                    break;

                case GameStates.MENU:
                    score = 0;
                    difficulty = 1;
                    Entities = new List<IEntity>();
                    titleScreen.Sprite = Content.Load<Texture2D>("minvaders/title");
                    break;

                case GameStates.WIN:
                    difficulty++;
                    Entities = new List<IEntity>();
                    titleScreen.Sprite = Content.Load<Texture2D>("minvaders/win");
                    break;
            }
        }

        public void MathAddNumber(int number)
        {
            if (mathNum1 == 0)
            {
                mathNum1 = number;
                return;
            }
            if (mathNum2 == 0)
            {
                mathNum2 = number;
                return;
            }
            if (mathNum3 == 0)
            {
                mathNum3 = number;
                return;
            }
            if (mathNum4 == 0)
            {
                mathNum4 = number;
                return;
            }
        }

        public void ResetMaths()
        {
            bool isPossible = false;
            List<IEntity> listOfEnemies = Entities.FindAll(x => x.Name == "enemy");
            int startIndex = listOfEnemies.Count() - 5;
            while (!isPossible)
            {
                mathTarget = mathRnd.Next(0, maxTarget);

                List<int> results = new List<int>();

                if (startIndex >= 0)
                {
                    for (int i = startIndex; i < listOfEnemies.Count(); i++)
                    {
                        for (int x = startIndex; x < listOfEnemies.Count(); x++)
                        {
                            results.Add(listOfEnemies[i].GetEnemyNumber() + listOfEnemies[x].GetEnemyNumber());
                        }
                    }
                    foreach (int result in results)
                    {
                        if (result == mathTarget)
                            isPossible = true;
                    }
                }
                else
                {
                    isPossible = true;
                }
            }
            mathNum1 = 0;
            mathNum2 = 0;
            mathNum3 = 0;
            mathNum4 = 0;
        }

        public void AddEntity(IEntity e)
        {
            Entities.Add(e);
        }

        public void RemoveEntity(IEntity e)
        {
            Entities.Remove(e);
        }

        public IEntity GetEntites(Predicate<IEntity> e)
        {
            return Entities.Find(e);
        }

        public List<IEntity> GetEntityList()
        {
            return Entities;
        }
    }
}