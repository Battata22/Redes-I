using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RunnerHandler : MonoBehaviour, INetworkRunnerCallbacks
{
    public event Action OnJoinedLobbySuccesfully;
    public event Action<List<SessionInfo>> OnSessionsUpdate;

    [SerializeField] NetworkRunner _runnerPrefab;
    NetworkRunner _currentRunner;

    #region Lobby
    public void JoinLobby()
    {
        if (_currentRunner)
            Destroy(_currentRunner.gameObject);

        _currentRunner = Instantiate(_runnerPrefab);

        _currentRunner.AddCallbacks(this);

        JoinLobbyAsync();
    }

    async void JoinLobbyAsync()
    {
        var result = await _currentRunner.JoinSessionLobby(SessionLobby.Custom, "Custom Lobby");

        if (result.Ok)
        {
            print("Connected to lobby");
            OnJoinedLobbySuccesfully?.Invoke();
        }
        else
        {
            print("Error trying to connect to lobby");
        }
    }
    #endregion

    #region Host - Client
    public void HostGame(string SessionName, string Scene)
    {
        CreateGame(GameMode.Host, SessionName, SceneUtility.GetBuildIndexByScenePath("Parcial2/" + Scene));
    }

    public void JoinGame(SessionInfo SessionInfo)
    {
        CreateGame(GameMode.Client, SessionInfo.Name);
    }

    async void CreateGame(GameMode GameMode, string SessionName, int SceneIndex = 0)
    {
        _currentRunner.ProvideInput = true;

        var result = await _currentRunner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode,
            SessionName = SessionName,
            Scene = SceneRef.FromIndex(SceneIndex),
            PlayerCount = 8
        });


        if (result.Ok)
        {
            print("Connected to session");
        }
        else
        {
            print("Error trying to connect to session");
        }
    }
    #endregion

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        OnSessionsUpdate?.Invoke(sessionList);
    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) { }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }
    public void OnInput(NetworkRunner runner, NetworkInput input) { }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }

}
