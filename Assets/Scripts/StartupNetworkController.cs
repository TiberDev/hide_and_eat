using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fusion;
using Fusion.Sockets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartupNetworkController : MonoBehaviour
{
    [SerializeField] private NetworkRunner networkRunnerPrefab;
    public string RoomName { get; set; } = "DefaultRoom";


    // Init singleton instance
    public static StartupNetworkController Instance { private set; get; }

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
    
    public void Connect(GameMode serverMode)
    {
        // Ensure the NetworkRunner prefab is instantiated only once
        networkRunnerPrefab = Instantiate(networkRunnerPrefab);
        networkRunnerPrefab.name = "NetworkRunner";
        DontDestroyOnLoad(networkRunnerPrefab);
        
        // Get scene reference for the current scene
        var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);

        InitializeNetworkRunner(networkRunnerPrefab,serverMode, NetAddress.Any(), scene, null);
    }

    protected virtual Task InitializeNetworkRunner(NetworkRunner runner, GameMode gameMode, NetAddress address,
        SceneRef scene, Action<NetworkRunner> onGameStarted,
        INetworkRunnerUpdater updater = null)
    {
        var sceneManager = runner.GetComponent<INetworkSceneManager>();
        if (sceneManager == null)
        {
            Debug.Log(
                $"NetworkRunner does not have any component implementing {nameof(INetworkSceneManager)} interface, adding {nameof(NetworkSceneManagerDefault)}.",
                runner);
            sceneManager = runner.gameObject.AddComponent<NetworkSceneManagerDefault>();
        }

        var objectProvider = runner.GetComponent<INetworkObjectProvider>();
        if (objectProvider == null)
        {
            Debug.Log(
                $"NetworkRunner does not have any component implementing {nameof(INetworkObjectProvider)} interface, adding {nameof(NetworkObjectProviderDefault)}.",
                runner);
            objectProvider = runner.gameObject.AddComponent<NetworkObjectProviderDefault>();
        }

        var sceneInfo = new NetworkSceneInfo();
        if (scene.IsValid)
        {
            sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
        }

        return runner.StartGame(new StartGameArgs
        {
            GameMode = gameMode,
            Address = address,
            Scene = sceneInfo,
            SessionName = RoomName,
            OnGameStarted = onGameStarted,
            SceneManager = sceneManager,
            Updater = updater,
            ObjectProvider = objectProvider,
        });
    }
}