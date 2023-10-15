using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConnectionManager : MonoBehaviour
{
    private NetworkRunner _networkRunner;
    private NetworkEvents _networkEvents;
    private NetworkObject[] _networkObjects;

    private void Awake()
    {
        _networkObjects = GetNetworkObjects();
        _networkRunner = GetComponent<NetworkRunner>();
        _networkEvents = GetComponent<NetworkEvents>();

        _networkEvents.OnConnectedToServer.AddListener(RegisterNetworkObjects);
    }

    private void OnDestroy()
    {
        _networkEvents.OnConnectedToServer.RemoveListener(RegisterNetworkObjects);
    }

    // Start is called before the first frame update
    void Start()
    {
        var newGame = new StartGameArgs
        {
            GameMode = GameMode.Shared,
            SessionName = SceneManager.GetActiveScene().buildIndex.ToString()
        };

        _networkRunner.StartGame(newGame);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private NetworkObject[] GetNetworkObjects()
    {
        return FindObjectsByType<NetworkObject>(FindObjectsSortMode.None);
    }

    private void RegisterNetworkObjects(NetworkRunner runner)
    {
        runner.RegisterSceneObjects(_networkObjects);
    }
}