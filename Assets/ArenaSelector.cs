using System.Collections.Generic;
using UnityEngine;

public class ArenaSelector : MonoBehaviour
{
	[SerializeField]
	private GameObject prefab;

	[SerializeField]
	private GameObject scrollGameObject;

	[SerializeField]
	private GameObject arrow;

	private float speed;

	private bool isScrolling;

	private List<ArenaSelectCell> cells = new List<ArenaSelectCell>();

	public void Scroll()
	{
		if (isScrolling)
		{
			return;
		}

		scrollGameObject.GetComponent<RectTransform>().localPosition = new Vector3(1080,0);

		speed = Random.Range(3,5);

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
		scrollGameObject.transform.position = Vector2.MoveTowards(scrollGameObject.transform.position, scrollGameObject.transform.position + Vector3.left * 100, speed * Time.deltaTime *1500);

		if (speed > 0)
		{
			speed-=Time.deltaTime;
		} else
		{
			speed = 0;
			isScrolling = false;
		}
	}
}
