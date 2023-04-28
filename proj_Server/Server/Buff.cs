using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Buff
    {
        public string bufftag;
        public float amount;
        public int remainturn;
        public Buff(string tag, int am, int turn)
        {
            bufftag = tag;
            amount = am;
            remainturn = turn;
        }
    }
}
