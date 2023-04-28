using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using Server;

namespace Server
{
	public partial class Form1 : Form
	{
        public static List<int> TCPdata;
		Player player;
		Player boss;
		
		public Form1()
		{
			InitializeComponent();
            TCPdata = new List<int>();
			player = new Player(true, 1);
			boss = new Player(true, 2);
			player.Setenemy(boss);
			boss.Setenemy(player);
            boss.Characters[0].Hit(9999999);
			Turn();
		}

		public void Turn()
		{
			while (true)
			{
				Character nowturn = null;
				int gauge = 99;
				foreach (Character character in player.Characters)
				{
					int chgauge = character.Turngaugeadd();
					if (chgauge > gauge)
					{
						nowturn = character;
						gauge = chgauge;
					}
				}
				foreach (Character character in boss.Characters)
				{
					int chgauge = character.Turngaugeadd();
					if (chgauge > gauge)
					{
						nowturn = character;
						gauge = chgauge;
					}
				}
				if (nowturn != null)
				{
					nowturn.Turnstart();
					break;
				}
			}
			Set();
		}

		private void Set()
		{
			field.Text = "";
			bool write = false;
			for (int y = 4; y >= 0; y--)
			{
				for (int x = 0; x < 8; x++)
				{
					foreach (Character ch in player.Characters)
					{
						if(ch.X==x&&ch.Y==y)
						{
							field.AppendText(ch.Characternum.ToString());
							write = true;
						}
					}
					foreach (Character ch in boss.Characters)
					{
						if (ch.X == x && ch.Y == y)
						{
							field.AppendText(ch.Characternum.ToString());
							write = true;
						}
					}
					if (!write)
						field.AppendText("0");
					write = false;
				}
				field.AppendText("\r\n");
			}
			foreach (Character ch in player.Characters)
			{
				if(ch.Myturn)
				{
					if (ch.Hand[0] != null)
						hand0.Text = ch.Hand[0].GetType().ToString();
					else
						hand0.Text = "0";
					if (ch.Hand[1] != null)
						hand1.Text = ch.Hand[1].GetType().ToString();
					else
						hand1.Text = "0";
					if (ch.Hand[2] != null)
						hand2.Text = ch.Hand[2].GetType().ToString();
					else
						hand2.Text = "0";
					if (ch.Hand[3] != null)
						hand3.Text = ch.Hand[3].GetType().ToString();
					else
						hand3.Text = "0";
					if (ch.Hand[4] != null)
						hand4.Text = ch.Hand[4].GetType().ToString();
					else
						hand4.Text = "0";
				}
			}
			foreach (Character ch in boss.Characters)
			{
				if (ch.Myturn)
				{
					if (ch.Hand[0] != null)
						hand0.Text = ch.Hand[0].GetType().ToString();
					else
						hand0.Text = "0";
					if (ch.Hand[1] != null)
						hand1.Text = ch.Hand[1].GetType().ToString();
					else
						hand1.Text = "0";
					if (ch.Hand[2] != null)
						hand2.Text = ch.Hand[2].GetType().ToString();
					else
						hand2.Text = "0";
					if (ch.Hand[3] != null)
						hand3.Text = ch.Hand[3].GetType().ToString();
					else
						hand3.Text = "0";
					if (ch.Hand[4] != null)
						hand4.Text = ch.Hand[4].GetType().ToString();
					else
						hand4.Text = "0";
				}
			}
			stat1.Text = "HP:" + player.Characters[0].Hp.ToString() + "/" + player.Characters[0].Maxhp.ToString() + "\r\nattack:" + player.Characters[0].Attack.ToString() + "\r\narmor:" + player.Characters[0].Armor.ToString() + "\r\ncrirate" + player.Characters[0].Criticalrate.ToString() + "\r\ncridmg:" + player.Characters[0].Criticaldmg.ToString() + "\r\nturn:" + player.Characters[0].Turngauge.ToString();
			stat2.Text = "HP:" + player.Characters[1].Hp.ToString() + "/" + player.Characters[1].Maxhp.ToString() + "\r\nattack:" + player.Characters[1].Attack.ToString() + "\r\narmor:" + player.Characters[1].Armor.ToString() + "\r\ncrirate" + player.Characters[1].Criticalrate.ToString() + "\r\ncridmg:" + player.Characters[1].Criticaldmg.ToString() + "\r\nturn:" + player.Characters[1].Turngauge.ToString();
			stat3.Text = "HP:" + player.Characters[2].Hp.ToString() + "/" + player.Characters[2].Maxhp.ToString() + "\r\nattack:" + player.Characters[2].Attack.ToString() + "\r\narmor:" + player.Characters[2].Armor.ToString() + "\r\ncrirate" + player.Characters[2].Criticalrate.ToString() + "\r\ncridmg:" + player.Characters[2].Criticaldmg.ToString() + "\r\nturn:" + player.Characters[2].Turngauge.ToString();
			stat4.Text = "HP:" + boss.Characters[0].Hp.ToString() + "/" + boss.Characters[0].Maxhp.ToString() + "\r\nattack:" + boss.Characters[0].Attack.ToString() + "\r\narmor:" + boss.Characters[0].Armor.ToString() + "\r\ncrirate" + boss.Characters[0].Criticalrate.ToString() + "\r\ncridmg:" + boss.Characters[0].Criticaldmg.ToString() + "\r\nturn:" + boss.Characters[0].Turngauge.ToString();

            seosun.Text = "";
            foreach(int c in TCPdata)
            {
                seosun.AppendText(c.ToString() + " ");
            }
            TCPdata.Clear();
        }

		private void btnend_Click(object sender, EventArgs e)
		{
			foreach (Character ch in player.Characters)
			{
				ch.Turnend();
			}
			foreach (Character ch in boss.Characters)
			{
				ch.Turnend();
			}
			Turn();
		}

		private void hand0_Click(object sender, EventArgs e)
		{
			foreach (Character ch in player.Characters)
			{
				ch.Usecard(0);
			}
			foreach (Character ch in boss.Characters)
			{
				ch.Usecard(0);
			}
			Set();
		}

		private void hand1_Click(object sender, EventArgs e)
		{

			foreach (Character ch in player.Characters)
			{
				ch.Usecard(1);
			}
			foreach (Character ch in boss.Characters)
			{
				ch.Usecard(1);
			}
			Set();
		}

		private void hand2_Click(object sender, EventArgs e)
		{

			foreach (Character ch in player.Characters)
			{
				ch.Usecard(2);
			}
			foreach (Character ch in boss.Characters)
			{
				ch.Usecard(2);
			}
			Set();
		}

		private void hand3_Click(object sender, EventArgs e)
		{

			foreach (Character ch in player.Characters)
			{
				ch.Usecard(3);
			}
			foreach (Character ch in boss.Characters)
			{
				ch.Usecard(3);
			}
			Set();
		}

		private void hand4_Click(object sender, EventArgs e)
		{

			foreach (Character ch in player.Characters)
			{
				ch.Usecard(4);
			}
			foreach (Character ch in boss.Characters)
			{
				ch.Usecard(4);
			}
			Set();
		}
	}
}
