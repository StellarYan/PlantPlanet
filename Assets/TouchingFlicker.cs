using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchingFlicker : MonoBehaviour {
	public Text text;
	public Color SelectedColor;
	public Color unSelectColor;

	public bool selected;
	public void hover()
	{
		selected = true;
		
	}

	public void exitHover()
	{
		selected = false;
	}
	// Use this for initialization
	void Start () {
		
	}


	
	// Update is called once per frame
	void Update () {
		Color target = selected ? SelectedColor : unSelectColor;
		text.color = Color.Lerp(text.color, target, 0.2f);
	}
}
