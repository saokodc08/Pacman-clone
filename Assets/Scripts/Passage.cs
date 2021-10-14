using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passage : MonoBehaviour
{
    public Transform connection;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag != "ClydeTrigger")
        {
            Vector3 position = collision.transform.position;
            position.x = connection.position.x;
            position.y = connection.position.y;
            collision.transform.position = position;
        }

    }
}
