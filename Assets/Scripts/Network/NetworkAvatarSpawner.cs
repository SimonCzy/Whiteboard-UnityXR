using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class NetworkAvatarSpawner : MonoBehaviour
{
    [Header("References")] [SerializeField]
    private NetworkRunner _networkRunner;

    [SerializeField] private NetworkEvents _networkEvents;
    [SerializeField] private UserEntitlement _userEntitlement;

    [Header("Prefabs")] [SerializeField] private NetworkObject _avatarPrefab;
    [Header("Ovr Rig")] [SerializeField] private Transform _cameraRigTransform;

    [Header("Spawn Points")] [SerializeField]
    private Transform[] _spawnPoints;

    private bool _isServerConnected = false;
    private bool _isEntitlementGranted = false;

    private void Awake()
    {
        _networkEvents.OnConnectedToServer.AddListener(ConnectedToServer);
        _userEntitlement.onEntitlementGranted += EntitlementGranted;
    }

    private void OnDestroy()
    {
        _networkEvents.OnConnectedToServer.RemoveListener(ConnectedToServer);
        _userEntitlement.onEntitlementGranted -= EntitlementGranted;
    }

    private void ConnectedToServer(NetworkRunner networkRunner)
    {
        _isServerConnected = true;
        TrySpawnAvatar();
    }

    private void EntitlementGranted()
    {
        _isEntitlementGranted = true;
        TrySpawnAvatar();
    }

    private void TrySpawnAvatar()
    {
        if (!_isServerConnected || !_isEntitlementGranted) return;

        SetPlayerSpawnPosition();
        SpawnAvatar();
    }

    private void SetPlayerSpawnPosition()
    {
        var index = (_networkRunner.SessionInfo.PlayerCount - 1) % _spawnPoints.Length;

        _cameraRigTransform.SetPositionAndRotation(_spawnPoints[index].position, _spawnPoints[index].rotation);
    }

    private void SpawnAvatar()
    {
        var avatar = _networkRunner.Spawn(_avatarPrefab, _cameraRigTransform.position, _cameraRigTransform.rotation,
            _networkRunner.LocalPlayer);
        avatar.transform.SetParent(_cameraRigTransform);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}