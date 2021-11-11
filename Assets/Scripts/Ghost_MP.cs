using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(Movement_MP))]
public class Ghost_MP : MonoBehaviour
{
    public bool isAcceptingInput = true;
    private Movement_MP movement;
    public ParticleSystem[] deathFX;
    private CircleCollider2D _circleCollider2D;
    private SpriteRenderer _spriteRenderer;
    [SerializeField] GameObject powerPalletEatenFX;
    [SerializeField] GameObject pacmanWinFX;
    [SerializeField] GameObject MobileControlPanel;
    private bool up, down, left, right = false;

    private PhotonView view;

    private void Awake()
    {
        movement = GetComponent<Movement_MP>();
        _circleCollider2D = GetComponent<CircleCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        view = GetComponent<PhotonView>();
        if(view.IsMine)
            MobileControlPanel.SetActive(true);
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager_MP>().ghost = this;

    }
    private void Update()
    {
        if(isAcceptingInput && view.IsMine)
        {
            if(Input.GetKeyDown(KeyCode.W)|| Input.GetKeyDown(KeyCode.UpArrow)||up)
                {
                    movement.SetDirection(Vector2.up);
                }
                if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow) || right)
                {
                    movement.SetDirection(Vector2.right);
                }
                if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow) || left)
                {
                    movement.SetDirection(Vector2.left);
                }
                if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) || down)
                {
                    movement.SetDirection(Vector2.down);
                }
        }
        
    }

    public void ResetState()
    {
        _circleCollider2D.enabled = true;
        _spriteRenderer.enabled = true;
        gameObject.SetActive(true);
        movement.ResetState();
    }

    public void PacmanDied()
    {
        _circleCollider2D.enabled = false;
        _spriteRenderer.enabled = false;
        movement.SetDirection(Vector2.zero,true);
        foreach(ParticleSystem fx in deathFX)
        {
            fx.Play();
        }
    }

    public void PacmanWin()
    {
        GameObject newGameObject = Instantiate(pacmanWinFX, transform.position, Quaternion.identity);
        Destroy(newGameObject, 2f);
    }

    public void SetUp(bool set)
    {
        up = set;
    }
    public void SetDown(bool set)
    {
        down = set;
    }
    public void SetLeft(bool set)
    {
        left = set;
    }
    public void SetRight(bool set)
    {
        right = set;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
/*            if (frightened.enabled)
            {
                FindObjectOfType<GameManager>().GhostEaten(this);
            }
            else
            {*/
                FindObjectOfType<GameManager_MP>().PacmanEaten();
            //}
        }
    }
}
