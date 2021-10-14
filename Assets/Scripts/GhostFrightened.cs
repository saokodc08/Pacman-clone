using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostFrightened : GhostBehaviour
{
    [SerializeField] private GameObject ghostEatenFX;
    public SpriteRenderer body;
    public SpriteRenderer eyes;
    public SpriteRenderer blue;
    public SpriteRenderer white;

    //private Vector2 prevDir;

    public bool eaten { get; private set; }
    public override void Enable(float duration)
    {
        base.Enable(duration);
        body.enabled = false;
        eyes.enabled = false;
        blue.enabled = true;
        white.enabled = false;

        Invoke(nameof(Flash), duration / 2.0f);
    }

    public override void Disable()
    {
        base.Disable();
        body.enabled = true;
        eyes.enabled = true;
        blue.enabled = false;
        white.enabled = false;
    }
    private void Flash()
    {
        if (!eaten)
        {
            blue.enabled = false;
            white.enabled = true;
            white.GetComponent<AnimateSprite>().Restart();
        }
    }

    private void OnEnable()
    {
        ghost.movement.speedMultipliter = 0.5f;
        ghost.movement.SetDirection(-ghost.movement.direction);

    }

    private void OnDisable()
    {
        ghost.movement.speedMultipliter = 1f;
        eaten = false;
    }
    private void Eaten()
    {
        eaten = true;
        GhostEatenFX();
        Physics2D.IgnoreCollision(gameObject.GetComponent<CircleCollider2D>(),GameObject.FindWithTag("Player").GetComponent<CircleCollider2D>());
        CancelInvoke();
        ghost.movement.speedMultipliter = 2f;
        ///////////////////////////////////////////
        body.enabled = false;
        eyes.enabled = true;
        blue.enabled = false;
        white.enabled = false;
    }
    private void GhostEatenFX()
    { 
         GameObject newGameObject = Instantiate(ghostEatenFX, transform.position, Quaternion.identity);
         Destroy(newGameObject, 0.3f);

    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            if (enabled)
            {
                Eaten();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Node node = collision.GetComponent<Node>();
        if(enabled)
        {
            if (node != null)
            {
                Vector2 direction = Vector2.zero;
                if (!eaten)
                {
                    int index = Random.Range(0, node.availableDirections.Count);
                    if (node.availableDirections[index] == -ghost.movement.direction && node.availableDirections.Count > 1)
                    {
                        index++;
                        if (index >= node.availableDirections.Count)
                            index = 0;
                    }
                    ghost.movement.SetDirection(node.availableDirections[index]);

                }
                else
                {
                    float minDistance = float.MaxValue;

                    foreach (Vector2 availableDirection in node.availableDirections)
                    {
                        if(availableDirection != -ghost.movement.prevDir)
                        {
                            Vector3 newPosition = transform.position + new Vector3(availableDirection.x, availableDirection.y, 0f);
                            float distance = (ghost.homeTarget.position - newPosition).sqrMagnitude;
                            if (distance < minDistance)
                            {
                                direction = availableDirection;
                                minDistance = distance;
                            }
                        }
                    }
                    ghost.movement.SetDirection(direction);                  
                }
                //prevDir = direction;
            }
            else
            {
                if (collision.gameObject.layer == LayerMask.NameToLayer("Outside") && eaten)
                {
                    if (gameObject.activeSelf)
                        StartCoroutine(ExitTransition());

                }
            }
        }       
    }

    private IEnumerator ExitTransition()
    {
        ghost.movement.SetDirection(Vector2.down, true);
        ghost.movement.rigidbody2D.isKinematic = true;
        ghost.movement.enabled = false;

        Vector3 position = transform.position;

        float duration = 0.5f;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            Vector3 newPosition = Vector3.Lerp(position, ghost.home.inside.position, elapsed / duration);
            newPosition.z = position.z;
            ghost.transform.position = newPosition;
            elapsed += Time.deltaTime;
            yield return null;
        }
        ghost.movement.rigidbody2D.isKinematic = false;
        ghost.movement.enabled = true;
        if(GameObject.FindGameObjectWithTag("Player"))
            Physics2D.IgnoreCollision(gameObject.GetComponent<CircleCollider2D>(), GameObject.FindWithTag("Player").GetComponent<CircleCollider2D>(),false);
        ghost.home.Enable(1f);
        Disable();
    }
}
