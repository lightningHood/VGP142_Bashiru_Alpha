using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Transform target;
    NavMeshAgent agent;
    Animator anim;
    public CanvasManager CM;

    public enum EnemyState
    {
        Chase, Patrol, Dead
    }
    public EnemyState currentState = EnemyState.Patrol;
    public GameObject[] path;
    public int pathIndex;
    public float distThreshhold;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        if (path.Length <= 0)
        {
            path = GameObject.FindGameObjectsWithTag("Patrol");
        }
        if (currentState == EnemyState.Chase)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;

            if (target)
                agent.SetDestination(target.position);
        }
        if (distThreshhold <= 0)
        {
            distThreshhold = 0.5f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState == EnemyState.Dead)
        {
            agent.Stop();
        }
            if (currentState == EnemyState.Patrol)
        {
            if (target)
                Debug.DrawLine(transform.position, target.position, Color.red);

            if (agent.remainingDistance < distThreshhold)
            {
                pathIndex++;
                pathIndex %= path.Length;

                target = path[pathIndex].transform;
            }
        }



        if (currentState == EnemyState.Chase)
        {
                target = GameObject.FindGameObjectWithTag("Player").transform;
        }

        if (target)
            agent.SetDestination(target.position);

        
    }
     public void Die()
    {
        anim.SetTrigger("IsDeath");
        currentState = EnemyState.Dead;
        Destroy(gameObject, 3.0f);
        GetComponent<CapsuleCollider>().enabled = false;
        CM.Win();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Projectile"))
        {
            Die();
        }
    }

}
