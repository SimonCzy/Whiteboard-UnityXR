using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteboardSetupManager : MonoBehaviour
{
    public GameObject visual;
    public GameObject pen;
    public GameObject topControlPanel;
    public GameObject downControlPanel;
    public GameObject leftControlPanel;

    public Transform pivot;
    public Transform creationHand;

    private float _defaultWidth = 0.001f;
    private float _defaultHeight = 0.001f;
    private float _objectOffset = 0.05f;

    private Vector3 _startPosition;
    private bool _isLineExist, _isUpdatingLine;
    private bool _isWhiteboardExist, _isUpdatingWhiteboard;
    private Vector3 _linePos;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        visual.SetActive(false);
        SetToolsStatus(false);
        _isLineExist = false;
        _isWhiteboardExist = false;
    }

    // Update is called once per frame
    void Update()
    {
        // press 'Y' to disable the whiteboard
        DisableWhiteboard();

        // use one line and one point to customize a whiteboard
        DrawWhiteboard();

        // use two point to define a line
        DrawLine();
    }

    private void SetToolsStatus(bool status)
    {
        pen.SetActive(status);
        topControlPanel.SetActive(status);
        downControlPanel.SetActive(status);
        leftControlPanel.SetActive(status);
    }

    private void DisableWhiteboard()
    {
        if (OVRInput.Get(OVRInput.Button.Four))
        {
            // Debug.Log("DisableWhiteboard Start: " + Time.time);
            Initialize();
            // Debug.Log("DisableWhiteboard End: " + Time.time);
        }
    }

    private void DrawLine()
    {
        if (_isLineExist) return;

        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            visual.SetActive(true);
            _startPosition = creationHand.position;
            _isUpdatingLine = true;
        }
        else if (OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger))
        {
            _isUpdatingLine = false;
            _isLineExist = true;
        }

        if (_isUpdatingLine)
        {
            UpdateLine();
        }
    }

    private void UpdateLine()
    {
        Vector3 line = Vector3.ProjectOnPlane(creationHand.position - _startPosition, Vector3.up);

        // Scaling
        Vector3 scale = new Vector3(line.magnitude, _defaultHeight, _defaultWidth);
        visual.transform.localScale = scale;

        // Rotation
        pivot.right = line;

        // position
        _linePos = _startPosition + pivot.rotation * new Vector3(line.magnitude / 2, 0, -_defaultWidth / 2);
        pivot.position = _linePos;
    }

    private void DrawWhiteboard()
    {
        if (!_isLineExist || _isWhiteboardExist) return;

        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            _isUpdatingWhiteboard = true;
        }
        else if (OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger))
        {
            _isUpdatingWhiteboard = false;
            _isWhiteboardExist = true;
            SetToolsStatus(true);
            // Debug.Log("PenSetActive: " + Time.time);
        }

        if (_isUpdatingWhiteboard)
        {
            UpdateWhiteboard();
            UpdateObject();
        }
    }

    private void UpdateWhiteboard()
    {
        Vector3 whiteboard = Vector3.ProjectOnPlane(creationHand.position - _startPosition, pivot.right);

        // Scaling
        Vector3 scale = new Vector3(visual.transform.localScale.x, _defaultHeight, whiteboard.magnitude);
        visual.transform.localScale = scale;

        // Rotation
        pivot.forward = -whiteboard;

        // position
        pivot.position = _linePos + pivot.rotation * new Vector3(0, 0, -whiteboard.magnitude / 2);
    }

    private void UpdateObject()
    {
        UpdatePen();
        UpdateControlPanel();
    }

    private void UpdatePen()
    {
        pen.transform.localPosition = new Vector3(visual.transform.localScale.x / 2 + _objectOffset, 0, 0);
        pen.transform.up = pivot.transform.forward;
    }

    private void UpdateControlPanel()
    {
        var localScale = visual.transform.localScale;
        var height = localScale.z / 2;
        topControlPanel.transform.localPosition = new Vector3(0, 0, height + _objectOffset);
        downControlPanel.transform.localPosition = new Vector3(0, 0, -height);
        leftControlPanel.transform.localPosition = new Vector3(-(localScale.x / 2 + _objectOffset), 0, 0);
    }
}