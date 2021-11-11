using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPallet_MP : Pallet_MP
{
    public float duration = 8f;

    protected override void Eat()
    {
        FindObjectOfType<GameManager_MP>().PowerPalletEaten(this);

    }
}
