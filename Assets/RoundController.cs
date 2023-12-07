using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class RoundController : MonoBehaviour
{
    [SerializeField]
    private Transform[] spawns;

    [SerializeField]
    private GameObject enemy1Prefab;

    [SerializeField]
    private int[] enemiesInRound;

    [SerializeField]
    private float secondsBetweenRounds;

    [SerializeField]
    private float secondsBetweenEnemySpawn;

    private int roundIndex;

    private List<GameObject> currentEnemies;

    private GameObject[] playersInGame;

    [SerializeField]
    private Animator coliseumAnimator;

    private int playersCount;

    public bool finalRound;

    public int playersDied;

    [SerializeField]
    private GameObject roundsUI;

    private Animator roundUIAnim;

    [SerializeField]
    private TMP_Text roundsUIText;
    // Start is called before the first frame update
    void Start()
    {
        roundUIAnim = roundsUI.GetComponent<Animator>();
        playersDied = 0;
        finalRound = false;
        playersInGame = GameObject.FindGameObjectsWithTag("Player");
        playersCount = playersInGame.Length;
        //Invoke("GetPlayers",0.5f);        
        roundIndex = 0;
        currentEnemies = new List<GameObject>();
        //Invoke("StartNextRound", secondsBetweenRounds);
        StartCoroutine(IStartNextRound());
    }

    // Update is called once per frame
    void Update()
    {
        if (roundIndex <= enemiesInRound.Length) CheckCurrentEnemiesDeath();
        else
        {
            if (!finalRound)
            {
                roundsUIText.text = "Final Round";
                roundUIAnim.SetTrigger("ChangeRound");
            }
            finalRound = true;
            if (playersCount > 1) CheckEndGame();
        }
    }

    /*void GetPlayers()
    {
        playersInGame = GameObject.FindGameObjectsWithTag("Player");
        playersCount = playersInGame.Length;
    }*/

    /*void StartNextRound()
    {

        int enemyNumberInCurrentRound = enemiesInRound[roundIndex];

        for (int i = 0; i < enemyNumberInCurrentRound; i++)
        {
            int randomSpawn = Random.Range(0, spawns.Length);
            currentEnemies.Add(Instantiate(enemy1Prefab, spawns[randomSpawn].position, enemy1Prefab.transform.rotation));

        }

        roundIndex++;
    }*/

    IEnumerator IStartNextRound()
    {
        roundsUIText.text = "Round " + ToRoman(roundIndex+1);
        roundUIAnim.SetTrigger("ChangeRound");
        yield return new WaitForSeconds(secondsBetweenRounds);

        coliseumAnimator.SetBool("DoorOpen", true);
        yield return new WaitForSeconds(1);

        int enemyNumberInCurrentRound = enemiesInRound[roundIndex] + playersCount;

        for (int i = 0; i < enemyNumberInCurrentRound; i++)
        {
            int randomSpawn = Random.Range(0, spawns.Length);
            yield return new WaitForSeconds(secondsBetweenEnemySpawn);
            currentEnemies.Add(Instantiate(enemy1Prefab, spawns[randomSpawn].position, enemy1Prefab.transform.rotation));

        }
        coliseumAnimator.SetBool("DoorOpen", false);
        roundIndex++;
    }

    void CheckCurrentEnemiesDeath()
    {
        if (currentEnemies.Count == 0) return;

        for (int i = 0; i < currentEnemies.Count; i++)
        {
            if (currentEnemies[i] != null) return; //Si hay alguno que no sea null, para de mirar el resto
        }

        //Si todos son null

        currentEnemies.Clear();
        //Invoke("StartNextRound", secondsBetweenRounds);

        if (roundIndex < enemiesInRound.Length) StartCoroutine(IStartNextRound());
        else if (roundIndex == enemiesInRound.Length) roundIndex++;

        //if()
        //Debug.Log(currentEnemies[0]);
    }

    void CheckEndGame()
    {
        if (playersDied == playersCount - 1) //Solo queda uno en pie
        {
            Debug.Log("Has ganado!!");
            Invoke("EndMatch", 5f);
        }
    }

    void EndMatch()
    {
        SceneManager.LoadScene("Win");
    }

    // Integer to Roman numerals - mgear - http://unitycoder.com/blog/
    // Unity3D version converted from this: http://rosettacode.org/wiki/Roman_numerals/Encode

    string ToRoman(int number)
    {
        if (number < 1) return string.Empty;
        if (number >= 1000) return "M" + ToRoman(number - 1000);
        if (number >= 900) return "CM" + ToRoman(number - 900);
        if (number >= 500) return "D" + ToRoman(number - 500);
        if (number >= 400) return "CD" + ToRoman(number - 400);
        if (number >= 100) return "C" + ToRoman(number - 100);
        if (number >= 90) return "XC" + ToRoman(number - 90);
        if (number >= 50) return "L" + ToRoman(number - 50);
        if (number >= 40) return "XL" + ToRoman(number - 40);
        if (number >= 10) return "X" + ToRoman(number - 10);
        if (number >= 9) return "IX" + ToRoman(number - 9);
        if (number >= 5) return "V" + ToRoman(number - 5);
        if (number >= 4) return "IV" + ToRoman(number - 4);
        if (number >= 1) return "I" + ToRoman(number - 1);
        return "Impossible state reached";
        //throw new UnreachableException("Impossible state reached");
    }
}
