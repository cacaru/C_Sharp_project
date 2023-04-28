using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class MultiClient : MonoBehaviour
{
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

    private bool m_Connect = false;
    private bool reading = true;
    private bool wait = false;
    private bool canread = true;
    private bool mytrun = false;
    private byte[] ins = new byte[1] { 111 };

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
        target = GameObject.Find("EventManager");
        uselist = new List<byte>();
        string ipa = GameObject.Find("SorM").GetComponent<OptionManager_select_multi>().IP;
        try
        {
            IPAddress ip = IPAddress.Parse(ipa);
            m_client = new TcpClient();
            m_client.Connect(ip, 1111);
            if (m_client.Connected)
            {
                byte[] initdata = new byte[1];
                initdata[0] = 200;
                m_networkstream = m_client.GetStream();
                m_Connect = true;
                m_networkstream.Write(initdata, 0, 1);
                m_networkstream.Flush();
                reading = true;
            }
        }
        catch
        {
            Application.Quit();
        }
        GameObject.Find("Card1").GetComponent<card1_move>().switch_packandDeck();
        GameObject.Find("Card2").GetComponent<card2_move>().switch_packandDeck();
        GameObject.Find("Card3").GetComponent<card3_move>().switch_packandDeck();
        GameObject.Find("Card4").GetComponent<card4_move>().switch_packandDeck();
        GameObject.Find("Card5").GetComponent<card5_move>().switch_packandDeck();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            target = GetClickedObject();
        if (canread && false == target.Equals(GameObject.Find("EventManager")))
        {
            if (true == target.Equals(GameObject.Find("Sphere")))
            {
                if (!mytrun)
                    return;
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
                ins = new byte[uselist.Count + 1];
                uselist.CopyTo(ins);
                ins[uselist.Count] = 6;
                uselist.Clear();
            }
            else if (true == target.Equals(GameObject.Find("Card1")))
            {
                if (!mytrun)
                    return;
                if (uselist.Contains(1))
                {
                    cardRenderer = GameObject.Find("Card1").GetComponent<MeshRenderer>();
                    cardRenderer.material.mainTexture = Resources.Load(cardlist[hand[0]]) as Texture;
                    uselist.Remove(1);
                }
                else
                {
                    ins = new byte[uselist.Count + 2];
                    uselist.CopyTo(ins);
                    ins[uselist.Count] = 1;
                    ins[uselist.Count + 1] = 255;
                    m_networkstream.Write(ins, 0, ins.Length);
                    m_networkstream.Flush();
                    ins = new byte[1] { 111 };
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
                if (!mytrun)
                    return;
                if (uselist.Contains(2))
                {
                    cardRenderer = GameObject.Find("Card2").GetComponent<MeshRenderer>();
                    cardRenderer.material.mainTexture = Resources.Load(cardlist[hand[1]]) as Texture;
                    uselist.Remove(2);
                }
                else
                {
                    ins = new byte[uselist.Count + 2];
                    uselist.CopyTo(ins);
                    ins[uselist.Count] = 2;
                    ins[uselist.Count + 1] = 255;
                    m_networkstream.Write(ins, 0, ins.Length);
                    m_networkstream.Flush();
                    ins = new byte[1] { 111 };
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
                if (!mytrun)
                    return;
                if (uselist.Contains(3))
                {
                    cardRenderer = GameObject.Find("Card3").GetComponent<MeshRenderer>();
                    cardRenderer.material.mainTexture = Resources.Load(cardlist[hand[2]]) as Texture;
                    uselist.Remove(3);
                }
                else
                {
                    ins = new byte[uselist.Count + 2];
                    uselist.CopyTo(ins);
                    ins[uselist.Count] = 3;
                    ins[uselist.Count + 1] = 255;
                    m_networkstream.Write(ins, 0, ins.Length);
                    m_networkstream.Flush();
                    ins = new byte[1] { 111 };
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
                if (!mytrun)
                    return;
                if (uselist.Contains(4))
                {
                    cardRenderer = GameObject.Find("Card4").GetComponent<MeshRenderer>();
                    cardRenderer.material.mainTexture = Resources.Load(cardlist[hand[3]]) as Texture;
                    uselist.Remove(4);
                }
                else
                {
                    ins = new byte[uselist.Count + 2];
                    uselist.CopyTo(ins);
                    ins[uselist.Count] = 4;
                    ins[uselist.Count + 1] = 255;
                    m_networkstream.Write(ins, 0, ins.Length);
                    m_networkstream.Flush();
                    ins = new byte[1] { 111 };
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
                if (!mytrun)
                    return;
                if (uselist.Contains(5))
                {
                    cardRenderer = GameObject.Find("Card5").GetComponent<MeshRenderer>();
                    cardRenderer.material.mainTexture = Resources.Load(cardlist[hand[4]]) as Texture;
                    uselist.Remove(5);
                }
                else
                {
                    ins = new byte[uselist.Count + 2];
                    uselist.CopyTo(ins);
                    ins[uselist.Count] = 5;
                    ins[uselist.Count + 1] = 255;
                    m_networkstream.Write(ins, 0, ins.Length);
                    m_networkstream.Flush();
                    ins = new byte[1] { 111 };
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
                    ins = new byte[1] { 11 };
                }
                else if (target.GetComponent<Transform>().position.x == GameObject.Find("Lady_Fairy").GetComponent<Transform>().position.x && target.GetComponent<Transform>().position.z == GameObject.Find("Lady_Fairy").GetComponent<Transform>().position.z)
                {
                    ins = new byte[1] { 12 };
                }
                else if (target.GetComponent<Transform>().position.x == GameObject.Find("MouseKnight01").GetComponent<Transform>().position.x && target.GetComponent<Transform>().position.z == GameObject.Find("MouseKnight01").GetComponent<Transform>().position.z)
                {
                    ins = new byte[1] { 13 };
                }
                if (target.GetComponent<Transform>().position.x == GameObject.Find("eLancer").GetComponent<Transform>().position.x && target.GetComponent<Transform>().position.z == GameObject.Find("eLancer").GetComponent<Transform>().position.z)
                {
                    ins = new byte[1] { 14 };
                }
                else if (target.GetComponent<Transform>().position.x == GameObject.Find("eLady_Fairy").GetComponent<Transform>().position.x && target.GetComponent<Transform>().position.z == GameObject.Find("eLady_Fairy").GetComponent<Transform>().position.z)
                {
                    ins = new byte[1] { 15 };
                }
                else if (target.GetComponent<Transform>().position.x == GameObject.Find("eMouseKnight01").GetComponent<Transform>().position.x && target.GetComponent<Transform>().position.z == GameObject.Find("eMouseKnight01").GetComponent<Transform>().position.z)
                {
                    ins = new byte[1] { 16 };
                }
            }
            target = GameObject.Find("EventManager");
        }
        if (canread)
            StartCoroutine(Read());
    }

    public IEnumerator Read()
    {
        reading = false;
        canread = false;
        m_networkstream.Write(ins, 0, ins.Length);
        m_networkstream.Flush();
        ins = new byte[1] { 111 };
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
                    StartCoroutine(GameObject.Find("eLancer").GetComponent<move_ewarrior>().Motioning(i - 3));
                else if (i % 10 == 5)
                    StartCoroutine(GameObject.Find("eLady_Fairy").GetComponent<move_ehealer>().Motioning(i - 3));
                else if (i % 10 == 6)
                    StartCoroutine(GameObject.Find("eMouseKnight01").GetComponent<move_etanker>().Motioning(i - 3));
            }

            if (i >= 1 && i < 15)
            {
                yield return StartCoroutine(GameObject.Find("Lancer").GetComponent<move_warrior>().Motioning(i + 6));
            }
            else if (i >= 15 && i < 24)
            {
                yield return StartCoroutine(GameObject.Find("Lady_Fairy").GetComponent<move_healer>().Motioning(i + 6));
            }
            else if (i > 24 && i < 34)
            {
                yield return StartCoroutine(GameObject.Find("MouseKnight01").GetComponent<move_tanker>().Motioning(i + 6));
            }
            else if (i >= 34 && i < 49)
            {
                yield return StartCoroutine(GameObject.Find("eLancer").GetComponent<move_ewarrior>().Motioning(i + 6 - 35));
            }
            else if (i > 49 && i < 59)
            {
                yield return StartCoroutine(GameObject.Find("eLady_Fairy").GetComponent<move_ehealer>().Motioning(i + 6 - 35));
            }
            else if (i > 59 && i < 69)
            {
                yield return StartCoroutine(GameObject.Find("eMouseKnight01").GetComponent<move_etanker>().Motioning(i + 6 - 35));
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
                    yield return StartCoroutine(GameObject.Find("eLancer").GetComponent<move_ewarrior>().Motioning(i - 3));
                else if (i % 10 == 5)
                    yield return StartCoroutine(GameObject.Find("eLady_Fairy").GetComponent<move_ehealer>().Motioning(i - 3));
                else if (i % 10 == 6)
                    yield return StartCoroutine(GameObject.Find("eMouseKnight01").GetComponent<move_etanker>().Motioning(i - 3));
            }

            if (i == 161 || i == 162 || i == 163)
            {
                draw = true;
                mytrun = true;
            }

            if(i == 164 || i == 165 || i == 166)
            {
                mytrun = false;
            }
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
        
        GameObject.Find("turn1").GetComponent<m_turn>().whos_turn(tturn[0]);
        GameObject.Find("turn2").GetComponent<m_turn>().whos_turn(tturn[1]);
        GameObject.Find("turn3").GetComponent<m_turn>().whos_turn(tturn[2]);
        GameObject.Find("turn4").GetComponent<m_turn>().whos_turn(tturn[3]);

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
        m_networkstream.Read(data, 0, 1);
        crid += data[0];
        GameObject.Find("UIcontrol").GetComponent<Funcs>().Statement_change(hp, dmg, guard, speed, crip, crid);

        m_networkstream.Read(data, 0, 1);
        hand[0] = data[0];
        m_networkstream.Read(data, 0, 1);
        hand[1] = data[0];
        m_networkstream.Read(data, 0, 1);
        hand[2] = data[0];
        m_networkstream.Read(data, 0, 1);
        hand[3] = data[0];
        m_networkstream.Read(data, 0, 1);
        hand[4] = data[0];

        if (draw)
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

        if (!alive)
        {
            StartCoroutine(GameObject.Find("GameMgn").GetComponent<nextboss>().fai());
            mytrun = false;
        }

        m_networkstream.Read(data, 0, 1);
        alive = false;
        x = (data[0] % 100 % 8) * 10;
        y = (data[0] % 100 / 8) * 10;
        if (data[0] / 100 == 1)
            alive = true;
        GameObject.Find("eLancer").GetComponent<Transform>().position = new Vector3(x, 0, y);

        m_networkstream.Read(data, 0, 1);
        x = (data[0] % 100 % 8) * 10;
        y = (data[0] % 100 / 8) * 10;
        if (data[0] / 100 == 1)
            alive = true;
        GameObject.Find("eLady_Fairy").GetComponent<Transform>().position = new Vector3(x, 0, y);

        m_networkstream.Read(data, 0, 1);
        x = (data[0] % 100 % 8) * 10;
        y = (data[0] % 100 / 8) * 10;
        if (data[0] / 100 == 1)
            alive = true;
        GameObject.Find("eMouseKnight01").GetComponent<Transform>().position = new Vector3(x, 0, y);

        if (!alive)
        {
            StartCoroutine(GameObject.Find("GameMgn").GetComponent<nextboss>().vic());
            mytrun = false;
        }

        yield return new WaitForSeconds(0.5f);
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