using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{

    public float speed = 8f;
    public float speedMultipliter = 1f;
    public Vector2 initialDirection;
    public LayerMask obstacleLayer;
    public Vector2 direction { get; private set; }
    public Vector2 prevDir { get; private set; }
    [HideInInspector] public Rigidbody2D rigidbody2D;
    [HideInInspector] public Vector2 nextDirection;
    [HideInInspector] public Vector3 startingPosition;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        startingPosition = transform.position;
    }

    private void Start()
    {
        //ResetState();
    }

    public void ResetState()
    {
        speedMultipliter = 1f;
        direction = initialDirection;
        nextDirection = Vector2.zero;
        transform.position = startingPosition;
        rigidbody2D.isKinematic = false;
        enabled = true;
    }
    private void Update()
    {
        if(nextDirection != Vector2.zero)
        {
            SetDirection(nextDirection);
        }
    }
    private void FixedUpdate()
    {
        Vector2 position = rigidbody2D.position;
        Vector2 translation = direction * speed * speedMultipliter * Time.fixedDeltaTime;
        rigidbody2D.MovePosition(position+translation);
    }

    public void SetDirection(Vector2 direction, bool forced = false)
    {
        if(forced || !Occupied(direction))
        {
            this.direction = direction;
            nextDirection = Vector2.zero;
        }
        else
        {
            nextDirection = direction;
        }
        prevDir = direction;
    }
    public bool Occupied(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one * 0.75f, 0.0f, direction, 1.5f, obstacleLayer);
        return hit.collider != null;
    }



}
