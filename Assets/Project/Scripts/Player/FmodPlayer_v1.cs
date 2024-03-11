using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FmodPlayer_v1 : MonoBehaviour {

        void PlaySwordWhooshEvent(string path)
        {
            FMODUnity.RuntimeManager.PlayOneShot(path, GetComponent<Transform>().position);
        }
}