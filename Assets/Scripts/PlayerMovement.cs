using System;
using Fusion;
using Fusion.Addons.SimpleKCC;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private float lookSensitivity = 0.15f;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 10f;
    
    [Networked] private NetworkButtons PreviousButtons { get; set; }

    private Transform cachedTransform;
    
    private PlayerObject playerObj;
    
    public SimpleKCC simpleKCC { get; protected set; }

    private void Awake()
    {
        playerObj = GetComponent<PlayerObject>();
    }

    public override void Spawned()
    {
        simpleKCC = GetComponent<SimpleKCC>();
        
        cachedTransform = transform;

        simpleKCC.Collider.tag = "Player";
        simpleKCC.SetGravity(Physics.gravity.y * 2f);

        if (HasInputAuthority)
        {
            // Set the camera target to the player
            CameraFollow.Instance.SetTarget(cachedTransform);
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (!GetInput(out PlayerInputData input)) return;
        float jump = 0f;

        if (input.Buttons.WasPressed(PreviousButtons, InputButton.Jump) && simpleKCC.IsGrounded)
            jump = jumpForce;
            
        Vector3 moveDirection = new Vector3(input.Direction.x, 0, input.Direction.y);
        var velocity = moveDirection * speed * Runner.DeltaTime;

        PreviousButtons = input.Buttons;
        simpleKCC.Move(velocity, jump);

        // Rotate the player to face the movement direction
        if (velocity.x == 0 && velocity.z == 0) return;
        Vector3 lookDir = new Vector3(-velocity.x, 0, -velocity.z);
        Quaternion targetRot = Quaternion.LookRotation(lookDir);
        simpleKCC.SetLookRotation(Quaternion.Slerp(cachedTransform.rotation, targetRot, 0.2f));
    }
}