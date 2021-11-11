using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GhostEyes : MonoBehaviour
{
    public Sprite up;
    public Sprite down;
    public Sprite left;
    public Sprite right;
    private SpriteRenderer spriteRenderer;
    private Movement movement;
    private Scene scene;

    private void Awake()
    {
        scene = SceneManager.GetActiveScene();
        spriteRenderer = GetComponent<SpriteRenderer>();
        movement = GetComponentInParent<Movement>();
    }
    private void Update()
    {
        if (scene.name == "GAMEOVER_LV1") spriteRenderer.sprite = left;
        else
        {
            if (movement.direction == Vector2.up)
                ChangeSprite(up);
            else if (movement.direction == Vector2.down)
                ChangeSprite(down);
            else if (movement.direction == Vector2.left)
                ChangeSprite(left);
            else
                ChangeSprite(right);
        }
    }
    void ChangeSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }
}
