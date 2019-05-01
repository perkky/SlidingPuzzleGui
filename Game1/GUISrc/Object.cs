using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game1.GUISrc
{
    class Object
    {
        protected Texture2D texture;
        protected Vector2 position;
        protected Vector2 velocity;

        public Object(Game game1, String textureLoc, Vector2 pos)
        {
            texture = game1.Content.Load<Texture2D>(textureLoc);
            this.setPos(pos);
            velocity = new Vector2(0, 0);
        }

        //Sets the position
        public virtual void setPos(Vector2 pos)
        {
            position = pos;
        }

        //moves the object by the inputted vector amount
        public void move(Vector2 pos)
        {
            position += pos;
        }

        public virtual void Update(GameTime gameTime)
        {
            position += velocity* (float)gameTime.ElapsedGameTime.TotalSeconds;
            
        }

        public void Draw(GameTime gameTime, ref SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position);
        }
    }
}
