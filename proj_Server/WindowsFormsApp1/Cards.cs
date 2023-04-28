using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Server
{
    public class Card
    {
        protected int cardnum;
        protected int ownernum;
        protected int cost;
        protected int[] rangex;
        protected int[] rangey;

        public int Cardnum { get => cardnum; set => cardnum = value; }
        public int Ownernum { get => ownernum; set => ownernum = value; }
        public int Cost { get => cost; set => cost = value; }
        public int[] Rangex { get => rangex; set => rangex = value; }
        public int[] Rangey { get => rangey; set => rangey = value; }

        protected Card() { }

        public virtual void Use(Character owner)
        {
            if (owner.GetType() == typeof(Boss))
            {
                if (((Boss)owner).original)
                    Form1.TCPdata.Add((byte)cardnum);
            }
            else
                Form1.TCPdata.Add((byte)cardnum);
        }

        public void Reverse()
        {
            for (int i = 0; i < rangex.Length; i++)
                rangex[i] = rangex[i] * (-1);
        }
    }

    public class Move : Card
    {
        protected Move() : base() { }

        public override void Use(Character owner)
        {
            for (int i = 0; i < rangex.Length; i++)
                owner.Move(Rangex[i], Rangey[i]);
        }
    }

    public class Attackcard : Card
    {
        protected float coefficient;

        protected Attackcard() { }

        public override void Use(Character owner)
        {
            base.Use(owner);
            foreach (Character enemy in owner.Enemy)
            {
                for (int i = 0; i < Rangex.Length; i++)
                {
                    if (enemy.X == (Rangex[i] + owner.X) && enemy.Y == (Rangey[i] + owner.Y))
                    {
                        Random rand = new Random();
                        if (rand.Next(0, 100) < owner.Criticalrate)
                            enemy.Hit((int)(owner.Attack * owner.Criticaldmg * coefficient));
                        else
                            enemy.Hit((int)(owner.Attack * coefficient));
                    }
                }
            }
        }
    }

    public class MoveR : Move
    {
        public MoveR(int owner) : base()
        {
            cardnum = 1;
            ownernum = owner;
            cost = 1;
            rangex = new int[1] { 1 };
            rangey = new int[1] { 0 };
        }

        public override void Use(Character owner)
        {
            if (owner.GetType() == typeof(Boss))
            {
                if (((Boss)owner).original)
                    Form1.TCPdata.Add((byte)(160 + ownernum));
            }
            else
                Form1.TCPdata.Add((byte)(160 + ownernum));
            base.Use(owner);
        }
    }

    public class MoveL : Move
    {
        public MoveL(int owner) : base()
        {
            cardnum = 2;
            ownernum = owner;
            cost = 1;
            rangex = new int[1] { -1 };
            rangey = new int[1] { 0 };
        }

        public override void Use(Character owner)
        {
            if (owner.GetType() == typeof(Boss))
            {
                if (((Boss)owner).original)
                    Form1.TCPdata.Add((byte)(170 + ownernum));
            }
            else
                Form1.TCPdata.Add((byte)(170 + ownernum));
            base.Use(owner);
        }
    }

    public class MoveU : Move
    {
        public MoveU(int owner) : base()
        {
            cardnum = 3;
            ownernum = owner;
            cost = 1;
            rangex = new int[1] { 0 };
            rangey = new int[1] { 1 };
        }

        public override void Use(Character owner)
        {
            if (owner.GetType() == typeof(Boss))
            {
                if (((Boss)owner).original)
                    Form1.TCPdata.Add((byte)(180 + ownernum));
            }
            else
                Form1.TCPdata.Add((byte)(180 + ownernum));
            base.Use(owner);
        }
    }

    public class MoveD : Move
    {
        public MoveD(int owner) : base()
        {
            cardnum = 4;
            ownernum = owner;
            cost = 1;
            rangex = new int[1] { 0 };
            rangey = new int[1] { -1 };
        }

        public override void Use(Character owner)
        {
            if (owner.GetType() == typeof(Boss))
            {
                if (((Boss)owner).original)
                    Form1.TCPdata.Add((byte)(190 + ownernum));
            }
            else
                Form1.TCPdata.Add((byte)(190 + ownernum));
            base.Use(owner);
        }
    }

    public class Dash : Move
    {
        public Dash(int owner) : base()
        {
            cardnum = 5;
            ownernum = owner;
            cost = 2;
            rangex = new int[2] { 1, 1 };
            rangey = new int[2] { 0, 0 };
        }

        public override void Use(Character owner)
        {
            if (owner.GetType() == typeof(Boss))
            {
                if (((Boss)owner).original)
                    Form1.TCPdata.Add((byte)(200 + ownernum));
            }
            else
                Form1.TCPdata.Add((byte)(200 + ownernum));
            base.Use(owner);
        }
    }

    public class Dodge : Move
    {
        public Dodge(int owner) : base()
        {
            cardnum = 6;
            ownernum = owner;
            cost = 2;
            rangex = new int[2] { -1, -1 };
            rangey = new int[2] { 0, 0 };
        }

        public override void Use(Character owner)
        {
            if (owner.GetType() == typeof(Boss))
            {
                if (((Boss)owner).original)
                    Form1.TCPdata.Add((byte)(210 + ownernum));
            }
            else
                Form1.TCPdata.Add((byte)(210 + ownernum));
            base.Use(owner);
        }
    }

    public class BasicAttack : Attackcard
    {
        public BasicAttack(int owner) : base()
        {
            cardnum = 7;
            ownernum = owner;
            cost = 1;
            coefficient = 1;
            rangex = new int[3] { 0, 1, 0 };
            rangey = new int[3] { 1, 0, -1 };
        }

        public override void Use(Character owner)
        {
            Form1.TCPdata.Add((byte)(220 + ownernum));
            foreach (Character enemy in owner.Enemy)
            {
                for (int i = 0; i < Rangex.Length; i++)
                {
                    if (enemy.X == (Rangex[i] + owner.X) && enemy.Y == (Rangey[i] + owner.Y))
                    {
                        Random rand = new Random();
                        if (rand.Next(0, 100) < owner.Criticalrate)
                            enemy.Hit((int)(owner.Attack * owner.Criticaldmg * coefficient));
                        else
                            enemy.Hit((int)(owner.Attack * coefficient));
                    }
                }
            }
        }
    }

    public class Guard : Card
    {
        public Guard(int owner):base()
        {
            cardnum = 8;
            ownernum = owner;
            cost = 1;
            rangex = new int[0];
            rangey = new int[0];
        }
        public override void Use(Character owner)
        {
            Form1.TCPdata.Add((byte)(230 + ownernum));
            Buff armor = new Buff("D", 15, 1);
            owner.Addbuff(armor);
        }
    }

    public class DrawingASword : Attackcard
    {
        public DrawingASword()
        {
            cardnum = 11;
            ownernum = 1;
            cost= 5;
            coefficient = 5;
            rangex = new int[12] { -2, -1, -1, -1, 0, 0, 0, 0, 1, 1, 1, 2 };
            rangey = new int[12] { 0, 1, 0, -1, 2, 1, -1, -2, 1, 0, -1, 0 };
        }
    }

    public class Trisāhasramahāsāhāsrolocadhātu : Attackcard
    {
        public Trisāhasramahāsāhāsrolocadhātu()
        {
            cardnum = 12;
            ownernum = 1;
            cost = 3;
            coefficient = 2.5f;
            rangex = new int[5] { 0, 0, 1, 1, 1 };
            rangey = new int[5] { 1, -1, 1, 0, -1 };
        }
        public override void Use(Character owner)
        {
            owner.Move(1, 0);
            owner.Move(1, 0);
            base.Use(owner);
        }
    }

    public class Backstep : Attackcard
    {
        public Backstep()
        {
            cardnum = 13;
            ownernum = 1;
            cost = 2;
            coefficient = 1.5f;
            rangex = new int[4] { 1, 1, 2, 2 };
            rangey = new int[4] { 1, -1, 1, -1 };
        }
        public override void Use(Character owner)
        {
            owner.Move(-1, 0);
            base.Use(owner);
        }
    }

    public class Dealer4 : Attackcard
    {
        public Dealer4()
        {
            cardnum = 14;
            ownernum = 1;
            cost = 2;
            coefficient = 2;
            rangex = new int[4] { -1, -1, 1, 1 };
            rangey = new int[4] { 1, -1, 1, -1 };
        }
    }
    public class Rage : Card
    {
        public Rage()
        {
            cardnum = 15;
            ownernum = 1;
            cost = 1;
            rangex = new int[0];
            rangey = new int[0];
        }
        public override void Use(Character owner)
        {
            base.Use(owner);
            Buff attack = new Buff("AP", 50, 1);
            owner.Addbuff(attack);
        }
    }

    public class NyangnyangPunch : Attackcard
    {
        public NyangnyangPunch()
        {
            cardnum = 16;
            ownernum = 1;
            cost = 3;
            coefficient = 3;
            rangex = new int[3] { 1, 2, 3 };
            rangey = new int[3] { 0, 0, 0, };
        }
    }

    public class GroundCut : Attackcard
    {
        public GroundCut()
        {
            cardnum = 17;
            ownernum = 1;
            cost = 2;
            coefficient = 2;
            rangex = new int[3] { 0, 1, 2 };
            rangey = new int[3] { 1, 0, -1, };
        }

        public override void Use(Character owner)
        {
            base.Use(owner);
            foreach (Character enemy in owner.Enemy)
            {
                for (int i = 0; i < Rangex.Length; i++)
                {
                    if (enemy.X == (Rangex[i] + owner.X) && enemy.Y == (Rangey[i] + owner.Y))
                    {
                        enemy.Turngaugeadd(-50);
                    }
                }
            }
        }
    }

    public class Heal : Card
    {
        public Heal()
        {
            cardnum = 21;
            ownernum = 2;
            cost = 2;
            rangex = new int[5] { 0, -1, 0, 0, 1 };
            rangey = new int[5] { 0, 0, 1, -1, 0 };
        }
        public override void Use(Character owner)
        {
            base.Use(owner);
            foreach (Character ally in owner.Ally)
            {
                for (int i = 0; i < Rangex.Length; i++)
                {
                    if (ally.X == (Rangex[i] + owner.X) && ally.Y == (Rangey[i] + owner.Y))
                    {
                        ally.Recovery(150);
                        if (ally != owner)
                            ally.Turngaugeadd(50);
                    }
                }
            }
        }
    }

    public class Revive : Card
    {
        public Revive()
        {
            cardnum = 22;
            ownernum = 2;
            cost = 4;
            rangex = new int[1] { 1 };
            rangey = new int[1] { 0 };
        }

        public override void Use(Character owner)
        {
            base.Use(owner);
            foreach (Character ally in owner.Ally)
            {
                for (int i = 0; i < Rangex.Length; i++)
                {
                    if (ally.X == (Rangex[i] + owner.X) && ally.Y == (Rangey[i] + owner.Y))
                        ally.Revive();
                }
            }
        }
    }

    public class Heal3 : Card
    {
        public Heal3()
        {
            cardnum = 23;
            ownernum = 2;
            cost = 3;
            rangex = new int[6] { 1, 1, 1, 2, 2, 2 };
            rangey = new int[6] { 1, 0, -1, 1, 0, -1 };
        }

        public override void Use(Character owner)
        {
            base.Use(owner);
            foreach (Character ally in owner.Ally)
            {
                for (int i = 0; i < Rangex.Length; i++)
                {
                    if (ally.X == (Rangex[i] + owner.X) && ally.Y == (Rangey[i] + owner.Y))
                    {
                        Buff attack = new Buff("AM", 30, 2);
                        Buff critical = new Buff("C", 30, 2);
                        ally.Addbuff(attack);
                        ally.Addbuff(critical);
                    }
                }
            }
        }
    }

    public class Heal4 : Card
    {
        public Heal4()
        {
            cardnum = 24;
            ownernum = 2;
            cost = 2;
            rangex = new int[25] { -2, -2, -2, -2, -2, -1, -1, -1, -1, -1, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2 };
            rangey = new int[25] { 2, 1, 0, -1, -2, 2, 1, 0, -1, -2, 2, 1, 0, -1, -2, 2, 1, 0, -1, -2, 2, 1, 0, -1, -2 };
        }

        public override void Use(Character owner)
        {
            base.Use(owner);
            foreach (Character ally in owner.Ally)
            {
                for (int i = 0; i < Rangex.Length; i++)
                {
                    if (ally.X == (Rangex[i] + owner.X) && ally.Y == (Rangey[i] + owner.Y))
                        ally.Recovery(90);
                }
            }
        }
    }

    public class Heal5 : Card
    {
        public Heal5()
        {
            cardnum = 25;
            ownernum = 2;
            cost = 3;
            rangex = new int[7] { 1, 1, 2, 2, 2, 3, 3 };
            rangey = new int[7] { 1, -1, 1, 0, -1, 1, -1 };
        }

        public override void Use(Character owner)
        {
            base.Use(owner);
            foreach (Character enemy in owner.Enemy)
            {
                for (int i = 0; i < Rangex.Length; i++)
                {
                    if (enemy.X == (Rangex[i] + owner.X) && enemy.Y == (Rangey[i] + owner.Y))
                    {
                        Buff attack = new Buff("AM", -50, 2);
                        enemy.Addbuff(attack);
                    }
                }
            }
        }
    }

    public class Indurate : Card
    {
        public Indurate()
        {
            cardnum = 31;
            ownernum = 3;
            cost = 3;
            rangex = new int[21] { -2, -2, -2, -1, -1, -1, -1, -1, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 2, 2, 2 };
            rangey = new int[21] { 1, 0, -1, 2, 1, 0, -1, -2, 2, 1, 0, -1, -2, 2, 1, 0, -1, -2, 1, 0, -1 };
        }

        public override void Use(Character owner)
        {
            base.Use(owner);
            foreach (Character ally in owner.Ally)
            {
                for (int i = 0; i < Rangex.Length; i++)
                {
                    if (ally.X == (Rangex[i] + owner.X) && ally.Y == (Rangey[i] + owner.Y))
                    {
                        Buff armor = new Buff("D", 15, 1);
                        ally.Addbuff(armor);
                    }
                }
            }
        }
    }

    public class Tank2 : Card
    {
        public Tank2()
        {
            cardnum = 32;
            ownernum = 3;
            cost = 1;
            rangex = new int[3] { 1, 1, 1 };
            rangey = new int[3] { 1, 0, -1 };
        }

        public override void Use(Character owner)
        {
            base.Use(owner);
            foreach (Character enemy in owner.Enemy)
            {
                for (int i = 0; i < Rangex.Length; i++)
                {
                    if (enemy.X == (Rangex[i] + owner.X) && enemy.Y == (Rangey[i] + owner.Y))
                    {
                        Buff armor = new Buff("D", -50, 1);
                        enemy.Addbuff(armor);
                    }
                }
            }
        }
    }

    public class Tank3 : Attackcard
    {
        public Tank3()
        {
            cardnum = 33;
            ownernum = 3;
            cost = 4;
            coefficient = 3.5f;
            rangex = new int[1] { 1 };
            rangey = new int[1] { 0 };
        }
    }

    public class Smite : Attackcard
    {
        public Smite()
        {
            cardnum = 34;
            ownernum = 3;
            cost = 3;
            coefficient = 2;
            rangex = new int[8] { -1, -1, -1, 0, 0, 1, 1, 1 };
            rangey = new int[8] { 1, 0, -1, 1, -1, 1, 0, -1 };
        }

        public override void Use(Character owner)
        {
            base.Use(owner);
            foreach (Character enemy in owner.Enemy)
            {
                for (int i = 0; i < Rangex.Length; i++)
                {
                    if (enemy.X == (Rangex[i] + owner.X) && enemy.Y == (Rangey[i] + owner.Y))
                    {
                        Random rand = new Random();
                        if (rand.Next(0, 100) < 50)
                            enemy.Stun();
                    }
                }
            }
        }
    }

    public class Tank5 : Card
    {
        public Tank5()
        {
            Cardnum = 35;
            Ownernum = 3;
            Cost = 2;
            Rangex = new int[0];
            Rangey = new int[0];
        }

        public override void Use(Character owner)
        {
            base.Use(owner);
            Buff armor = new Buff("D", 50, 1);
            owner.Addbuff(armor);
        }
    }

    public class TG1 : Attackcard
    {
        public TG1()
        {
            cardnum = 41;
            ownernum = 4;
            cost = 1;
            coefficient = 1;
            rangex = new int[5] { -1, -1, -1, 0, 0 };
            rangey = new int[5] { 1, 0, -1, 1, -1 };
        }
    }

    public class TG2 : Card
    {
        public TG2()
        {
            Cardnum = 42;
            Ownernum = 4;
            Cost = 1;
            Rangex = new int[0];
            Rangey = new int[0];
        }

        public override void Use(Character owner)
        {
            base.Use(owner);
            Buff armor = new Buff("D", 50, 1);
            owner.Addbuff(armor);
        }
    }

    public class TG3 : Attackcard
    {
        public TG3()
        {
            cardnum = 43;
            ownernum = 4;
            cost = 4;
            coefficient = 1.5f;
            rangex = new int[9] { -3, -3, -3, -2, -2, -2, -1, -1, -1 };
            rangey = new int[9] { 1, 0, -1, 1, 0, -1, 1, 0, -1 };
        }

        public override void Use(Character owner)
        {
            base.Use(owner);
        }
    }

    public class TG4 : Card
    {
        public TG4()
        {
            Cardnum = 44;
            Ownernum = 4;
            Cost = 2;
            Rangex = new int[0];
            Rangey = new int[0];
        }
        public override void Use(Character owner)
        {
            base.Use(owner);
            owner.Recovery(100);
        }
    }

    public class TG5 : Card
    {
        public TG5()
        {
            Cardnum = 45;
            Ownernum = 4;
            Cost = 3;
            Rangex = new int[24] { -2, -2, -2, -2, -2, -1, -1, -1, -1, -1, 0, 0, 0, 0, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2 };
            Rangey = new int[24] { 2, 1, 0, -1, -2, 2, 1, 0, -1, -2, 2, 1, -1, -2, 2, 1, 0, -1, -2, 2, 1, 0, -1, -2 };
        }

        public override void Use(Character owner)
        {
            base.Use(owner);
            foreach (Character enemy in owner.Enemy)
            {
                for (int i = 0; i < Rangex.Length; i++)
                {
                    if (enemy.X == (Rangex[i] + owner.X) && enemy.Y == (Rangey[i] + owner.Y))
                    {
                        Buff armor = new Buff("D", -20, 2);
                        enemy.Addbuff(armor);
                    }
                }
            }
        }
    }

    public class TG6 : Attackcard
    {
        public TG6()
        {
            Cardnum = 46;
            Ownernum = 4;
            Cost = 2;
            coefficient = 1;
            Rangex = new int[10] { -3, -3, -2, -2, -2, -1, -1, -1, 0, 0 };
            Rangey = new int[10] { 0, -1, 1, 0, -1, 2, 1, 0, 2, 1 };
        }
    }

    public class Boss21 : Attackcard
    {
        public Boss21()
        {
            cardnum = 51;
            ownernum = 5;
            cost = 1;
            coefficient = 1;
            rangex = new int[2] { -2, -1 };
            rangey = new int[2] { 0, 0 };
        }
    }

    public class Boss22 : Attackcard
    {
        public Boss22()
        {
            cardnum = 52;
            ownernum = 5;
            cost = 4;
            coefficient = 1.2f;
            rangex = new int[6] { -3, -3, -2, -2, -1, -1 };
            rangey = new int[6] { 1, -1, 1, -1, 1, -1 };
        }
    }

    public class Boss23 : Card
    {
        public Boss23()
        {
            Cardnum = 53;
            Ownernum = 5;
            Cost = 4;
            Rangex = new int[0];
            Rangey = new int[0];
        }

        public override void Use(Character owner)
        {
            base.Use(owner);
            Buff attack = new Buff("AP", 20, 1);
            owner.Addbuff(attack);
        }
    }

    public class Boss24 : Attackcard
    {
        public Boss24()
        {
            cardnum = 54;
            ownernum = 5;
            cost = 3;
            coefficient = 0.5f;
            rangex = new int[14] { -2, -2, -2, -1, -1, -1, 0, 0, 1, 1, 1, 2, 2, 2 };
            rangey = new int[14] { 1, 0, -1, 1, 0, -1, 1, -1, 1, 0, -1, 1, 0, -1 };
        }

        public override void Use(Character owner)
        {
            base.Use(owner);
            Buff attack = new Buff("AP", 20, 1);
            owner.Addbuff(attack);
        }
    }

    public class Boss25 : Attackcard
    {
        public Boss25()
        {
            cardnum = 55;
            ownernum = 5;
            cost = 5;
            coefficient = 1.5f;
            rangex = new int[10] { -3, -3, -3, -3, -3, -2, -2, -2, -2, -2 };
            rangey = new int[10] { 2, 1, 0, -1, -2, 2, 1, 0, -1, -2 };
        }
    }
    
    public class Boss26 : Attackcard
    {
        public Boss26()
        {
            cardnum = 56;
            ownernum = 5;
            cost = 8;
            coefficient = 2;
            rangex = new int[12] { -4, -4, -4, -3, -3, -3, -2, -2, -2, -1, -1, -1 };
            rangey = new int[12] { 1, 0, -1, 1, 0, -1, 1, 0, -1, 1, 0, -1 };
        }
    }

    public class Boss31 : Attackcard
    {
        public Boss31()
        {
            cardnum = 61;
            ownernum = 6;
            cost = 2;
            coefficient = 1;
            rangex = new int[5] { -2, -2, -1, 0, 0 };
            rangey = new int[5] { 1, -1, 0, 1, -1 };
        }
    }

    public class Boss32 : Attackcard
    {
        public Boss32()
        {
            cardnum = 62;
            ownernum = 6;
            cost = 6;
            coefficient = 1.5f;
            rangex = new int[4] { -4, -3, -2, -1 };
            rangey = new int[4] { 0, 0, 0, 0 };
        }
    }

    public class Boss33 : Attackcard
    {
        public Boss33()
        {
            cardnum = 63;
            ownernum = 6;
            cost = 4;
            coefficient = 0.8f;
            rangex = new int[24] { -2, -2, -2, -2, -2, -1, -1, -1, -1, -1, 0, 0, 0, 0, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2 };
            rangey = new int[24] { 2, 1, 0, -1, -2, 2, 1, 0, -1, -2, 2, 1, -1, -2, 2, 1, 0, -1, -2, 2, 1, 0, -1, -2 };
        }
    }

    public class Boss34 : Card
    {
        public Boss34()
        {
            cardnum = 64;
            ownernum = 6;
            cost = 8;
            rangex = new int[0];
            rangey = new int[0];
        }

        public override void Use(Character owner)
        {
            base.Use(owner);
            Buff attack = new Buff("AP", 20, 2);
            Buff armor = new Buff("D", 50, 2);
            owner.Addbuff(attack);
            owner.Addbuff(armor);
        }
    }

    public class Boss35 : Card
    {
        public Boss35()
        {
            cardnum = 65;
            ownernum = 6;
            cost = 6;
            rangex = new int[9] { -4, -4, -4, -3, -3, -3, -2, -2, -2 };
            rangey = new int[9] { 1, 0, -1, 1, 0, -1, 1, 0, -1 };
        }

        public override void Use(Character owner)
        {
            base.Use(owner);
            foreach (Character enemy in owner.Enemy)
            {
                for (int i = 0; i < Rangex.Length; i++)
                {
                    if (enemy.X == (Rangex[i] + owner.X) && enemy.Y == (Rangey[i] + owner.Y))
                    {
                        Buff armor = new Buff("D", -15, 2);
                        Buff attck = new Buff("AP", -15, 2);
                        enemy.Addbuff(armor);
                        enemy.Addbuff(attck);
                    }
                }
            }
        }
    }

    public class Boss36 : Attackcard
    {
        public Boss36()
        {
            cardnum = 66;
            ownernum = 6;
            cost = 6;
            coefficient = 2;
            rangex = new int[8] { -1, -1, -1, 0, 0, 1, 1, 1 };
            rangey = new int[8] { 1, 0, -1, 1, -1, 1, 0, -1 };
        }
    }

    public class Boss37 : Attackcard
    {
        public Boss37()
        {
            cardnum = 67;
            ownernum = 6;
            cost = 8;
            coefficient = 10;
            rangex = new int[1] { -1 };
            rangey = new int[1] { 0 };
        }
    }
}