using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nextboss : MonoBehaviour
{
    public GameObject victory;
    public GameObject fail;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator vic()
    {
        victory.SetActive(true);
        yield return new WaitForSeconds(3);
    }

    public IEnumerator fai()
    {
        fail.SetActive(true);
        yield return new WaitForSeconds(3);
    }
}
