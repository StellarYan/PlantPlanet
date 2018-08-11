using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStarter : MonoBehaviour {


	public bool endlessMode;
	public static GameStarter Instance { get; private set; }
	private void Awake()
	{
		if (Instance == null) Instance = this;
		else
		{
			Destroy(this.gameObject);
			return;
		}
	}


	public void ToGame()
	{
		SceneManager.LoadScene(1);
	}
	public void ToEndlessMode()
	{
		endlessMode = true;
		SceneManager.LoadScene(1);
	}
	// Use this for initialization
	void Start () {
				
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
