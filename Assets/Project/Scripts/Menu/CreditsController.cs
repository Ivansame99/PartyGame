using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CreditsController : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
		if ((Gamepad.current != null && Gamepad.current.buttonEast.isPressed) || (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame))
		{
			SceneManager.LoadScene("Menu");
		}
	}
}
