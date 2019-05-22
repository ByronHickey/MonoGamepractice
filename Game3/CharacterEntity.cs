using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using Color = Microsoft.Xna.Framework.Color;

namespace Game3
{


    public class CharacterEntity
    {
        
        static Texture2D characterSheetTexture;
        Animation walkDown;
        Animation walkUp;
        Animation walkLeft;
        Animation walkRight;
        Animation standDown;
        Animation standUp;
        Animation standLeft;
        Animation standRight;
        Animation food;
        Animation currentAnimation;
        Animation walkDownBad;
        Vector2 foodLocation;
        List<Vector2> trail = new List<Vector2>();

        int counter = 0;
        
        
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
 


        public CharacterEntity(Texture2D character)
        {
            characterSheetTexture = character;

            food = new Animation();
            food.AddFrame(new Rectangle(0, 112, 16, 16), TimeSpan.FromSeconds(.25));
            food.AddFrame(new Rectangle(16, 112, 16, 16), TimeSpan.FromSeconds(.25));
            food.AddFrame(new Rectangle(0, 112, 16, 16), TimeSpan.FromSeconds(.25));
            food.AddFrame(new Rectangle(32, 112, 16, 16), TimeSpan.FromSeconds(.25));

            foodLocation = PickLocation();

            walkDownBad = new Animation();
            walkDownBad.AddFrame(new Rectangle(0, 96, 16, 16), TimeSpan.FromSeconds(.25));
            walkDownBad.AddFrame(new Rectangle(16, 96, 16, 16), TimeSpan.FromSeconds(.25));
            walkDownBad.AddFrame(new Rectangle(0, 96, 16, 16), TimeSpan.FromSeconds(.25));
            walkDownBad.AddFrame(new Rectangle(32, 96, 16, 16), TimeSpan.FromSeconds(.25));

            walkDown = new Animation();
            walkDown.AddFrame(new Rectangle(0, 96, 16, 16), TimeSpan.FromSeconds(.25));
            walkDown.AddFrame(new Rectangle(16, 96, 16, 16), TimeSpan.FromSeconds(.25));
            walkDown.AddFrame(new Rectangle(0, 96, 16, 16), TimeSpan.FromSeconds(.25));
            walkDown.AddFrame(new Rectangle(32, 96, 16, 16), TimeSpan.FromSeconds(.25));

            walkUp = new Animation();
            walkUp.AddFrame(new Rectangle(144, 96, 16, 16), TimeSpan.FromSeconds(.25));
            walkUp.AddFrame(new Rectangle(160, 96, 16, 16), TimeSpan.FromSeconds(.25));
            walkUp.AddFrame(new Rectangle(144, 96, 16, 16), TimeSpan.FromSeconds(.25));
            walkUp.AddFrame(new Rectangle(176, 96, 16, 16), TimeSpan.FromSeconds(.25));

            walkLeft = new Animation();
            walkLeft.AddFrame(new Rectangle(48, 96, 16, 16), TimeSpan.FromSeconds(.25));
            walkLeft.AddFrame(new Rectangle(64, 96, 16, 16), TimeSpan.FromSeconds(.25));
            walkLeft.AddFrame(new Rectangle(48, 96, 16, 16), TimeSpan.FromSeconds(.25));
            walkLeft.AddFrame(new Rectangle(80, 96, 16, 16), TimeSpan.FromSeconds(.25));

            walkRight = new Animation();
            walkRight.AddFrame(new Rectangle(96, 96, 16, 16), TimeSpan.FromSeconds(.25));
            walkRight.AddFrame(new Rectangle(112, 96, 16, 16), TimeSpan.FromSeconds(.25));
            walkRight.AddFrame(new Rectangle(96, 96, 16, 16), TimeSpan.FromSeconds(.25));
            walkRight.AddFrame(new Rectangle(128, 96, 16, 16), TimeSpan.FromSeconds(.25));

            // Standing animations only have a single frame of animation:
            standDown = new Animation();
            standDown.AddFrame(new Rectangle(0, 96, 16, 16), TimeSpan.FromSeconds(.25));

            standUp = new Animation();
            standUp.AddFrame(new Rectangle(144, 96, 16, 16), TimeSpan.FromSeconds(.25));

            standLeft = new Animation();
            standLeft.AddFrame(new Rectangle(48, 96, 16, 16), TimeSpan.FromSeconds(.25));

            standRight = new Animation();
            standRight.AddFrame(new Rectangle(96, 96, 16, 16), TimeSpan.FromSeconds(.25));
        }

        Vector2 PickLocation()
        {
            Random random = new Random();
            int randY = random.Next(0, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
            int randX = random.Next(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width);
           
            Vector2 location = new Vector2(randX, randY);

            return location;
        }

        

        Vector2 GetDesiredVelocityFromInput()
        {
            Vector2 desiredVelocity = new Vector2();

            TouchCollection touchCollection = TouchPanel.GetState();
            
            if (touchCollection.Count > 0)
            {
                desiredVelocity.X = touchCollection[0].Position.X  - this.X;
                desiredVelocity.Y = touchCollection[0].Position.Y - this.Y;

                if (desiredVelocity.X != 0 || desiredVelocity.Y != 0)
                {
                    desiredVelocity.Normalize();
                    const float desiredSpeed = 400;
                    desiredVelocity *= desiredSpeed;
                }
            }

            return desiredVelocity;
        }


        private void GrowTrail() {
           
            if(trail.Count == counter)
                for (var i = 0; i < trail.Count; i++)
                {
                    Vector2 t = new Vector2(this.X, this.Y);
                    trail.Add(t);
                }
               
            

        }

        private bool Eat(Vector2 pos)
        {
            Vector2 charPos = new Vector2(this.X, this.Y);

            if(Vector2.Distance(charPos, pos) > 16)
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
            // temporary - we'll replace this with logic based off of which way the
            // character is moving when we add movement logic

            food.Update(gameTime);
            
            var velocity = GetDesiredVelocityFromInput();

           
            this.X += velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds;
            this.Y += velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (velocity != Vector2.Zero)
            {
                bool movingHorizontally = Math.Abs(velocity.X) > Math.Abs(velocity.Y);
                if (movingHorizontally)
                {
                    if (velocity.X > 0)
                    {
                        currentAnimation = walkRight;
                    }
                    else
                    {
                        currentAnimation = walkLeft;
                    }
                }
                else
                {
                    if (velocity.Y > 0)
                    {
                        currentAnimation = walkDown;
                        
                    }
                    else
                    {
                        currentAnimation = walkUp;
                    }
                }
            }
            else
            {
                // If the character was walking, we can set the standing animation
                // according to the walking animation that is playing:
                if (currentAnimation == walkRight)
                {
                    currentAnimation = standRight;
                }
                else if (currentAnimation == walkLeft)
                {
                    currentAnimation = standLeft;
                }
                else if (currentAnimation == walkUp)
                {
                    currentAnimation = standUp;
                }
                else if (currentAnimation == walkDown)
                {
                    currentAnimation = standDown;
                }
                else if (currentAnimation == null)
                {
                    currentAnimation = standDown;
                }

                // if none of the above code hit then the character
                // is already standing, so no need to change the animation.
            }

            Eat(foodLocation);
            GrowTrail();
            
            walkDownBad.Update(gameTime);

            currentAnimation.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 topLeftOfSprite = new Vector2(this.X, this.Y);
            Vector2 foodVector = foodLocation;
            
            System.Drawing.Color tintColor = System.Drawing.Color.White;
            var foodRectangle = food.CurrentRectangle;
            var sourceRectangle = currentAnimation.CurrentRectangle;
            var badGuyRectangle = walkDownBad.CurrentRectangle;
            spriteBatch.Draw(characterSheetTexture, topLeftOfSprite, sourceRectangle, Color.White);
            spriteBatch.Draw(characterSheetTexture, foodVector, foodRectangle, Color.White);
            foreach(Vector2 v in trail)
            spriteBatch.Draw(characterSheetTexture, v, badGuyRectangle, Color.White);
        }
    }
}