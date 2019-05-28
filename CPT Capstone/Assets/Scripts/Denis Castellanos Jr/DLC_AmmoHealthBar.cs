using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DLC_AmmoHealthBar : MonoBehaviour
{
    private Transform ammoBar;

    private void Start()
    {
        ammoBar = transform.Find("Bar");
    }

    public void SetSize(float sizeNormalized)
    {
        ammoBar.localScale = new Vector3(sizeNormalized, 1.0f);
    }

}