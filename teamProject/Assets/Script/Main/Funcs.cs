using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Funcs : MonoBehaviour
{

    public Text txtHp;
    public Text txtDmg;
    public Text txtGuard;
    public Text txtSpeed;
    public Text txtcriper;
    public Text txtcridmg;
    public Text txtBossHp;
    public GameObject Card_one;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Change_Card_Image()
    {
    }

    public void Statement_change(int hp,int dmg, int guard, int speed, int crip, int crid)
    {
        txtHp.GetComponent<Text>().text = hp.ToString();
        txtDmg.GetComponent<Text>().text = dmg.ToString();
        txtGuard.GetComponent<Text>().text = guard.ToString();
        txtSpeed.GetComponent<Text>().text = speed.ToString();
        txtcriper.GetComponent<Text>().text = crip.ToString() + "%";
        txtcridmg.GetComponent<Text>().text = crid.ToString() + "%";
    }

    public void Boss_Hp_change(int hp)
    {
        txtBossHp.GetComponent<Text>().text = hp.ToString();
    }
}
