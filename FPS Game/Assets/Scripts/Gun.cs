using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{

    public GameObject bullet;
    public Transform firePoint;
    public bool canAutoFire;
    public float fireRate;
    [HideInInspector]
    public float fireCounter;
    public int currentAmmo;
    public int pickupAmount;
    public float zoomAmount;
    public string gunName;

    void Start()
    {
       
    }

    
    void Update()
    {
        if(fireCounter > 0)
        {
            fireCounter -= Time.deltaTime;
        }
    }


    public void GetAmmo()
    {
        currentAmmo += pickupAmount;

        if (UIController.instance != null)
        {
            UIController.instance.ammoText.text = "AMMO: " + currentAmmo;
        }
    }

}
