using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBoo : MonoBehaviour
{
    public Transform target;
    NavMeshAgent agent;
    Animator anim;
    public CanvasManager CM;

    public enum booEnemyState
    {
        Chase, Freeze, Dead
    }
    public booEnemyState currentState = booEnemyState.Chase;
    

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        
        
        if (currentState == booEnemyState.Chase)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;

            if (target)
                agent.SetDestination(target.position);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState == booEnemyState.Dead)
        {
            agent.isStopped = true;
        }
        if (currentState == booEnemyState.Freeze)
        {
            agent.isStopped = true;


            if (target)
                Debug.DrawLine(transform.position, target.position, Color.red);
            

            
        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position, target.position, out hit, 20))
        {
            if (hit.transform.gameObject.CompareTag("Player")) {
                Vector3 playerDirection = target.transform.forward;
                Vector3 enemyDirection = transform.forward;
                float faceAngle = Vector3.Angle(playerDirection, enemyDirection);
                if (faceAngle < 90.0f)
                {
                  currentState = booEnemyState.Chase;
                }
                else
                {
                    currentState = booEnemyState.Freeze;
                }



            }
               
        }

        if (currentState == booEnemyState.Chase)
        {
            agent.isStopped = false;
        }

        if (target)
            agent.SetDestination(target.position);


    }
    public void Die()
    {
        anim.SetTrigger("IsDeath");
        currentState = booEnemyState.Dead;
        Destroy(gameObject, 3.0f);
        GetComponent<CapsuleCollider>().enabled = false;

        if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            CM.Win();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))
        {
            Die();
        }
    }

}