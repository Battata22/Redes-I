using Fusion;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : NetworkBehaviour, IPlayerJoined
{

    List<PlayerRef> _conectedUsers = new();

    [SerializeField] Image _winImage;
    [SerializeField] Image _defeatImage;

    public int MinPlayerRequiredToStart = 2;
    //public bool GameStarted = false;

    //public event Action OnGameStart;
    public event Action OnGameEnded;

    #region Singleton
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    #endregion

    public void AddToUsersList(PlayerBehaviour player)
    {
        var newClient = player.Object.StateAuthority;

        if (_conectedUsers.Contains(newClient)) return;

        _conectedUsers.Add(newClient);
    }

    void RemoveFromoUsersList(PlayerRef client)
    {
        _conectedUsers.Remove(client);
    }

    public void PlayerDeath(PlayerBehaviour player)
    {
        OnGameEnded();

        _defeatImage.enabled = true;

        RPC_PlayerDefeated(player.Object.StateAuthority);
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    void RPC_PlayerDefeated(PlayerRef clientToRemove)
    {
        RemoveFromoUsersList(clientToRemove);

        if (_conectedUsers.Count == 1 && HasStateAuthority)
        {
            RPC_Win(_conectedUsers[0]);
        }
    }

    [Rpc]
    void RPC_Win([RpcTarget] PlayerRef _clientTarget)
    {
        OnGameEnded();

        _winImage.enabled = true;
    }

    public void PlayerJoined(PlayerRef player)
    {
        //print(GameStarted + " " + Runner.SessionInfo.PlayerCount);
        //if (!GameStarted && Runner.SessionInfo.PlayerCount >= MinPlayerRequiredToStart)
        //{
        //    GameStarted = true;
        //    OnGameStart();
        //}
    }
}
