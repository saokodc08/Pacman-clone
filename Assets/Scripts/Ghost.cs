using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public Movement movement { get; private set; }
    public GhostHome home { get; private set; }
    public GhostChase chase { get; private set; }
    public GhostFrightened frightened { get; private set; }
    public GhostScatter scatter { get; private set; }
    public GhostBehaviour initialBehavior;
    public Transform target;
    public Transform homeTarget;
    public Transform scatterTarget;
    public bool changeBehaviour = false;
    public int points = 200;
    public bool isClyde = false;
    private void Awake()
    {
        movement = GetComponent<Movement>();
        home = GetComponent<GhostHome>();
        chase = GetComponent<GhostChase>();
        frightened = GetComponent<GhostFrightened>();
        scatter = GetComponent<GhostScatter>();
    }
    // Start is called before the first frame update
    void Start()
    {
        //ResetState();
    }

    public void ResetState()
    {
        gameObject.SetActive(true);
        movement.ResetState();
        frightened.Disable();
        if(!isClyde)
        {
            chase.Disable();
            scatter.Enable();
        }
        else
        {
            chase.Enable();
            scatter.Disable();
        }

        if(home != initialBehavior)
        {
            home.Disable();
        }
        if(initialBehavior != null)
        {
            initialBehavior.Enable();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            if(frightened.enabled)
            {
                FindObjectOfType<GameManager>().GhostEaten(this);
            }
            else
            {
                FindObjectOfType<GameManager>().PacmanEaten(); 
            }
        }
    }

    public void ChangeBehaviour(bool change)
    {
        changeBehaviour = change;
        if(change)
            scatter.Disable();
    }
}
