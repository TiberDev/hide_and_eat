using Fusion;
using Fusion.Addons.SimpleKCC;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private SimpleKCC kcc;

    // [SerializeField] private MeshRenderer[] modelParts;
    // [SerializeField] private Transform camTarget;
    [SerializeField] private float lookSensitivity = 0.15f;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 10f;

    [Networked] private NetworkButtons PreviousButtons { get; set; }

    private Transform cachedTransform;

    public override void Spawned()
    {
        cachedTransform = transform;

        kcc.Collider.tag = "Player";
        // kcc.SetGravity(Physics.gravity.y * 2f);

        if (HasInputAuthority)
        {
            // Set the camera target to the player
            CameraFollow.Instance.SetTarget(cachedTransform);
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out PlayerInputData input))
        {
            // Vector3 moveDirection = new Vector3(input.Direction.x, 0, input.Direction.y);
            //
            // // Get the camera a forward direction
            // Vector3 camForward = CameraFollow.Instance.Transform.forward;
            // camForward.y = 0;
            // camForward.Normalize();
            //
            // // Get the camera a right direction
            // Vector3 camRight = CameraFollow.Instance.Transform.right;
            // camRight.y = 0;
            // camRight.Normalize();
            //
            // Vector3 moveDir = camForward * moveDirection.z + camRight * moveDirection.x;
            // moveDir.Normalize();
            float jump = 0f;

            if (input.Buttons.WasPressed(PreviousButtons, InputButton.Jump) && kcc.IsGrounded)
                jump = jumpForce;
            
            Vector3 moveDirection = new Vector3(input.Direction.x, 0, input.Direction.y);
            var velocity = moveDirection * speed;

            PreviousButtons = input.Buttons;

            if (Object.HasStateAuthority)
            {
                kcc.Move(velocity, jump);
                Debug.Log(
                    $"Instance ID: {GetInstanceID()} velocity: {velocity}");
            }

            // // Rotate the player to face the movement direction
            // if (moveDir.x == 0 && moveDir.z == 0) return;
            // Vector3 lookDir = new Vector3(-moveDir.x, 0, -moveDir.z);
            // Debug.Log($"Instance ID: {GetInstanceID()}  Look Direction: {lookDir}");
            // Quaternion targetRot = Quaternion.LookRotation(lookDir);
            // kcc.SetLookRotation(Quaternion.Slerp(cachedTransform.rotation, targetRot, 0.2f));
        }
    }
}