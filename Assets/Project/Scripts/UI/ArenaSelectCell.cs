using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArenaSelectCell : MonoBehaviour
{
    [System.Serializable]
    private class SpriteList
    {
        public List<Sprite> sprites;
    }

    [SerializeField]
    private List<SpriteList> spriteList;

    [SerializeField]
    private int[] chances;

    [SerializeField]
    private Color[] colors;

    public void Setup()
    {
        int index = Randomize();

        GetComponent<Image>().sprite = spriteList[index].sprites[Random.Range(0, spriteList[index].sprites.Count)];
    }

    private int Randomize()
    {
        int id = 1;

        for(int i =0; i < chances.Length; i++)
        {
            int rand = Random.Range(0, 100);

            if(rand > chances[i])
            {
                return i;
            }

            id++;
        }
        return id;
    }

}
