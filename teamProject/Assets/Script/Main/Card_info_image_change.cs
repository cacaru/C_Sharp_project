using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Card_info_image_change : MonoBehaviour
{
    public GameObject itObject;
    public Image Myimage;
    List<string> card_info_list = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        Card_info_set();
        itObject.SetActive(false);
        Myimage = itObject.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Card_info_up(int What_Card)
    {
        itObject.GetComponent<Image>().sprite = Resources.Load(card_info_list[What_Card], typeof(Sprite)) as Sprite;
    }

    public void Card_info_set()
    {
        card_info_list.Add("anything");
        card_info_list.Add("card_1");
        card_info_list.Add("card_2");
        card_info_list.Add("card_3");
        card_info_list.Add("card_4");
        card_info_list.Add("card_5");
        card_info_list.Add("card_6");
        card_info_list.Add("card_7");
        card_info_list.Add("card_8");
        // 무쓸모
        card_info_list.Add("9");
        card_info_list.Add("10");
        // 딜러
        card_info_list.Add("Lancer_11");
        card_info_list.Add("Lancer_12");
        card_info_list.Add("Lancer_13");
        card_info_list.Add("Lancer_14");
        card_info_list.Add("Lancer_15");
        card_info_list.Add("Lancer_16");
        card_info_list.Add("Lancer_17");
        // 무쓸모
        card_info_list.Add("18");
        card_info_list.Add("19");
        card_info_list.Add("20");
        // 힐러
        card_info_list.Add("Healer_21");
        card_info_list.Add("Healer_22");
        card_info_list.Add("Healer_23");
        card_info_list.Add("Healer_24");
        card_info_list.Add("Healer_25");
        // 무쓸모
        card_info_list.Add("26");
        card_info_list.Add("27");
        card_info_list.Add("28");
        card_info_list.Add("29");
        card_info_list.Add("30");
        // 탱커
        card_info_list.Add("Tanker_31");
        card_info_list.Add("Tanker_32");
        card_info_list.Add("Tanker_33");
        card_info_list.Add("Tanker_34");
        card_info_list.Add("Tanker_35");
    }

}
