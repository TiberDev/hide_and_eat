using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerInputController : SimulationBehaviour, INetworkRunnerCallbacks, IBeforeUpdate
{
    private PlayerInputData accumulateInput;
    private bool resetInput;

    public void BeforeUpdate()
    {
        if (resetInput)
        {
            resetInput = false;
            accumulateInput = default;
        }

        // Keyboard keyboard = Keyboard.current;
        // if (keyboard != null && (keyboard.enterKey.wasPressedThisFrame || keyboard.numpadEnterKey.wasPressedThisFrame ||
        //                          keyboard.escapeKey.wasPressedThisFrame))
        // {
        //     if (Cursor.lockState == CursorLockMode.Locked)
        //     {
        //         Cursor.lockState = CursorLockMode.None;
        //         Cursor.visible = true;
        //     }
        // }
        // else
        // {
        //     Cursor.lockState = CursorLockMode.Locked;
        //     Cursor.visible = false;
        // }

        // Accumulate input if the cursor is locked
        if (Cursor.lockState != CursorLockMode.Locked)
            return;

        NetworkButtons buttons = default;

        Keyboard keyboard = Keyboard.current;

        if (keyboard != null)
        {
            Vector2 moveDirection = Vector2.zero;
            if (keyboard.wKey.isPressed)
                moveDirection += Vector2.up;
            if (keyboard.sKey.isPressed)
                moveDirection += Vector2.down;
            if (keyboard.aKey.isPressed)
                moveDirection += Vector2.left;
            if (keyboard.dKey.isPressed)
                moveDirection += Vector2.right;

            // Get the camera a forward direction
            Vector3 camForward = CameraFollow.Instance.Transform.forward;
            camForward.y = 0;
            camForward.Normalize();
            Vector2 camForward2D = new Vector2(camForward.x, camForward.z);

            // Get the camera a right direction
            Vector3 camRight = CameraFollow.Instance.Transform.right;
            camRight.y = 0;
            camRight.Normalize();
            Vector2 camRight2D = new Vector2(camRight.x, camRight.z);

            Vector2 moveDir = camForward2D * moveDirection.y + camRight2D * moveDirection.x;
            moveDir.Normalize();

            Debug.Log(
                $"Move Direction: {moveDirection}  moveDir: {moveDir} accumulateInput.Direction: {accumulateInput.Direction}      camForward: {camForward} camRight: {camRight}");
            accumulateInput.Direction += moveDir;

            buttons.Set(InputButton.Jump, keyboard.spaceKey.isPressed);
        }

        accumulateInput.Buttons = new NetworkButtons(accumulateInput.Buttons.Bits | buttons.Bits);
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        accumulateInput.Direction.Normalize();
        input.Set(accumulateInput);
        resetInput = true;
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (player == runner.LocalPlayer)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
    }
}