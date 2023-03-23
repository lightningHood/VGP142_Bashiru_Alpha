using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyTurret : MonoBehaviour
{
    public Transform target;
    NavMeshAgent agent;
    Animator anim;
    public CanvasManager CM;

    public enum TurretEnemyState
    {
        Chase, Shoot, Dead
    }
    public TurretEnemyState currentState = TurretEnemyState.Chase;
    float dist;
    public float howClose;
    public Transform Head, Barrel;
    public GameObject Projectile1;
    public float projectileSpeed;
    public float fireRate, nextFire;


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
      
        if (currentState == TurretEnemyState.Chase)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;

            if (target)
                agent.SetDestination(target.position);
        }

        if (currentState == TurretEnemyState.Shoot)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }


    }

    // Update is called once per frame
    void Update()
    {
        if (currentState == TurretEnemyState.Dead)
        {
            agent.Stop();
        }
        if (currentState == TurretEnemyState.Shoot)
        {
            dist = Vector3.Distance(target.position, transform.position);
            if (dist <= howClose)
            {
                Head.LookAt(target);
                if (Time.time >= nextFire)
                {
                    nextFire = Time.time + 1f / fireRate;
                    Shoot();
                }
            }
            

         
        }

        if (target)
        {
            currentState = TurretEnemyState.Shoot;
        }



        if (currentState == TurretEnemyState.Chase)
        {
            agent.isStopped = false;
        }

        if (target)
            agent.SetDestination(target.position);


    }
    public void Die()
    {
        anim.SetTrigger("IsDeath");
        currentState = TurretEnemyState.Dead;
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

    void Shoot()
    {
      GameObject clone = Instantiate(Projectile1, Barrel.position, transform.rotation);
        clone.GetComponent<Rigidbody>().AddForce(transform.forward * projectileSpeed);
        //force Foward
    }

}