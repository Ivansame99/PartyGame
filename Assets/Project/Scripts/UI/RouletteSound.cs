using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouletteSound : MonoBehaviour
{
    // Start is called before the first frame update
    public void RouletteScrollSound()
    {
    FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Roulette", transform.position);      
    }
    public void RouletteFinishSound()
    {
    FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Roulette_Accept", transform.position);      
    }
}
