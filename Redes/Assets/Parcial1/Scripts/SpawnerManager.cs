using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : SimulationBehaviour, IPlayerJoined
{
    [SerializeField] NetworkPrefabRef _playerPrefab;
    [SerializeField] Transform[] _spawnPoints;

    [SerializeField] Material _orangeMaterial;
    [SerializeField] Material _blueMaterial;

    [SerializeField] PlayerBehaviour _playerSpawned;
    [SerializeField] PlayerTeam _lastPlayerSpawnedTeam;

    public void PlayerJoined(PlayerRef player)
    {
        if (player == Runner.LocalPlayer)
        {
            var _spawned = Runner.Spawn(_playerPrefab, _spawnPoints[CheckForTeam()].position, Quaternion.identity);
            
            _playerSpawned = _spawned.GetComponent<PlayerBehaviour>();
            _playerSpawned.ApplyTeam(_lastPlayerSpawnedTeam, CheckForMaterial());
        }

        int CheckForTeam()
        {
            if (Runner.SessionInfo.PlayerCount % 2 == 1)
            {
                //Team Blue
                _lastPlayerSpawnedTeam = PlayerTeam.Blue;
                return 0;
            }
            else
            {
                //Team Orange
                _lastPlayerSpawnedTeam = PlayerTeam.Orange;
                return 1;
            }

        }

        Material CheckForMaterial()
        {
            if (_lastPlayerSpawnedTeam == PlayerTeam.Orange)
            {
                return _orangeMaterial;
            }
            else
            {
                return _blueMaterial;
            }
        }

    }
}

