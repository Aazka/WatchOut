using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;

public class PlayfabManager : MonoBehaviour
{
    //private string userEmail, userPassword;
    public static PlayfabManager instance;
    private int multiplier;
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    private void Start()
    {
        Login();
        
    }
    public int GetMultiplier()
    {
        return multiplier;
    }
    #region Login
    void Login()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };
        Debug.Log("?");
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginReject);//Simple Annoymous account
       
        //var request = new LoginWithEmailAddressRequest {Email=userEmail,Password=userPassword };
        //PlayFabClientAPI.LoginWithEmailAddress(request, OnSuccessEmailLogin, OnRejectEmailLogin);
    }
    void OnLoginSuccess(LoginResult result)// simple login success
    {
        Debug.Log("Login SuccessFull");
        GetMessage();

    }
    void OnLoginReject(PlayFabError error)// simple login reject
    {
        Debug.Log("LoginFailed");
    }
    #endregion
    #region Leaderboard
    public void Send_Leaderboard(int score)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName="PlateformScore",
                    Value=score
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, LeaderBoardUpdate, OnLoginReject);
    }
    void LeaderBoardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Successfully DataSend To leaderboard");
    }
    public void GetLeaderboard()
    {
        var result = new GetLeaderboardRequest
        {
            StatisticName = "PlateformScore",
            StartPosition = 0,
            MaxResultsCount = 10
        };
        PlayFabClientAPI.GetLeaderboard(result, OnLeaderboardGet, OnLoginReject);
    }
    void OnLeaderboardGet(GetLeaderboardResult result)
    {
        foreach(var item in result.Leaderboard)
        {
            Debug.Log(item.Position + " " + item.PlayFabId + " " + item.StatValue);
        }
    }
    #endregion
    #region SaveDataServer/UserData
    public void GetSaveData()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), GetData, OnLoginReject);
    }
    void GetData(GetUserDataResult result)
    {
        if(result.Data!=null&&result.Data.ContainsKey("Look"))
        {
            CharacterSelection.instance.selectedObject =int.Parse(result.Data["Look"].Value);
            CharacterSelection.instance.SelectedObejct();
        }
    }
    public void SaveUserData()
    {
        var Request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                {"Look",CharacterSelection.instance.selectedObject.ToString() }
            }
        };
        PlayFabClientAPI.UpdateUserData(Request, OnDataRecive, OnLoginReject);
    }
    void OnDataRecive(UpdateUserDataResult result)
    {
        Debug.Log("Data Save");
    }
    #endregion
    #region LoginEmail
    //void OnSuccessEmailLogin(LoginResult result)
    //{
    //    Debug.Log("Congratulation you have successfully logged in");
    //}
    //void OnRejectEmailLogin(PlayFabError error)
    //{
    //  //we will call another call of APi to register the user oherwise it will be infinite loop

    //}
    //public void GetUserEmail(string email)
    //{
    //    userEmail = email;
    //}
    //public void GetUserEmailPassword(string password)
    //{
    //    userPassword = password;
    //}
    //public void LoggedInButton()
    //{
    //    var request = new LoginWithEmailAddressRequest { Email = userEmail, Password = userPassword };
    //    PlayFabClientAPI.LoginWithEmailAddress(request, OnSuccessEmailLogin, OnRejectEmailLogin);
    //}
    #endregion
    #region TitleData
    void GetMessage()
    {
        PlayFabClientAPI.GetTitleData(new GetTitleDataRequest(), GetTitleDataRuntime, OnLoginReject);
    }
    void GetTitleDataRuntime(GetTitleDataResult result)
    {
        if(result.Data==null || !result.Data.ContainsKey("Message") || !result.Data.ContainsKey("multiplier_Score"))
        {
            Debug.Log("No Message");
            return;
        }
        else
        {
            CharacterSelection.instance.messageText.text = result.Data["Message"];
            multiplier =int.Parse(result.Data["multiplier_Score"]);
        }
    }
    #endregion
}
