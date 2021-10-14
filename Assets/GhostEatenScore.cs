using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostEatenScore : MonoBehaviour
{
    public Transform Pacman;
    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        Invoke("Disable", 1f);
    }
    private void Disable()
    {
        gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
         transform.position = Pacman.position;
    }
}
