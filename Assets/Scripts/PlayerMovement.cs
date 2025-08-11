using System.Collections;
using System.Collections.Generic;
using Fusion;
using Fusion.Addons.SimpleKCC;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private SimpleKCC kcc;
    // [SerializeField] private MeshRenderer[] modelParts;
    // [SerializeField] private Transform camTarget;
    [SerializeField] private float lookSensitivity = 0.15f;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 10f;

    [Networked] private NetworkButtons PreviousButtons { get; set; }

    public override void Spawned()
    {
        kcc.SetGravity(Physics.gravity.y * 2f);
    
        if (HasInputAuthority)
        {
            // foreach (var model in modelParts)
            // {
            //     model.shadowCastingMode = ShadowCastingMode.ShadowsOnly;
            // }
            // CameraFollow.Singleton.SetTarget(camTarget);
        }
    }
    
    public override void FixedUpdateNetwork()
    {
        if (GetInput(out PlayerInputData input))
        {
            kcc.AddLookRotation(input.LookDelta * lookSensitivity);
            UpdateCamTarget();
            Vector3 moveDirection = new Vector3(input.Direction.x, 0, input.Direction.y);
            Vector3 worldDirection = kcc.TransformRotation * moveDirection;
            float jump = 0f;
    
            if (input.Buttons.WasPressed(PreviousButtons, InputButton.Jump) && kcc.IsGrounded)
                jump = jumpForce;
    
            kcc.Move(worldDirection.normalized * speed, jump);
            PreviousButtons = input.Buttons;
        }
    }
    
    public override void Render()
    {
        UpdateCamTarget();
    }
    
    private void UpdateCamTarget()
    {
        // camTarget.localRotation = Quaternion.Euler(kcc.GetLookRotation().x, 0f, 0f);
    }
}
