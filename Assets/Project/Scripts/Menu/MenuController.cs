using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MenuController : MonoBehaviour
{
    public void Start()
    {
        Destroy(GameObject.Find("PlayerMultiManager"));
    }

    public void PlayButton()
    {
        SceneManager.LoadScene("HUB");
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}
