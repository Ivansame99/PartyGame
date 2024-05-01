using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using static System.TimeZoneInfo;

public class GMSceneManager : MonoBehaviour
{
	#region Inspector Variables
	[Header("Circle Transition")]
	[SerializeField]
	private Material transitionMaterial;
	[SerializeField]
	private float transitionTime = 2f;
	[SerializeField]
	private string propertyName = "_Progress";
	#endregion

	#region Change Scene Methods
	public void ChangeSceneToMenu(bool transition=false, float waitTime=0)
    {
		if (transition)
		{
			StartCoroutine(CloseTranition(GameEnums.Scenes.Menu, waitTime));
		} else
		{
			SceneManager.LoadScene(GameEnums.Scenes.Menu.ToString());
		}	
	}

	public void ChangeSceneToHUB(bool transition = false, float waitTime = 0)
	{
		if (transition)
		{
			StartCoroutine(CloseTranition(GameEnums.Scenes.HUB, waitTime));
		}
		else
		{
			SceneManager.LoadScene(GameEnums.Scenes.HUB.ToString());
		}
	}

	public void ChangeSceneToCredits(bool transition = false, float waitTime = 0)
	{
		if (transition)
		{
			StartCoroutine(CloseTranition(GameEnums.Scenes.Credits, waitTime));
		}
		else
		{
			SceneManager.LoadScene(GameEnums.Scenes.Credits.ToString());
		}
	}

	public void ChangeSceneToArena1(bool transition = false, float waitTime = 0)
	{
		if (transition)
		{
			StartCoroutine(CloseTranition(GameEnums.Scenes.Arena1, waitTime));
		}
		else
		{
			SceneManager.LoadScene(GameEnums.Scenes.Arena1.ToString());
		}
	}

	public void ChangeSceneToArenaSnow(bool transition = false, float waitTime = 0)
	{
		if (transition)
		{
			StartCoroutine(CloseTranition(GameEnums.Scenes.ArenaSnow, waitTime));
		}
		else
		{
			SceneManager.LoadScene(GameEnums.Scenes.ArenaSnow.ToString());
		}
	}

	public void ChangeSceneToArenaLeaf(bool transition = false, float waitTime = 0)
	{
		if (transition)
		{
			StartCoroutine(CloseTranition(GameEnums.Scenes.ArenaLeaf, waitTime));
		}
		else
		{
			SceneManager.LoadScene(GameEnums.Scenes.ArenaLeaf.ToString());
		}
	}

	public void ChangeSceneToGameOver(bool transition = false, float waitTime = 0)
	{
		PlayerPrefs.SetInt("arenaType", (int)GetArenaType());
		PlayerPrefs.Save();

		if (transition)
		{
			StartCoroutine(CloseTranition(GameEnums.Scenes.GameOver, waitTime));
		}
		else
		{
			SceneManager.LoadScene(GameEnums.Scenes.GameOver.ToString());
		}
	}
	#endregion

	#region Utils
	public bool isHubScene()
	{
		if (SceneManager.GetActiveScene().name == GameEnums.Scenes.HUB.ToString())
		{
			return true;
		}

		return false;
	}

	public GameEnums.Arenas GetArenaType()
	{
		if (SceneManager.GetActiveScene().name == GameEnums.Scenes.Arena1.ToString())
		{
			return GameEnums.Arenas.StandardArena;
		}

		if (SceneManager.GetActiveScene().name == GameEnums.Scenes.ArenaSnow.ToString())
		{
			return GameEnums.Arenas.SnowArena;
		}

		if (SceneManager.GetActiveScene().name == GameEnums.Scenes.ArenaLeaf.ToString())
		{
			return GameEnums.Arenas.ArenaLeaf;
		}

		return GameEnums.Arenas.None;
	}

	#endregion

	#region Coroutines
	private IEnumerator CloseTranition(GameEnums.Scenes scene, float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		float currentTime = transitionTime;
		while (currentTime > 0)
		{
			currentTime -= Time.deltaTime;
			transitionMaterial.SetFloat(propertyName, Mathf.Clamp01(currentTime / transitionTime));
			yield return null;
		}
		Time.timeScale = 1.0f;
		SceneManager.LoadScene(scene.ToString());
	}
	#endregion
}
