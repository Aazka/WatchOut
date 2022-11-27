using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using Newtonsoft.Json;
public class PlayfabManager : MonoBehaviour
{
    //private string userEmail, userPassword;
    public static PlayfabManager instance;
    private int multiplier;
    public CharacterSelection cs;
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
            CreateAccount = true,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true
            }
        };
        Debug.Log("?");
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginReject);//Simple Annoymous account
       
        //var request = new LoginWithEmailAddressRequest {Email=userEmail,Password=userPassword };
        //PlayFabClientAPI.LoginWithEmailAddress(request, OnSuccessEmailLogin, OnRejectEmailLogin);
    }
    void OnLoginSuccess(LoginResult result)// simple login success
    {
        Debug.Log("Login SuccessFull");
        GetVirtualCurrency();
        if (result.InfoResultPayload.PlayerProfile!=null)
        {
            name = result.InfoResultPayload.PlayerProfile.DisplayName;
        }
        if(name==null)
        {
            cs.introPanel.SetActive(true);
        }
        else
        {
            cs.leaderboardPanel.SetActive(true);
            GetLeaderboard();
        }
        GetMessage();

    }
    public void SubmitNameButton()
    {
        var req = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = cs.introName.text,
        };
        PlayFabClientAPI.UpdateUserTitleDisplayName(req, OnDisplayNameUpdate, OnLoginReject);
    }
    void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult result)
    {
        cs.leaderboardPanel.SetActive(true);
        GetLeaderboard();
    }
    void OnLoginReject(PlayFabError error)// simple login reject
    {
        Debug.Log(error);
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
                },
                //new StatisticUpdate
                //{
                //    StatisticName="PlateformName",
                //    Value=27
                //},
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
        cs.DestroyAllLeaderBoard();
        foreach (var item in result.Leaderboard)
        {
            cs.Instant_LeaderPanel(item.Position.ToString(), item.DisplayName, item.StatValue.ToString());
            //Debug.Log(item.Position + " " + item.PlayFabId + " " + item.StatValue);
        }
    }
    public void OnLeaderBoardAroundThePlayer()
    {
        var request = new GetLeaderboardAroundPlayerRequest
        {
            StatisticName = "PlateformScore",
            MaxResultsCount = 10
        };
        PlayFabClientAPI.GetLeaderboardAroundPlayer(request, OnLeaderBoardGetAroundThePlayers, OnLoginReject);
    }
    void OnLeaderBoardGetAroundThePlayers(GetLeaderboardAroundPlayerResult result)
    {
        cs.DestroyAllLeaderBoard();
        foreach (var item in result.Leaderboard)
        {
            cs.Instant_LeaderPanel(item.Position.ToString(), item.DisplayName, item.StatValue.ToString());
            //Debug.Log(item.Position + " " + item.PlayFabId + " " + item.StatValue);
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
        if(result.Data!=null&&result.Data.ContainsKey("Look")&& result.Data.ContainsKey("Character"))
        {
            cs.selectedObject =int.Parse(result.Data["Look"].Value);
            cs.SelectedObejct();
            UserData user = JsonConvert.DeserializeObject<UserData>(result.Data["Character"].Value);
            cs.DisplayUserData(user);
            //cs.DisplayUserData(JsonConvert.DeserializeObject(result.Data["Character"].Value));
        }
    }
    public void SaveUserData()
    {
        var Request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                {"Look",cs.selectedObject.ToString() },
                {"Character",JsonConvert.SerializeObject(cs.GetDataInfo()) }
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
            cs.messageText.text = result.Data["Message"];
            multiplier =int.Parse(result.Data["multiplier_Score"]);
        }
    }
    #endregion
    #region Cloud
    public void ExcuteCloudData()
    {
        var request = new ExecuteCloudScriptRequest
        {
            FunctionName = "hello",
            FunctionParameter = new
            {
                name=cs.name.text
            }
        };
        PlayFabClientAPI.ExecuteCloudScript(request, OnExecuteSuccess, OnLoginReject);
    }
    void OnExecuteSuccess(ExecuteCloudScriptResult result)
    {
        cs.welcomeText.gameObject.SetActive(true);
        cs.name.gameObject.SetActive(false);
        cs.welcomeText.text = result.FunctionResult.ToString();
    }
    #endregion
    #region Currency
    public void GetVirtualCurrency()
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), GetInventory, OnLoginReject);
    }
    void GetInventory(GetUserInventoryResult result)
    {
        cs.coinDisplay.text = result.VirtualCurrency["CN"].ToString();
    }
    public void BuyItems()
    {
        var request = new SubtractUserVirtualCurrencyRequest
        {
            VirtualCurrency = "CN",
            Amount = cs.coinToDecrease
        };
        PlayFabClientAPI.SubtractUserVirtualCurrency(request, SustractItem, OnLoginReject);
    }
    void SustractItem(ModifyUserVirtualCurrencyResult result)
    {
        Debug.Log("Decrease");
        GetVirtualCurrency();
    }// same will goes for adding currency, but make sure to add some amount in playfab account to add select your player-> virtual currency than add some amount and save
    #endregion
}
