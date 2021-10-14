using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimateImage : MonoBehaviour
{
    [HideInInspector] public Image image;
    public Sprite[] sprites;
    public float animationTime = 0.25f;
    [HideInInspector] public int animationFrame;
    public bool loop = true;
    private void Awake()
    {
        image = GetComponent<Image>();
    }
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(Advance), animationTime, animationTime);
    }

    private void Advance()
    {
        if (!image.enabled)
        {
            enabled = false;
            return;

        }
        animationFrame++;
        if (animationFrame >= sprites.Length && loop)
        {
            animationFrame = 0;
        }
        if (animationFrame >= 0 && animationFrame < sprites.Length)
        {
            image.sprite = sprites[animationFrame];
        }
    }

    public void Restart()
    {
        animationFrame = -1;
        Advance();
    }
}
