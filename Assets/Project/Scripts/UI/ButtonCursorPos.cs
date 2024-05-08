using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCursorPos : MonoBehaviour
{
    public Transform cursorPos;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SelectButton()
    {
		animator.ResetTrigger("Select");
		animator.SetTrigger("Select");
    }
}
