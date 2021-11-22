using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(Movement_MP))]
public class Ghost_MP : MonoBehaviour
{
    public bool isEaten = false;
    public bool isFrightened = false;
    public bool isAcceptingInput = true;
    private Movement_MP movement;
    public ParticleSystem[] deathFX;
    private CircleCollider2D _circleCollider2D;
    [SerializeField] GameObject powerPalletEatenFX;
    [SerializeField] GameObject pacmanWinFX;
    [SerializeField] GameObject MobileControlPanel;
    private bool up, down, left, right = false;
    public SpriteRenderer Body, Eyes, Blue, White;
    [SerializeField] GameObject ghostEatenFX;
    public Vector3 insidePosition, outsidePosition;
    [SerializeField] GameObject ghostFake;
    [SerializeField] GameObject arrow;
    public float fakeDuration;
    private GameObject fake;
    private PhotonView view;
    private float speedBoost = 1;
    [SerializeField] PowerUse power;

    private void Awake()
    {
        movement = GetComponent<Movement_MP>();
        _circleCollider2D = GetComponent<CircleCollider2D>();
    }
    private void Start()
    {
        view = GetComponent<PhotonView>();
        if (view.IsMine)
        {
            MobileControlPanel.SetActive(true);
            power.gameObject.SetActive(true);

        }
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
            //POWERRRRR
            if (Input.GetKeyDown(KeyCode.Space))
                UsePower();
        }
        
    }

    public void UsePower()
    {
        if (!power.isCoolDown && isAcceptingInput)
        {
            power.UsePower();
            arrow.SetActive(true);
            fake = PhotonNetwork.Instantiate(ghostFake.name, transform.position,Quaternion.identity);
            fake.GetComponent<Movement>().SetDirection(movement.direction);
            Invoke(nameof(DestroyFake), fakeDuration);
        }
    }
    void DestroyFake()
    {
        arrow.SetActive(false);
        if(fake != null)
            PhotonNetwork.Destroy(fake);
    }
    public void ResetState()
    {
        _circleCollider2D.enabled = true;
        gameObject.SetActive(true);
        movement.ResetState();
        movement.speedMultipliter = 1f * speedBoost;
        power.ResetCooldown();
        CancelInvoke();
        DestroyFake();
    }

    public void PacmanDied()
    {
        _circleCollider2D.enabled = false;
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
            if (!isFrightened)
                FindObjectOfType<GameManager_MP>().PacmanEaten();
            else
            {
                FindObjectOfType<GameManager_MP>().GhostEaten();
                Eaten();
                speedBoost += 0.2f;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isEaten && collision.gameObject.layer == LayerMask.NameToLayer("Outside"))
        {
            isAcceptingInput = false;
            StartCoroutine(EnterHome());
        }
    }
    private IEnumerator EnterHome()
    {
        isFrightened = false;
        isEaten = false;
        movement.speedMultipliter = 1f*speedBoost;
        movement.SetDirection(Vector2.down, true);
        movement.rgbody.isKinematic = true;
        movement.enabled = false;

        Vector3 position = transform.position;

        float duration = 0.5f;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            Vector3 newPosition = Vector3.Lerp(position, insidePosition, elapsed / duration);
            newPosition.z = position.z;
            transform.position = newPosition;
            elapsed += Time.deltaTime;
            yield return null;
        }
        Body.enabled = true;
        StartCoroutine(ExitTransition());
    }

    private IEnumerator ExitTransition()
    {
        movement.SetDirection(Vector2.up, true);
        movement.rgbody.isKinematic = true;
        movement.enabled = false;

        Vector3 position = transform.position;

        float duration = 0.5f;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            Vector3 newPosition = Vector3.Lerp(insidePosition, outsidePosition, elapsed / duration);
            newPosition.z = position.z;
            transform.position = newPosition;
            elapsed += Time.deltaTime;
            yield return null;
        }
        movement.rgbody.isKinematic = false;
        movement.enabled = true;
        if (GameObject.FindGameObjectWithTag("Player"))
            Physics2D.IgnoreCollision(gameObject.GetComponent<CircleCollider2D>(), GameObject.FindWithTag("Player").GetComponent<CircleCollider2D>(), false);
        isAcceptingInput = true;
    }
    public void GhostFrightened(float duration)
    {
        movement.speedMultipliter = 0.75f;
        isFrightened = true;
        Blue.enabled = true;
        White.enabled = false;
        Body.enabled = false;
        Eyes.enabled = false;
        CancelInvoke(nameof(SetWhite));
        CancelInvoke(nameof(DisableFrightened));
        Invoke(nameof(SetWhite), duration / 3.0f);
        Invoke(nameof(DisableFrightened), duration);
    }

    private void SetWhite()
    {
        if(!isEaten)
        {
            Blue.enabled = false;
            White.enabled = true;
            White.GetComponent<AnimateSprite>().Restart();
        }
    }

    private void DisableFrightened()
    {
        movement.speedMultipliter = 1f*speedBoost;
        isFrightened = false;
        Blue.enabled = false;
        White.enabled = false;
        Body.enabled = true;
        Eyes.enabled = true;
    }

    private void Eaten()
    {
        CancelInvoke(nameof(SetWhite));
        CancelInvoke(nameof(DisableFrightened));
        isEaten = true;
        Physics2D.IgnoreCollision(gameObject.GetComponent<CircleCollider2D>(), GameObject.FindWithTag("Player").GetComponent<CircleCollider2D>());
        GhostEatenFX();
        movement.speedMultipliter = 2f;
        Blue.enabled = false;
        White.enabled = false;
        Body.enabled = false;
        Eyes.enabled = true;
    }

    private void GhostEatenFX()
    {
        GameObject newGameObject = Instantiate(ghostEatenFX, transform.position, Quaternion.identity);
        Destroy(newGameObject, 0.3f);

    }
}
