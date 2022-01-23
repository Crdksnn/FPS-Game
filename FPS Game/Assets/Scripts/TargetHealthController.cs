using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetHealthController : MonoBehaviour
{
    [SerializeField] int health = 50;
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    public void TakeDamage(int damageBullet)
    {
        health -= damageBullet;

        if (health <= 0)
            Destroy(gameObject);
    }
}
