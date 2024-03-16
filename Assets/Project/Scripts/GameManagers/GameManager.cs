using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public EndGameController endGameController;
    public RoundController roundController;
    public PlayersRespawn playersRespawn;
    public EventsController eventsController;

    private void Awake()
    {
        if(Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
        }
    }
}
