using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArenaSelectCell : MonoBehaviour
{
	public GameEnums.Arenas arena;

	[SerializeField]
	private List<Sprite> spriteList;

	public void Setup()
	{
		int index = Random.Range(0, spriteList.Count);

		GetComponent<Image>().sprite = spriteList[index];

		arena = (GameEnums.Arenas) index;
	}
}
