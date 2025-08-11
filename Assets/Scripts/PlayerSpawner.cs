using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined, IPlayerLeft
{
    [Tooltip("Prefab of that character that will be spawned.")]
    public NetworkObject playerObject;

    public void PlayerJoined(PlayerRef player)
    {
        // Only the server can spawn.
        if (Runner.IsServer)
        {
            var networkPlayerObject = Runner.Spawn(playerObject, GetRandomSpawnPosition(), Quaternion.identity, player);
        }
    }

    public void PlayerLeft(PlayerRef player)
    {
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