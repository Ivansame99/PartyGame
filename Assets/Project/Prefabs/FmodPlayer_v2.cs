using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class FmodPlayer_v2 : MonoBehaviour {
    private float distance = 0.1f;
    private float Material;

    void PlayMeleeEvent(string path)
    {
        FMODUnity.RuntimeManager.PlayOneShot(path, transform.position);
    }

    void FixedUpdate()
    {
        MaterialCheck();
        Debug.DrawRay(transform.position, Vector2.down * distance, Color.blue);
    }

    void MaterialCheck()
    {
        RaycastHit2D hit;
        
        hit = Physics2D.Raycast(transform.position, Vector2.down, distance, 1 << 31);

        if (hit.collider)
        {
            if (hit.collider.tag == "Material: Sand")
                Material = 1f;
            else if (hit.collider.tag == "Material: Stone")
                Material = 2f;
            else
                Material = 1f;
        }
    }

    void PlayFootstepsEvent(string path)
    {
        FMOD.Studio.EventInstance Footsteps = FMODUnity.RuntimeManager.CreateInstance(path);
        Footsteps.setParameterByName("Material", Material);
        Footsteps.start();
        Footsteps.release();
    }
}