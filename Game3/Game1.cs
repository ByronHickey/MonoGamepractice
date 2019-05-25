using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game3
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        CharacterEntity character;
        CharacterEntityBad character2;
        Food food;
        List<CharacterEntity> snakeList = new List<CharacterEntity>();

        Vector2 startV = new Vector2(0, 0);

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.IsFullScreen = true;
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 480;
            graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {

            character = new CharacterEntity(Content.Load<Texture2D>("charactersheet"), 96)
            {
                X = 200,
                Y = 200,

            };

            character2 = new CharacterEntityBad(Content.Load<Texture2D>("charactersheet"), 0)
            {
                X = - 400,
                Y = 200,

            };

            snakeList = new List<CharacterEntity>();

            food = new Food(Content.Load<Texture2D>("charactersheet"), 112);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //characterSheetTexture = Content.Load<Texture2D>("charactersheet");
           /* using (System.IO.Stream stream = TitleContainer.OpenStream(@"Content/charactersheet"))
            {
                characterSheetTexture = Texture2D.FromStream(this.GraphicsDevice, stream);

            }
            */

            // TODO: use this.Content to load your game content here
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                Exit();         

            if (food.counter != snakeList.Count)
            {
                CharacterEntity ce = (CharacterEntity)character.Clone();             
                snakeList.Add(ce);             
                
            }
            character.Update(gameTime);
            character2.Update(gameTime);
            food.Update(gameTime);
            foreach (var s in snakeList.ToArray())
            {
                s.Update(gameTime);
            }
            character2.goodX = character.X;
            character2.goodY = character.Y;
            food.goodX = character.X;
            food.goodY = character.Y;
            Console.WriteLine("snakeList.Count");
            Console.WriteLine(snakeList.Count);
            foreach (var s in snakeList.Select((value, i) => new { i, value })) { }
         
            Console.WriteLine("food.counter");
            Console.WriteLine(food.counter);

            base.Update(gameTime);
        }



        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            character.Draw(spriteBatch);
            character2.Draw(spriteBatch);
            food.Draw(spriteBatch);
            foreach(var s in snakeList)
            {
                s.Draw(spriteBatch);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
