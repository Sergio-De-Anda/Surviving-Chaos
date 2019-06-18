using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    
    Weapon wp;
    PlayerHealth playerHealth;
    SpawnEnemies spawn;

	public Text ammoText;
	public Slider healthBar;
    public Text zombieText;

    void Start()
    {
        wp = GameObject.FindGameObjectWithTag("Weapon").GetComponent<Weapon>();
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        spawn = GameObject.FindGameObjectWithTag("Respawn").GetComponent<SpawnEnemies>();
    }


    void Update()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        ammoText.text = wp.ammo.clipAmmo + "/" + wp.ammo.carryingAmmo;
        healthBar.value = playerHealth.playerHealth;
        zombieText.text = spawn.enemiesCounter.ToString();
    }


}
