using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    PlayerHealth playerHealth;
    // Use this for initialization
    void Start()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * 50f * Time.deltaTime, Space.World);
    }

    void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            if (playerHealth.playerHealth == 100f)
                return;
            playerHealth.playerHealth += 40f;
            if (playerHealth.playerHealth > 100f)
                playerHealth.playerHealth = 100f;
            Destroy(transform.parent.gameObject);
        }
       
    }
}
