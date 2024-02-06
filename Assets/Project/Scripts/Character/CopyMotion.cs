using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyMotion : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    private bool mirror;

    private ConfigurableJoint joint;
    // Start is called before the first frame update
    void Start()
    {
        joint = GetComponent<ConfigurableJoint>();
    }

    // Update is called once per frame
    void Update()
    {
        if (mirror)
        {
            joint.targetRotation = Quaternion.Inverse(target.rotation);
        }
        else
        {
            joint.targetRotation = target.rotation;
        }
    }
}
