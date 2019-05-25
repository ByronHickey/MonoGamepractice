using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using Color = Microsoft.Xna.Framework.Color;

namespace Game3
{


    public class CharacterEntity : ICloneable
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
        Animation currentAnimation;
        Vector2 desiredVelocity = new Vector2();
        public bool clone = false;
        int csY;   
        
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

        public float cloneX
        {
            get;
            set;
        }

        public float cloneY
        {
            get;
            set;
        }




        public CharacterEntity(Texture2D character, int spriteSheetY)
        {
            characterSheetTexture = character;
            csY = spriteSheetY;              

            walkDown = new Animation();
            walkDown.AddFrame(new Rectangle(0, csY, 16, 16), TimeSpan.FromSeconds(.25));
            walkDown.AddFrame(new Rectangle(16, csY, 16, 16), TimeSpan.FromSeconds(.25));
            walkDown.AddFrame(new Rectangle(0, csY, 16, 16), TimeSpan.FromSeconds(.25));
            walkDown.AddFrame(new Rectangle(32, csY, 16, 16), TimeSpan.FromSeconds(.25));

            walkUp = new Animation();
            walkUp.AddFrame(new Rectangle(144, csY, 16, 16), TimeSpan.FromSeconds(.25));
            walkUp.AddFrame(new Rectangle(160, csY, 16, 16), TimeSpan.FromSeconds(.25));
            walkUp.AddFrame(new Rectangle(144, csY, 16, 16), TimeSpan.FromSeconds(.25));
            walkUp.AddFrame(new Rectangle(176, csY, 16, 16), TimeSpan.FromSeconds(.25));

            walkLeft = new Animation();
            walkLeft.AddFrame(new Rectangle(48, csY, 16, 16), TimeSpan.FromSeconds(.25));
            walkLeft.AddFrame(new Rectangle(64, csY, 16, 16), TimeSpan.FromSeconds(.25));
            walkLeft.AddFrame(new Rectangle(48, csY, 16, 16), TimeSpan.FromSeconds(.25));
            walkLeft.AddFrame(new Rectangle(80, csY, 16, 16), TimeSpan.FromSeconds(.25));

            walkRight = new Animation();
            walkRight.AddFrame(new Rectangle(96, csY, 16, 16), TimeSpan.FromSeconds(.25));
            walkRight.AddFrame(new Rectangle(112, csY, 16, 16), TimeSpan.FromSeconds(.25));
            walkRight.AddFrame(new Rectangle(96, csY, 16, 16), TimeSpan.FromSeconds(.25));
            walkRight.AddFrame(new Rectangle(128, csY, 16, 16), TimeSpan.FromSeconds(.25));

            // Standing animations only have a single frame of animation:
            standDown = new Animation();
            standDown.AddFrame(new Rectangle(0, csY, 16, 16), TimeSpan.FromSeconds(.25));

            standUp = new Animation();
            standUp.AddFrame(new Rectangle(144, csY, 16, 16), TimeSpan.FromSeconds(.25));

            standLeft = new Animation();
            standLeft.AddFrame(new Rectangle(48, csY, 16, 16), TimeSpan.FromSeconds(.25));

            standRight = new Animation();
            standRight.AddFrame(new Rectangle(96, csY, 16, 16), TimeSpan.FromSeconds(.25));
        }

      

        

        Vector2 GetDesiredVelocityFromInput()
        {
            

            TouchCollection touchCollection = TouchPanel.GetState();
            
            if (touchCollection.Count > 0 && !clone)
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
            else
            {
                Vector2 gap = new Vector2(cloneX, cloneY);
                desiredVelocity.X = gap.X - this.X;
                desiredVelocity.Y = gap.Y - this.Y;

                if (desiredVelocity.X != 0 || desiredVelocity.Y != 0)
                {
                    desiredVelocity.Normalize();
                    const float desiredSpeed = 400;
                    desiredVelocity *= desiredSpeed;
                }
            }


            return desiredVelocity;
        }


        public void Update(GameTime gameTime)
        {   
            
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

            currentAnimation.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 topLeftOfSprite = new Vector2(this.X, this.Y);          
            
            System.Drawing.Color tintColor = System.Drawing.Color.White;
      
            var sourceRectangle = currentAnimation.CurrentRectangle;            
            spriteBatch.Draw(characterSheetTexture, topLeftOfSprite, sourceRectangle, Color.White);
         
            
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}