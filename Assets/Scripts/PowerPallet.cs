using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPallet : Pallet
{
    public float duration = 8f;

    protected override void Eat()
    {
        FindObjectOfType<GameManager>().PowerPalletEaten(this);

    }
}
