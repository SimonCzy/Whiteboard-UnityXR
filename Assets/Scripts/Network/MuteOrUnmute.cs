using System.Collections;
using System.Collections.Generic;
using Photon.Voice.Fusion;
using TMPro;
using UnityEngine;

public class MuteOrUnmute : MonoBehaviour
{
    public GameObject displayScreen;
    public TextMeshPro muteButtonText;
    public GameObject connectionManager;

    private bool _isMuted;

    // Start is called before the first frame update
    void Start()
    {
        displayScreen.SetActive(false);
        _isMuted = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.Get(OVRInput.Button.Four) && OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
        {
            displayScreen.SetActive(true);
        }

        if (OVRInput.Get(OVRInput.Button.Three) && OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
        {
            displayScreen.SetActive(false);
        }
    }

    public void SwitchStatus()
    {
        _isMuted = !_isMuted;

        // Debug.Log("SwitchStatus: " + _isUnlock);

        var text = muteButtonText.GetComponent<TextMeshPro>();
        if (text != null)
        {
            // Debug.Log("TextMeshPro: " + _isUnlock);
            text.text = _isMuted ? "Unmute All" : "Mute All";
        }

        var fusionVoiceClient = connectionManager.GetComponent<FusionVoiceClient>();
        fusionVoiceClient.enabled = !_isMuted;
    }
}