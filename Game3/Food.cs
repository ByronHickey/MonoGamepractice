using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using Color = Microsoft.Xna.Framework.Color;

namespace Game3
{
    class Food
    {
        static Texture2D characterSheetTexture;
        Animation food;
        Vector2 foodLocation;
        int csY;
        public int counter = 0;

        public float X
        {
            get;
            set;
        }

        public float Y
        {
            get;
            set;
        }

        public float goodX
        {
            get;
            set;
        }

        public float goodY
        {
            get;
            set;
        }

        public Food(Texture2D character, int spriteSheetY)
        {
            characterSheetTexture = character;
            csY = spriteSheetY;

            food = new Animation();
            food.AddFrame(new Rectangle(0, 112, 16, 16), TimeSpan.FromSeconds(.25));
            food.AddFrame(new Rectangle(16, 112, 16, 16), TimeSpan.FromSeconds(.25));
            food.AddFrame(new Rectangle(0, 112, 16, 16), TimeSpan.FromSeconds(.25));
            food.AddFrame(new Rectangle(32, 112, 16, 16), TimeSpan.FromSeconds(.25));

            foodLocation = PickLocation();
        }

        Vector2 PickLocation()
        {
            Random random = new Random();
            int randY = random.Next(16, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 16);
            int randX = random.Next(16, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 16);

            Vector2 location = new Vector2(randX, randY);

            return location;
        }

        private bool Eat(Vector2 pos)
        {
            Vector2 charPos = new Vector2(goodX, goodY);

            if (Vector2.Distance(charPos, pos) > 16)
            {
                return false;
            }
            else
            {
                foodLocation = PickLocation();
                counter++;
                return true;

            }


        }

        public void Update(GameTime gameTime)
        {
            food.Update(gameTime);
            Eat(foodLocation);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
           
            Vector2 foodVector = foodLocation;
            System.Drawing.Color tintColor = System.Drawing.Color.White;
            var foodRectangle = food.CurrentRectangle;     
            spriteBatch.Draw(characterSheetTexture, foodVector, foodRectangle, Color.White);

        }



    }

}
