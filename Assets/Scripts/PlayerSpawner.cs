using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined, IPlayerLeft
{
    [Tooltip("Prefab of that character that will be spawned.")]
    public NetworkObject playerObject;

    [Tooltip("Player Registry that keeps track of all the players in the game.")]
    public PlayerRegistry playerRegistryPrefab;

    public void PlayerJoined(PlayerRef player)
    {
        // Only the server can spawn.
        Debug.Log($"Player Joined {player.IsRealPlayer}   Runner: {Runner.IsServer}");
        if (Runner.IsServer)
        {
            Runner.Spawn(playerRegistryPrefab);
            var networkPlayerObject = Runner.Spawn(playerObject, GetRandomSpawnPosition(), Quaternion.identity, player)
                .GetComponent<PlayerObject>();
            PlayerRegistry.Instance.Server_Add(Runner, player, networkPlayerObject);
        }
    }

    public void PlayerLeft(PlayerRef player)
    {
        PlayerObject playerObj = PlayerRegistry.GetPlayer(player);
        Debug.Log($"Player Left {player.IsRealPlayer}   Runner: {Runner.IsServer}   name: {playerObj?.Nickname}");
        // Only the server can despawn.
        if (Runner.IsServer)
        {
            PlayerObject leftPlayer = PlayerRegistry.GetPlayer(player);
            if (leftPlayer != null)
            {
                Runner.Despawn(leftPlayer.Object);
            }
        }
    }

    public static Vector3 GetRandomSpawnPosition()
    {
        float x = Random.Range(-3f, 3f);
        float y = 0f; // Usually spawn on the ground at y = 0
        float z = Random.Range(-3f, 3f);
        return new Vector3(x, y, z);
    }
}