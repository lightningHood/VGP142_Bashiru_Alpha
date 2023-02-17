using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class Character : MonoBehaviour
{
    public float moveSpeed = 10.0f;
    public float gravity = 9.81f;
    public float jumpSpeed = 500.0f;

    Rigidbody rb;
    Controller inputController;
    Animator anim;

    public CanvasManager CM;

    Vector3 curMoveInput;
    Plane plane = new Plane(Vector3.up, 0);
    Vector3 moveDir;

    public LayerMask isGroundLayer;
    public float groundCheckRadius;
    public Transform groundCheck;
    public bool isGrounded;

    public Rigidbody projectilePrefab;
    public Transform spawnPoint;
    public int projectileSpeed = 5;

    public float lookthreshold = 0.6f;

    public GameObject PauseMenu;

    int errorCounter = 0;
    // Start is called before the first frame update
    void Start()
    {
        try
        {
            rb = GetComponent<Rigidbody>();
            inputController = GetComponent<Controller>();
            anim = GetComponent<Animator>();

            if (moveSpeed <= 0.0f)
            {
                moveSpeed = 10.0f;
                throw new UnassignedReferenceException("Speed not set on " + name + "defaulting to " + moveSpeed.ToString());
            }
        }
        catch (NullReferenceException e)
        {
            Debug.Log(e.Message);
            errorCounter++;
        }
        catch (UnassignedReferenceException e)
        {
            Debug.Log(e.Message);
            errorCounter++;
        }
        finally
        {
            Debug.Log("The script ran with " + errorCounter.ToString() + "errors.");
        }

        PauseMenu.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        Collider[] col = Physics.OverlapSphere(groundCheck.position, groundCheckRadius, isGroundLayer);
        isGrounded = (col.Length > 0);

        curMoveInput.y = rb.velocity.y;
        rb.velocity = curMoveInput;



        if (Mathf.Abs(moveDir.magnitude) > 0)
        {
            float dotProduct = Vector3.Dot(moveDir, transform.forward);

            if (dotProduct >= -lookthreshold && dotProduct <= lookthreshold)
            {
                float horizontalDotProduct = Vector3.Dot(Vector3.left, moveDir);
                float horizontalValue = (horizontalDotProduct > 0) ? -1 : 1;
                anim.SetFloat("horizontal", horizontalValue);
                anim.SetFloat("vertical", 0);
            }

            if (dotProduct >= lookthreshold)
            {
                anim.SetFloat("horizontal", 0);
                anim.SetFloat("vertical", 1);
            }


            if (dotProduct <= -lookthreshold)
            {
                anim.SetFloat("horizontal", 0);
                anim.SetFloat("vertical", -1);
            }

            
        }
        else 
        {
            anim.SetFloat("horizontal", 0);
            anim.SetFloat("vertical", 0);
        }

        float distance;
        Ray mouseRay = inputController.MousePos();
        Vector3 worldPos;

        if (plane.Raycast(mouseRay, out distance))
        {
            worldPos = mouseRay.GetPoint(distance);

            Vector3 relativePos = worldPos - transform.position;

            relativePos.y = 0;

            transform.rotation = Quaternion.LookRotation(relativePos);
        }


        RaycastHit hit;
        if (Physics.Raycast(spawnPoint.position, transform.forward, out hit, 20))
        {
            if (hit.transform.gameObject.CompareTag("Pickup"))
                Debug.Log("Pickup Hit by Raycast");

            if (hit.transform.gameObject.CompareTag("Enemy"))
                Debug.Log("Enemy Hit by Raycast");
        }

        Debug.DrawRay(spawnPoint.position, transform.forward * 20, Color.red);

        

    }

    public void MovePlayer(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            curMoveInput = Vector3.zero;
            moveDir = Vector3.zero;
            return;
        }

        Vector2 move = context.action.ReadValue<Vector2>();
        move.Normalize();

        moveDir = new Vector3(move.x, 0, move.y).normalized;

        curMoveInput = moveDir * moveSpeed;
    }

    public void Fire(InputAction.CallbackContext context)
    {
        if (context.action.WasPressedThisFrame())
        {
            Rigidbody temp = Instantiate(projectilePrefab, spawnPoint.position, spawnPoint.rotation);

            temp.AddForce(transform.forward * projectileSpeed);
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            rb.AddForce(jumpSpeed * Vector3.up);
                anim.SetTrigger("IsJumping");
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pickup"))
        {
            Debug.Log("Collided with: " + other.name);
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Collided with: " + other.name);
            Die(other.gameObject);
        }
    }
    public void Punch(InputAction.CallbackContext context)
    {
        anim.SetTrigger("IsPunching");
       

    }

    public void Kick(InputAction.CallbackContext context)
    {
        anim.SetTrigger("IsKicking");
        

    }

    public void Die(GameObject diedBy)
    {
        anim.SetTrigger("IsDeath");
        Destroy(gameObject, 3.0f);
        diedBy.GetComponent<Enemy>().currentState = Enemy.EnemyState.Patrol;
        CM.GameOver();
    }

    public void Pause(InputAction.CallbackContext context)
    {
        PauseMenu.SetActive(true);
        Time.timeScale = 0.0f;
        this.enabled = false;
    }



}
