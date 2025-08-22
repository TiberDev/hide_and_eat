using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class PlayerObject : NetworkBehaviour
{
    private static readonly int MOVE_SPEED_ANIM_PARAMATER = Animator.StringToHash("moveSpeed");

    [Networked, OnChangedRender(nameof(OnNicknameChanged))]
    public NetworkString<_16> Nickname { get; set; }

    [SerializeField] private Animator anim;

    private PlayerMovement playerMovement;

    float moveSpeed = 0;

    public override void Spawned()
    {
        playerMovement = GetComponent<PlayerMovement>();
        if (Object.HasInputAuthority)
        {
            // Set the player name from PlayerPrefs or a default value
            gameObject.name = Nickname.Value;
            Rpc_SetPlayerName(StartupNetworkController.Instance.NickName);
        }
        else
        {
            if (!string.IsNullOrEmpty(Nickname.Value))
            {
                gameObject.name = Nickname.Value;
                GameManager.Instance.CreateWorldCanvasNickname(Nickname.Value, transform, false);
            }
        }
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    void Rpc_SetPlayerName(string nickName)
    {
        Nickname = nickName;
    }

    private void OnNicknameChanged()
    {
        gameObject.name = Nickname.Value;
        GameManager.Instance.CreateWorldCanvasNickname(Nickname.Value, transform, Object.HasInputAuthority);
    }

    public override void Render()
    {
        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        moveSpeed = Mathf.Min(1f,
            Mathf.MoveTowards(moveSpeed, playerMovement.simpleKCC.RealSpeed, Time.deltaTime * 3f));
        anim.SetFloat(MOVE_SPEED_ANIM_PARAMATER, moveSpeed);
    }
}