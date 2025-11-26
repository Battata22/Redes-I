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

    #region Singleton
    public static GameManager2 Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    #endregion

    public void AddToUsersList(PlayerBehaviour2 player)
    {
        var newClient = player.Object.StateAuthority;

        if (_conectedUsers.Contains(newClient)) return;

        _conectedUsers.Add(newClient);
    }

    void RemoveFromoUsersList(PlayerRef client)
    {
        _conectedUsers.Remove(client);
    }

    public void PlayerDeath(PlayerBehaviour2 player)
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

    }
}
