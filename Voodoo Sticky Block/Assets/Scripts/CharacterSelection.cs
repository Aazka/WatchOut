using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Text.RegularExpressions;

public class CharacterSelection : MonoBehaviour
{
    public UserData userInfo;
    public int selectedObject = 0,coinToDecrease;
    public Text messageText,coinDisplay;
    public GameObject error,introPanel,leaderboardPanel,initLeaderBoardPanel,rowParent;
    public InputField name,introName;
    public Text welcomeText;
    public static CharacterSelection instance;
    [SerializeField] private InputField userName,age,boxSize;
    [SerializeField] private Dropdown gender;
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
    
        if (PlayerPrefs.GetInt("PreferenceSave") == 0)
        {
            SaveCash();
            PlayerPrefs.SetInt("PreferenceSave", 1);
        }
        //SelectedObejct();
    }
    public void nextBtn()
    {
  
        if (selectedObject >= transform.childCount - 1)
        {
            selectedObject = 0;
        }
        else
        {
            selectedObject++;
        }
        SelectedObejct();
     
    }
    public void previousBtn()
    {
  
        if (selectedObject <= 0)
        {
            selectedObject = transform.childCount - 1;
        }
        else
        {
            selectedObject--;
        }
        SelectedObejct();
    }
    public void SelectedObejct()
    {
        int i = 0;
        foreach (Transform character in transform)
        {
            if (i == selectedObject)
            {
                character.gameObject.SetActive(true);
                PlayerPrefs.SetInt("Skin", selectedObject);
              
            }
            else
            {
                character.gameObject.SetActive(false);
            }
            i++;
        }
    }
    void SaveCash()
    { 
        PlayerPrefs.SetInt("CharacterCost" + 0, 1);
    }
    public void CloseAllItems()
    {
        foreach (Transform character in transform)
        {
            character.gameObject.SetActive(false);
        }
    }
    public void CheckAgeRegex()
    {
       // string pattern = @"^([0 - 9]|[1 - 9])$";
        Regex reg = new Regex(@"^(\d\d)$", RegexOptions.IgnoreCase);
        Debug.Log(age.text);
        //Debug.Log(regs);
        if(reg.IsMatch(age.text.ToString()))//Regex.IsMatch(age.text,pattern))//reg.IsMatch(age.text))
        {
            Debug.Log("Match");
        }
        else
        {
            age.text = " ";
            Debug.Log("Not_Match");
        }
    }
    public void CheckBoxSizeRegex()
    {
        Regex reg = new Regex(@"^(\d\d)$", RegexOptions.IgnoreCase);
        if (reg.IsMatch(boxSize.text))
        {
            Debug.Log("Match");
        }
        else
        {
            boxSize.text = " ";
            Debug.Log("Not_Match");
        }
    }
    public UserData GetDataInfo()//Save data
    {
        return new UserData(userName.text, gender.captionText.ToString(),int.Parse (age.text), int.Parse(boxSize.text));
    }
    public void DisplayUserData(UserData userData)//Get Data from server
    {
        userName.text = userData.name;
        gender.captionText.text=userData.gender;
        age.text = userData.age.ToString();
        boxSize.text = userData.boxSize.ToString();
    }
    public void CheckIfFiledNotEmpty()
    {
        if(string.IsNullOrEmpty(userName.text)|| string.IsNullOrEmpty(age.text)|| string.IsNullOrEmpty(boxSize.text))
        {
            error.SetActive(true);
        }
        else
        {
            PlayfabManager.instance.SaveUserData();
        }
    }
    public void Instant_LeaderPanel(string position,string displayName,string stats)
    {
        var p = Instantiate(initLeaderBoardPanel, rowParent.transform);
        Text[] t = p.GetComponentsInChildren<Text>();
        t[0].text = position;
        t[1].text = displayName;
        t[2].text = stats;
    }
    public void DestroyAllLeaderBoard()
    {
        for (int i = 0; i < rowParent.transform.childCount; i++)
        {
            Destroy(rowParent.transform.GetChild(i));
        }
    }
    
}
[System.Serializable]
public class UserData
{
    public string name,gender;
    public int age, boxSize;
    public UserData(string uNmae,string uGender, int uAge,int BS)
    {
        name = uNmae;
        gender = uGender;
        age = uAge;
        boxSize = BS;
    }
} 