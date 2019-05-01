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
    class Tile : Object
    {
        Vector2 newPos;
        float time;
        int num;

        public Tile(Game game1, String textureLoc, Vector2 pos, int inNum) : base(game1, textureLoc, pos)
        {
            num = inNum;
            newPos = position;
        }

        public int GetNum() { return num; }

        public void MoveTile(Vector2 inputNewPos, float newTime)
        {
            newPos = inputNewPos;
            time = newTime;
            velocity = (newPos - position) / new Vector2(time, time);
        }
        public override void setPos(Vector2 pos)
        {
            position = pos;
            newPos = pos;
        }

        public void Reset()
        {
            velocity = new Vector2(0, 0);
            position = newPos;
            time = 0;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (time > 0)
            {
                time -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (time < 0)
                {
                    velocity = new Vector2(0, 0);
                    position = newPos;
                }
            } 
        }

    }
}
