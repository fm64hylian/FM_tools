using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMIntroLoaderController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FMPlayfabLogin.LoginCustomID("187046C5E478B9F0", onLogin);
    }

    void onLogin(LoginResult result) {
        Debug.Log("logged in, ye -"+ result.PlayFabId);
    }

}
