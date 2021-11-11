using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(Movement_MP))]
public class Pacman_MP : MonoBehaviour
{
    public bool isAcceptingInput = false;
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

    }
    private void Start()
    {
        movement = GetComponent<Movement_MP>();
        _circleCollider2D = GetComponent<CircleCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        view = GetComponent<PhotonView>();
        if (view.IsMine)
            MobileControlPanel.SetActive(true);
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager_MP>().pacman = this;
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
                float angle = Mathf.Atan2(movement.direction.y, movement.direction.x);
                transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);
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
    public void PowerPalletEatenFX()
    {
        GameObject newGameObject = Instantiate(powerPalletEatenFX, transform.position, Quaternion.identity);
        Destroy(newGameObject, 1f);
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


}
