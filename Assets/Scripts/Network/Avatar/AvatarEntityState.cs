using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Avatar2;
using Fusion;

public class AvatarEntityState : OvrAvatarEntity
{
    [SerializeField] private AvatarStateSync _avatarStateSync;
    [SerializeField] private OvrAvatarLipSyncContext _lipSyncContext;

    [SerializeField] private float _intervalDataStream = 0.05f;
    [SerializeField] private int _maxDataBuffer = 6;
    [SerializeField] private StreamLOD _streamLOD = StreamLOD.Low;

    [SerializeField] private NetworkObject _networkObject;

    private List<byte[]> _receivedDataBuffer = new();
    private float _lastStreamedTime;

    protected override void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        ConfigureAvatarEntity();

        base.Awake();

        SetActiveView(_networkObject.HasStateAuthority
            ? CAPI.ovrAvatar2EntityViewFlags.FirstPerson
            : CAPI.ovrAvatar2EntityViewFlags.ThirdPerson);
        StartCoroutine(LoadAvatarID());
    }

    private void ConfigureAvatarEntity()
    {
        if (_networkObject.HasStateAuthority)
        {
            SetIsLocal(true);

            _creationInfo.features = CAPI.ovrAvatar2EntityFeatures.Preset_Default;
            SetBodyTracking(OvrAvatarManager.Instance.gameObject.GetComponent<SampleInputManager>());

            gameObject.name = "Local Avatar";
        }
        else
        {
            SetIsLocal(false);
            SetLipSync(null);

            _lipSyncContext.CaptureAudio = false;

            _creationInfo.features = CAPI.ovrAvatar2EntityFeatures.Preset_Remote;

            gameObject.name = "Remote Avatar";
        }
    }

    private IEnumerator LoadAvatarID()
    {
        while (_avatarStateSync.oculusID == 0) yield return null;

        _userId = _avatarStateSync.oculusID;

        var avatarRequest = OvrAvatarManager.Instance.UserHasAvatarAsync(_userId);

        while (!avatarRequest.IsCompleted) yield return null;

        LoadUser();
    }

    private void LateUpdate()
    {
        if (!IsLocal || CurrentState != AvatarState.UserAvatar) return;

        if (_intervalDataStream > Time.time - _lastStreamedTime) return;

        _avatarStateSync.RecordAvatarState(_streamLOD);

        _lastStreamedTime = Time.time;
    }

    public void AddToDataBuffer(Byte[] avatarStateData)
    {
        if (_receivedDataBuffer.Count >= _maxDataBuffer) _receivedDataBuffer.RemoveAt(_receivedDataBuffer.Count - 1);

        _receivedDataBuffer.Add(avatarStateData);
    }

    // Update is called once per frame
    void Update()
    {
        if (IsLocal || CurrentState != AvatarState.UserAvatar || _receivedDataBuffer.Count <= 0) return;

        ApplyStreamData(_receivedDataBuffer[0]);

        SetPlaybackTimeDelay(_intervalDataStream);

        _receivedDataBuffer.RemoveAt(0);
    }
}