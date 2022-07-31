using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public GameObject cam,mainCamera;
    public Material collisionMaterial;
    public PlayerState playerState;
    public List<GameObject> collisionGO = new List<GameObject>();
    public GameObject collectedPoolTransform,lvlPanel;
    public bool isFinished;
    public GameObject particles;
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
        AdsManager.instance.ShowInit();
        BG.Stop();
        lvlPanel.SetActive(true);
    }
    public void NextLevel()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void Home()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
