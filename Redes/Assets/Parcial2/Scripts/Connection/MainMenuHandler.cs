using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviour
{
    [SerializeField] RunnerHandler _runnerHandler;

    [Header("Panels")]
    [SerializeField] GameObject _initialPanel;
    [SerializeField] GameObject _joiningPanel;
    [SerializeField] GameObject _sessionBrowserPanel;
    [SerializeField] GameObject _hostPanel;

    [Header("Buttons")]
    [SerializeField] Button _joinButton;
    [SerializeField] Button _goToHostPanelButton;
    [SerializeField] Button _hostButton;

    [Header("Input Fields")]
    [SerializeField] TMP_InputField _sessionNameField;


    private void Awake()
    {
        _joinButton.onClick.AddListener(AskToJoinLobby);

        _goToHostPanelButton.onClick.AddListener(() =>
        {
            _sessionBrowserPanel.SetActive(false);
            _hostPanel.SetActive(true);
        });

        _hostButton.onClick.AddListener(StartGameAsHost);

        _runnerHandler.OnJoinedLobbySuccesfully += () =>
        {
            _joiningPanel.SetActive(false);
            _sessionBrowserPanel.SetActive(true);
        };
    }

    void AskToJoinLobby()
    {
        _joinButton.interactable = false;
        _runnerHandler.JoinLobby();

        _initialPanel.SetActive(false);
        _joiningPanel.SetActive(true);
    }

    private void StartGameAsHost()
    {
        _hostButton.interactable = false;
        _runnerHandler.HostGame(_sessionNameField.text, "Gameplay");
    }

}
