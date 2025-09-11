using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined
{
    [SerializeField] NetworkPrefabRef _playerPrefab;
    [SerializeField] Transform[] _spawnPoints;

    private void Start()
    {
        
    }


    public void PlayerJoined(PlayerRef player)
    {
        if (player == Runner.LocalPlayer)
        {
            var SpawnIndex = Runner.SessionInfo.PlayerCount - 1;

            Runner.Spawn(_playerPrefab, _spawnPoints[SpawnIndex].position, Quaternion.identity);
        }
    }

}
