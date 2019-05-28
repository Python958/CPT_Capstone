using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForceField : MonoBehaviour
{
    [HideInInspector]
    public int HP;
    public int MaxHP;
    public float refillRate;
    private float refillAmount;
    public GameObject owner;
    public Slider HealthBar;

    private void Start()
    {
        HP = MaxHP;
        HealthBar.value = HealthPercent();
    }

    private void Update()
    {
        refillAmount += Time.deltaTime * refillRate;
        while (refillAmount >= 1f) { HP++; refillAmount--; }
        HP = Mathf.Min(HP, MaxHP);

        HealthBar.value = HealthPercent();

        if(owner == null) { Destroy(gameObject); }
    }

    private float HealthPercent()
    {
        return ((float)HP / MaxHP);
    }
}
