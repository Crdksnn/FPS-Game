using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulltPickup : MonoBehaviour
{
    private bool collected;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && !collected)
        {
            //Give ammo
            PlayerController.instance.activeGun.GetAmmo();
            Destroy(gameObject);
            collected = true;
        }
    }
}
