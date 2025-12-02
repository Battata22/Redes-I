using System.Collections;
using UnityEngine;
using Fusion;
using System.Linq;
using System.Collections.Generic;

public class LobbyManager : NetworkBehaviour
{
    [SerializeField] List<PlayerBehaviour2> _players = new List<PlayerBehaviour2>();
    [SerializeField] ReadyOrNotScript _readyScript;

    public static LobbyManager instance;
    private void Awake()
    {
        instance = this;
    }
    public override void FixedUpdateNetwork()
    {
        if (!Object.HasStateAuthority)
            return;

        CheckAllPlayersReady();
    }

    public void JointTheList(PlayerBehaviour2 player)
    {
        if (!_players.Contains(player))
        {
            _players.Add(player);
        }
    }

    private void CheckAllPlayersReady()
    {

        if (Runner.SessionInfo.PlayerCount >= 2 && _players.Count >= 2)
        {
            bool allReady = true;

            foreach (var player in _players)
            {
                if (!player.IsReady)
                {
                    allReady = false;
                    break;
                }
            }

            if (allReady)
            {
                RPCStartGame();
            }
        }

    }


    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPCStartGame()
    {
        print("readyyyyyyy");
        _readyScript._readyCanvas.enabled = false;
        _readyScript._cursorManager.BothOn();

        var localPlayerBehaviour = FindObjectsOfType<PlayerBehaviour2>();
        foreach (var player in localPlayerBehaviour)
        {
            player.RPCSetCanPlay(true);
        }

        enabled = false;
    }
}