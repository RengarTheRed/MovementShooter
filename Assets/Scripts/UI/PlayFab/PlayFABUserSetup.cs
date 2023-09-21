using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayFABUserSetup : MonoBehaviour
{
    [SerializeField] private TMP_InputField _inputFieldDisplayName;
    [SerializeField] private PlayerInput _playerInput;

    private string _usernameString;
    private int _playerTime=0;

    [SerializeField] private Leaderboard _leaderboardScript;
    private List<LeaderboardEntry> _leaderboardEntries = new List<LeaderboardEntry>();

    [SerializeField] private Transform _VictoryScreen;
    [SerializeField] private Transform _LeaderboardPanel;
    
    // Disables / Enables Player Input on Show
    private void OnEnable()
    {
        //SetupPanel();
        StartCoroutine(StartDelay());
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
        //RegisterPlayer();
        LoginPlayer();
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
        GetLeaderBoard();
        
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
            Debug.Log(_inputFieldDisplayName.text);
            var request = new UpdateUserTitleDisplayNameRequest()
            {
                DisplayName = _inputFieldDisplayName.text
            };
            PlayFabClientAPI.UpdateUserTitleDisplayName(request, UpdateUserTitleSuccess, UpdateUserTitleFailed);
        }
    }


    private void UpdateUserTitleSuccess(UpdateUserTitleDisplayNameResult obj)
    {
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
        //Debug.Log("Register user successfully");
        LoginPlayer();
    }
    private void OnRegisterFailure(PlayFabError obj)
    {
        Debug.Log("Register Failed " +obj.ErrorMessage);
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
        Debug.Log("Player logged in Successfully");
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
    }
    private void GetLeaderboardFail(PlayFabError obj)
    {
        Debug.Log("Get Leaderboard failed " + obj.ErrorMessage);
    }

    //DELETE BEFORE PUBLISH
    IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(1f);
        SetupPanel();
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