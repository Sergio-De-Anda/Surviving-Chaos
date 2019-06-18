using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    Weapon wep;

    void Start()
    {
        wep = GameObject.FindGameObjectWithTag("Weapon").GetComponent<Weapon>();
    }
    
    void Update()
    {
        transform.Rotate(Vector3.up * 50f * Time.deltaTime, Space.World);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            wep.ammo.carryingAmmo += 60;
            Destroy(transform.parent.gameObject);
        }

        
    }
}
