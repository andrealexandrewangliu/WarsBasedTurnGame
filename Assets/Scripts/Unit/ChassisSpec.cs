using UnityEngine;
using System.Collections;

public class ChassisSpec : MonoBehaviour {
	public float frontArmor = 0;
	public float sideArmor = 0;
	public float rearArmor = 0;
	public float health = 0;
	public float weigth = 0;
	public float horsepower = 0;
	public GameObject turret;
	private TurretSpec turretStat;
	public UnitType type = UnitType.Drone;
	public UnitSize size;
	public UnitSize turretSize;

	public enum UnitType{
		Drone,			//Basic Infantry
		HoverDrone,		//Hover Infantry
		Walker,			//Robots
		Wheeled,		//Wheeled Vehicles
		Track,			//Tanks
		Hover,			//Hover Tanks
		Boat,			//Cruisers, landers and battleships
		Sub,			//Submarines
		Heli,			//Helicopters and slow stationary flyers
		Jet				//Fighters and Bombers
	};
	private enum UnitMove{
		Walk,			//Drone, Walker
		Wheel,			//Wheeled
		Thread,			//Track
		Swim,			//Boat
		Dive,			//Sub
		Float,			//Hover, HoverDrone
		Balloon,		//Heli
		Fly				//Jet
	};
	public enum UnitSize{
		T,          //This size is only for weapons
		S,			//12 small units
		M,			//6 medium units
		L,			//3 large units
		H,			//1 dreadnaught sized unit
	};

	// Use this for initialization
	void Start () {
	
	}
	
	public float getWeight(){
		float calcweigth = weigth;
		if (turretStat != null) {
			calcweigth += turretStat.getWeight();
		}
		return calcweigth;
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
		turretStat.paint (color);
	}
	
	public void GenerateBody(GameObject turretPrefab, GameObject mainPrefab, GameObject subPrefab){
		turret = (GameObject)Instantiate (turretPrefab, new Vector3(0,0,0), Quaternion.identity);
		turret.transform.parent = this.transform.FindChild("TurretHardpoint").transform;
		turret.transform.position = turret.transform.parent.transform.position;
		turretStat = (TurretSpec) turret.GetComponent ("TurretSpec");
		turretStat.GenerateBody (mainPrefab, subPrefab);
	}

}
