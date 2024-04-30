using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PowerCircle : MonoBehaviour
{
	[SerializeField]
	private TMP_Text text;

	[SerializeField]
	private Transform circleParticles1;

	[SerializeField]
	private Transform circleParticles2;

	[SerializeField]
	private float reduceSpeed;

	[SerializeField]
	private float powerReceive;

	[SerializeField]
	private GameObject cage;

	[SerializeField]
	private GameObject explosionParticles;

	[SerializeField]
	private Rigidbody[] particles;

	private int currentPlayersIn;
	private int expectedPlayers;

	private List<PowerController> powerControllerList = new List<PowerController>();

	private void Start()
	{
		int numPlayers = GameManager.Instance.selectPlayerController.GetNumPlayers();
		expectedPlayers = Random.Range(1, numPlayers + 1);
		Initialize(expectedPlayers);
	}

	private void Update()
	{
		if (currentPlayersIn == expectedPlayers)
		{
			//circleParticles1.localScale -= new Vector3(reduceSpeed * Time.deltaTime, reduceSpeed * Time.deltaTime, 0f);
			circleParticles2.localScale -= new Vector3(reduceSpeed * Time.deltaTime, reduceSpeed * Time.deltaTime, 0f);

			if (circleParticles2.localScale.x <= 0f && circleParticles2.localScale.y <= 0f)
			{
				//for(int i=0; i < powerControllerList.Count; i++)
				//{
				//	powerControllerList[i].AddPowerLevel(powerReceive);
				//}

				Instantiate(explosionParticles, cage.transform.position, Quaternion.identity);
				Destroy(cage);

				for (int i = 0; i < particles.Length; i++)
				{
					particles[i].useGravity = true;
					particles[i].AddForce(new Vector3(Random.Range(-0.3f, 0.3f), 0.3f, Random.Range(-0.3f, 0.3f)), ForceMode.Impulse);
					particles[i].transform.parent = null;
				}

				Destroy(this.gameObject);
			}
		}
	}

	public void Initialize(int players)
	{
		//expectedPlayers = players;

		text.text = currentPlayersIn.ToString() + "/" + expectedPlayers.ToString();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			currentPlayersIn++;
			text.text = currentPlayersIn.ToString() + "/" + expectedPlayers.ToString();
			powerControllerList.Add(other.gameObject.GetComponent<PowerController>());
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			currentPlayersIn--;
			text.text = currentPlayersIn.ToString() + "/" + expectedPlayers.ToString();

			PowerController playerPowerController = other.gameObject.GetComponent<PowerController>();

			if (powerControllerList.Contains(playerPowerController))
			{
				powerControllerList.Remove(playerPowerController);
			}
		}
	}
}
