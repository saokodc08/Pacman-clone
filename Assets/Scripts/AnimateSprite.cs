using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AnimateSprite : MonoBehaviour
{
    [HideInInspector]public SpriteRenderer spriteRenderer;
    public Sprite[] sprites;
    public float animationTime = 0.25f;
    [HideInInspector]public int animationFrame;
    public bool loop = true;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(Advance), animationTime, animationTime);
    }

    private void Advance()
    {
        if (!spriteRenderer.enabled)
            return;
        animationFrame++;
        if(animationFrame >= sprites.Length && loop)
        {
            animationFrame = 0;
        }
        if(animationFrame>=0 && animationFrame < sprites.Length)
        {
            spriteRenderer.sprite = sprites[animationFrame];
        }
    }

    public void Restart()
    {
        animationFrame = -1;
        Advance();
    }

}
