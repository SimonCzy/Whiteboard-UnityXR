using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using static Oculus.Avatar2.OvrAvatarEntity;

public class AvatarStateSync : NetworkBehaviour
{
    [SerializeField] private AvatarEntityState _avatarEntityState;

    [Networked] public ulong oculusID { get; set; }
    [Networked] private uint _avatarDataCount { get; set; }

    private const int AvatarDataSize = 1200;

    [Networked(OnChanged = nameof(OnAvatarDataChanged)), Capacity(AvatarDataSize)]
    private NetworkArray<byte> AvatarData { get; }

    private byte[] _byteArray = new byte[AvatarDataSize];

    public override void Spawned()
    {
        if (Object.HasStateAuthority) oculusID = UserEntitlement.oculusID;
    }

    public void RecordAvatarState(StreamLOD streamLOD)
    {
        _avatarDataCount = _avatarEntityState.RecordStreamData_AutoBuffer(streamLOD, ref _byteArray);

        AvatarData.CopyFrom(_byteArray, 0, _byteArray.Length);
    }

    static void OnAvatarDataChanged(Changed<AvatarStateSync> changed) => changed.Behaviour.ApplyAvatarData();

    private void ApplyAvatarData()
    {
        if (Object.HasStateAuthority) return;

        var slicedData = new byte[_avatarDataCount];
        AvatarData.CopyTo(slicedData, throwIfOverflow: false);
        _avatarEntityState.AddToDataBuffer(slicedData);
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