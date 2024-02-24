using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenuController : MonoBehaviour
{
    private Animator anim;
    bool pause = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        
    }

    void Update()
    {
		if (Gamepad.current != null && Gamepad.current.startButton.wasPressedThisFrame)
		{
            if (pause)
            {
				Time.timeScale = 1.0f;
				pause = false;
				anim.SetBool("PauseAppear", false);
			} else
            {
                Time.timeScale = 0.0f;
                pause = true;
				anim.SetBool("PauseAppear", true);
			}
		}
	}
}
