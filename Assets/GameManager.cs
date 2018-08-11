using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GameManager : MonoBehaviour {
	public List<GravityObject> gravityObjectList;
	public List<GameObject> planetPrefabList;
	public static GameManager Instance { get; private set; }
	public const float GravitationalConstant = 6.67408f; //实际上是6.67408*10^-11 m kg s
	public float timescale;

	private void Awake()
	{
		if(Instance==null)
		{
			Instance = this;
		}
		else
		{
			Destroy(this);
			return;
		}
	}

	public Vector3 MyGravityAcc(float otherMass, Vector3 myPosition, Vector3 otherPosition)
	{
		return ((GravitationalConstant * otherMass) / (otherPosition - myPosition).magnitude) *
					(otherPosition - myPosition).normalized; ;
	}


	void Simulation(float deltaTime)
	{
		foreach (var g in gravityObjectList)
		{
			if (!g.active) continue;
			Vector3 allAcc = Vector3.zero;
			foreach (var otherG in gravityObjectList)
			{
				if (!otherG.active) continue;
				if (otherG == g) continue;
				if ((otherG.position - g.position).sqrMagnitude < (otherG.radius + g.radius) * (otherG.radius + g.radius)) continue;
				allAcc += MyGravityAcc(otherG.mass, g.position, otherG.position);
			}
			g.cachedAcc = allAcc;
		}
		foreach (var g in gravityObjectList)
		{
			if (!g.active) continue;
			g.position += g.velocity * deltaTime + 0.5f * g.cachedAcc * (deltaTime * deltaTime);
			g.velocity += g.cachedAcc * deltaTime;
		}
	}

	public void InitPlanet(GameObject prefab,Vector3 pivotPosition,Color trailColor,float mass,float radius,Vector3 initVelocity)
	{
		GameObject planet = GameObject.Instantiate(prefab, pivotPosition, prefab.transform.rotation);
		planet.GetComponent<TrailRenderer>().material.color = trailColor;
		planet.GetComponent<GravityObject>().mass = mass;
		planet.GetComponent<GravityObject>().radius = radius;
		planet.GetComponent<GravityObject>().velocity = initVelocity;
	}

	
	// Update is called once per frame
	void Update () {
		Simulation(Time.deltaTime* timescale);

		if(Input.GetMouseButtonDown(0))
		{
			Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), 9999999, LayerMask.GetMask("Planet"));

		}

		Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), 9999999, LayerMask.GetMask("Ecliptic"));
	}
}
