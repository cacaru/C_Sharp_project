using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Player
    {
        private int playernum;
        private Character[] characters;
        private Player enemy;

        public int Playernum { get => playernum; set => playernum = value; }
        public Character[] Characters { get => characters; set => characters = value; }
        public Player Enemy { get => enemy; set => enemy = value; }

        public Player(bool player,  int num)
        {
            if (player)
            {
                if (num == 0)
                {
                    characters = new Character[3];
                    characters[0] = new PlayableDealer();
                    characters[1] = new PlayableHealer();
                    characters[2] = new PlayableTanker();
                }
                else
                {
                    characters = new Character[3];
                    characters[0] = new PlayableDealer(true);
                    characters[1] = new PlayableHealer(true);
                    characters[2] = new PlayableTanker(true);
                }

                if (num == 2)
                {
                    characters[0].Init(true);
                    characters[1].Init(true);
                    characters[2].Init(true);
                }
                else
                {
                    characters[0].Init();
                    characters[1].Init();
                    characters[2].Init();
                }
            }
            else
            {
                characters = new Character[1];
                characters[0] = new Boss();
				((Boss)characters[0]).TrollGiant();
				characters[0].Init();
			}
        }

        public void Setenemy(Player enemy)
        {
            this.enemy = enemy;
            foreach(Character ch in characters)
            {
                ch.Enemy = enemy.characters;
                ch.Ally = this.characters;
            }
        }
    }
}
