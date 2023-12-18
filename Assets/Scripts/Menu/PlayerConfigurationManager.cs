using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerConfigurationManager : MonoBehaviour
{
    private List<PlayerConfiguration> playerConfigs;

    [SerializeField]
    public GameObject[] playerText;

    [SerializeField]
    private int MaxPlayers;
    private int playerIndex=0;

    [Header("Level names")]
    [SerializeField]
    public string firstLevel,levelName;

    [Header("Circle Transition")]
    [SerializeField]
    private Material transitionMaterial;
    [SerializeField]
    private float transitionTime = 3f;
    [SerializeField]
    private string propertyName = "_Progress";

    [Header("Loading")]
    [SerializeField]
    private GameObject loadingCanvas;

    [SerializeField]
    private TextMeshProUGUI loadingNumber;

    public static PlayerConfigurationManager Instance { get; private set; }
    private int firstLevelFinished;

    public bool goToPlay = false;

    public void Awake()
    {
        if (Instance != null)
        {
            //Debug.Log("olis");
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
            playerConfigs = new List<PlayerConfiguration>();
        }
       // transitionMaterial.SetFloat(propertyName, 1);
    }

    public List<PlayerConfiguration> GetPlayerConfigs()
    {
        return playerConfigs;
    }

    public void setPlayerColor(int index, Material color)
    {
        playerConfigs[index].PlayerMaterial = color;
    }

    public void ReadyPlayer(int index)
    {
        playerConfigs[index].IsReady = true;
        if (playerConfigs.Count == MaxPlayers && playerConfigs.All(p => p.IsReady == true))
        {
            //StartCoroutine(CloseTranition());
            //Invoke("ChangeScene", 2.0f);
            //ChangeScene();
        }
    }

    public void HandlePlayerJoin(PlayerInput pi)
    {
        if (!playerConfigs.Any(p => p.PlayerIndex == pi.playerIndex))
        {
            for(int i = 0; i < playerIndex; i++)
            {
                if (playerIndex < playerText.Length)
                {
                    //Debug.Log("AAA");
                    playerText[playerIndex].SetActive(false);
                }
            }
            pi.transform.SetParent(transform);
            playerConfigs.Add(new PlayerConfiguration(pi));
        }
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene(firstLevel);
        //loadingCanvas.SetActive(true);
        //if (firstLevelFinished == 0) StartCoroutine(LoadYourAsyncScene(firstLevel));
        //else StartCoroutine(LoadYourAsyncScene(levelName));

        /*if (firstLevelFinished == 0) SceneManager.LoadScene(firstLevel);
        else SceneManager.LoadScene(levelName);
        StartCoroutine(LoadYourAsyncScene(firstLevel));*/
    }

    private IEnumerator CloseTranition()
    {
        float currentTime = transitionTime;
        while (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            transitionMaterial.SetFloat(propertyName, Mathf.Clamp01(currentTime / transitionTime));
            yield return null;
        }
        ChangeScene();
        
        //StartCoroutine(LoadYourAsyncScene(firstLevel));
    }

    
    IEnumerator LoadYourAsyncScene(string sceneName)
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.
        
        
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            //loadingCanvas.SetActive(true);
            float progressValue = Mathf.Clamp01(asyncLoad.progress/0.9f);
            int progressInt = (int)progressValue * 100;
            loadingNumber.text = progressInt.ToString() + "%";
            yield return null;
        }
    }
}

public class PlayerConfiguration
{
    public PlayerConfiguration(PlayerInput pi)
    {
        PlayerIndex = pi.playerIndex;
        Input = pi;
    }
    public PlayerInput Input { get; set; }
    public int PlayerIndex { get; set; }
    public bool IsReady { get; set; }
    public Material PlayerMaterial { get; set; }
}


