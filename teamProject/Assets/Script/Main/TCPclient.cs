using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class TCPclient : MonoBehaviour
{
    public Text text;
    private Animation ani;
    Camera _mainCam = null;
    /// 
    /// 마우스의 상태 
    /// 
    private bool _mouseState;

    /// 
    /// 마우스가 다운된 오브젝트 
    /// 
    private GameObject target;
    /// 
    /// 마우스 좌표 
    /// 
    private Vector3 MousePos;
    private NetworkStream m_networkstream;
    private TcpClient m_client;
    
    private bool deadboss = false;
    private bool reading = false;
    private bool isbossturn = false;
    private bool bossturning = false;
    private bool bossturnend = false;
    private bool wait = false;
    private bool canread = true;
    private bool initing = false;
    private bool init = true;

    private MeshRenderer cardRenderer;
    private List<string> cardlist = new List<string>();
    private List<string> checkcard_list = new List<string>();
    private List<byte> uselist;
    private int[] hand = new int[5];

    // Start is called before the first frame update
    void Start()
    {
        Card_set();
        CheckCard_set();
        ani = GetComponent<Animation>();
        _mainCam = Camera.main;
        uselist = new List<byte>();
        string ipa = GameObject.Find("SorM").GetComponent<OptionManager_select_multi>().IP;
        try
        {
            IPAddress ip = IPAddress.Parse(ipa);
            m_client = new TcpClient();
            m_client.Connect(ip, 1111);
            if (m_client.Connected)
            {
                m_networkstream = m_client.GetStream();
            }
        }
        catch
        {
            Application.Quit();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (reading)
            StartCoroutine(Read());
        else if (init)
        {
            if (!canread)
                return;
            byte[] ins = new byte[1];
            ins[0] = 100;
            m_networkstream.Write(ins, 0, 1);
            m_networkstream.Flush();
            reading = true;
            init = false;
            initing = true;
        }
        else if (isbossturn)
        {
            if (!canread)
                return;
            byte[] ins = new byte[1];
            ins[0] = 7;
            m_networkstream.Write(ins, 0, 1);
            m_networkstream.Flush();
            reading = true;
            isbossturn = false;
            bossturning = true;
        }
        else if (bossturnend)
        {
            if (!canread)
                return;
            byte[] ins = new byte[1];
            ins[0] = 8;
            m_networkstream.Write(ins, 0, 1);
            m_networkstream.Flush();
            reading = true;
            bossturnend = false;
        }
        else if (Input.GetMouseButtonDown(0))
        {
            if (!canread)
                return;
            target = GetClickedObject();
            if (true == target.Equals(GameObject.Find("Sphere")))
            {
                wait = true;
                GameObject.Find("Sphere").GetComponent<Animation>().Play("levorani");
                GameObject.Find("Card1").GetComponent<card1_move>().moveTopack();
                GameObject.Find("Card2").GetComponent<card2_move>().moveTopack();
                GameObject.Find("Card3").GetComponent<card3_move>().moveTopack();
                GameObject.Find("Card4").GetComponent<card4_move>().moveTopack();
                GameObject.Find("Card5").GetComponent<card5_move>().moveTopack();

                GameObject.Find("Card1").GetComponent<card1_move>().switch_packandDeck();
                GameObject.Find("Card2").GetComponent<card2_move>().switch_packandDeck();
                GameObject.Find("Card3").GetComponent<card3_move>().switch_packandDeck();
                GameObject.Find("Card4").GetComponent<card4_move>().switch_packandDeck();
                GameObject.Find("Card5").GetComponent<card5_move>().switch_packandDeck();
                byte[] ins = new byte[uselist.Count + 2];
                uselist.CopyTo(ins);
                ins[uselist.Count] = 6;
                ins[uselist.Count + 1] = 255;
                m_networkstream.Write(ins, 0, ins.Length);
                m_networkstream.Flush();
                reading = true;
                uselist.Clear();
            }
            else if (true == target.Equals(GameObject.Find("Card1")))
            {
                if (uselist.Contains(1))
                {
                    cardRenderer = GameObject.Find("Card1").GetComponent<MeshRenderer>();
                    cardRenderer.material.mainTexture = Resources.Load(cardlist[hand[0]]) as Texture;
                    uselist.Remove(1);
                }
                else
                {
                    byte[] ins = new byte[uselist.Count + 2];
                    uselist.CopyTo(ins);
                    ins[uselist.Count] = 1;
                    ins[uselist.Count + 1] = 255;
                    m_networkstream.Write(ins, 0, ins.Length);
                    byte[] reply = new byte[1];
                    m_networkstream.Read(reply, 0, 1);
                    if (reply[0] != 0)
                    {
                        cardRenderer = GameObject.Find("Card1").GetComponent<MeshRenderer>();
                        cardRenderer.material.mainTexture = Resources.Load(checkcard_list[hand[0]]) as Texture;
                        uselist.Add(1);
                    }
                }
            }
            else if (true == target.Equals(GameObject.Find("Card2")))
            {
                if (uselist.Contains(2))
                {
                    cardRenderer = GameObject.Find("Card2").GetComponent<MeshRenderer>();
                    cardRenderer.material.mainTexture = Resources.Load(cardlist[hand[1]]) as Texture;
                    uselist.Remove(2);
                }
                else
                {
                    byte[] ins = new byte[uselist.Count + 2];
                    uselist.CopyTo(ins);
                    ins[uselist.Count] = 2;
                    ins[uselist.Count + 1] = 255;
                    m_networkstream.Write(ins, 0, ins.Length);
                    byte[] reply = new byte[1];
                    m_networkstream.Read(reply, 0, 1);
                    if (reply[0] == 1)
                    {
                        cardRenderer = GameObject.Find("Card2").GetComponent<MeshRenderer>();
                        cardRenderer.material.mainTexture = Resources.Load(checkcard_list[hand[1]]) as Texture;
                        uselist.Add(2);
                    }
                }
            }
            else if (true == target.Equals(GameObject.Find("Card3")))
            {
                if (uselist.Contains(3))
                {
                    cardRenderer = GameObject.Find("Card3").GetComponent<MeshRenderer>();
                    cardRenderer.material.mainTexture = Resources.Load(cardlist[hand[2]]) as Texture;
                    uselist.Remove(3);
                }
                else
                {
                    byte[] ins = new byte[uselist.Count + 2];
                    uselist.CopyTo(ins);
                    ins[uselist.Count] = 3;
                    ins[uselist.Count + 1] = 255;
                    m_networkstream.Write(ins, 0, ins.Length);
                    byte[] reply = new byte[1];
                    m_networkstream.Read(reply, 0, 1);
                    if (reply[0] == 1)
                    {
                        cardRenderer = GameObject.Find("Card3").GetComponent<MeshRenderer>();
                        cardRenderer.material.mainTexture = Resources.Load(checkcard_list[hand[2]]) as Texture;
                        uselist.Add(3);
                    }
                }
            }
            else if (true == target.Equals(GameObject.Find("Card4")))
            {
                if (uselist.Contains(4))
                {
                    cardRenderer = GameObject.Find("Card4").GetComponent<MeshRenderer>();
                    cardRenderer.material.mainTexture = Resources.Load(cardlist[hand[3]]) as Texture;
                    uselist.Remove(4);
                }
                else
                {
                    byte[] ins = new byte[uselist.Count + 2];
                    uselist.CopyTo(ins);
                    ins[uselist.Count] = 4;
                    ins[uselist.Count + 1] = 255;
                    m_networkstream.Write(ins, 0, ins.Length);
                    byte[] reply = new byte[1];
                    m_networkstream.Read(reply, 0, 1);
                    if (reply[0] == 1)
                    {
                        cardRenderer = GameObject.Find("Card4").GetComponent<MeshRenderer>();
                        cardRenderer.material.mainTexture = Resources.Load(checkcard_list[hand[3]]) as Texture;
                        uselist.Add(4);
                    }
                }
            }
            else if (true == target.Equals(GameObject.Find("Card5")))
            {
                if (uselist.Contains(5))
                {
                    cardRenderer = GameObject.Find("Card5").GetComponent<MeshRenderer>();
                    cardRenderer.material.mainTexture = Resources.Load(cardlist[hand[4]]) as Texture;
                    uselist.Remove(5);
                }
                else
                {
                    byte[] ins = new byte[uselist.Count + 2];
                    uselist.CopyTo(ins);
                    ins[uselist.Count] = 5;
                    ins[uselist.Count + 1] = 255;
                    m_networkstream.Write(ins, 0, ins.Length);
                    byte[] reply = new byte[1];
                    m_networkstream.Read(reply, 0, 1);
                    if (reply[0] == 1)
                    {
                        cardRenderer = GameObject.Find("Card5").GetComponent<MeshRenderer>();
                        cardRenderer.material.mainTexture = Resources.Load(checkcard_list[hand[4]]) as Texture;
                        uselist.Add(5);
                    }
                }
            }
            else
            {
                if (target.GetComponent<Transform>().position.x == GameObject.Find("Lancer").GetComponent<Transform>().position.x && target.GetComponent<Transform>().position.z == GameObject.Find("Lancer").GetComponent<Transform>().position.z)
                {
                    byte[] ins = new byte[1];
                    ins[0] = 11;
                    m_networkstream.Write(ins, 0, 1);
                    reading = true;
                }
                else if (target.GetComponent<Transform>().position.x == GameObject.Find("Lady_Fairy").GetComponent<Transform>().position.x && target.GetComponent<Transform>().position.z == GameObject.Find("Lady_Fairy").GetComponent<Transform>().position.z)
                {
                    byte[] ins = new byte[1];
                    ins[0] = 12;
                    m_networkstream.Write(ins, 0, 1);
                    reading = true;
                }
                else if (target.GetComponent<Transform>().position.x == GameObject.Find("MouseKnight01").GetComponent<Transform>().position.x && target.GetComponent<Transform>().position.z == GameObject.Find("MouseKnight01").GetComponent<Transform>().position.z)
                {
                    byte[] ins = new byte[1];
                    ins[0] = 13;
                    m_networkstream.Write(ins, 0, 1);
                    reading = true;
                }
                else if (target.GetComponent<Transform>().position.x == GameObject.Find("TrollGiant").GetComponent<Transform>().position.x && target.GetComponent<Transform>().position.z == GameObject.Find("TrollGiant").GetComponent<Transform>().position.z)
                {
                    byte[] ins = new byte[1];
                    ins[0] = 14;
                    m_networkstream.Write(ins, 0, 1);
                    reading = true;
                }
                else if (target.GetComponent<Transform>().position.x == GameObject.Find("Demon Blade Lord").GetComponent<Transform>().position.x && target.GetComponent<Transform>().position.z == GameObject.Find("Demon Blade Lord").GetComponent<Transform>().position.z)
                {
                    byte[] ins = new byte[1];
                    ins[0] = 15;
                    m_networkstream.Write(ins, 0, 1);
                    reading = true;
                }
                else if (target.GetComponent<Transform>().position.x == GameObject.Find("EvilArmor_SKEL").GetComponent<Transform>().position.x && target.GetComponent<Transform>().position.z == GameObject.Find("EvilArmor_SKEL").GetComponent<Transform>().position.z)
                {
                    byte[] ins = new byte[1];
                    ins[0] = 16;
                    m_networkstream.Write(ins, 0, 1);
                    reading = true;
                }
            }
        }
    }

    public IEnumerator Read()
    {
        reading = false;
        canread = false;
        byte[] data = new byte[1];
        data[0] = 0;
        List<int> motionl = new List<int>();
        while (data[0] != 255)
        {
            m_networkstream.Read(data, 0, 1);
            motionl.Add(data[0]);
        }

        bool draw = false;
        foreach (int i in motionl)
        {
            if (i > 130 && i < 159)
            {
                if (i % 10 == 1)
                    StartCoroutine(GameObject.Find("Lancer").GetComponent<move_warrior>().Motioning(i));
                else if (i % 10 == 2)
                    StartCoroutine(GameObject.Find("Lady_Fairy").GetComponent<move_healer>().Motioning(i));
                else if (i % 10 == 3)
                    StartCoroutine(GameObject.Find("MouseKnight01").GetComponent<move_tanker>().Motioning(i));
                else if (i % 10 == 4)
                    StartCoroutine(GameObject.Find("TrollGiant").GetComponent<move_boss1>().Motioning(i));
                else if (i % 10 == 5)
                    StartCoroutine(GameObject.Find("Demon Blade Lord").GetComponent<move_boss2>().Motioning(i));
                else if (i % 10 == 6)
                    StartCoroutine(GameObject.Find("EvilArmor_SKEL").GetComponent<move_boss3>().Motioning(i));

                if (i == 144 || i == 145 || i == 146)
                    deadboss = true;
            }

            if (i >= 7 && i < 18)
            {
                yield return StartCoroutine(GameObject.Find("Lancer").GetComponent<move_warrior>().Motioning(i));
            }
            else if (i > 20 && i < 29)
            {
                yield return StartCoroutine(GameObject.Find("Lady_Fairy").GetComponent<move_healer>().Motioning(i));
            }
            else if (i > 30 && i < 39)
            {
                yield return StartCoroutine(GameObject.Find("MouseKnight01").GetComponent<move_tanker>().Motioning(i));
            }
            else if (i > 40 && i < 49)
            {
                yield return StartCoroutine(GameObject.Find("TrollGiant").GetComponent<move_boss1>().Motioning(i));
            }
            else if (i > 50 && i < 59)
            {
                yield return StartCoroutine(GameObject.Find("Demon Blade Lord").GetComponent<move_boss2>().Motioning(i));
            }
            else if (i > 60 && i < 69)
            {
                yield return StartCoroutine(GameObject.Find("EvilArmor_SKEL").GetComponent<move_boss3>().Motioning(i));
            }
            else if (i > 70 && i < 129)
            {
                if (i % 10 == 1)
                    yield return StartCoroutine(GameObject.Find("Lancer").GetComponent<move_warrior>().Motioning(i));
                else if (i % 10 == 2)
                    yield return StartCoroutine(GameObject.Find("Lady_Fairy").GetComponent<move_healer>().Motioning(i));
                else if (i % 10 == 3)
                    yield return StartCoroutine(GameObject.Find("MouseKnight01").GetComponent<move_tanker>().Motioning(i));
                else if (i % 10 == 4)
                    yield return StartCoroutine(GameObject.Find("TrollGiant").GetComponent<move_boss1>().Motioning(i));
                else if (i % 10 == 5)
                    yield return StartCoroutine(GameObject.Find("Demon Blade Lord").GetComponent<move_boss2>().Motioning(i));
                else if (i % 10 == 6)
                    yield return StartCoroutine(GameObject.Find("EvilArmor_SKEL").GetComponent<move_boss3>().Motioning(i));
            }

            if (i == 161 || i == 162 || i == 163)
                draw = true;
            if (i == 164 || i == 165 || i == 166)
                isbossturn = true;
        }

        if (wait)
        {
            yield return new WaitForSeconds(2);
            wait = false;
        }
        
        int[] tturn = new int[4];
        int aliveboss = 0;
        m_networkstream.Read(data, 0, 1); // 턴순서
        tturn[0] = data[0] - 1;
        if (data[0] >= 4)
            aliveboss = data[0] - 1;

        m_networkstream.Read(data, 0, 1);
        tturn[1] = data[0] - 1;
        if (data[0] >= 4)
            aliveboss = data[0] - 1;

        m_networkstream.Read(data, 0, 1);
        tturn[2] = data[0] - 1;
        if (data[0] >= 4)
            aliveboss = data[0] - 1;

        m_networkstream.Read(data, 0, 1);
        tturn[3] = data[0] - 1;
        if (data[0] >= 4)
            aliveboss = data[0] - 1;

        m_networkstream.Read(data, 0, 1);   // 누구턴?

        m_networkstream.Read(data, 0, 1);   //코스트

        int hp = 0;
        m_networkstream.Read(data, 0, 1);
        hp += data[0] * 100;
        m_networkstream.Read(data, 0, 1);
        hp += data[0];

        m_networkstream.Read(data, 0, 1);
        m_networkstream.Read(data, 0, 1);

        m_networkstream.Read(data, 0, 1);
        int dmg = data[0];

        m_networkstream.Read(data, 0, 1);
        int guard = data[0];

        m_networkstream.Read(data, 0, 1);
        int speed = data[0];

        m_networkstream.Read(data, 0, 1);
        int crip = data[0];

        m_networkstream.Read(data, 0, 1);
        int crid = 0;
        crid += data[0] * 100;
        m_networkstream.Read(data, 0, 1); // 치명타 데미지
        crid += data[0];
        GameObject.Find("UIcontrol").GetComponent<Funcs>().Statement_change(hp, dmg, guard, speed, crip, crid);
        
        m_networkstream.Read(data, 0, 1); // 카드 1부터
        hand[0] = data[0];
        m_networkstream.Read(data, 0, 1);
        hand[1] = data[0];
        m_networkstream.Read(data, 0, 1);
        hand[2] = data[0];
        m_networkstream.Read(data, 0, 1);
        hand[3] = data[0];
        m_networkstream.Read(data, 0, 1);
        hand[4] = data[0];

        if(draw)
        {
           
            GameObject.Find("Card1").GetComponent<card1_change>().Card_Image_Change(hand[0]);
            GameObject.Find("Card1").GetComponent<card1_change>().Card_filp();
            GameObject.Find("Card1").GetComponent<card1_move>().moveTowhere();

            GameObject.Find("Card2").GetComponent<card1_change>().Card_Image_Change(hand[1]);
            GameObject.Find("Card2").GetComponent<card1_change>().Card_filp();
            GameObject.Find("Card2").GetComponent<card2_move>().moveTowhere();

            GameObject.Find("Card3").GetComponent<card1_change>().Card_Image_Change(hand[2]);
            GameObject.Find("Card3").GetComponent<card1_change>().Card_filp();
            GameObject.Find("Card3").GetComponent<card3_move>().moveTowhere();

            GameObject.Find("Card4").GetComponent<card1_change>().Card_Image_Change(hand[3]);
            GameObject.Find("Card4").GetComponent<card1_change>().Card_filp();
            GameObject.Find("Card4").GetComponent<card4_move>().moveTowhere();

            GameObject.Find("Card5").GetComponent<card1_change>().Card_Image_Change(hand[4]);
            GameObject.Find("Card5").GetComponent<card1_change>().Card_filp();
            GameObject.Find("Card5").GetComponent<card5_move>().moveTowhere();
        }
        GameObject.Find("turn1").GetComponent<turnimage_change>().whos_turn(tturn[0]);
        GameObject.Find("turn2").GetComponent<turnimage_change>().whos_turn(tturn[1]);
        GameObject.Find("turn3").GetComponent<turnimage_change>().whos_turn(tturn[2]);
        GameObject.Find("turn4").GetComponent<turnimage_change>().whos_turn(tturn[3]);

        m_networkstream.Read(data, 0, 1);   // 보스 HP 100의 자리
        int bosshp = 0;
        bosshp += data[0] * 100;
        m_networkstream.Read(data, 0, 1);
        bosshp += data[0];
        GameObject.Find("UIcontrol").GetComponent<Funcs>().Boss_Hp_change(bosshp);

        m_networkstream.Read(data, 0, 1);
        bool alive = false;
        int x = (data[0] % 100 % 8) * 10;
        int y = (data[0] % 100 / 8) * 10;
        if (data[0] / 100 == 1)
            alive = true;
        GameObject.Find("Lancer").GetComponent<Transform>().position = new Vector3(x, 0, y);

        m_networkstream.Read(data, 0, 1);
        x = (data[0] % 100 % 8) * 10;
        y = (data[0] % 100 / 8) * 10;
        if (data[0] / 100 == 1)
            alive = true;
        GameObject.Find("Lady_Fairy").GetComponent<Transform>().position = new Vector3(x, 0, y);

        m_networkstream.Read(data, 0, 1);
        x = (data[0] % 100 % 8) * 10;
        y = (data[0] % 100 / 8) * 10;
        if (data[0] / 100 == 1)
            alive = true;
        GameObject.Find("MouseKnight01").GetComponent<Transform>().position = new Vector3(x, 0, y);

        if(!alive)
        {
            yield return StartCoroutine(GameObject.Find("GameMgn").GetComponent<nextboss>().fai());
            GameObject.Find("GameMgn").GetComponent<nextboss>().fail.SetActive(false);
            init = true;
        }

        m_networkstream.Read(data, 0, 1);
        x = (data[0] % 100 % 8) * 10;
        y = (data[0] % 100 / 8) * 10;
        if (aliveboss == 3)
        {
            GameObject.Find("TrollGiant").GetComponent<Transform>().position = new Vector3(x, 0, y);
            GameObject.Find("Demon Blade Lord").GetComponent<Transform>().position = new Vector3(x, 500, y);
            GameObject.Find("EvilArmor_SKEL").GetComponent<Transform>().position = new Vector3(x, 500, y);
        }
        else if (aliveboss == 4)
        {
            GameObject.Find("TrollGiant").GetComponent<Transform>().position = new Vector3(x, 500, y);
            GameObject.Find("Demon Blade Lord").GetComponent<Transform>().position = new Vector3(x, 0, y);
            GameObject.Find("EvilArmor_SKEL").GetComponent<Transform>().position = new Vector3(x, 500, y);
        }
        else
        {
            GameObject.Find("TrollGiant").GetComponent<Transform>().position = new Vector3(x, 500, y);
            GameObject.Find("Demon Blade Lord").GetComponent<Transform>().position = new Vector3(x, 500, y);
            GameObject.Find("EvilArmor_SKEL").GetComponent<Transform>().position = new Vector3(x, 0, y);
        }

        if (deadboss)
        {
            GameObject.Find("Lancer").GetComponent<move_warrior>().mylocationset();
            GameObject.Find("Lady_Fairy").GetComponent<move_healer>().mylocationset();
            GameObject.Find("MouseKnight01").GetComponent<move_tanker>().mylocationset();
            GameObject.Find("TrollGiant").GetComponent<move_boss1>().mylocationset();
            GameObject.Find("Demon Blade Lord").GetComponent<move_boss2>().mylocationset();
            GameObject.Find("EvilArmor_SKEL").GetComponent<move_boss3>().mylocationset();

            yield return StartCoroutine(GameObject.Find("GameMgn").GetComponent<nextboss>().vic());
            GameObject.Find("GameMgn").GetComponent<nextboss>().victory.SetActive(false);
            deadboss = false;
        }
        m_networkstream.Read(data, 0, 1);
        if (bossturning)
        {
            bossturnend = true;
            bossturning = false;
        }
        canread = true;
    }

    private GameObject GetClickedObject()
    {
        //충돌이 감지된 영역 
        RaycastHit hit;
        //찾은 오브젝트 
        GameObject target = null;

        //마우스 포이트 근처 좌표를 만든다. 
        Ray ray = _mainCam.ScreenPointToRay(Input.mousePosition);

        //마우스 근처에 오브젝트가 있는지 확인 
        if (true == (Physics.Raycast(ray.origin, ray.direction * 10, out hit)))
        {
            //있다! 

            //있으면 오브젝트를 저장한다. 
            target = hit.collider.gameObject;
        }

        return target;
    }
    public void CheckCard_set()
    {
        checkcard_list.Add("anything");
        checkcard_list.Add("check_card_1");
        checkcard_list.Add("check_card_2");
        checkcard_list.Add("check_card_3");
        checkcard_list.Add("check_card_4");
        checkcard_list.Add("check_card_5");
        checkcard_list.Add("check_card_6");
        checkcard_list.Add("check_card_7");
        checkcard_list.Add("check_card_8");
        // 무쓸모
        checkcard_list.Add("9");
        checkcard_list.Add("10");
        // 딜러
        checkcard_list.Add("check_Lancer_11");
        checkcard_list.Add("check_Lancer_12");
        checkcard_list.Add("check_Lancer_13");
        checkcard_list.Add("check_Lancer_14");
        checkcard_list.Add("check_Lancer_15");
        checkcard_list.Add("check_Lancer_16");
        checkcard_list.Add("check_Lancer_17");
        // 무쓸모
        checkcard_list.Add("18");
        checkcard_list.Add("19");
        checkcard_list.Add("20");
        // 힐러
        checkcard_list.Add("check_Healer_21");
        checkcard_list.Add("check_Healer_22");
        checkcard_list.Add("check_Healer_23");
        checkcard_list.Add("check_Healer_24");
        checkcard_list.Add("check_Healer_25");
        // 무쓸모           
        checkcard_list.Add("26");
        checkcard_list.Add("27");
        checkcard_list.Add("28");
        checkcard_list.Add("29");
        checkcard_list.Add("30");
        // 탱커
        checkcard_list.Add("check_Tanker_31");
        checkcard_list.Add("check_Tanker_32");
        checkcard_list.Add("check_Tanker_33");
        checkcard_list.Add("check_Tanker_34");
        checkcard_list.Add("check_Tanker_35");
    }
    public void Card_set()
    {
        cardlist.Add("anything");
        cardlist.Add("card_1");
        cardlist.Add("card_2");
        cardlist.Add("card_3");
        cardlist.Add("card_4");
        cardlist.Add("card_5");
        cardlist.Add("card_6");
        cardlist.Add("card_7");
        cardlist.Add("card_8");
        // 무쓸모
        cardlist.Add("9");
        cardlist.Add("10");
        // 딜러
        cardlist.Add("Lancer_11");
        cardlist.Add("Lancer_12");
        cardlist.Add("Lancer_13");
        cardlist.Add("Lancer_14");
        cardlist.Add("Lancer_15");
        cardlist.Add("Lancer_16");
        cardlist.Add("Lancer_17");
        // 무쓸모
        cardlist.Add("18");
        cardlist.Add("19");
        cardlist.Add("20");
        // 힐러
        cardlist.Add("Healer_21");
        cardlist.Add("Healer_22");
        cardlist.Add("Healer_23");
        cardlist.Add("Healer_24");
        cardlist.Add("Healer_25");
        // 무쓸모
        cardlist.Add("26");
        cardlist.Add("27");
        cardlist.Add("28");
        cardlist.Add("29");
        cardlist.Add("30");
        // 탱커
        cardlist.Add("Tanker_31");
        cardlist.Add("Tanker_32");
        cardlist.Add("Tanker_33");
        cardlist.Add("Tanker_34");
        cardlist.Add("Tanker_35");

    }
}