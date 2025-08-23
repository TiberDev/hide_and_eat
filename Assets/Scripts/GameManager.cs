using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public WorldCanvasNickname worldCanvasNicknamePrefab;
    public Transform nicknameHolder;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void CreateWorldCanvasNickname(string playerName, Transform target, bool inputAuthority, bool stateAuthority)
    {
        if (worldCanvasNicknamePrefab == null || nicknameHolder == null)
        {
            Debug.LogError("WorldCanvasNickname prefab or nicknameHolder is not set.");
            return;
        }

        var canvasNickname = Instantiate(worldCanvasNicknamePrefab, nicknameHolder);
        canvasNickname.SetTarget(target);
        canvasNickname.SetNickname(playerName, inputAuthority, stateAuthority);
    }
}