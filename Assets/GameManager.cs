using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class GameManager : MonoBehaviour {
	public List<GravityObject> gravityObjectList;
	public List<Planet> inWorldPlanetList;
	public GameObject planetPrefab;
	public LineRenderer VelocityLineRenderer;

	public static GameManager Instance { get; private set; }
	public const float GravitationalConstant = 0.001f; 
	public const float throwVelocityFactor = 1f;
	public float timescale;
	

	public GameObject SelectedPlanet;
	public GameObject SelectingPlanet;
	public Vector3 SelectingplanetInitPosition;

	public bool throwing;
	public Vector3 throwVelocity;

	public List<Transform> PlanetInitPivotList;
	public List<GameObject> PreparedPlanets;

	public float limitNarrowSpeed;
	public Limit limit;
	public float planetAddLimitSpeed;
	public float planetDestroySubLimit;

	public const float maxPlanetMass = 10f;
	public const float minPlanetMass = 1f;

	public GameObject Sun;

	public const int minPopulationRank = 5;
	public const int maxPopulationRank = 10;
	public float zeroPopulationRate = 0.3f;

	public TextMesh populationText;

	public string[] PlanetNameList = { "Qodrieter", "Oihiri", "Piothea", "Gewherth", "Shacacury", "Speutis",
	"Plodetov","Blumutune","Yepluowei","Xaplauphus","Ruabos","Buolea","Naprapus","Uspuoria","Restanus","Swippe",
	"Fruxothea","Qiyzuno","Rechonoe","Fusnapus","Dethautera","Yunus","Meswolla","Bostrienope","Gramurus",
	"Cloetov","Feuphus","Jeagawa","Raskosie","Smapus","Anov","Roynus","Glaogawa","Otune"};


	public float year;
	public long allPopulation;
	public float winGameradius;
	public enum Result
	{
		none,win,lose
	};
	public Result result;


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

	private void Start()
	{
		DontDestroyOnLoad(this.gameObject);
	}

	public Vector3 MyGravityAcc(float otherMass, Vector3 myPosition, Vector3 otherPosition)
	{
		return ((GravitationalConstant * otherMass) / (otherPosition - myPosition).sqrMagnitude) *
					(otherPosition - myPosition).normalized; ;
	}

	int simultationCount = 30;


	void SimulationPlanet(float deltaTime)
	{
		foreach (var g in gravityObjectList)
		{
			if (!g.simulationActive || g.gameObject.layer != LayerMask.NameToLayer("Planet")) continue;
			Vector3 allAcc = Vector3.zero;
			foreach (var otherG in gravityObjectList)
			{
				if (!otherG.simulationActive || otherG == g) continue;
				if ((otherG.position - g.position).sqrMagnitude < (otherG.radius + g.radius) * (otherG.radius + g.radius)) continue;
				allAcc += MyGravityAcc(otherG.mass, g.position, otherG.position);
			}
			g.cachedAcc = allAcc;
		}
		foreach (var g in gravityObjectList)
		{
			if (!g.simulationActive || g.Fixed || g.gameObject.layer != LayerMask.NameToLayer("Planet")) continue;
			g.position += (g.velocity + 0.5f * g.cachedAcc * deltaTime) * deltaTime;
			g.velocity += g.cachedAcc * deltaTime;
		}
	}

	void SimulationDebris(float deltaTime)
	{
		foreach (var g in gravityObjectList)
		{
			if (!g.simulationActive || g.gameObject.layer != LayerMask.NameToLayer("Debris")) continue;
			Vector3 allAcc = Vector3.zero;
			foreach (var otherG in gravityObjectList)
			{
				if (!otherG.simulationActive || otherG == g) continue;
				if ((otherG.position - g.position).sqrMagnitude < (otherG.radius + g.radius) * (otherG.radius + g.radius)) continue;
				allAcc += MyGravityAcc(otherG.mass, g.position, otherG.position);
			}
			g.cachedAcc = allAcc;
		}
		foreach (var g in gravityObjectList)
		{
			if (!g.simulationActive || g.Fixed || g.gameObject.layer != LayerMask.NameToLayer("Debris")) continue;
			g.position += (g.velocity + 0.5f * g.cachedAcc * deltaTime) * deltaTime;
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
		year += Time.deltaTime * 100;

		for (int i = 0; i < simultationCount; i++)
		{
			for (int j = 0; j < gravityObjectList.Count;)
			{
				if (gravityObjectList[j] == null) gravityObjectList.RemoveAt(j);
				else j++;
			}
			SimulationPlanet(Time.deltaTime * timescale/ simultationCount);
		}
		SimulationDebris(Time.deltaTime);



		for (int i = 0; i < PlanetInitPivotList.Count; i++)
		{
			if(PreparedPlanets[i]==null || PreparedPlanets[i].GetComponent<GravityObject>().simulationActive)
			{
				PreparedPlanets[i]=RespawnRandomPlanet(PlanetInitPivotList[i]);
			}
		}

		RaycastHit PlanetHitInfo;
		Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out PlanetHitInfo, 9999999, LayerMask.GetMask("Planet"));	
		if (PlanetHitInfo.collider!=null )
		{
			PlanetHitInfo.collider.GetComponent<Planet>().DescriptionAlpha = 1.0f;
			if(Input.GetMouseButtonDown(0) && SelectedPlanet == null && PlanetHitInfo.collider.gameObject.GetComponent<GravityObject>().simulationActive == false)
			{
				SelectingPlanet = PlanetHitInfo.collider.gameObject;
				SelectingplanetInitPosition = SelectingPlanet.transform.position;
			}
		}
		if(SelectingPlanet!=null && Input.GetMouseButtonUp(0))
		{
			SelectedPlanet = SelectingPlanet;
			SelectingPlanet = null;
		}


		if(SelectedPlanet !=null )
		{
			RaycastHit EclipticHitInfo;
			Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out EclipticHitInfo, 9999999, LayerMask.GetMask("Ecliptic"));
			if (!throwing)
			{
				SelectedPlanet.transform.position = EclipticHitInfo.point;
				if (Input.GetMouseButtonDown(1))
				{
					SelectedPlanet.transform.position = SelectingplanetInitPosition;
					SelectedPlanet = null;
					SelectingplanetInitPosition = Vector3.zero;
				}
				if (Input.GetMouseButtonDown(0))
				{
					throwing = true;
				}
			}
			else
			{
				throwVelocity = (EclipticHitInfo.point - SelectedPlanet.transform.position) * throwVelocityFactor;
				DrawLineIn(SelectedPlanet.transform.position, EclipticHitInfo.point);
				if(Input.GetMouseButtonUp(0))
				{
					inWorldPlanetList.Add(SelectedPlanet.GetComponent<Planet>());

					SelectedPlanet.GetComponent<GravityObject>().simulationActive = true;
					SelectedPlanet.GetComponent<GravityObject>().velocity = throwVelocity;
					SelectedPlanet = null;
					throwing = false;
					
					SelectingplanetInitPosition = Vector3.zero;
					VelocityLineRenderer.enabled = false;
				}
			}
		}
		limit.radius -= limitNarrowSpeed*Time.deltaTime;
		if(limit.radius<0)
		{
			LoseGame();
		}
		if(limit.radius> winGameradius)
		{
			WinGame();
		}

		if(GameStarter.Instance.endlessMode)
		{
			allPopulation = 0;
			foreach (var p in inWorldPlanetList)
			{
				if (p != null) allPopulation = allPopulation + p.GetComponent<Planet>().population;
			}
			populationText.gameObject.SetActive(true);
			populationText.text = "Population  -  "+ string.Format("{0:n0}", allPopulation);
		}
	}

	Collider[] limitCollisionRes = new Collider[1];
	public bool CheckInLimit(Vector3 position,float radius)
	{
		int t = Physics.OverlapSphereNonAlloc(
		position,
		radius,
		limitCollisionRes, LayerMask.GetMask("Limit"),
		QueryTriggerInteraction.Collide);
		return t == 1;
	}



	void DrawLineIn(Vector3 start,Vector3 end)
	{
		VelocityLineRenderer.enabled = true;
		VelocityLineRenderer.SetPositions(new Vector3[] { start, end });
	}


	public GameObject RespawnRandomPlanet(Transform pivot)
	{
		string name = PlanetNameList[Random.Range(0, PlanetNameList.Length)];
		Color color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
		float mass = Random.Range(minPlanetMass, maxPlanetMass);
		long pop = (long)(Random.Range(0f, 1f) * Mathf.Pow(10, Random.Range(minPopulationRank, maxPopulationRank)));
		if (Random.Range(0f, 1f) < zeroPopulationRate) pop = 0;

		var planetGo=Instantiate(planetPrefab, pivot.position,pivot.rotation);
		planetGo.GetComponent<GravityObject>().mass = mass;
		planetGo.GetComponent<Planet>().planetColor = color;
		planetGo.GetComponent<Planet>().name = name;
		planetGo.GetComponent<Planet>().population = pop;
		return planetGo;
	}

	public void PlanetDestroy(Planet planet)
	{
		limit.radius -= planetDestroySubLimit;
	}


	public void LoseGame()
	{
		if(Sun!=null) Sun.GetComponent<Planet>().Fracture(Sun.transform.position);
		result = Result.lose;
		SceneManager.LoadScene(2);
	}

	public void WinGame()
	{
		allPopulation = 0;
		foreach (var p in inWorldPlanetList)
		{
			if (p != null) allPopulation = allPopulation+ p.GetComponent<Planet>().population;
		}
		result = Result.win;
		SceneManager.LoadScene(2);
	}
}



