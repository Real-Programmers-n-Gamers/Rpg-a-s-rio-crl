using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    public bool playerTriggered;

    // Start is called before the first frame update
    void Start()
    {
        playerTriggered = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player") playerTriggered = true;
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player") playerTriggered = false;
    }
}
