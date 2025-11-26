using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager2 : SimulationBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] NetworkPrefabRef _playerPrefab;

    [SerializeField] Material _orangeMaterial;
    [SerializeField] Material _blueMaterial;

    [SerializeField] PlayerBehaviour2 _playerSpawned;
    [SerializeField] PlayerTeam _lastPlayerSpawnedTeam;


    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer)
        {
            var _spawned = Runner.Spawn(_playerPrefab, SpawnPointsLoader.Instance.SpawnPoints[CheckForTeam()].position, Quaternion.identity, player);

            _playerSpawned = _spawned.GetComponent<PlayerBehaviour2>();
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


    

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        if (!LocalInputs.Instance) return;

        input.Set(LocalInputs.Instance.SetInputs());
    }


    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }

}

