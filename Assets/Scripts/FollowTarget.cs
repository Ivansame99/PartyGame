using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private Vector3 offset;

    private void Update()
    {
        transform.position = new Vector3(target.position.x, target.position.y + offset.y + target.localScale.y*2, target.position.z);
    }
}
