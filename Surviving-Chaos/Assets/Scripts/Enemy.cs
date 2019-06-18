using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    Leaderboard leader;
    Animator animator;
    public float Health;
    public float distanceToAttack;
    NavMeshAgent enemy;
    NavMeshAgent player;
    SpawnEnemies spawn;
    bool dead;

    // Use this for initialization
    void Start ()
    {
        dead = false;
        animator = GetComponent<Animator>();
        enemy = GetComponent<NavMeshAgent>();
        animator.SetBool("Walking", true);
        player = GameObject.FindGameObjectWithTag("Player Position").GetComponent<NavMeshAgent>();
        spawn = GameObject.FindGameObjectWithTag("Respawn").GetComponent<SpawnEnemies>();
        leader = GameObject.FindGameObjectWithTag("Leaderboard").GetComponent<Leaderboard>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        follow_player();
    }
    
    void ApplyDamage(float damage)
    {
        if (!dead)
        {
            leader.enemyShot();
            animator.SetBool("Hit", true);
            damage = Random.Range(15f, damage);
            Health -= damage;
            StartCoroutine(hitReaction());
            if (Health <= 0)
                Dead();
        }
    }

    IEnumerator hitReaction()
    {
        
        yield return new WaitForSeconds(1f);
        animator.SetBool("Hit", false);
    }

    void follow_player()
    {
        transform.LookAt(player.transform);
        //Debug.Log("Hit " + animator.GetBool("Hit"));
        if (!animator.GetBool("Hit") && !animator.GetBool("Dead"))
        {
            enemy.SetDestination(player.transform.position);
            float dis = Vector3.Distance(transform.position, player.transform.position);

            if (dis <= distanceToAttack)
                animator.SetBool("Attack", true);
            else if (!animator.GetBool("Dead"))
                animator.SetBool("Attack", false);
        }
        else
            enemy.SetDestination(enemy.transform.position);
    }

    void Dead()
    {
        leader.enemyKill();
        print("Enemy Dead");
        animator.SetBool("Dead", true);
        Destroy(this.gameObject, 3.0f);
        spawn.decreaseEnemies();
        dead = true;

    }
}
