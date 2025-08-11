using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public enum InputButton
{
    Jump
}

public struct PlayerInputData : INetworkInput
{
    public Vector2 Direction;
    public Vector2 LookDelta;
    public NetworkButtons Buttons;
}