using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityObject : MonoBehaviour {
	public float mass;
	public Vector3 velocity;
	public Vector3 cachedAcc;
	public Vector3 position { get { return transform.position; } set { transform.position = value; } }
	public float radius;
	public bool simulationActive;
	public bool Fixed;









	private void Awake()
	{
		GameManager.Instance.gravityObjectList.Add(this);
		if(GetComponent<SphereCollider>()!=null)
		radius = GetComponent<SphereCollider>().radius * transform.localScale.z;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawLine(position, position + velocity);
	}

	private void Update()
	{



		velocity.z = 0;
		cachedAcc.z = 0;
		position = new Vector3(position.x, position.y, 0);
	}


}
