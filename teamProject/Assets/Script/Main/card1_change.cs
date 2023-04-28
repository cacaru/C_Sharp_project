using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class card1_change : MonoBehaviour
{
    public GameObject card_info;
    List<string> cardlist = new List<string>();
    public MeshRenderer cardRenderer;
    public int cardnum;

    void OnMouseEnter()
    {
        card_info.SetActive(true);
        GameObject.Find("Card_info_image").GetComponent<Card_info_image_change>().Card_info_up(cardnum);
    }

    void OnMouseExit()
    {
        card_info.SetActive(false);
    }

    // public GameObject morgeta3;
    // Start is called before the first frame update
    void Start()
    {
        Card_set();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Card_Image_Change(int card_Image_num)
    {
        cardnum = card_Image_num;
        cardRenderer = gameObject.GetComponent<MeshRenderer>();
        cardRenderer.material.mainTexture = Resources.Load(cardlist[card_Image_num]) as Texture;
    }

    public void Card_filp()
    {
        // cardRenderer.material.SetTextureScale("_MainTex", new Vector2(0, -1));
        transform.rotation = Quaternion.Euler(0, 180, 0);
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
