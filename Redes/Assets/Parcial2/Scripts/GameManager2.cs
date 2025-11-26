using Fusion;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager2 : NetworkBehaviour, IPlayerJoined
{

    List<PlayerRef> _conectedUsers = new();

    [SerializeField] Image _winImage;
    [SerializeField] Image _defeatImage;

    public int MinPlayerRequiredToStart = 2;

    public event Action OnGameEnded;

    [SerializeField] CloseMeManager _closeMeManager;

    #region Singleton
    public static GameManager2 Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    #endregion

    public void AddToUsersList(PlayerBehaviour2 player)
    {
        var newClient = player.Object.InputAuthority;

        if (_conectedUsers.Contains(newClient)) return;

        _conectedUsers.Add(newClient);
    }

    void RemoveFromoUsersList(PlayerRef client)
    {
        _conectedUsers.Remove(client);
    }

    public void PlayerDeath(PlayerBehaviour2 player)
    {
        RPC_PlayerDefeated(player.Object.InputAuthority);
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    void RPC_PlayerDefeated(PlayerRef losingPlayer)
    {
        if (Runner.LocalPlayer == losingPlayer)
        {
            _defeatImage.enabled = true;
            OnGameEnded();
            StartCoroutine(_closeMeManager.GoToLobby(Runner));
        }

        RemoveFromoUsersList(losingPlayer);

        if (_conectedUsers.Count == 1)
        {
            var winner = _conectedUsers[0];
            RPC_Win(winner);
        }
    }

    [Rpc]
    void RPC_Win([RpcTarget] PlayerRef _clientTarget)
    {
        _winImage.enabled = true;
        OnGameEnded();
        StartCoroutine(_closeMeManager.GoToLobby(Runner));
    }

    public void PlayerJoined(PlayerRef player)
    {

    }


}
