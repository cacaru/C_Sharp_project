using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class card4_move : MonoBehaviour
{
    Vector3 pack = new Vector3(-18f, 1.6f, -15f);
    public Animation moveTopackani;

    // Start is called before the first frame update
    void Start()
    {
        moveTopackani = GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void moveTopack()
    {
        moveTopackani.Play("moveTopack");
    }

    public void switch_packandDeck()
    {
        transform.position = pack;
    }

    public void moveTowhere()
    {
        moveTopackani.Play("moveTowhere");
    }
}
