using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;


public class UISoundManager : MonoBehaviour
{
    // Start is called before the first frame update
    public void ChangeButtonSound()
    {
    FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/UpDown", transform.position);      }

    public void SubmitButtonSound()
    {
    FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Accept", transform.position);  
    }

    public void CancelButtonSound()
    {
    FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Back", transform.position); 
    }
}
