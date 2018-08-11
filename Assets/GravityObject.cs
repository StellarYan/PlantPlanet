using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityObject : MonoBehaviour {
	public float mass;
	public Vector3 velocity;
	public Vector3 cachedAcc;
	public Vector3 position { get { return transform.position; } set { transform.position = value; } }
	public float radius;
	public bool active;


	



	private void Awake()
	{
		GameManager.Instance.gravityObjectList.Add(this);
		radius = GetComponent<SphereCollider>().radius;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawLine(position, position + velocity);
	}


}
