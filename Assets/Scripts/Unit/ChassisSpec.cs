using UnityEngine;
using System.Collections;

public class ChassisSpec : TurretableSpec {
	public float frontArmor = 0;
	public float sideArmor = 0;
	public float rearArmor = 0;
	public float health = 0;
	public float weigth = 0;
	public float horsepower = 0;
	public int supply = 100; // Supply
	public bool hasSub = false;
	public bool amphibious = false;
	public UnitSpec.UnitType type = UnitSpec.UnitType.Walker;
	public UnitSpec.UnitSize size = UnitSpec.UnitSize.S;
	public UnitSpec.UnitSize turretSize;

	public int getNumbers(){
		switch (size) {
		case UnitSpec.UnitSize.H:
			return 1;
		case UnitSpec.UnitSize.L:
			return 3;
		case UnitSpec.UnitSize.M:
			return 6;
		case UnitSpec.UnitSize.S:
			return 12;
		case UnitSpec.UnitSize.T:
			return 12;
		}
		return 1;
	}

	public override float TurretArmorInfluence(){
		switch (size) {
		case UnitSpec.UnitSize.H:
			return 0.1f;
		case UnitSpec.UnitSize.L:
			return 0.25f;
		case UnitSpec.UnitSize.M:
			return 0.5f;
		case UnitSpec.UnitSize.S:
			return 0.5f;
		case UnitSpec.UnitSize.T:
			return 0.5f;
		default:
			return 0;
		}
	}

	// Use this for initialization
	void Start () {
	
	}

	public TurretSpec[] getTurretsGameObjects (){
		ArrayList objects = new ArrayList ();
		TurretSpec[] turrets;
		addTurretsGameobjects (objects);

		turrets = new TurretSpec[objects.Count];
		for (int i = 0; i < turrets.Length; i++)
			turrets[i] = (TurretSpec) objects[i];


		return turrets;
	}
	
	public float getMaxHealth(){
		return (health + getTurretsHealth ()) * getNumbers();
	}
	
	public int getMaxSupply(){
		return supply + getTurretsSupply ();
	}

	public float[] getArmor(){
		float [] armors = new float[3];
		armors [0] = frontArmor;
		armors [1] = sideArmor;
		armors [2] = rearArmor;
		return armors;
	}
	
	public float[] getOverallArmor(){
		float [] armors = getArmor();
		float [] turretarmors = getTurretArmor();
		if (turretarmors [0] > 0) {
			armors [0] += turretarmors[0] * TurretArmorInfluence();
			armors [1] += turretarmors[0] * TurretArmorInfluence();
			armors [2] += turretarmors[0] * TurretArmorInfluence();
		}
		return armors;
	}
	
	
	public float[] getTurretArmor(){
		return getTurretsArmor ();
	}

	
	public float getWeight(){
		return weigth + getTurretsWeights ();
	}
	
	public float getPowerWeightRatio(){
		float calcratio = horsepower / getWeight();

		return calcratio;
	}

	// Update is called once per frame
	void Update () {
	
	}

	
	public void paint(Color color){
		((UnitPainter)this.GetComponent ("UnitPainter")).paint(color);
		paintTurrets (color);
	}
	
	public void GenerateBody(GameObject[] turretsPrefab, GameObject[] subPrefab){
		GenerateTurrets (turretsPrefab, 0, getNumbers());
		GenerateGuns (subPrefab, 0);
	}
	
	public bool RotateTurret(float angle){
//		if (turretStat.turnable) 
//			transform.FindChild ("TurretHardpoint").transform.localEulerAngles = new Vector3 (0, angle, 0);
//		return turretStat.turnable;
		return false;
	}
}
