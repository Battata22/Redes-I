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

    [SerializeField] PlayerBehaviour _lastPlayerSpawned;
    [SerializeField] PlayerTeam _lastPlayerSpawnedTeam;

    public void PlayerJoined(PlayerRef player)
    {
        if (player == Runner.LocalPlayer)
        {
            var _spawned = Runner.Spawn(_playerPrefab, _spawnPoints[CheckForTeam()].position, Quaternion.identity);
            _lastPlayerSpawned = _spawned.GetComponent<PlayerBehaviour>();
            _lastPlayerSpawned.ApplyTeam(_lastPlayerSpawnedTeam, CheckForMaterial());
        }

        int CheckForTeam()
        {
            if (_lastPlayerSpawnedTeam == PlayerTeam.Orange)
            {
                _lastPlayerSpawnedTeam = PlayerTeam.Blue;
                return 0;
            }
            else
            {
                _lastPlayerSpawnedTeam = PlayerTeam.Orange;
                return 1;
            }
        }

        Color CheckForMaterial()
        {
            if (_lastPlayerSpawnedTeam == PlayerTeam.Orange)
            {
                return _orangeMaterial.color;
            }
            else
            {
                return _blueMaterial.color;
            }
        }

    }
}

