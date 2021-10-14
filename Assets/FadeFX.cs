using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeFX : MonoBehaviour
{
    Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();

    }
    // Start is called before the first frame update
    void Start()
    {
        animator.SetTrigger("Out");
    }

    
}
