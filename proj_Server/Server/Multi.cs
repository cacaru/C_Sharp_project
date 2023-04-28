using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace Server
{
    public class multiplayer
    {
        public List<byte> setdata;
        public Player myp;
        public Player enemyp;


        private NetworkStream networkstream;
        private TcpClient tcpclient;
        public Thread readthread;
        private bool bclienton = false;
        private Queue<int> databuffer;
        private int statusnow = 1;

        public multiplayer(TcpClient tcpclient, NetworkStream networkstream)
        {
            this.tcpclient = tcpclient;
            this.networkstream = networkstream;
            setdata = new List<byte>();
            bclienton = true;
            databuffer = new Queue<int>();
            readthread = new Thread(new ThreadStart(Read));
        }

        private void Read()
        {
            int nRead = 0;
            int connecttest = 0;
            bool indicatereply = false;
            bool turnend = false;
            bool senddata = false;
            List<int> uselist;
            byte[] ins;
            while (bclienton)
            {
                indicatereply = false;
                turnend = false;
                senddata = false;
                uselist = new List<int>();
                ins = new byte[1] { 0 };
                while (ins[0] != 255)
                {
                    nRead = 0;
                    try
                    {
                        nRead = networkstream.Read(ins, 0, 1);
                    }
                    catch (Exception ex)
                    {
                        Disconnect();
                        return;
                    }

                    if (nRead == 0)
                    {
                        if (connecttest++ >= 100)
                        {
                            Disconnect();
                            return;
                        }
                    }
                    else
                        connecttest = 0;

                    if (ins[0] == 0)
                        continue;
                    switch (ins[0])
                    {

                        case 1:
                            {
                                uselist.Add(0);
                                indicatereply = true;
                                break;
                            }
                        case 2:
                            {
                                uselist.Add(1);
                                indicatereply = true;
                                break;
                            }
                        case 3:
                            {
                                uselist.Add(2);
                                indicatereply = true;
                                break;
                            }
                        case 4:
                            {
                                uselist.Add(3);
                                indicatereply = true;
                                break;
                            }
                        case 5:
                            {
                                uselist.Add(4);
                                indicatereply = true;
                                break;
                            }
                        case 6:
                            {
                                indicatereply = false;
                                turnend = true;
                                ins[0] = 255;
                                break;
                            }
                        case 11:
                            {
                                senddata = true;
                                statusnow = 1;
                                ins[0] = 255;
                                break;
                            }
                        case 12:
                            {
                                senddata = true;
                                statusnow = 2;
                                ins[0] = 255;
                                break;
                            }
                        case 13:
                            {
                                senddata = true;
                                statusnow = 3;
                                ins[0] = 255;
                                break;
                            }
                        case 14:
                            {
                                senddata = true;
                                statusnow = 4;
                                ins[0] = 255;
                                break;
                            }
                        case 15:
                            {
                                senddata = true;
                                statusnow = 5;
                                ins[0] = 255;
                                break;
                            }
                        case 16:
                            {
                                senddata = true;
                                statusnow = 6;
                                ins[0] = 255;
                                break;
                            }
                        case 111:
                            {
                                senddata = true;
                                ins[0] = 255;
                                break;
                            }
                    }
                }

                if (turnend)
                {
                    if (Form1.nturn == 1 || Form1.nturn == 4)
                    {
                        foreach (int be in uselist)
                        {
                            myp.Characters[0].Usecard(be);
                        }
                        myp.Characters[0].Turnend();
                    }
                    else if (Form1.nturn == 2 || Form1.nturn == 5)
                    {
                        foreach (int be in uselist)
                        {
                            myp.Characters[1].Usecard(be);
                        }
                        myp.Characters[1].Turnend();
                    }
                    else if (Form1.nturn == 3 || Form1.nturn == 6)
                    {
                        foreach (int be in uselist)
                        {
                            myp.Characters[2].Usecard(be);
                        }
                        myp.Characters[2].Turnend();
                    }
                    databuffer.Enqueue(6);
                }

                byte[] senddatalist = null;
                if (indicatereply)
                {
                    senddatalist = new byte[1];
                    if (Form1.nturn == 1 || Form1.nturn == 4)
                    {
                        if (myp.Characters[0].InvestigateCanUse(uselist))
                            senddatalist[0] = 1;
                        else
                            senddatalist[0] = 0;
                    }
                    else if (Form1.nturn == 2 || Form1.nturn == 5)
                    {
                        if (myp.Characters[1].InvestigateCanUse(uselist))
                            senddatalist[0] = 1;
                        else
                            senddatalist[0] = 0;
                    }
                    else if (Form1.nturn == 3 || Form1.nturn == 6)
                    {
                        if (myp.Characters[2].InvestigateCanUse(uselist))
                            senddatalist[0] = 1;
                        else
                            senddatalist[0] = 0;
                    }
                    networkstream.Write(senddatalist, 0, 1);
                    networkstream.Flush();
                }

                if (senddata)
                    databuffer.Enqueue(100);
            }
        }

        private void DataSet()
        {
            List<byte> numlist = new List<byte>(new byte[6] { 1, 2, 3, 4, 5, 6 });
            for (int i = 0; i < 4; i++)
            {
                byte maxi = 0;
                int max;
                max = -1;
                foreach (byte num in numlist)
                {
                    if (num == 1)
                    {
                        if (myp.Characters[0].Turngauge > max)
                        {
                            maxi = num;
                            max = myp.Characters[0].Turngauge;
                        }
                    }
                    else if (num == 2)
                    {
                        if (myp.Characters[1].Turngauge > max)
                        {
                            maxi = num;
                            max = myp.Characters[1].Turngauge;
                        }
                    }
                    else if (num == 3)
                    {
                        if (myp.Characters[2].Turngauge > max)
                        {
                            maxi = num;
                            max = myp.Characters[2].Turngauge;
                        }
                    }
                    else if (num == 4)
                    {
                        if (enemyp.Characters[0].Turngauge > max)
                        {
                            maxi = num;
                            max = enemyp.Characters[0].Turngauge;
                        }
                    }
                    else if (num == 5)
                    {
                        if (enemyp.Characters[1].Turngauge > max)
                        {
                            maxi = num;
                            max = enemyp.Characters[0].Turngauge;
                        }
                    }
                    else
                    {
                        if (enemyp.Characters[2].Turngauge > max)
                        {
                            maxi = num;
                            max = enemyp.Characters[0].Turngauge;
                        }
                    }
                }
                setdata.Add(maxi);
                numlist.Remove(maxi);
            }

            Character stn = null;
            switch (statusnow)
            {
                case 1:
                    {
                        stn = myp.Characters[0];
                        break;
                    }
                case 2:
                    {
                        stn = myp.Characters[1];
                        break;
                    }
                case 3:
                    {
                        stn = myp.Characters[2];
                        break;
                    }
                case 4:
                    {
                        stn = enemyp.Characters[0];
                        break;
                    }
                case 5:
                    {
                        stn = enemyp.Characters[1];
                        break;
                    }
                case 6:
                    {
                        stn = enemyp.Characters[2];
                        break;
                    }
            }

            setdata.Add((byte)(stn.Hp / 100));
            setdata.Add((byte)(stn.Hp % 100));
            setdata.Add((byte)(stn.Maxhp / 100));
            setdata.Add((byte)(stn.Maxhp % 100));
            setdata.Add((byte)(stn.Attack));
            setdata.Add((byte)(stn.Armor));
            setdata.Add((byte)(stn.Speed));
            setdata.Add((byte)(stn.Criticalrate));
            setdata.Add((byte)((stn.Criticaldmg * 100) / 100));
            setdata.Add((byte)((stn.Criticaldmg * 100) % 100));

            Character nt;

            switch (Form1.nturn)
            {
                case 1:
                    {
                        nt = myp.Characters[0];
                        if (nt.Hand[0] == null)
                            setdata.Add(0);
                        else
                            setdata.Add((byte)(nt.Hand[0].Cardnum));

                        if (nt.Hand[1] == null)
                            setdata.Add(0);
                        else
                            setdata.Add((byte)(nt.Hand[1].Cardnum));

                        if (nt.Hand[2] == null)
                            setdata.Add(0);
                        else
                            setdata.Add((byte)(nt.Hand[2].Cardnum));

                        if (nt.Hand[3] == null)
                            setdata.Add(0);
                        else
                            setdata.Add((byte)(nt.Hand[3].Cardnum));

                        if (nt.Hand[4] == null)
                            setdata.Add(0);
                        else
                            setdata.Add((byte)(nt.Hand[4].Cardnum));
                        break;
                    }
                case 2:
                    {
                        nt = myp.Characters[1];
                        if (nt.Hand[0] == null)
                            setdata.Add(0);
                        else
                            setdata.Add((byte)(nt.Hand[0].Cardnum));

                        if (nt.Hand[1] == null)
                            setdata.Add(0);
                        else
                            setdata.Add((byte)(nt.Hand[1].Cardnum));

                        if (nt.Hand[2] == null)
                            setdata.Add(0);
                        else
                            setdata.Add((byte)(nt.Hand[2].Cardnum));

                        if (nt.Hand[3] == null)
                            setdata.Add(0);
                        else
                            setdata.Add((byte)(nt.Hand[3].Cardnum));

                        if (nt.Hand[4] == null)
                            setdata.Add(0);
                        else
                            setdata.Add((byte)(nt.Hand[4].Cardnum));
                        break;
                    }
                case 3:
                    {
                        nt = myp.Characters[2];
                        if (nt.Hand[0] == null)
                            setdata.Add(0);
                        else
                            setdata.Add((byte)(nt.Hand[0].Cardnum));

                        if (nt.Hand[1] == null)
                            setdata.Add(0);
                        else
                            setdata.Add((byte)(nt.Hand[1].Cardnum));

                        if (nt.Hand[2] == null)
                            setdata.Add(0);
                        else
                            setdata.Add((byte)(nt.Hand[2].Cardnum));

                        if (nt.Hand[3] == null)
                            setdata.Add(0);
                        else
                            setdata.Add((byte)(nt.Hand[3].Cardnum));

                        if (nt.Hand[4] == null)
                            setdata.Add(0);
                        else
                            setdata.Add((byte)(nt.Hand[4].Cardnum));
                        break;
                    }
                case 4:
                    {
                        nt = myp.Characters[0];
                        if (nt.Hand[0] == null)
                            setdata.Add(0);
                        else
                            setdata.Add((byte)(nt.Hand[0].Cardnum));

                        if (nt.Hand[1] == null)
                            setdata.Add(0);
                        else
                            setdata.Add((byte)(nt.Hand[1].Cardnum));

                        if (nt.Hand[2] == null)
                            setdata.Add(0);
                        else
                            setdata.Add((byte)(nt.Hand[2].Cardnum));

                        if (nt.Hand[3] == null)
                            setdata.Add(0);
                        else
                            setdata.Add((byte)(nt.Hand[3].Cardnum));

                        if (nt.Hand[4] == null)
                            setdata.Add(0);
                        else
                            setdata.Add((byte)(nt.Hand[4].Cardnum));
                        break;
                    }
                case 5:
                    {
                        nt = myp.Characters[1];
                        if (nt.Hand[0] == null)
                            setdata.Add(0);
                        else
                            setdata.Add((byte)(nt.Hand[0].Cardnum));

                        if (nt.Hand[1] == null)
                            setdata.Add(0);
                        else
                            setdata.Add((byte)(nt.Hand[1].Cardnum));

                        if (nt.Hand[2] == null)
                            setdata.Add(0);
                        else
                            setdata.Add((byte)(nt.Hand[2].Cardnum));

                        if (nt.Hand[3] == null)
                            setdata.Add(0);
                        else
                            setdata.Add((byte)(nt.Hand[3].Cardnum));

                        if (nt.Hand[4] == null)
                            setdata.Add(0);
                        else
                            setdata.Add((byte)(nt.Hand[4].Cardnum));
                        break;
                    }
                case 6:
                    {
                        nt = myp.Characters[2];
                        if (nt.Hand[0] == null)
                            setdata.Add(0);
                        else
                            setdata.Add((byte)(nt.Hand[0].Cardnum));

                        if (nt.Hand[1] == null)
                            setdata.Add(0);
                        else
                            setdata.Add((byte)(nt.Hand[1].Cardnum));

                        if (nt.Hand[2] == null)
                            setdata.Add(0);
                        else
                            setdata.Add((byte)(nt.Hand[2].Cardnum));

                        if (nt.Hand[3] == null)
                            setdata.Add(0);
                        else
                            setdata.Add((byte)(nt.Hand[3].Cardnum));

                        if (nt.Hand[4] == null)
                            setdata.Add(0);
                        else
                            setdata.Add((byte)(nt.Hand[4].Cardnum));
                        break;
                    }
                default:
                    {
                        setdata.Add(0);
                        setdata.Add(0);
                        setdata.Add(0);
                        setdata.Add(0);
                        setdata.Add(0);
                        break;
                    }
            }
            
            bool reverse;
            if (myp.Characters[0].Characternum == 1)
            {
                reverse = false;
            }
            else
            {
                reverse = true;
            }

            byte alivelocation = 0;
            if (myp.Characters[0].Alive)
                alivelocation += 100;
            else
                alivelocation += 200;
            if (reverse)
            {
                alivelocation += (byte)(myp.Characters[0].Y * 8);
                alivelocation += (byte)(7 - myp.Characters[0].X);
            }
            else
            {
                alivelocation += (byte)(myp.Characters[0].Y * 8);
                alivelocation += (byte)myp.Characters[0].X;
            }
            setdata.Add(alivelocation);

            alivelocation = 0;
            if (myp.Characters[1].Alive)
                alivelocation += 100;
            else
                alivelocation += 200;
            if (reverse)
            {
                alivelocation += (byte)(myp.Characters[1].Y * 8);
                alivelocation += (byte)(7 - myp.Characters[1].X);
            }
            else
            {
                alivelocation += (byte)(myp.Characters[1].Y * 8);
                alivelocation += (byte)myp.Characters[1].X;
            }
            setdata.Add(alivelocation);

            alivelocation = 0;
            if (myp.Characters[2].Alive)
                alivelocation += 100;
            else
                alivelocation += 200;
            if (reverse)
            {
                alivelocation += (byte)(myp.Characters[2].Y * 8);
                alivelocation += (byte)(7 - myp.Characters[2].X);
            }
            else
            {
                alivelocation += (byte)(myp.Characters[2].Y * 8);
                alivelocation += (byte)myp.Characters[2].X;
            }
            setdata.Add(alivelocation);

            alivelocation = 0;
            if (enemyp.Characters[0].Alive)
                alivelocation += 100;
            else
                alivelocation += 200;
            if (reverse)
            {
                alivelocation += (byte)(enemyp.Characters[0].Y * 8);
                alivelocation += (byte)(7 - enemyp.Characters[0].X);
            }
            else
            {
                alivelocation += (byte)(enemyp.Characters[0].Y * 8);
                alivelocation += (byte)enemyp.Characters[0].X;
            }
            setdata.Add(alivelocation);

            alivelocation = 0;
            if (enemyp.Characters[1].Alive)
                alivelocation += 100;
            else
                alivelocation += 200;
            if (reverse)
            {
                alivelocation += (byte)(enemyp.Characters[1].Y * 8);
                alivelocation += (byte)(7 - enemyp.Characters[1].X);
            }
            else
            {
                alivelocation += (byte)(enemyp.Characters[1].Y * 8);
                alivelocation += (byte)enemyp.Characters[1].X;
            }
            setdata.Add(alivelocation);

            alivelocation = 0;
            if (enemyp.Characters[2].Alive)
                alivelocation += 100;
            else
                alivelocation += 200;
            if (reverse)
            {
                alivelocation += (byte)(enemyp.Characters[2].Y * 8);
                alivelocation += (byte)(7 - enemyp.Characters[2].X);
            }
            else
            {
                alivelocation += (byte)(enemyp.Characters[2].Y * 8);
                alivelocation += (byte)enemyp.Characters[2].X;
            }
            setdata.Add(alivelocation);
        }

        public void Send()
        {
            setdata.Add(255);
            DataSet();
            byte[] senddatalist = new byte[setdata.Count];
            setdata.CopyTo(senddatalist);
            networkstream.Write(senddatalist, 0, setdata.Count);
            networkstream.Flush();
            setdata.Clear();
        }

        public bool Isconnect()
        {
            return bclienton;
        }

        public bool Havedata()
        {
            if (databuffer.Count == 0)
                return false;
            else
                return true;
        }

        public int Getdata()
        {
            return databuffer.Dequeue();
        }

        public void Disconnect()
        {
            myp.Characters[0].Hit(9999);
            myp.Characters[1].Hit(9999);
            myp.Characters[2].Hit(9999);
            if (!bclienton)
                return;
            bclienton = false;
            networkstream.Close();
            tcpclient.Close();
            readthread.Abort();
        }
    }
}
