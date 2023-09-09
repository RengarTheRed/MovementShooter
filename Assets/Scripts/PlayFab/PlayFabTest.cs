using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using Unity.VisualScripting.Dependencies.NCalc;

public class PlayFabTest : MonoBehaviour
{
    private string uName = "Test User";
    public void Start()
    {
        //var request = new LoginWithCustomIDRequest { CustomId = uName, CreateAccount = true};
        //PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
        
        //PlayFabClientAPI.LoginWithPlayFab();
        Register();
        //PostLeaderBoardTime();
    }

    private void OnLoginFailure(PlayFabError obj)
    {
        Debug.Log("Failed login");
    }

    private void OnLoginSuccess(LoginResult obj)
    {
        Debug.Log("Success login");
    }

    private void Register()
    {
        //var request = new RegisterPlayFabUserRequest();
        /*var request = new RegisterPlayFabUserRequest()
        {
            TitleId = PlayFabSettings.TitleId,
            Username = "NewUser",
            //Email = "JamesBond@Mi7.com",
            Password = "Password123",
            //RequireBothUsernameAndEmail = false
        };*/
        
        var request = new LoginWithPlayFabRequest()
        {
            TitleId = PlayFabSettings.TitleId,
            Username = "NewUser",
            //Email = "JamesBond@Mi7.com",
            Password = "Password123",
            //RequireBothUsernameAndEmail = false
        };

        //PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnRegisterFailure);
        PlayFabClientAPI.LoginWithPlayFab(request, OnRegisterSuccess, OnRegisterFailure);
    }

    void PostLeaderBoardTime()
    {
        PlayFabClientAPI.UpdatePlayerStatistics( new UpdatePlayerStatisticsRequest 
            {
                // request.Statistics is a list, so multiple StatisticUpdate objects can be defined if required.
                Statistics = new List<StatisticUpdate> {
                    new StatisticUpdate { StatisticName = "Run Timer", Value = 18 },
                }
            },
            result => { Debug.Log("User statistics updated"); },
            error => { Debug.LogError(error.GenerateErrorReport()); });
    }

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

    private void LeaderboardFailed(PlayFabError obj)
    {
        Debug.Log("Leaderboard Failed");
    }

    private void LeaderboardSuccess(UpdatePlayerStatisticsResult obj)
    {
        Debug.Log("Leaderboard Worked");
    }

    private void OnRegisterSuccess(LoginResult obj)
    {
        PostLeaderBoardTime();
        GetLeaderBoard();
    }

    private void OnRegisterFailure(PlayFabError obj)
    {
        Debug.Log("Failed Register");
        Debug.Log(obj.Error);
    }

    private void OnRegisterSuccess(RegisterPlayFabUserResult obj)
    {
        Debug.Log("Success Register");
    }
}