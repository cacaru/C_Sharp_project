using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace Server
{
    public partial class Form1 : Form {
        public static List<byte> TCPdata;
        public static int nturn = 0;

        TcpClient client;
        TcpListener listener;
        NetworkStream networkstream;
        Thread readthread;
        Thread multithread;
        Player player1;
        Player player2;
        multiplayer[] multi = null;
        bool bmulti = false;
        int statusnow = 1;
        bool canconnect = true;

        bool bConnect = false;
        bool gameon = false;

        bool multi0getd = false;
        bool multi1getd = false;

        public Form1() {
            InitializeComponent();
            TCPdata = new List<byte>();
        }

        public void Turn() {
            if (nturn != 0)
                return;
            while (true) {
                Character nowturn = null;
                int gauge = 99;
                foreach (Character character in player1.Characters) {
                    int chgauge = character.Turngaugeadd();
                    if (chgauge > gauge) {
                        nowturn = character;
                        gauge = chgauge;
                    }
                }
                foreach (Character character in player2.Characters) {
                    int chgauge = character.Turngaugeadd();
                    if (chgauge > gauge) {
                        nowturn = character;
                        gauge = chgauge;
                    }
                }
                if (nowturn != null) {
                    nowturn.Turnstart();
                    break;
                }
            }
        }

        private void btnServer_Click(object sender, EventArgs e) {
            if (!canconnect)
                return;
            canconnect = false;
            IPAddress ip = IPAddress.Parse(txtIP.Text);
            listener = new TcpListener(ip, 1111);
            Task.Run((Action)(Connect));
        }

        private void Connect() {
            listener.Start();
            Invoke(new MethodInvoker(delegate () {
                btnServer.Text = "서버 열림";
            }));
            try {
                client = listener.AcceptTcpClient();
                bConnect = true;
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
                Disconnect();
            }

            if (client.Connected) {
                try {
                    networkstream = client.GetStream();
                }
                catch {
                    listener.Stop();
                    return;
                }
                readthread = new Thread(new ThreadStart(Read));
                readthread.Start();
            }
        }

        private void Read() {
            int nRead = 0;
            int connecttest = 0; bool indicatereply = false;
            bool turnend = false;
            bool senddata = false;
            List<int> uselist;
            byte[] ins;
            while (bConnect) {
                indicatereply = false;
                turnend = false;
                senddata = false;
                uselist = new List<int>();
                ins = new byte[1] { 0 };
                while (ins[0] != 255) {
                    nRead = 0;
                    try {
                        nRead = networkstream.Read(ins, 0, 1);
                    }
                    catch (Exception ex) {
                        MessageBox.Show(ex.Message);
                        Disconnect();
                        return;
                    }

                    if (nRead == 0) {
                        if (connecttest++ >= 100) {
                            MessageBox.Show("연결이 불안정함");
                            Disconnect();
                            return;
                        }
                    }
                    else
                        connecttest = 0;

                    if (ins[0] == 0)
                        continue;
                    switch (ins[0]) {

                        case 1: {
                                uselist.Add(0);
                                indicatereply = true;
                                break;
                            }
                        case 2: {
                                uselist.Add(1);
                                indicatereply = true;
                                break;
                            }
                        case 3: {
                                uselist.Add(2);
                                indicatereply = true;
                                break;
                            }
                        case 4: {
                                uselist.Add(3);
                                indicatereply = true;
                                break;
                            }
                        case 5: {
                                uselist.Add(4);
                                indicatereply = true;
                                break;
                            }
                        case 6: {
                                indicatereply = false;
                                turnend = true;
                                senddata = true;
                                ins[0] = 255;
                                break;
                            }

                        case 7: {
                                indicatereply = false;
                                foreach (int be in ((Boss)(player2.Characters[0])).behaviorlist) {
                                    player2.Characters[0].Usecard(be);
                                }
                                indicatereply = false;
                                player2.Characters[0].Turnend();
                                Turn();
                                senddata = true;
                                ins[0] = 255;
                                break;
                            }
                        case 8: {
                                player2.Characters[0].Turnend();
                                Turn();
                                senddata = true;
                                ins[0] = 255;
                                break;
                            }
                        case 11: {
                                senddata = true;
                                statusnow = 1;
                                ins[0] = 255;
                                break;
                            }
                        case 12: {
                                senddata = true;
                                statusnow = 2;
                                ins[0] = 255;
                                break;
                            }
                        case 13: {
                                senddata = true;
                                statusnow = 3;
                                ins[0] = 255;
                                break;
                            }
                        case 14: {
                                senddata = true;
                                statusnow = 4;
                                ins[0] = 255;
                                break;
                            }
                        case 15: {
                                senddata = true;
                                statusnow = 4;
                                ins[0] = 255;
                                break;
                            }
                        case 16: {
                                senddata = true;
                                statusnow = 4;
                                ins[0] = 255;
                                break;
                            }
                        case 100: {
                                Invoke(new MethodInvoker(delegate () {
                                    btnServer.Text = "싱글 모드";
                                }));
                                senddata = true;
                                player1 = new Player(true, 0);
                                player2 = new Player(false, 0);
                                player1.Setenemy(player2);
                                player2.Setenemy(player1);
                                Turn();
                                ins[0] = 255;
                                break;
                            }
                        case 200: {
                                Invoke(new MethodInvoker(delegate () {
                                    btnServer.Text = "멀티 모드";
                                }));
                                if (!bmulti) {
                                    bmulti = true;
                                    multi = new multiplayer[2];
                                    multi[0] = new multiplayer(client, networkstream);
                                    client = null;
                                    networkstream = null;
                                    Task.Run((Action)(Connect));
                                    bConnect = false;
                                }
                                else {
                                    multi[1] = new multiplayer(client, networkstream);
                                    player1 = new Player(true, 1);
                                    player2 = new Player(true, 2);
                                    player1.Setenemy(player2);
                                    player2.Setenemy(player1);
                                    multi[0].myp = player1;
                                    multi[0].enemyp = player2;
                                    multi[1].myp = player2;
                                    multi[1].enemyp = player1;
                                    Turn();
                                    multi[0].readthread.Start();
                                    multi[1].readthread.Start();
                                    gameon = true;
                                    bConnect = false;
                                    multithread = new Thread(new ThreadStart(Multidataprocess));
                                    multithread.Start();
                                    readthread.Abort();
                                }
                                ins[0] = 255;
                                break;
                            }
                    }
                }

                if (turnend) {
                    if (nturn == 1) {
                        foreach (int be in uselist) {
                            player1.Characters[0].Usecard(be);
                        }
                        player1.Characters[0].Turnend();
                    }
                    else if (nturn == 2) {
                        foreach (int be in uselist) {
                            player1.Characters[1].Usecard(be);
                        }
                        player1.Characters[1].Turnend();
                    }
                    else if (nturn == 3) {
                        foreach (int be in uselist) {
                            player1.Characters[2].Usecard(be);
                        }
                        player1.Characters[2].Turnend();
                    }
                    else {
                        foreach (int be in uselist) {
                            player2.Characters[0].Usecard(be);
                        }
                        player2.Characters[0].Turnend();
                    }
                    Turn();
                }

                byte[] senddatalist = null;
                if (indicatereply) {
                    senddatalist = new byte[1];
                    if (nturn == 1) {
                        if (player1.Characters[0].InvestigateCanUse(uselist))
                            senddatalist[0] = 1;
                        else
                            senddatalist[0] = 0;
                    }
                    else if (nturn == 2) {
                        if (player1.Characters[1].InvestigateCanUse(uselist))
                            senddatalist[0] = 1;
                        else
                            senddatalist[0] = 0;
                    }
                    else if (nturn == 3) {
                        if (player1.Characters[2].InvestigateCanUse(uselist))
                            senddatalist[0] = 1;
                        else
                            senddatalist[0] = 0;
                    }
                    else {
                        if (player2.Characters[0].InvestigateCanUse(uselist))
                            senddatalist[0] = 1;
                        else
                            senddatalist[0] = 0;
                    }
                    networkstream.Write(senddatalist, 0, 1);
                }

                Invoke(new MethodInvoker(delegate () { }));
                if (senddata) {
                    TCPdata.Add(255);
                    DataSet();
                    TCPdata.Add(255);
                    senddatalist = new byte[TCPdata.Count];
                    TCPdata.CopyTo(senddatalist);
                    networkstream.Write(senddatalist, 0, TCPdata.Count);
                    TCPdata.Clear();
                }
            }
        }

        private void DataSet() {
            List<byte> numlist = new List<byte>(new byte[4] { 1, 2, 3, (byte)player2.Characters[0].Characternum });
            for (int i = 0; i < 4; i++) {
                byte maxi = 0;
                int max;
                max = -1;
                foreach (byte num in numlist) {
                    if (num == 1) {
                        if (player1.Characters[0].Turngauge > max) {
                            maxi = num;
                            max = player1.Characters[0].Turngauge;
                        }
                    }
                    else if (num == 2) {
                        if (player1.Characters[1].Turngauge > max) {
                            maxi = num;
                            max = player1.Characters[1].Turngauge;
                        }
                    }
                    else if (num == 3) {
                        if (player1.Characters[2].Turngauge > max) {
                            maxi = num;
                            max = player1.Characters[2].Turngauge;
                        }
                    }
                    else {
                        if (player2.Characters[0].Turngauge > max) {
                            maxi = num;
                            max = player2.Characters[0].Turngauge;
                        }
                    }
                }
                TCPdata.Add(maxi);
                numlist.Remove(maxi);
            }

            TCPdata.Add((byte)nturn);
            switch (nturn) {
                case 1: {
                        TCPdata.Add((byte)player1.Characters[0].Cost);
                        break;
                    }
                case 2: {
                        TCPdata.Add((byte)player1.Characters[1].Cost);
                        break;
                    }
                case 3: {
                        TCPdata.Add((byte)player1.Characters[2].Cost);
                        break;
                    }
                default: {
                        TCPdata.Add((byte)player2.Characters[0].Cost);
                        break;
                    }
            }

            Character stn = null;
            switch (statusnow) {
                case 1: {
                        stn = player1.Characters[0];
                        break;
                    }
                case 2: {
                        stn = player1.Characters[1];
                        break;
                    }
                case 3: {
                        stn = player1.Characters[2];
                        break;
                    }
                default: {
                        stn = player2.Characters[0];
                        break;
                    }
            }
            TCPdata.Add((byte)(stn.Hp / 100));
            TCPdata.Add((byte)(stn.Hp % 100));
            TCPdata.Add((byte)(stn.Maxhp / 100));
            TCPdata.Add((byte)(stn.Maxhp % 100));
            TCPdata.Add((byte)(stn.Attack));
            TCPdata.Add((byte)(stn.Armor));
            TCPdata.Add((byte)(stn.Speed));
            TCPdata.Add((byte)(stn.Criticalrate));
            TCPdata.Add((byte)((stn.Criticaldmg * 100) / 100));
            TCPdata.Add((byte)((stn.Criticaldmg * 100) % 100));

            Character nt;

            switch (nturn) {
                case 1: {
                        nt = player1.Characters[0];
                        if (nt.Hand[0] == null)
                            TCPdata.Add(0);
                        else
                            TCPdata.Add((byte)(nt.Hand[0].Cardnum));

                        if (nt.Hand[1] == null)
                            TCPdata.Add(0);
                        else
                            TCPdata.Add((byte)(nt.Hand[1].Cardnum));

                        if (nt.Hand[2] == null)
                            TCPdata.Add(0);
                        else
                            TCPdata.Add((byte)(nt.Hand[2].Cardnum));

                        if (nt.Hand[3] == null)
                            TCPdata.Add(0);
                        else
                            TCPdata.Add((byte)(nt.Hand[3].Cardnum));

                        if (nt.Hand[4] == null)
                            TCPdata.Add(0);
                        else
                            TCPdata.Add((byte)(nt.Hand[4].Cardnum));
                        break;
                    }
                case 2: {
                        nt = player1.Characters[1];
                        if (nt.Hand[0] == null)
                            TCPdata.Add(0);
                        else
                            TCPdata.Add((byte)(nt.Hand[0].Cardnum));

                        if (nt.Hand[1] == null)
                            TCPdata.Add(0);
                        else
                            TCPdata.Add((byte)(nt.Hand[1].Cardnum));

                        if (nt.Hand[2] == null)
                            TCPdata.Add(0);
                        else
                            TCPdata.Add((byte)(nt.Hand[2].Cardnum));

                        if (nt.Hand[3] == null)
                            TCPdata.Add(0);
                        else
                            TCPdata.Add((byte)(nt.Hand[3].Cardnum));

                        if (nt.Hand[4] == null)
                            TCPdata.Add(0);
                        else
                            TCPdata.Add((byte)(nt.Hand[4].Cardnum));
                        break;
                    }
                case 3: {
                        nt = player1.Characters[2];
                        if (nt.Hand[0] == null)
                            TCPdata.Add(0);
                        else
                            TCPdata.Add((byte)(nt.Hand[0].Cardnum));

                        if (nt.Hand[1] == null)
                            TCPdata.Add(0);
                        else
                            TCPdata.Add((byte)(nt.Hand[1].Cardnum));

                        if (nt.Hand[2] == null)
                            TCPdata.Add(0);
                        else
                            TCPdata.Add((byte)(nt.Hand[2].Cardnum));

                        if (nt.Hand[3] == null)
                            TCPdata.Add(0);
                        else
                            TCPdata.Add((byte)(nt.Hand[3].Cardnum));

                        if (nt.Hand[4] == null)
                            TCPdata.Add(0);
                        else
                            TCPdata.Add((byte)(nt.Hand[4].Cardnum));
                        break;
                    }
                default: {
                        nt = player2.Characters[0];
                        TCPdata.Add(0);
                        TCPdata.Add(0);
                        TCPdata.Add(0);
                        TCPdata.Add(0);
                        TCPdata.Add(0);
                        break;
                    }
            }

            TCPdata.Add((byte)(player2.Characters[0].Hp / 100));
            TCPdata.Add((byte)(player2.Characters[0].Hp % 100));

            byte alivelocation = 0;
            if (player1.Characters[0].Alive)
                alivelocation += 100;
            else
                alivelocation += 200;
            alivelocation += (byte)(player1.Characters[0].Y * 8);
            alivelocation += (byte)player1.Characters[0].X;
            TCPdata.Add(alivelocation);

            alivelocation = 0;
            if (player1.Characters[1].Alive)
                alivelocation += 100;
            else
                alivelocation += 200;
            alivelocation += (byte)(player1.Characters[1].Y * 8);
            alivelocation += (byte)player1.Characters[1].X;
            TCPdata.Add(alivelocation);

            alivelocation = 0;
            if (player1.Characters[2].Alive)
                alivelocation += 100;
            else
                alivelocation += 200;
            alivelocation += (byte)(player1.Characters[2].Y * 8);
            alivelocation += (byte)player1.Characters[2].X;
            TCPdata.Add(alivelocation);

            alivelocation = 0;
            if (player2.Characters[0].Alive)
                alivelocation += 100;
            else
                alivelocation += 200;
            alivelocation += (byte)(player2.Characters[0].Y * 8);
            alivelocation += (byte)player2.Characters[0].X;
            TCPdata.Add(alivelocation);
        }

        private void Disconnect() {
            bConnect = false;
            gameon = false;
            if (!bConnect && !gameon)
                return;
            if (multi != null) {
                multi[0].Disconnect();
                multi[1].Disconnect();
            }
            else {
                client.Close();
                networkstream.Close();
            }
            listener.Stop();
            if (readthread.IsAlive)
                readthread.Abort();
            if (multithread != null) {
                if (multithread.IsAlive)
                    multithread.Abort();
            }
        }

        private void Multidataprocess() {
            while (gameon) {
                if (multi[0].Havedata()) {
                    int ins = multi[0].Getdata();
                    if (ins == 6) {
                        Turn();
                        Setmotion(0, true);
                    }
                    else {
                        Setmotion(0, false);
                    }
                    multi[0].Send();
                }
                if (multi[1].Havedata()) {
                    int ins = multi[1].Getdata();
                    if (ins == 6) {
                        Turn();
                        Setmotion(1, true);
                    }
                    else {
                        Setmotion(1, false);
                    }
                    multi[1].Send();
                }
            }
        }

        private void Setmotion(int i, bool levordown) {
            if (TCPdata.Count == 0)
                return;
            lock (TCPdata) {
                foreach (byte be in TCPdata) {
                    if (i == 0) {
                        if (multi0getd)
                            break;
                        if (be < 70) {
                            if (levordown)
                                multi[0].setdata.Add((byte)(be - 6));
                            else
                                multi[0].setdata.Add((byte)(be + 35 - 6));

                        }
                        else {
                            if (be % 10 == 1 || be % 10 == 2 || be % 10 == 3)
                                multi[0].setdata.Add(be);
                            else {
                                if (be / 10 == 7 || be / 10 == 11)
                                    multi[0].setdata.Add((byte)(be + 10));
                                else if (be / 10 == 8 || be / 10 == 12)
                                    multi[0].setdata.Add((byte)(be - 10));
                                else
                                    multi[0].setdata.Add((byte)(be));
                            }
                        }
                    }
                    else {
                        if (multi1getd)
                            break;
                        if (be < 70) {
                            if (levordown)
                                multi[1].setdata.Add((byte)(be - 6));
                            else
                                multi[1].setdata.Add((byte)(be + 35 - 6));
                        }
                        else {
                            if (be % 10 == 1 || be % 10 == 2 || be % 10 == 3) {
                                multi[1].setdata.Add((byte)(be + 3));
                            }
                            else {
                                if (be / 10 == 7 || be / 10 == 11)
                                    multi[1].setdata.Add((byte)(be - 3 + 10));
                                else if (be / 10 == 8 || be / 10 == 12)
                                    multi[1].setdata.Add((byte)(be - 3 - 10));
                                else
                                    multi[1].setdata.Add((byte)(be - 3));
                            }
                        }
                    }
                }
                if (i == 0)
                    multi0getd = true;
                else
                    multi1getd = true;

                if (multi0getd && multi1getd) {
                    multi0getd = false;
                    multi1getd = false;
                    TCPdata.Clear();
                }
            }
        }


        private void Form1_FormClosed(object sender, FormClosedEventArgs e) {
            Disconnect();
        }

    }
}
