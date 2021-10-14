using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostScatter : GhostBehaviour
{
    private void OnEnable()
    {
        if (ghost.changeBehaviour)
            Disable();
    }
    private void OnDisable()
    {
        ghost.chase.Enable();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Node node = collision.GetComponent<Node>();
        if (node != null && enabled && !ghost.frightened.enabled)
        {
            Vector2 direction = Vector2.zero;
            float minDistance = float.MaxValue;

            foreach (Vector2 availableDirection in node.availableDirections)
            {
                if (availableDirection != -ghost.movement.prevDir)
                {
                    Vector3 newPosition = transform.position + new Vector3(availableDirection.x, availableDirection.y, 0f);
                    float distance = (ghost.scatterTarget.position - newPosition).sqrMagnitude;
                    if (distance < minDistance)
                    {
                        direction = availableDirection;
                        minDistance = distance;
                    }
                }
            }
            ghost.movement.SetDirection(direction);
        }
    }
}
