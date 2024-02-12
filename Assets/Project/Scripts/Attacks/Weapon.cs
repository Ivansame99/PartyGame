using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    //Variables que tienen que ser publicas
    public string weaponName;
    public List<AttackSO> combo;
    public float damage;
    public float pushForce;

    private GameObject target;
    private bool attack;

    public GameObject arrow;

	public GameObject trailParent;
	public TrailRenderer[] trails;
}
