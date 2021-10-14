using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkyTarget : MonoBehaviour
{
    public Transform Blinky;
    public Transform TwoStepAhead;
    private Vector3 _pos = Vector3.zero;

    private void FixedUpdate()
    {
        _pos.x = TwoStepAhead.position.x - Blinky.position.x;
        _pos.y = TwoStepAhead.position.y - Blinky.position.y;
        transform.position = _pos + TwoStepAhead.position;
    }
}
