using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClydeTrigger : MonoBehaviour
{
    public ClydeChangeBehaviour clydeChangeBehaviour;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Clyde")
        {
            clydeChangeBehaviour.ChangeToScatter();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Clyde")
        {
            clydeChangeBehaviour.ChangeToScatter();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Clyde")
        {
            clydeChangeBehaviour.ChangeToChase();
        }
    }

}
