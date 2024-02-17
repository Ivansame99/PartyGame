using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialController : MonoBehaviour
{
    [SerializeField]
    private TMP_Text dialogBoxText;

    [SerializeField]
    private float textSpeed;

	[SerializeField]
	private string[] lines;

    private int index=0;

    private int tutorialSteps;

    // Start is called before the first frame update
    void Start()
    {
		dialogBoxText.text = string.Empty;
        StartDialogue();
	}

    // Update is called once per frame
    void Update()
    {
        
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            dialogBoxText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }
}
