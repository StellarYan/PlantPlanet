using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Limit : MonoBehaviour {

	private float initRadius;
	private float initColliderRadius;
	public TextMesh limitText;

	public Color ExpandColor;
	public Color NarrowColor;

	public float radius;
	public float lastRadius;

	private void Awake()
	{
		initRadius = transform.localScale.x;
		initColliderRadius = GetComponent<SphereCollider>().radius;
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (radius < 0) radius = 0;
		if (GameStarter.Instance.endlessMode) radius = initRadius;

		if (radius > lastRadius) GetComponent<MeshRenderer>().material.color = ExpandColor;
		else GetComponent<MeshRenderer>().material.color = NarrowColor;
		SetRadius(radius);
		limitText.text = "Radius-" + radius.ToString("0.0");
		lastRadius = radius;
	}

	private void SetRadius(float r)
	{
		transform.localScale = new Vector3(r, r, transform.localScale.z);
		//GetComponent<SphereCollider>().radius = initColliderRadius * (transform.localScale.x/ initRadius);
	}


	private void OnTriggerExit(Collider other)
	{
		if(other.gameObject.layer == LayerMask.NameToLayer("Planet"))
		{
			var planet = other.GetComponent<Planet>();
			if (planet.GetComponent<GravityObject>().simulationActive == true)
				planet.Fracture(other.transform.position);
		}
		if (other.gameObject.layer == LayerMask.NameToLayer("Debris"))
		{
			other.gameObject.GetComponent<LifeTimer>().lifeTime = 0;
		}
	}
}
