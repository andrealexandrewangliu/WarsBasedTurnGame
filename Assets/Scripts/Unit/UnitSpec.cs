using UnityEngine;
using System.Collections;

public class UnitSpec : MonoBehaviour {
	public string name;
	public GameObject chassisPrefab;	// ChassisWeapon prefab
	public GameObject[] turretsPrefab;		// TurretWeapon prefab
	public GameObject[] gunsPrefab; 		// SubWeapon prefab
	private GameObject chassis;	        // Chassis Instance
	private ChassisSpec chassisStat; 	// Chassis Spec Instance
	public TurretSpec[] turrets; 		// All Main turret Specs
	private Color UnitColor;
	private bool isInit = false;

	public int getNumbers(){
		return chassisStat.getNumbers ();
	}

	public bool isAmphibious(){
		return chassisStat.amphibious;
	}

	public enum UnitType{
		Walker,			//Robots
		Wheeled,		//Wheeled Vehicles
		Track,			//Tanks
		Hover,			//Hover Tanks
		Boat,			//Cruisers, landers and battleships
		Sub,			//Submarines
		Heli,			//Helicopters and slow stationary flyers
		Jet				//Fighters and Bombers
	};
	public enum UnitMove{
		Walk,			//Drone, Walker
		Wheel,			//Wheeled
		HalfTrack,		//Wheeled and Tracked
		Thread,			//Track
		Sail,			//Boat
		Dive,			//Sub
		Float,			//Hover, HoverDrone
		Heli,		    //Heli
		Fly				//Jet
	};
	public enum UnitSize{
		T,          //This size is only for weapons
		S,			//12 small units
		M,			//6 medium units
		L,			//3 large units
		H,			//1 dreadnaught sized unit
	};
	
	public float[] getArmor(){
		return chassisStat.getOverallArmor ();
	}

	public float getMaxHealth(){
		return chassisStat.getMaxHealth ();
	}
	
	public int getMaxSupply(){
		return chassisStat.getMaxSupply ();
	}

	public UnitSize getUnitClassSize(){
		return chassisStat.size;
	}
	
	public float getWeight(){
		float calcweigth = 0;
		if (chassisStat != null) {
			calcweigth += chassisStat.getWeight();
		}
		return calcweigth;
	}
	
	public float getPowerWeightRatio(){
		float calcratio = 0;
		if (chassisStat != null) {
			calcratio += chassisStat.getPowerWeightRatio();
		}
		return calcratio;
	}
	
	public int getBaseMP(){
		int mp = Mathf.RoundToInt(chassisStat.getPowerWeightRatio() / 2);
		if (mp <= 0)
			mp = 1;
		return mp;
	}

	public int getMaxRange(){
		int max = 0;
		foreach (TurretSpec turret in turrets) {
			GunSpec weapon = turret.gunStat;
			if (weapon.maxrange > 0)
				max = Mathf.Max(max, weapon.maxrange);
		}
		return max;
	}

	// Use this for initialization
	void Start () {
		GenerateBody ();
//		Debug.Log (getWeight ());
//		Debug.Log (getBaseMP ());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void paintUnit(Color color){
		UnitColor = color;
		if (chassisStat != null)
			chassisStat.paint (UnitColor);
	}

	public void GenerateBody(){
		if (!isInit) {
			chassis = (GameObject)Instantiate (chassisPrefab, new Vector3 (0, 0, 0), Quaternion.identity);
			chassis.transform.parent = this.transform;
			chassis.transform.position = chassis.transform.parent.transform.position;
			chassisStat = (ChassisSpec)chassis.GetComponent ("ChassisSpec");
			chassisStat.GenerateBody (turretsPrefab, gunsPrefab);
			chassisStat.paint (UnitColor);
			turrets = chassisStat.getTurretsGameObjects();
			isInit = true;
		}
	}
	
	public UnitType getUnitType(){
		return chassisStat.type;
	}
	
	
	public UnitMove getUnitMovementType(){
		switch (chassisStat.type) {
		case UnitType.Boat:
			return UnitMove.Sail;
		case UnitType.Walker:
			return UnitMove.Walk;
		case UnitType.Heli:
			return UnitMove.Heli;
		case UnitType.Hover:
			return UnitMove.Float;
		case UnitType.Jet:
			return UnitMove.Fly;
		case UnitType.Sub:
			return UnitMove.Sail;
		case UnitType.Track:
			return UnitMove.Thread;
		case UnitType.Wheeled:
			return UnitMove.Wheel;
		}
		return UnitMove.Walk;
	}

	public bool RotateBody(float angle){
		chassis.transform.localEulerAngles = new Vector3 (0, angle, 0);
		return true;
	}
	public bool RotateTurret(float angle){
		return chassisStat.RotateTurret(angle);
	}
}
