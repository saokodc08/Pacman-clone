using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeFX : MonoBehaviour
{
    Animator animator;
    public GameObject canvas;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        canvas.SetActive(true);
    }
    // Start is called before the first frame update
    void Start()
    {
        animator.SetTrigger("Out");
    }

    
}
