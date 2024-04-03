using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public EndGameController endGameController;
    public RoundController roundController;
	public EnemyDirector enemyDirector;
	public PlayersHealthManager playersHealthManager;
    public EventsController eventsController;
    public MultipleTargetCamera multipleTargetCamera;
    public SelectPlayerController selectPlayerController;
	public GMSceneManager gmSceneManager;

	private void Awake()
    {
        if(Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
        }

        if(selectPlayerController!=null) selectPlayerController.Initialize();
    }
}
