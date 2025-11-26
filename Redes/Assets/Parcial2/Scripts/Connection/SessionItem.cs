using Fusion;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SessionItem : MonoBehaviour
{

    [SerializeField] TMP_Text _name;
    [SerializeField] TMP_Text _playersCount;
    [SerializeField] Button _joinButton;

    public void Initialize(SessionInfo session, RunnerHandler RunnerHandler)
    {
        _name.text = session.Name;

        _playersCount.text = (session.PlayerCount + "/" + session.MaxPlayers);

        _joinButton.interactable = session.PlayerCount <= session.MaxPlayers;

        _joinButton.onClick.AddListener(() => RunnerHandler.JoinGame(session));
    }

}
