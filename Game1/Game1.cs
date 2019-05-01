using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StateModel.InformedSearch;
using StateModel.Interface;
using StateModel.BoardGame;
using System.Collections;

namespace Game1
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        //Constants the should be changed
        int BOARD_SIZE = 3;             //board size
        float ANIMATION_TIME = 0.25F;    //animation time in seconds

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GUISrc.Board board;
        bool rightDown = false, leftDown = false;
        bool animation = true;
        float aniTime;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        private int[] ConvertToIntArray(string str)
        {
            List<int> list = new List<int>();

            string tempStr = "";
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] > 47 && str[i] < 58)
                {
                    tempStr += str[i];
                }
                else if (tempStr != "")
                {
                    list.Add(int.Parse(tempStr));
                    tempStr = "";
                }
                   
            }

            int[] intArray = new int[list.Count];

            int count = 0;
            foreach (int i in list)
            {
                intArray[count] = i;
                count++;
            }

            return intArray;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();

            aniTime = ANIMATION_TIME;

            //Preforms the Puzzle initialisation
            SlidingPuzzle puzzle = new SlidingPuzzle();
            //puzzle.SetState("{1,2,3,4,8,6,7,5,0}");
            puzzle.ShuffleBoard();
            var path = A_StarSearch<string, SlidingPuzzleAction>.Search(
                            puzzle,
                            "{1,2,3,4,5,6,7,8,0}");

            System.Console.WriteLine("Num steps = " + path.Count);

            //adds the steps to the board object
            board = new GUISrc.Board(this, ConvertToIntArray(path.Pop()), BOARD_SIZE);
            while (path.Count > 0)
            {

                board.AddState(ConvertToIntArray(path.Pop()));
            }

            
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            int[] initialState = { 1, 0, 3, 2, 4, 5, 6 ,7 ,8 };
            
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            //Left and Right keys can control go back and forward
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                if (!rightDown)
                {
                    board.NextState((float)ANIMATION_TIME);
                }
                rightDown = true;
                animation = false;
            }
            else
                rightDown = false;
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                if (!leftDown)
                {
                    board.PreviousState((float)ANIMATION_TIME);
                }
                leftDown = true;
                animation = false;
            }
            else
                leftDown = false;

            //Plays the animation of the moves when it is started
            if (animation)
            {
                if (aniTime > 0)
                    aniTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                else
                {
                    aniTime = ANIMATION_TIME; ;
                    board.NextState((float)ANIMATION_TIME);
                }
            }

            // TODO: Add your update logic here
            board.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            board.Draw(gameTime, ref spriteBatch);
            spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
