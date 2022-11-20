using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PlayerManager : MonoBehaviour
{
    public Text lvlComplete_txt;
    public GameObject cam,mainCamera;
    public Material[] collisionMaterial;
    public PlayerState playerState;
    public List<GameObject> collisionGO = new List<GameObject>();
    public GameObject collectedPoolTransform,lvlPanel;
    public bool isFinished;
    public GameObject particles;
    public int multiplier=3;
    public AudioSource BG;
    public enum PlayerState
    {
        Move,
        Stop
    }
    public static PlayerManager instance;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        mainCamera.GetComponent<Camera>().backgroundColor = new Color(Random.value, Random.value, Random.value, 1);
    }
    public void CallSphere()
    {
        foreach(GameObject obj in collisionGO)
        {
            obj.GetComponent<CollectableObjectController>().SphereOn();
        }
        cam.gameObject.SetActive(false);
        Invoke("LevelCompleteDis",3f);
    }
    void LevelCompleteDis()
    {
        //AdsManager.instance.ShowInit();
        BG.Stop();
        multiplier=PlayfabManager.instance.GetMultiplier();
        var r = Random.Range(10, 15);
        var c = multiplier * r;
        lvlComplete_txt.text = multiplier + " " + "X" + " " + r+" "+"="+" "+c.ToString();
        PlayfabManager.instance.Send_Leaderboard(c);
        lvlPanel.SetActive(true);
    }
    public void NextLevel()
    {
        PlayfabManager.instance.GetLeaderboard();
        SceneManager.LoadScene("SampleScene");
    }
    public void Home()
    {
        PlayfabManager.instance.GetLeaderboard();
        SceneManager.LoadScene("MainMenu");
    }
}
