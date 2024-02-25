using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class CameraEntryCinematicController : MonoBehaviour
{
    [SerializeField]
    private GameObject gameManager;

    private Animator anim;

    private bool onlyOnce=true;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        gameManager.SetActive(false);    
    }

    // Update is called once per frame
    void Update()
    {
		if (!AnimatorIsPlaying("EntryAnimation") && onlyOnce)
        {
            gameManager.SetActive(true);
            this.anim.enabled = false;
			onlyOnce=false;
        }

	}

	bool AnimatorIsPlaying()
	{
		return anim.GetCurrentAnimatorStateInfo(0).length > anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
	}

	bool AnimatorIsPlaying(string stateName)
	{
		return AnimatorIsPlaying() && anim.GetCurrentAnimatorStateInfo(0).IsName(stateName);
	}
}
