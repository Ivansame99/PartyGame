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

	[SerializeField]
	private GameObject backgroundImg;

	private float maxSpeed;
	private float speed;

	private bool isScrolling;

	private List<ArenaSelectCell> cells = new List<ArenaSelectCell>();

	private bool start;

	private GameEnums.Arenas selectedArena = GameEnums.Arenas.None;

	[SerializeField] private RouletteSound rouletteSound;

	public void Scroll()
	{
		if (isScrolling)
		{
			return;
		}

		scrollGameObject.GetComponent<RectTransform>().localPosition = new Vector3(1080, 0);

		maxSpeed = Random.Range(2, 3);
		speed = 1;
		start = true;
		isScrolling = true;
		//rouletteSound.RouletteScrollSound();

		arrow.SetActive(true);
		backgroundImg.SetActive(true);

        arrow.SetActive(true);

		if (cells.Count == 0)
		{
			for (int i = 0; i < 50; i++)
			{
				cells.Add(Instantiate(prefab, scrollGameObject.transform).GetComponentInChildren<ArenaSelectCell>());
			}
		}

		foreach (ArenaSelectCell cell in cells)
		{
			cell.Setup();
		}
	}

	private void Update()
	{
		if (!isScrolling) return;

		scrollGameObject.transform.position = Vector2.MoveTowards(scrollGameObject.transform.position, scrollGameObject.transform.position + Vector3.left * 100, speed * Time.deltaTime * 1500);

		if (start && speed < maxSpeed)
		{
			speed += Time.deltaTime;
		}
		else if (speed >= maxSpeed)
		{
			start = false;
		}

		if (!start && speed > 0)
		{
			speed -= Time.deltaTime;
		}
		else if (!start && speed <= 0)
		{
			DetectArena();
			speed = 0;
			isScrolling = false;
		}
	}

	private void DetectArena()
	{
		//rouletteSound.RouletteFinishSound();

        float minDistance = Mathf.Infinity;
		ArenaSelectCell closestCell = null;

		foreach (ArenaSelectCell cell in cells)
		{

			float distance = Vector2.Distance(cell.transform.position, new Vector3(Screen.width / 2f, Screen.height / 2f, 0));

			if (distance < minDistance)
			{
				minDistance = distance;
				closestCell = cell;
			}
		}

		if (closestCell != null)
		{
			selectedArena = closestCell.arena;
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
