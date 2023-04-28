using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class m_turn : MonoBehaviour
{
    public GameObject turn_one_obj;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void whos_turn(int n)
    {
        switch (n)
        {
            case 0:
                turn_one_obj.GetComponent<Image>().sprite = Resources.Load("Lancer_face", typeof(Sprite)) as Sprite;
                break;
            case 1:
                turn_one_obj.GetComponent<Image>().sprite = Resources.Load("Healer_face", typeof(Sprite)) as Sprite;
                break;
            case 2:
                turn_one_obj.GetComponent<Image>().sprite = Resources.Load("Tanker_face", typeof(Sprite)) as Sprite;
                break;
            case 3:
                turn_one_obj.GetComponent<Image>().sprite = Resources.Load("eLancer_face", typeof(Sprite)) as Sprite;
                break;
            case 4:
                turn_one_obj.GetComponent<Image>().sprite = Resources.Load("eHealer_face", typeof(Sprite)) as Sprite;
                break;
            case 5:
                turn_one_obj.GetComponent<Image>().sprite = Resources.Load("eTanker_face", typeof(Sprite)) as Sprite;
                break;
        }

    }
}
