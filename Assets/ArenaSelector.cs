using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class ArenaSelector : MonoBehaviour
{
	[SerializeField]
	private Camera guiCamera;

	[SerializeField]
	private GameObject prefab;

	[SerializeField]
	private GameObject scrollGameObject;

	[SerializeField]
	private GameObject arrow;

	private float maxSpeed;
	private float speed;

	private bool isScrolling;

	private List<ArenaSelectCell> cells = new List<ArenaSelectCell>();

	private bool start;

	private GameEnums.Arenas selectedArena = GameEnums.Arenas.None;

	public void Scroll()
	{
		if (isScrolling)
		{
			return;
		}

		scrollGameObject.GetComponent<RectTransform>().localPosition = new Vector3(1080,0);

		maxSpeed = Random.Range(2,4);
		speed = 1;
		start = true;
		isScrolling = true;

		arrow.SetActive(true);

		if (cells.Count == 0)
		{
			for(int i = 0; i<50; i++)
			{
				cells.Add(Instantiate(prefab, scrollGameObject.transform).GetComponentInChildren<ArenaSelectCell>());
			}
		}

		foreach(ArenaSelectCell cell in cells)
		{
			cell.Setup();
		}
	}

	private void Update()
	{
		if (!isScrolling) return;

		scrollGameObject.transform.position = Vector2.MoveTowards(scrollGameObject.transform.position, scrollGameObject.transform.position + Vector3.left * 100, speed * Time.deltaTime *1500);

		if (start && speed < maxSpeed)
		{
			speed += Time.deltaTime;
		} else if(speed > maxSpeed)
		{
			start = false;
		}

		if (!start && speed > 0)
		{
			speed-=Time.deltaTime;
		} else if (!start && speed <= 0)
		{
			DetectArena();
			speed = 0;
			isScrolling = false;
		}
	}

	private void DetectArena()
	{
		Vector3 centerPosition = guiCamera.WorldToScreenPoint(scrollGameObject.transform.position);

		float minDistance = float.MaxValue;
		ArenaSelectCell selectedCell = null;

		foreach (ArenaSelectCell cell in cells)
		{
			Vector3 cellPosition = guiCamera.WorldToScreenPoint(cell.transform.position);
			float distance = Vector3.Distance(centerPosition, cellPosition);

			if (distance < minDistance)
			{
				minDistance = distance;
				selectedCell = cell;
			}
		}

		if (selectedCell != null)
		{
			selectedArena = selectedCell.arena;
		} else
		{
			selectedArena = GameEnums.Arenas.StandardArena;
		}
	}

	public GameEnums.Arenas GetSelectedArena()
	{
		return selectedArena;
	}
}
