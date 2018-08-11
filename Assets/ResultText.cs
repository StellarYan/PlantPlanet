using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultText : MonoBehaviour {
	public TextMesh resText;
	public TextMesh StatisticsText;
	// Use this for initialization
	void Start () {
		if (GameManager.Instance.result== GameManager.Result.win)
		{
			resText.text = "you  saved  the  world";
		}
		else
		{
			resText.text = "darkness  has  swallow  the  world";
		}
		StatisticsText.text = "Final  Population  -  " + string.Format("{0:n0}", GameManager.Instance.allPopulation)+ System.Environment.NewLine +
		"Time  -  " + GameManager.Instance.year.ToString("0.") + "  years";

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
