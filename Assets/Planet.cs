using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour {
	public float DescriptionAlpha;
	public const float DescriptionDecaySpeed = 1.0f;
	public TextMesh Title;
	public TextMesh Description;
	public Color planetColor;
	public GravityObject gravityObject;

	public List<GameObject> fractureList;
	public int fractureCount;
	public float fractureSpeed;

	public long population;
	public const float IncreaseBaseSpeed = 0.01f;

	public void IncreasePopulation(float deltaTime)
	{
		population += (long)(deltaTime * population * IncreaseBaseSpeed);
	}



	// Use this for initialization
	void Start () {
		GetComponent<MeshRenderer>().material.color = planetColor;
		GetComponent<TrailRenderer>().material.color = planetColor;
		GetComponent<TrailRenderer>().enabled = false;
		Title.gameObject.SetActive(true);
		Description.gameObject.SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {
		if (gravityObject.simulationActive)
		{
			IncreasePopulation(Time.deltaTime);
			GetComponent<TrailRenderer>().enabled = true;
			GameManager.Instance.limit.radius += GameManager.Instance.planetAddLimitSpeed * Time.deltaTime;
			if (!GameManager.Instance.CheckInLimit(transform.position,GetComponent<SphereCollider>().radius))
			{
				Fracture(transform.position);
			}
		}

		DescriptionAlpha -= DescriptionDecaySpeed * Time.deltaTime;
		Color newColor = new Color(Title.color.r, Title.color.g, Title.color.b, DescriptionAlpha); ;
		Title.color = newColor;
		Description.color = newColor;

		Title.text = gameObject.name;
		Description.text = "mass  -  " + gravityObject.mass.ToString("0.00") + System.Environment.NewLine +
			"speed  -  " + gravityObject.velocity.magnitude.ToString("0.00") + System.Environment.NewLine +
			"population  -  " + string.Format("{0:n0}", population); ;
	}


	public void Fracture(Vector3 FracturePoint)
	{
		for (int i = 0; i < fractureCount; i++)
		{

			GameObject f = fractureList[Random.Range(0, fractureList.Count)];
			Vector3 randomOffset = new Vector3(Random.Range(-1f, 1f) * gravityObject.radius, Random.Range(-1f, 1f) * gravityObject.radius, 0);
			Vector3 randomPos = transform.position+ randomOffset;
			Vector3 randomVelocity = GetComponent<GravityObject>().velocity + fractureSpeed * (randomPos- FracturePoint).normalized;
			var debris= Instantiate(f, randomPos, Quaternion.LookRotation(randomOffset,Vector3.forward));
			debris.GetComponent<MeshRenderer>().material.color = planetColor;
			debris.GetComponent<GravityObject>().velocity = randomVelocity;
		}
		GameManager.Instance.PlanetDestroy(this);
		Destroy(this.gameObject);
	}


	private void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.layer == LayerMask.NameToLayer("Planet"))
		{
			if(!GetComponent<GravityObject>().Fixed && 
				GetComponent<GravityObject>().simulationActive && 
				other.GetComponent<GravityObject>().simulationActive )
			{
				Fracture(other.transform.position);
				
			}
				
		}
	}
}
