using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugCanvas : MonoBehaviour
{
    public Text display;
    public GameObject displayScreen;
    
    private Dictionary<string, string> _debugLogs = new Dictionary<string, string>();

    private void Start()
    {
        displayScreen.SetActive(false);
    }

    private void Update()
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

    private void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        if (type == LogType.Log)
        {
            var splitString = logString.Split(char.Parse(":"));
            var debugKey = splitString[0];
            var debugValue = splitString.Length > 1 ? splitString[1] : "";

            if (_debugLogs.ContainsKey(debugKey))
            {
                _debugLogs[debugKey] = debugValue;
            }
            else
            {
                _debugLogs.Add(debugKey, debugValue);
            }

            var displayText = "";
            foreach (var log in _debugLogs)
            {
                if (log.Value == "")
                {
                    displayText += log.Key + "\n";
                }
                else
                {
                    displayText += log.Key + ": " + log.Value + "\n";
                }
            }

            display.text = displayText;
        }
    }
}