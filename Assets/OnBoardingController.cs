using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnBoardingController : MonoBehaviour
{
	private Animator anim;
	private bool onBoarding = false;

	[SerializeField]
	private GameObject[] practiceDolls;
	// Start is called before the first frame update
	void Start()
	{
		anim = this.GetComponent<Animator>();

		if (PlayerPrefs.GetInt("onBoarding", 0) == 0)
		{
			onBoarding = false;
		}
		else
		{
			onBoarding = true;
			anim.SetBool("Onboarding", true);
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (!onBoarding)
		{
			for(int i = 0; i < practiceDolls.Length; i++)
			{
				if (practiceDolls[i] != null) return;
			}

			onBoarding=true;
			anim.SetBool("Onboarding", true);
			PlayerPrefs.SetInt("onBoarding", 1);
		}
	}
}
