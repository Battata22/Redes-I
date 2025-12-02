using Fusion;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager2 : NetworkBehaviour, IPlayerJoined, IPlayerLeft
{

    List<PlayerRef> _conectedUsers = new();

    [SerializeField] Image _winImage;
    [SerializeField] Image _defeatImage;

    public int MinPlayerRequiredToStart = 2;

    public event Action OnGameEnded;

    [SerializeField] CloseMeManager _closeMeManager;

    bool _hasMaxPlayers = false;

    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip _winSound;
    [SerializeField] AudioClip _loseSound;

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

        if (_conectedUsers.Count >= MinPlayerRequiredToStart)
        {
            _hasMaxPlayers = true;
        }
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

            _audioSource.PlayOneShot(_loseSound);

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

        _audioSource.PlayOneShot(_winSound);

        OnGameEnded();
        StartCoroutine(_closeMeManager.GoToLobby(Runner));
    }

    public void PlayerJoined(PlayerRef player)
    {

    }

    public void PlayerLeft(PlayerRef player)
    {
        if (_hasMaxPlayers)
        {
            OnGameEnded();
            StartCoroutine(_closeMeManager.GoToLobby(Runner));
        }
    }
}
