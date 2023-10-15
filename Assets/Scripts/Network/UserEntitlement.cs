using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Oculus.Avatar2;
using Oculus.Platform;
using Oculus.Platform.Models;
using Oculus.Platform.Samples.EntitlementCheck;


public class UserEntitlement : MonoBehaviour
{
    public static ulong oculusID;
    public Action onEntitlementGranted;

    private void Awake()
    {
        EntitlementCheck();
    }

    private void EntitlementCheck()
    {
        try
        {
            Core.AsyncInitialize();
            Entitlements.IsUserEntitledToApplication().OnComplete(IsUserEntitledToApplicationComplete);
        }
        catch (UnityException e)
        {
            Debug.LogError(e);
        }
    }

    private void IsUserEntitledToApplicationComplete(Message message)
    {
        if (message.IsError)
        {
            return;
        }

        Users.GetAccessToken().OnComplete(GetAccessTokenComplete);
    }

    private void GetAccessTokenComplete(Message<string> message)
    {
        if (message.IsError)
        {
            return;
        }

        OvrAvatarEntitlement.SetAccessToken(message.Data);

        Users.GetLoggedInUser().OnComplete(GetLoggedInUserComplete);
    }

    private void GetLoggedInUserComplete(Message<User> message)
    {
        if (message.IsError)
        {
            return;
        }

        oculusID = message.Data.ID;
        onEntitlementGranted?.Invoke();
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