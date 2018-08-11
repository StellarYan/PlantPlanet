using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroText : MonoBehaviour {


	public float lifeTime;
	float timer;
	public bool toDelete;
	string intro = "THE  SPACE  IS  RUNNING  OUT" + System.Environment.NewLine +
				   "FILLED  WITH  DARKNESS" + System.Environment.NewLine +
					"TRY  TO  SAVE  AS  MANY PEOPLE  AS  YOU  CAN" + System.Environment.NewLine +
					System.Environment.NewLine +
					System.Environment.NewLine +
					"DRAG  AND  DROP  PLANET  IN  LIGHT  OF  SUN" + System.Environment.NewLine +
					"HOLD  TO  GIVE  VELOCITY" + System.Environment.NewLine +
					"DON'T  LET  PLANETS  ESCAPE  OR  COLLIDE" + System.Environment.NewLine +
					"PLANETS  CAN  INCREASE  LIGHT  INTENSITY";


	// Use this for initialization
	void Start () {
		GetComponent<TextMesh>().text = intro;
	}
	
	// Update is called once per frame
	void Update () {
		if((timer+=Time.deltaTime)>lifeTime)
		{
			toDelete = true;
		}
		if (toDelete && GetComponent<TextMesh>().text.Length != 0)
		{
			GetComponent<TextMesh>().text = GetComponent<TextMesh>().text.Substring(0, GetComponent<TextMesh>().text.Length - 1);
		}
	}
}
