using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Character), typeof(WeaponPickup))]
public class Controller : MonoBehaviour
{

    PlayerInputs input;
    Character player;
    WeaponPickup pickup;

    private void Awake()
    {
        input = new PlayerInputs();
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }


    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Character>();
        pickup = GetComponent<WeaponPickup>();
        input.Player.Move.performed += ctx => player.MovePlayer(ctx);
        input.Player.Move.canceled += ctx => player.MovePlayer(ctx);
        input.Player.Jump.performed += ctx => player.Jump(ctx);
        input.Player.Punch.performed += ctx => player.Punch(ctx);
        input.Player.Kick.performed += ctx => player.Kick(ctx);
        input.Player.Pause.performed += ctx => player.Pause(ctx);

        input.Player.Fire.performed += ctx => pickup.Fire(ctx);
        input.Player.Throw.performed += ctx => pickup.Throw(ctx);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public Ray MousePos()
    {
        Vector3 screenSpacePos = input.Player.Look.ReadValue<Vector2>();

        return Camera.main.ScreenPointToRay(screenSpacePos);
    }
}
