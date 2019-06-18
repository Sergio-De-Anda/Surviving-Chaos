using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    Leaderboard leader;
    public float playerHealth;


    // Use this for initialization
    void Start()
    {
        leader = GameObject.FindGameObjectWithTag("Leaderboard").GetComponent<Leaderboard>();
    }

   
    void OnTriggerEnter(Collider other)
    {
        if (other.name == "EnemyHandRight")
        {
            float damage = Random.Range(5f, 15f);
            playerHealth -= damage;
            if (playerHealth <= 0.0f)
            {
                leader.printScore();
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
            }
        }
    }
}
