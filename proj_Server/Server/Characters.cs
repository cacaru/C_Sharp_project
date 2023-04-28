using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server
{
    public class Character
    {
        protected int characternum;
        protected int maxhp;
        protected int hp;
        protected int attack;
        protected int armor;
        protected int criticalrate;
        protected float criticaldmg;
        protected int x;
        protected int y;
        protected int speed;
        protected int turngauge;
        protected int cost;
        protected bool myturn = false;
        protected List<Buff> buffs;

        protected Character[] enemy;
        protected Character[] ally;

        protected List<Card> cards;
        protected Queue<Card> deck;
        protected Card[] hand;
        protected bool alive = true;
        protected bool stun = false;

        public bool original = true;

        public int Characternum { get => characternum; }
        public int Maxhp { get => maxhp; }
        public int Hp { get => hp; }
        public int Attack
        {
            get
            {
                int tempa = attack;
                foreach(Buff buff in buffs)
                {
                    if(buff.bufftag=="AP")
                        tempa = (int)(tempa + buff.amount + 0.001);
                }
                foreach (Buff buff in buffs)
                {
                    if (buff.bufftag == "AM")
                        tempa = (int)((tempa * (1 + buff.amount / 100f)) + 0.001);
                }
                return tempa;
            }
        }
        
        public int Armor
        {
            get
            {
                int tempa = armor;
                foreach (Buff buff in buffs)
                {
                    if (buff.bufftag == "D")
                        tempa = (int)(tempa + buff.amount + 0.001);
                }
                return tempa;
            }
        }
        public int Criticalrate
        {
            get
            {
                int tempc = criticalrate;
                foreach (Buff buff in buffs)
                {
                    if (buff.bufftag == "C")
                        tempc = (int)(tempc + buff.amount + 0.001);
                }
                return tempc;
            }
        }
        public float Criticaldmg { get => criticaldmg; }
        public int Speed { get => speed; }
        public int Turngauge { get => turngauge; }
        public int X { get => x; }
        public int Y { get => y; }
        public Character[] Enemy { get => enemy; set => enemy = value; }
        public Character[] Ally { get => ally; set => ally = value; }
        public List<Card> Cards { get => cards; }
        public Queue<Card> Deck { get => deck; }
        public Card[] Hand { get => hand; }
        public List<Buff> Buffs { get => buffs; set => buffs = value; }
        public bool Myturn { get => myturn; }
        public int Cost { get => cost; }
        public bool Alive { get => alive; }

        protected Character()
        {
            cards = new List<Card>(20);
            deck = new Queue<Card>(20);
            hand = new Card[5];
			buffs = new List<Buff>();
            for (int i = 0; i < 5; i++)
                hand[i] = null;
        }

        public virtual void Init() { }
        public virtual void Init(bool asdf) { }

        public void Hit(int damage)
        {
            if (!alive)
                return;
            hp -= (int)((damage * (100f / (float)(Armor + 100))) + 0.001);
            if (hp <= 0)
                Dead();
            else
            {
                if (original)
                    Form1.TCPdata.Add((byte)(130 + characternum));
            }
        }

        protected virtual void Dead()
        {
            if (original)
                Form1.TCPdata.Add((byte)(140 + characternum));
            alive = false;
            hp = 0;
            turngauge = 0;
        }

        public void Recovery(int amount)
        {
            if (!alive)
                return;
            hp += amount;
            if (hp > maxhp)
                hp = maxhp;
        }

        public void Move(int x, int y)
        {
            if (!alive)
                return;
			if (this.x + x >= 8 || this.x + x < 0 || this.y + y >= 5 || this.y + y < 0)
				return;
            foreach (Character ch in ally)
			{
				if ((this.x + x) == ch.x && (this.y + y) == ch.y)
					return;
			}
			foreach (Character ch in enemy)
			{
				if ((this.x + x) == ch.x && (this.y + y) == ch.y)
					return;
			}
            if (original)
            {
                if (x == -1)
                    Form1.TCPdata.Add((byte)(70 + characternum));
                else if (x == 1)
                    Form1.TCPdata.Add((byte)(80 + characternum));
                else if (y == -1)
                    Form1.TCPdata.Add((byte)(90 + characternum));
                else if (y == 1)
                    Form1.TCPdata.Add((byte)(100 + characternum));
                else if (x == -2)
                    Form1.TCPdata.Add((byte)(110 + characternum));
                else if (x == 2)
                    Form1.TCPdata.Add((byte)(120 + characternum));
            }
			this.x += x;
            this.y += y;
        }

        public int Move(int x, int y, bool asdf)
        {
            if (!alive)
                return 0;
            if (this.x + x >= 8 || this.x + x < 0 || this.y + y >= 5 || this.y + y < 0)
                return 0;
            foreach (Character ch in ally)
            {
                if ((this.x + x) == ch.x && (this.y + y) == ch.y)
                    return 0;
            }
            foreach (Character ch in enemy)
            {
                if ((this.x + x) == ch.x && (this.y + y) == ch.y)
                    return 0;
            }
            this.x += x;
            this.y += y;
            return 1;
        }

        public void Revive()
        {
            if (alive)
                return;
            alive = true;
            if (original)
                Form1.TCPdata.Add((byte)(150 + characternum));
            hp = maxhp / 2;
        }

        public void Revive(int hpdenominator)
        {
            if (alive)
                return;
            alive = true;
            hp = maxhp / hpdenominator;
        }

        public void Addbuff(Buff buff)
        {
            if (!alive)
                return;
            buffs.Add(buff);
        }

        public void Usecard(int i)
        {
            if (!myturn)
                return;
            if (hand[i] == null)
                return;
            if (cost >= hand[i].Cost)
            {
                hand[i].Use(this);
                if (hand[i] != null)
                {
                    cost -= hand[i].Cost;
                    hand[i] = null;
                }
            }
        }

        public bool InvestigateCanUse(List<int> uselist)
        {
            if (!myturn)
                return false;
            int costsum = 0;
            foreach(int b in uselist)
            {
                costsum += hand[b].Cost;
            }
            if (cost >= costsum)
                return true;
            else
                return false;
        }

        protected void Draw()
        {
            if (!alive)
                return;
            if (original)
                Form1.TCPdata.Add((byte)(160 + characternum));
            for (int i = 0; i < 5; i++)
            {
                if (Deck.Count == 0)
                    Shuffle();
                Hand[i] = Deck.Dequeue();
            }
        }

        protected void Shuffle()
        {
            if (!alive)
                return;
            Deck.Clear();
            Card[] tempcards = new Card[Cards.Count];
            for (int i = 0; i < Cards.Count; i++)
                tempcards[i] = null;
            Random rand = new Random();
            foreach (Card card in Cards)
            {
                int index;
                do
                {
					index = rand.Next(0, Cards.Count);
                }
                while (tempcards[index] != null);
                tempcards[index] = card;
            }
            for (int i = 0; i < tempcards.Length; i++)
            {
                Deck.Enqueue(tempcards[i]);
            }
        }

        public int Turngaugeadd()
        {
            turngauge += speed;
            if (!alive)
                turngauge = 0;
            return turngauge;
        }

        public void Turngaugeadd(int amount)
        {
            if (!alive)
                return;
            turngauge += amount;
        }
        
        public void Stun()
        {
            stun = true;
        }

        public virtual void Turnstart()
        {
            for (int i = 0; i < buffs.Count; i++)
            {
                buffs[i].remainturn--;
                if (buffs[i].remainturn == 0)
                {
                    buffs.RemoveAt(i);
                    i--;
                }
            }
            if (!alive)
                return;
            Form1.nturn = characternum;
            if(stun)
            {
                stun = false;
                turngauge = 0;
                Turnend();
                return;
            }
            if (deck.Count < 5)
                Shuffle();
            Draw();
			cost = 5;
            myturn = true;
        }

        public void Turnend()
        {
			if (!myturn)
				return;
            for (int i = 0; i < 5; i++)
                hand[i] = null;
            myturn = false;
            turngauge = 0;
            Form1.nturn = 0;
        }

        public void CopyTo(Character target)
        {
            target.characternum = characternum;
            target.maxhp = maxhp;
            target.hp = hp;
            target.attack = attack;
            target.armor = armor;
            target.criticalrate = criticalrate;
            target.criticaldmg = criticaldmg;
            target.x = x;
            target.y = y;
            target.speed = speed;
            target.turngauge = turngauge;
            target.cost = cost;
            target.myturn = myturn;

            foreach(Buff b in buffs)
            {
                target.buffs.Add(b);
            }

            for(int i=0;i<5;i++)
            {
                target.hand[i] = hand[i];
            }

            target.alive = alive;
            target.stun = stun;
        }
    }

    public class PlayableDealer : Character
    {
        public PlayableDealer() : base()
        {
            characternum = 1;
            maxhp = 300;
            hp = 300;
            attack = 100;
            armor = 20;
            criticalrate = 20;
            criticaldmg = 2;
            speed = 12;
            cards.Add(new MoveR(1));
            cards.Add(new MoveR(1));
            cards.Add(new MoveL(1));
            cards.Add(new MoveL(1));
            cards.Add(new MoveU(1));
            cards.Add(new MoveU(1));
            cards.Add(new MoveD(1));
            cards.Add(new MoveD(1));
            cards.Add(new BasicAttack(1));
            cards.Add(new BasicAttack(1));
            cards.Add(new BasicAttack(1));
            cards.Add(new Guard(1));
            cards.Add(new Dash(1));
            cards.Add(new DrawingASword());
            cards.Add(new Trisāhasramahāsāhāsrolocadhātu());
            cards.Add(new Backstep());
            cards.Add(new Dealer4());
            cards.Add(new Rage());
            cards.Add(new NyangnyangPunch());
            cards.Add(new GroundCut());
        }

        public PlayableDealer(bool asdf) : base()
        {
            characternum = 1;
            maxhp = 300;
            hp = 300;
            attack = 30;
            armor = 20;
            criticalrate = 20;
            criticaldmg = 2;
            speed = 12;
            cards.Add(new MoveR(1));
            cards.Add(new MoveR(1));
            cards.Add(new MoveL(1));
            cards.Add(new MoveL(1));
            cards.Add(new MoveU(1));
            cards.Add(new MoveU(1));
            cards.Add(new MoveD(1));
            cards.Add(new MoveD(1));
            cards.Add(new BasicAttack(1));
            cards.Add(new BasicAttack(1));
            cards.Add(new BasicAttack(1));
            cards.Add(new Guard(1));
            cards.Add(new Dash(1));
            cards.Add(new DrawingASword());
            cards.Add(new Trisāhasramahāsāhāsrolocadhātu());
            cards.Add(new Backstep());
            cards.Add(new Dealer4());
            cards.Add(new Rage());
            cards.Add(new NyangnyangPunch());
            cards.Add(new GroundCut());
        }

        public override void Init()
        {
            Revive(10);
            for (int i = 0; i < 5; i++)
                hand[i] = null;
            myturn = false;
            turngauge = 0;
            Form1.nturn = 0;
            buffs.Clear();
            Recovery((int)(maxhp * 0.1));
            x = 0;
            y = 2;
        }

        public override void Init(bool asdf)
        {
            Revive(10);
            for (int i = 0; i < 5; i++)
                hand[i] = null;
            myturn = false;
            turngauge = 0;
            Form1.nturn = 0;
            buffs.Clear();
            Recovery((int)(maxhp * 0.1));
            x = 7;
            y = 2;
            foreach (Card c in cards)
            {
                c.Reverse();
            }
            characternum += 3;
        }
    }

    public class PlayableHealer : Character
    {
        public PlayableHealer() : base()
        {
            characternum = 2;
            maxhp = 400;
            hp = 400;
            attack = 30;
            armor = 30;
            criticalrate = 10;
            criticaldmg = 1.5f;
            speed = 11;
            cards.Add(new MoveR(2));
            cards.Add(new MoveR(2));
            cards.Add(new MoveL(2));
            cards.Add(new MoveL(2));
            cards.Add(new MoveU(2));
            cards.Add(new MoveU(2));
            cards.Add(new MoveD(2));
            cards.Add(new MoveD(2));
            cards.Add(new BasicAttack(2));
            cards.Add(new Guard(2));
            cards.Add(new Guard(2));
            cards.Add(new Dodge(2));
            cards.Add(new Dodge(2));
            cards.Add(new Heal());
            cards.Add(new Heal());
            cards.Add(new Revive());
            cards.Add(new Heal3());
            cards.Add(new Heal4());
            cards.Add(new Heal4());
            cards.Add(new Heal5());
        }

        public PlayableHealer(bool asdf) : base()
        {
            characternum = 2;
            maxhp = 400;
            hp = 400;
            attack = 10;
            armor = 30;
            criticalrate = 10;
            criticaldmg = 1.5f;
            speed = 11;
            cards.Add(new MoveR(2));
            cards.Add(new MoveR(2));
            cards.Add(new MoveL(2));
            cards.Add(new MoveL(2));
            cards.Add(new MoveU(2));
            cards.Add(new MoveU(2));
            cards.Add(new MoveD(2));
            cards.Add(new MoveD(2));
            cards.Add(new BasicAttack(2));
            cards.Add(new Guard(2));
            cards.Add(new Guard(2));
            cards.Add(new Dodge(2));
            cards.Add(new Dodge(2));
            cards.Add(new Heal());
            cards.Add(new Heal());
            cards.Add(new Revive());
            cards.Add(new Heal3());
            cards.Add(new Heal4());
            cards.Add(new Heal4());
            cards.Add(new Heal5());
        }

        public override void Init()
        {
            Revive(10);
            for (int i = 0; i < 5; i++)
                hand[i] = null;
            myturn = false;
            turngauge = 0;
            Form1.nturn = 0;
            buffs.Clear();
            Recovery((int)(maxhp * 0.1));
            x = 0;
            y = 1;
        }

        public override void Init(bool asdf)
        {
            Revive(10);
            for (int i = 0; i < 5; i++)
                hand[i] = null;
            myturn = false;
            turngauge = 0;
            Form1.nturn = 0;
            buffs.Clear();
            Recovery((int)(maxhp * 0.1));
            x = 7;
            y = 1;
            foreach (Card c in cards)
            {
                c.Reverse();
            }
            characternum += 3;
        }
    }

    public class PlayableTanker : Character
    {
        public PlayableTanker() : base()
        {
            characternum = 3;
            maxhp = 600;
            hp = 600;
            attack = 50;
            armor = 70;
            criticalrate = 5;
            criticaldmg = 5;
            speed = 10;
            cards.Add(new MoveR(3));
            cards.Add(new MoveR(3));
            cards.Add(new MoveL(3));
            cards.Add(new MoveL(3));
            cards.Add(new MoveU(3));
            cards.Add(new MoveU(3));
            cards.Add(new MoveD(3));
            cards.Add(new MoveD(3));
            cards.Add(new BasicAttack(3));
            cards.Add(new BasicAttack(3));
            cards.Add(new Guard(3));
            cards.Add(new Guard(3));
            cards.Add(new Guard(3));
            cards.Add(new Dodge(3));
            cards.Add(new Dash(3));
            cards.Add(new Indurate());
            cards.Add(new Tank2());
            cards.Add(new Tank3());
            cards.Add(new Smite());
            cards.Add(new Tank5());
        }

        public PlayableTanker(bool asdf) : base()
        {
            characternum = 3;
            maxhp = 600;
            hp = 600;
            attack = 15;
            armor = 70;
            criticalrate = 5;
            criticaldmg = 5;
            speed = 10;
            cards.Add(new MoveR(3));
            cards.Add(new MoveR(3));
            cards.Add(new MoveL(3));
            cards.Add(new MoveL(3));
            cards.Add(new MoveU(3));
            cards.Add(new MoveU(3));
            cards.Add(new MoveD(3));
            cards.Add(new MoveD(3));
            cards.Add(new BasicAttack(3));
            cards.Add(new BasicAttack(3));
            cards.Add(new Guard(3));
            cards.Add(new Guard(3));
            cards.Add(new Guard(3));
            cards.Add(new Dodge(3));
            cards.Add(new Dash(3));
            cards.Add(new Indurate());
            cards.Add(new Tank2());
            cards.Add(new Tank3());
            cards.Add(new Smite());
            cards.Add(new Tank5());
        }

        public override void Init()
        {
            Revive(10);
            for (int i = 0; i < 5; i++)
                hand[i] = null;
            myturn = false;
            turngauge = 0;
            Form1.nturn = 0;
            buffs.Clear();
            Recovery((int)(maxhp * 0.1));
            x = 0;
            y = 3;
        }

        public override void Init(bool asdf)
        {
            Revive(10);
            for (int i = 0; i < 5; i++)
                hand[i] = null;
            myturn = false;
            turngauge = 0;
            Form1.nturn = 0;
            buffs.Clear();
            Recovery((int)(maxhp * 0.1));
            x = 7;
            y = 3;
            foreach (Card c in cards)
            {
                c.Reverse();
            }
            characternum += 3;
        }
    }

    public class Boss : Character
    {
        public bool canrevive = false;
        public List<int> behaviorlist;
        private int value;

        public Boss() : base(){ }
        public override void Init()
        {
            for (int i = 0; i < 5; i++)
                hand[i] = null;
            myturn = false;
            turngauge = 0;
            Form1.nturn = 0;
            buffs.Clear();
            x = 7;
            y = 2;
        }

		public override void Turnstart()
        {
			base.Turnstart();
            
            cost = (characternum * 2) - 2;
            value = -999999999;
            List<int> empty = new List<int>();
            for (int i = 0; i < 5; i++)
                Simulration(enemy, ally, empty, i);

            /*while (myturn)
            {
                List<int> templist = new List<int>();
                for (int i = 0; i < 5; i++)
                {
                    if (hand[i] == null)
                        continue;
                    if (cost >= hand[i].Cost)
                        templist.Add(i);
                }
                if (templist.Count == 0)
                    break;
                Random rand = new Random();
                Usecard(templist[rand.Next(0, templist.Count)]);
            }*/
        }

        private void Simulration(Character[] player, Character[] boss, List<int> blist, int be)
        {
            Character[] copyp = new Character[3];
            copyp[0] = new PlayableDealer();
            copyp[1] = new PlayableHealer();
            copyp[2] = new PlayableTanker();
            player[0].CopyTo(copyp[0]);
            player[1].CopyTo(copyp[1]);
            player[2].CopyTo(copyp[2]);
            copyp[0].original = false;
            copyp[1].original = false;
            copyp[2].original = false;
            Character[] copyb = new Character[1];
            copyb[0] = new Boss();
            boss[0].CopyTo(copyb[0]);
            copyb[0].original = false;
            copyb[0].Enemy = copyp;
            copyb[0].Ally = copyb;
            List<int> copyl = new List<int>();
            foreach(int b in blist)
            {
                copyl.Add(b);
            }
            copyl.Add(be);
            copyb[0].Usecard(be);
            int newvalue = copyb[0].Hp - copyp[0].Hp - copyp[1].Hp - copyp[2].Hp + copyb[0].Armor - copyp[0].Armor - copyp[1].Armor - copyp[2].Armor;
            if (newvalue >= value)
            {
                behaviorlist = copyl;
                value = newvalue;
            }
            for (int i = 0; i < 5; i++)
            {
                if (copyl.Contains(i))
                    continue;
                if (hand[i]!=null)
                {
                    if (copyb[0].Cost >= hand[i].Cost)
                        Simulration(copyp, copyb, copyl, i);
                }
            }
        }

        protected override void Dead()
        {
            base.Dead();
            if (characternum == 4)
            {
                Boss2();
                alive = true;
                foreach (Character ch in enemy)
                {
                    ch.Init();
                }
                Init();
            }
            else if (characternum == 5)
            {
                Boss3();
                alive = true;
                foreach (Character ch in enemy)
                {
                    ch.Init();
                }
                Init();
            }
            else if (characternum == 6)
            {
                if (canrevive)
                {
                    Revive();
                    canrevive = false;
                }
            }
        }

        public void TrollGiant()
        {
            cards = new List<Card>(20);
            deck = new Queue<Card>(20);
            hand = new Card[5];
            for (int i = 0; i < 5; i++)
                hand[i] = null;
            characternum = 4;
            maxhp = 600;
            hp = 600;
            attack = 70;
            armor = 80;
            criticalrate = 0;
            criticaldmg = 0;
            speed = 10;
            cards.Add(new MoveR(4));
            cards.Add(new MoveR(4));
            cards.Add(new MoveL(4));
            cards.Add(new MoveL(4));
            cards.Add(new MoveU(4));
            cards.Add(new MoveU(4));
            cards.Add(new MoveD(4));
            cards.Add(new MoveD(4));
            cards.Add(new Dodge(4));
            cards.Add(new Dash(4));
            cards.Add(new TG1());
            cards.Add(new TG1());
            cards.Add(new TG2());
            cards.Add(new TG2());
            cards.Add(new TG2());
            cards.Add(new TG3());
            cards.Add(new TG3());
            cards.Add(new TG4());
            cards.Add(new TG5());
            cards.Add(new TG6());
        }

        public void Boss2()
        {
            cards = new List<Card>(20);
            deck = new Queue<Card>(20);
            hand = new Card[5];
            for (int i = 0; i < 5; i++)
                hand[i] = null;
            characternum = 5;
            maxhp = 1000;
            hp = 1000;
            attack = 80;
            armor = 70;
            criticalrate = 0;
            criticaldmg = 0;
            speed = 12;
            cards.Add(new MoveR(5));
            cards.Add(new MoveR(5));
            cards.Add(new MoveL(5));
            cards.Add(new MoveL(5));
            cards.Add(new MoveU(5));
            cards.Add(new MoveU(5));
            cards.Add(new MoveD(5));
            cards.Add(new MoveD(5));
            cards.Add(new Dodge(5));
            cards.Add(new Dash(5));
            cards.Add(new Boss21());
            cards.Add(new Boss21());
            cards.Add(new Boss21());
            cards.Add(new Boss22());
            cards.Add(new Boss22());
            cards.Add(new Boss23());
            cards.Add(new Boss24());
            cards.Add(new Boss24());
            cards.Add(new Boss25());
            cards.Add(new Boss26());
        }

        public void Boss3()
        {
            cards = new List<Card>(20);
            deck = new Queue<Card>(20);
            hand = new Card[5];
            for (int i = 0; i < 5; i++)
                hand[i] = null;
            characternum = 6;
            maxhp = 1600;
            hp = 1600;
            attack = 100;
            armor = 100;
            criticalrate = 0;
            criticaldmg = 0;
            speed = 13;
            canrevive = true;
            cards.Add(new MoveR(6));
            cards.Add(new MoveR(6));
            cards.Add(new MoveL(6));
            cards.Add(new MoveL(6));
            cards.Add(new MoveU(6));
            cards.Add(new MoveU(6));
            cards.Add(new MoveD(6));
            cards.Add(new MoveD(6));
            cards.Add(new Dodge(6));
            cards.Add(new Dash(6));
            cards.Add(new Boss31());
            cards.Add(new Boss31());
            cards.Add(new Boss32());
            cards.Add(new Boss32());
            cards.Add(new Boss33());
            cards.Add(new Boss33());
            cards.Add(new Boss34());
            cards.Add(new Boss35());
            cards.Add(new Boss36());
            cards.Add(new Boss37());
        }
    }
}
