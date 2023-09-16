using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] private TMP_InputField _inputFieldDisplayName;
    [SerializeField] InputActionMap _playerInput;

    private string _usernameString;
    private int _playerTime=0;
    
    // Disables / Enables Player Input on Show
    private void OnEnable()
    {
        _playerInput.Disable();
        _usernameString = SystemInfo.deviceUniqueIdentifier.Substring(0,20);
    }

    private void OnDisable()
    {
        _playerInput.Enable();
    }

    public void TryToPostScore()
    {
        if (_inputFieldDisplayName.text.Length > 0)
        {
            Debug.Log(_inputFieldDisplayName.text);
            RegisterPlayer(_inputFieldDisplayName.text);
            LoginPlayer();
        }
    }

    private void RegisterPlayer(string displayName)
    {
        Debug.Log(_usernameString);
        var request = new RegisterPlayFabUserRequest()
        {
            TitleId = PlayFabSettings.TitleId,
            DisplayName = _inputFieldDisplayName.text,
            Password = "Password",
            RequireBothUsernameAndEmail = false,
            Username = _usernameString
        };
        
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnRegisterFailure);
    }

    private void GetAccount()
    {
        
    }
    private void OnRegisterSuccess(RegisterPlayFabUserResult obj)
    {
        Debug.Log("Register user successfully");
    }
    
    private void OnRegisterFailure(PlayFabError obj)
    {
        Debug.Log("Register Failed");
    }


    // Login Variant
    private void LoginPlayer()
    {
        var request = new LoginWithPlayFabRequest()
        {
            TitleId = PlayFabSettings.TitleId,
            Username = _usernameString,
            Password = "Password",
            
        };
        PlayFabClientAPI.LoginWithPlayFab(request, OnLoginSuccess, OnLoginFailure);
    }

    private void OnLoginSuccess(LoginResult obj)
    {
        Debug.Log("Player logged in Successfully");
        PostLeaderBoardTime();
    }
    
    private void OnLoginFailure(PlayFabError obj)
    {
        Debug.Log("Failed login");
        Debug.Log(obj.Error);
    }

    
    // Post Time To Leaderboard
    
    void PostLeaderBoardTime()
    {
        var request = new UpdatePlayerStatisticsRequest()
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate()
                {
                    StatisticName = "Run Timer",
                    Value = _playerTime
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, UpdateLeaderboardSuccess, UpdateLeaderboardFailed);
    }

    private void UpdateLeaderboardSuccess(UpdatePlayerStatisticsResult obj)
    {
        Debug.Log("Leaderboard Updated Successfully");
    }
    
    private void UpdateLeaderboardFailed(PlayFabError obj)
    {
        Debug.Log("Failed to Post Time to Leaderboard");
        Debug.Log(obj.Error);
    }


    // Get Leaderboard
    void GetLeaderBoard()
    {
        var request = new GetLeaderboardRequest();
        request.StatisticName = "Run Timer";
        request.StartPosition = 0;

        PlayFabClientAPI.GetLeaderboard(request, GetLeaderboardSuccess, GetLeaderboardFail);
    }

    private void GetLeaderboardSuccess(GetLeaderboardResult obj)
    {
        foreach (var tmp in obj.Leaderboard)
        {
            Debug.Log(tmp.DisplayName);
            Debug.Log(tmp.StatValue);
        }
    }

    private void GetLeaderboardFail(PlayFabError obj)
    {
        throw new System.NotImplementedException();
    }
}