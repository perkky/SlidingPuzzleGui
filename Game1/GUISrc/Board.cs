using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game1.GUISrc
{
    class Board
    {
        const int TILE_SIZE = 80;
        Vector2 OFFSET = new Vector2(20, 20);

        List<Tile> tiles;
        List<int[]> states;
        int stateIndex = 0;
        int size;

        public Board(Game game1, int[] initialState, int inSize) //size eg 3, 4, 5
        {
            tiles = new List<Tile>();
            states = new List<int[]>();
            size = inSize;
            LoadContent(game1);
            AddState(initialState);
            InitialSetup();
        }

        private void LoadContent(Game game1)
        {
            for (int i = 0; i < size*size; i++)
            {
                tiles.Add(new Tile(game1, "Images\\" + (i).ToString(), new Vector2(0, 0), i));
            }
        }

        private void InitialSetup()
        {
            int[] initialState = states[0];

            int tileLocation = 0;
            foreach (int i in initialState)
            {
                tiles[i].setPos(new Vector2(OFFSET.X + (TILE_SIZE+5) * (tileLocation % size), OFFSET.Y + (TILE_SIZE+5) * (int)(tileLocation / size)));
                tileLocation++;
            }
        }

        public void AddState(int[] state)
        {
            states.Add(state);
        }

        public void NextState(float time)
        {
            stateIndex++;
            if (stateIndex < states.Count)
            {
                ChangeState(states[stateIndex], time);
            }
            else
                stateIndex--;
        }
        public void PreviousState(float time)
        {
            stateIndex--;
            if (stateIndex >= 0)
            {
                ChangeState(states[stateIndex], time);
            }
            else
                stateIndex++;
        }

        private void ChangeState(int[] nextState, float time)
        {
            int tileLocation = 0;
            foreach (int i in nextState)
            {
                tiles[i].Reset();
                tiles[i].MoveTile(new Vector2(OFFSET.X + (TILE_SIZE + 5) * (tileLocation % size), OFFSET.Y + (TILE_SIZE + 5) * (int)(tileLocation / size)), time);
                tileLocation++;
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (Tile t in tiles)
            {
                t.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime, ref SpriteBatch spriteBatch)
        {
            foreach (Tile t in tiles)
            {
                t.Draw(gameTime, ref spriteBatch);
            }
        }
    }
}
