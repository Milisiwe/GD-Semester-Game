using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class EnemyAI : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent enemy;
    [SerializeField]
    private Transform player;
    [SerializeField]
    private LayerMask ground;
    [SerializeField]
    private LayerMask agent;
    [SerializeField]
    private float health;

    //PATROLLING
    public Vector3 patrol;
    public bool patrolSet = false;
    public float patrolRange;

    //ATTACKING
    public float timeBetweenAttacks;
    public bool attacked;
    public Animator enemyAnim;
    //public GameObject sword;

    //STATES
    public float sightRange;
    public float attackRange;
    public bool playerInSightRange;
    public bool playerInAttackRange;

    //SWORDS
    public GameObject shieldSword;
    public GameObject attackSword;

    //BOOLEAN CHECKS
    public bool isPatrolling = false;
    public bool isAttacking = false;
    public bool isChasingPlayer = false;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        enemy = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //Check if player is in the sight range or the attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, agent);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, agent);

        if (!playerInSightRange && !playerInAttackRange)
        {
            Patrol();
            enemyAnim.SetBool("Attacking", false);
            enemyAnim.SetBool("Idle", false);
            enemyAnim.SetBool("Walking", true);
            attackSword.SetActive(false);
            shieldSword.SetActive(true);
            isPatrolling = true;
        }
        else
        {
            isPatrolling = false;
        }
        if (playerInSightRange && !playerInAttackRange)
        {
            ChasePlayer();
            attackSword.SetActive(false);
            shieldSword.SetActive(true);
            enemyAnim.SetBool("Idle", false);
            enemyAnim.SetBool("Walking", true);
            isChasingPlayer = true;
        }
        else
        {
            isChasingPlayer = false;
        }

        if (playerInSightRange && playerInAttackRange)
        {
            AttackPlayer();
            //Animation for sword attack
            enemyAnim.SetBool("Attacking", true);
            enemyAnim.SetBool("Walking", false);
            attackSword.SetActive(true);
            shieldSword.SetActive(false);
            isAttacking = true;
        }
        else
        {
            enemyAnim.SetBool("Attacking", false);
            enemyAnim.SetBool("Walking", true);
            isAttacking = false;
        }
            
    }

    private void SearchPatrol()
    {
        //Calculate random values for patrol range
        float randomZ = Random.Range(-patrolRange, patrolRange);
        float randomX = Random.Range(-patrolRange, patrolRange);

        patrol = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        //If the values exist on the "map", then set the boolean to true
        if (Physics.Raycast(patrol, -transform.up, 2f, ground))
            patrolSet = true;
    }

    private void Patrol()
    {
        if (!patrolSet)
            SearchPatrol();

        if (patrolSet)
            enemy.SetDestination(patrol);

        //When the enemy reaches the patrol destination, reset the boolean so it can calculate another one
        Vector3 distanceToPatrolPoint = transform.position - patrol;
        if (enemy.remainingDistance < 0.1f)
            patrolSet = false;
        enemyAnim.SetBool("Walking", true);
        enemyAnim.SetBool("Attacking", false);
    }

    private void ChasePlayer()
    {
        enemy.SetDestination(player.transform.position);

    }

    private void ResetAttack()
    {
        attacked = false;
    }
    private void AttackPlayer()
    {
        //Enemy should be stationary and looking at player when attacking
        enemy.SetDestination(transform.position);
        enemyAnim.SetBool("Idle", true);
        //transform.LookAt(player, Vector3.up);
        Vector3 targetDirection = player.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        transform.rotation = Quaternion.Euler(new Vector3(0, targetRotation.eulerAngles.y, 0));


        if (!attacked)
        {
            //code for deduction of points from the player is in Sword/Health script

            attacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }

    }
    //Revisit THIS!!!!!
    /*private void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
            Invoke(nameof(DestroyEnemy), 0.5f);

    }*/

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

}
