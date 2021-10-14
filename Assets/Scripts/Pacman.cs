using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement))]
public class Pacman : MonoBehaviour
{
    public bool isAcceptingInput = false;
    private Movement movement;
    public ParticleSystem[] deathFX;
    private CircleCollider2D _circleCollider2D;
    private SpriteRenderer _spriteRenderer;
    [SerializeField] GameObject powerPalletEatenFX;
    [SerializeField] GameObject pacmanWinFX;
    private bool up, down, left, right = false;

    private void Awake()
    {
        movement = GetComponent<Movement>();
        _circleCollider2D = GetComponent<CircleCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        if(isAcceptingInput)
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
