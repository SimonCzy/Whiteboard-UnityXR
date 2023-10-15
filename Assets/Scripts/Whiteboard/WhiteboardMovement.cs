using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using TMPro;
using UnityEngine;

public class WhiteboardMovement : MonoBehaviour
{
    public GameObject pivot;
    public GameObject board;
    public GameObject lockButtonText;

    private bool _isUnlock;

    // Start is called before the first frame update
    void Start()
    {
        _isUnlock = false;
        SetBoardGrabStatus();
    }

    private void SetBoardGrabStatus()
    {
        var grabbable = board.GetComponent<Grabbable>();
        var grabInteractable = board.GetComponent<GrabInteractable>();
        var handGrabInteractable = board.GetComponent<HandGrabInteractable>();

        if (grabbable != null && grabInteractable != null && handGrabInteractable != null)
        {
            grabbable.enabled = _isUnlock;
            grabInteractable.enabled = _isUnlock;
            handGrabInteractable.enabled = _isUnlock;

            // Debug.Log("SetBoardGrabStatus: " + _isUnlock);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_isUnlock)
        {
            Movement();
        }
    }

    private void Movement()
    {
        float positionThreshold = 0.001f;
        float rotationThreshold = 0.1f;

        if (Vector3.Distance(pivot.transform.position, board.transform.position) > positionThreshold)
        {
            pivot.transform.position = board.transform.position;
            board.transform.localPosition = Vector3.zero;
        }

        if (Quaternion.Angle(pivot.transform.rotation, board.transform.rotation) > rotationThreshold)
        {
            pivot.transform.rotation = board.transform.rotation;
            board.transform.localRotation = Quaternion.identity;
        }
    }

    public void SwitchStatus()
    {
        _isUnlock = !_isUnlock;

        // Debug.Log("SwitchStatus: " + _isUnlock);
        SetBoardGrabStatus();

        var text = lockButtonText.GetComponent<TextMeshPro>();
        if (text != null)
        {
            // Debug.Log("TextMeshPro: " + _isUnlock);
            text.text = _isUnlock ? "Lock" : "Unlock";
        }
    }
}