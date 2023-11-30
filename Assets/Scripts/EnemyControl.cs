using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public enum EnemyState
{
    Patrol,
    Chase,
    Attack,
    Death
}

public class EnemyControl : MonoBehaviour
{
    [SerializeField] private Transform player;

    [SerializeField] private Transform[] roamWaypoints;
    [SerializeField] private GameObject healDrop;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private GameObject punchHolder1;
    [SerializeField] private GameObject punchHolder2;

    private NavMeshAgent navMeshAgent;
    private Animator animator;

    private float health = 100f;
    private float lastAttackTime = 0f;
    private int currentWaypointIndex = 0;

    private EnemyState currentState = EnemyState.Patrol;
    [SerializeField] private float attackCooldownTimer = 2;

    void Start()
    {
        health = 100f;
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (player == null)
        { 
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        SetState(EnemyState.Patrol);
        healthSlider.value = health;
    }

    void Update()
    {
        UpdateState();
        if(Vector3.Distance(transform.position, player.position) < 1.5f)
        {
            navMeshAgent.SetDestination(transform.position);
            navMeshAgent.isStopped = true;
        }
    }

    void UpdateState()
    {
        switch (currentState)
        {
            case EnemyState.Patrol:
                Patrol();
                break;

            case EnemyState.Chase:
                Chase();
                break;

            case EnemyState.Attack:
                Attack();
                break;

            case EnemyState.Death:
                Death();
                break;
        }

        healthSlider.value = health;
    }

    void SetState(EnemyState newState)
    {
        currentState = newState;

        // Additional logic can be added here when transitioning between states
    }

    void Patrol()
    {
        if (roamWaypoints.Length == 0)
        {
            Debug.LogWarning("No roam waypoints defined for the enemy!");
            return;
        }

        // Set the destination to the current waypoint
        navMeshAgent.SetDestination(roamWaypoints[currentWaypointIndex].position);
        animator.SetBool("IsChasing", false);

        // Check if the enemy has reached the current waypoint
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            currentWaypointIndex = Random.Range(0, roamWaypoints.Length);
        }

        // Transition to Chase state if player is within a certain range
        if (Vector3.Distance(transform.position, player.position) < 10f)
        {
            SetState(EnemyState.Chase);
        }
    }

    void Chase()
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.SetDestination(player.position);
        animator.SetBool("IsChasing", true);

        // Transition to Attack state if player is within attack range
        if (Vector3.Distance(transform.position, player.position) < 2f)
        {
            SetState(EnemyState.Attack);
        }

       
        if (IsDead())
        {
            SetState(EnemyState.Death);
            return;
        }

        // Transition back to Patrol state if player is out of chase range
        if (Vector3.Distance(transform.position, player.position) > 15f)
        {
            SetState(EnemyState.Patrol);
        }

        punchHolder1.SetActive(false);
        punchHolder2.SetActive(false);
    }

    void Attack()
    {
        //transform.LookAt(player.position);
        navMeshAgent.isStopped = true;
        if (IsDead())
        {
            SetState(EnemyState.Death);
            return;
        }

        else if (Vector3.Distance(transform.position, player.position) > 2f)
        {
            SetState(EnemyState.Chase);
            return;
        }
        else
        {
            if(Time.time - lastAttackTime > attackCooldownTimer)
            {
                punchHolder1.SetActive(false); punchHolder2.SetActive(false);
                animator.SetBool("IsChasing", false);
                Punch();
                lastAttackTime = Time.time;
            }
        }

    }

    void Death()
    {
        animator.Play("Death");
        Invoke(nameof(SpawnHeal), 1.9f);
        Destroy(gameObject,2);
        
    }

    bool IsDead()
    {
        if (health <= 0f) return true;
        else return false;
    }

    private void Punch()
    {
        if(Random.Range(0,2) != 0)
        {
            punchHolder1.SetActive(true);
            animator.Play("PunchLeft");
        }
        else
        {
            punchHolder2.SetActive(true);
            animator.Play("PunchRight");
        }
    }

    public void TakeDamage(float amount)
    {
        animator.Play("GetHit");
        health -= amount;
    }

    void SpawnHeal()
    {
        Instantiate(healDrop, transform.position + new Vector3(0,1.5f,0), Quaternion.identity);
    }
}

