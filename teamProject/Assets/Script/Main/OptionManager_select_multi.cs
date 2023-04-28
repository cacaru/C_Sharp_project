using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net;

public class OptionManager_select_multi : MonoBehaviour
{
    public InputField inputIp;
    public string IP;
    // public Text text;
    // Start is called before the first frame update
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        IP = inputIp.text;
        // text.text = IP;
    }

    public void OnSingleBtn()
    {
        Application.LoadLevel("SampleScene");
    }

    public void OnMultiBtn()
    {
        Application.LoadLevel("M_S");
    }
}
