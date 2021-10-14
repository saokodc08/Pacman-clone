using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClydeChangeBehaviour : MonoBehaviour
{
    Ghost ghost;

    private void Start()
    {
        ghost = GetComponent<Ghost>();
    }
    public void ChangeToScatter()
    {
        if(!ghost.home.enabled && ghost.chase.enabled)
        {
            ghost.chase.Disable();
        }
    }
    public void ChangeToChase()
    {
        if (!ghost.home.enabled)
        {
            ghost.scatter.Disable();
        }
    }
}
