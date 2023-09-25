using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.MultiplayerModels;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayFABUserSetup : MonoBehaviour
{
    [SerializeField] private TMP_InputField _inputFieldDisplayName;
    [SerializeField] private PlayerInput _playerInput;

    private string _usernameString;

    [SerializeField] private Leaderboard _leaderboardScript;
    private List<LeaderboardEntry> _leaderboardEntries = new List<LeaderboardEntry>();

    [SerializeField] private Transform _VictoryScreen;
    [SerializeField] private Transform _LeaderboardPanel;


    private int _playerTime=0;
    public void PlayFabStartUp(int tmpTime)
    {
        _playerTime = tmpTime;
        SetupPanel();
    }

    private void SetupPanel()
    {
        _usernameString = SystemInfo.deviceUniqueIdentifier.Substring(0,20);

        if (_playerInput == null)
        {
            Debug.Log("Can't get playerinput on the Leaderboard script");
            return;
        }

        _playerInput.SwitchCurrentActionMap("UI");
        EnableCursor(true);
        
        //Registers User Account, if already exists good otherwise creates one
        RegisterPlayer();
    }
    

    // Gets account info to preset the display name box
    private void GetAccountInfo()
    {
        var request = new GetAccountInfoRequest()
        {
            Username = _usernameString
        };
        
        PlayFabClientAPI.GetAccountInfo(request, AccountInfoSuccess, AccountInfoFailed);
    }

    private void AccountInfoSuccess(GetAccountInfoResult obj)
    {
        _inputFieldDisplayName.SetTextWithoutNotify(obj.AccountInfo.TitleInfo.DisplayName);
    }
    
    private void AccountInfoFailed(PlayFabError obj)
    {
        Debug.Log("Failed to get account info");
    }


    private void EnableCursor(bool toShow)
    {
        if (toShow)
        {
            Cursor.visible = true;
            Debug.Log("Cursor is visible");
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void OnDisable()
    {
        if (_playerInput == null)
        {
            Debug.Log("Can't get playerinput on the Leaderboard script");
            return;
        }
        _playerInput.SwitchCurrentActionMap("Game");
        EnableCursor(false);
    }

    public void TryToPostScore()
    {
        if (_inputFieldDisplayName.text.Length > 0)
        {
            var request = new UpdateUserTitleDisplayNameRequest()
            {
                DisplayName = _inputFieldDisplayName.text
            };
            PlayFabClientAPI.UpdateUserTitleDisplayName(request, UpdateUserTitleSuccess, UpdateUserTitleFailed);
        }
        else
        {
            Debug.Log("Please enter a name");
        }
    }


    private void UpdateUserTitleSuccess(UpdateUserTitleDisplayNameResult obj)
    {
        PostLeaderBoardTime();
    }
    private void UpdateUserTitleFailed(PlayFabError obj)
    {
        Debug.Log("Failed to update user display name "+ obj.ErrorMessage);
    }
    

    private void RegisterPlayer()
    {
        Debug.Log(_usernameString);
        var request = new RegisterPlayFabUserRequest()
        {
            TitleId = PlayFabSettings.TitleId,
            //DisplayName = _inputFieldDisplayName.text,
            Password = "Password",
            RequireBothUsernameAndEmail = false,
            Username = _usernameString
        };
        
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnRegisterFailure);
    }
    private void OnRegisterSuccess(RegisterPlayFabUserResult obj)
    {
        LoginPlayer();
    }
    private void OnRegisterFailure(PlayFabError obj)
    {
        LoginPlayer();
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
        GetAccountInfo();
    }
    private void OnLoginFailure(PlayFabError obj)
    {
        Debug.Log("Failed login " + obj.ErrorMessage);
    }


    
    // Post Time To Leaderboard
    
    void PostLeaderBoardTime()
    {
        var request = new UpdatePlayerStatisticsRequest()
        {
            Statistics = new List<StatisticUpdate>()
        };
        
        StatisticUpdate tmp = new StatisticUpdate();
        tmp.StatisticName = "Run Timer";
        tmp.Value = _playerTime;
        request.Statistics.Add(tmp);
        Debug.Log("New player time is " + _playerTime);
        
        PlayFabClientAPI.UpdatePlayerStatistics(request, UpdateLeaderboardSuccess, UpdateLeaderboardFailed);
    }

    private void UpdateLeaderboardSuccess(UpdatePlayerStatisticsResult obj)
    {
        Debug.Log("Leaderboard Updated Successfully");
        GetLeaderBoard();
    }
    private void UpdateLeaderboardFailed(PlayFabError obj)
    {
        Debug.Log("Failed to Post Time to Leaderboard");
        Debug.Log(obj.Error);
    }


    // Get Leaderboard
    private void GetLeaderBoard()
    {
        var request = new GetLeaderboardAroundPlayerRequest()
        {
            StatisticName = "Run Timer",
            MaxResultsCount = 10,
        };

        PlayFabClientAPI.GetLeaderboardAroundPlayer(request, GetLeaderboardSuccess, GetLeaderboardFail);
    }

    private void GetLeaderboardSuccess(GetLeaderboardAroundPlayerResult obj)
    {
        foreach (var tmp in obj.Leaderboard)
        {
            LeaderboardEntry var = new LeaderboardEntry(tmp.Position, tmp.StatValue, tmp.DisplayName);
            _leaderboardEntries.Add(var);
        }
        _leaderboardScript.LoadLeaderboard(_leaderboardEntries);
        _LeaderboardPanel.gameObject.SetActive(true);
        _VictoryScreen.gameObject.SetActive(false);
    }
    private void GetLeaderboardFail(PlayFabError obj)
    {
        Debug.Log("Get Leaderboard failed " + obj.ErrorMessage);
    }

}
public class LeaderboardEntry
    {
        private int position;
        private string name;
        private int score;

        public LeaderboardEntry(int tPos, int TScore, string tName)
        {
            position = tPos;
            score = TScore;
            name = tName;
        }
        public int GetPosition()
        {
            return position;
        }
        public int GetScore()
        {
            return score;
        }
        public string GetName()
        {
            return name;
        }
    }