using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class PlayerObject : NetworkBehaviour
{
    [Networked, OnChangedRender(nameof(OnNicknameChanged))]
    public NetworkString<_16> Nickname { get; set; }
    
    public override void Spawned()
    {
        base.Spawned();

        if (Object.HasInputAuthority)
        {
            // Set the player name from PlayerPrefs or a default value
            Rpc_SetPlayerName(StartupNetworkController.Instance.NickName);
        }
        else
        {
            if (!string.IsNullOrEmpty(Nickname.Value))
            {
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
        GameManager.Instance.CreateWorldCanvasNickname(Nickname.Value, transform, Object.HasInputAuthority);
    }
}