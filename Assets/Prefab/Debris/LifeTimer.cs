using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeTimer : MonoBehaviour {
	public float lifeTime;
	float timer;
	public float decaySpeed = 1;
	// Use this for initialization
	void Start () {
		
	}

	
	
	// Update is called once per frame
	void Update () {
		if((timer+= (decaySpeed*Time.deltaTime) )>lifeTime)
		{
			Destroy(this.gameObject);
		}
		var mat = GetComponent<MeshRenderer>().material;
		mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, 1f-timer / lifeTime);

		if(!GameManager.Instance.CheckInLimit(transform.position,0.1f))
		{
			decaySpeed = 8;
		}
	}
}
