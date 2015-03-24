using UnityEngine;
using System.Collections;

public class WorkshopPrefabs : MonoBehaviour {
	private static Hashtable ChassisPrefabs = new Hashtable ();
	private static Hashtable TurretsPrefabs = new Hashtable ();
	private static Hashtable GunsPrefabs = new Hashtable ();

	// Use this for initialization
	void Start () {
		loadChassisPrefabs ();
		loadTurretsPrefabs ();
		loadGunsPrefabs ();
	
	}

	//	public enum UnitType{
	//		Drone,			//Basic Infantry
	//		HoverDrone,		//Hover Infantry
	//		Walker,			//Robots
	//		Wheeled,		//Wheeled Vehicles
	//		Track,			//Tanks
	//		Hover,			//Hover Tanks
	//		Boat,			//Cruisers, landers and battleships
	//		Sub,			//Submarines
	//		Heli,			//Helicopters and slow stationary flyers
	//		Jet				//Fighters and Bombers
	//	};
	private void loadChassisPrefabs(){
		GameObject[] prefabs;
		prefabs = Resources.LoadAll<GameObject>("Chassis/Drone");
		foreach(GameObject prefab in prefabs){
			ChassisPrefabs.Add(prefab.name, prefab);
		}
		prefabs = Resources.LoadAll<GameObject>("Chassis/HoverDrone");
		foreach(GameObject prefab in prefabs){
			ChassisPrefabs.Add(prefab.name, prefab);
		}
		prefabs = Resources.LoadAll<GameObject>("Chassis/Walker");
		foreach(GameObject prefab in prefabs){
			ChassisPrefabs.Add(prefab.name, prefab);
		}
		prefabs = Resources.LoadAll<GameObject>("Chassis/Wheeled");
		foreach(GameObject prefab in prefabs){
			ChassisPrefabs.Add(prefab.name, prefab);
		}
		prefabs = Resources.LoadAll<GameObject>("Chassis/Track");
		foreach(GameObject prefab in prefabs){
			ChassisPrefabs.Add(prefab.name, prefab);
		}
		prefabs = Resources.LoadAll<GameObject>("Chassis/Hover");
		foreach(GameObject prefab in prefabs){
			ChassisPrefabs.Add(prefab.name, prefab);
		}
		prefabs = Resources.LoadAll<GameObject>("Chassis/Boat");
		foreach(GameObject prefab in prefabs){
			ChassisPrefabs.Add(prefab.name, prefab);
		}
		prefabs = Resources.LoadAll<GameObject>("Chassis/Sub");
		foreach(GameObject prefab in prefabs){
			ChassisPrefabs.Add(prefab.name, prefab);
		}
		prefabs = Resources.LoadAll<GameObject>("Chassis/Heli");
		foreach(GameObject prefab in prefabs){
			ChassisPrefabs.Add(prefab.name, prefab);
		}
		prefabs = Resources.LoadAll<GameObject>("Chassis/Jet");
		foreach(GameObject prefab in prefabs){
			ChassisPrefabs.Add(prefab.name, prefab);
		}
	}
	
	private void loadTurretsPrefabs(){
		GameObject[] prefabs;
		prefabs = Resources.LoadAll<GameObject>("Turrets/S");
		foreach(GameObject prefab in prefabs){
			ChassisPrefabs.Add(prefab.name, prefab);
		}
		prefabs = Resources.LoadAll<GameObject>("Turrets/M");
		foreach(GameObject prefab in prefabs){
			ChassisPrefabs.Add(prefab.name, prefab);
		}
		prefabs = Resources.LoadAll<GameObject>("Turrets/L");
		foreach(GameObject prefab in prefabs){
			ChassisPrefabs.Add(prefab.name, prefab);
		}
		prefabs = Resources.LoadAll<GameObject>("Turrets/H");
		foreach(GameObject prefab in prefabs){
			ChassisPrefabs.Add(prefab.name, prefab);
		}
	}
	
	private void loadGunsPrefabs(){
		GameObject[] prefabs;
		prefabs = Resources.LoadAll<GameObject>("Cannons/T");
		foreach(GameObject prefab in prefabs){
			ChassisPrefabs.Add(prefab.name, prefab);
		}
		prefabs = Resources.LoadAll<GameObject>("Cannons/S");
		foreach(GameObject prefab in prefabs){
			ChassisPrefabs.Add(prefab.name, prefab);
		}
		prefabs = Resources.LoadAll<GameObject>("Cannons/M");
		foreach(GameObject prefab in prefabs){
			ChassisPrefabs.Add(prefab.name, prefab);
		}
		prefabs = Resources.LoadAll<GameObject>("Cannons/L");
		foreach(GameObject prefab in prefabs){
			ChassisPrefabs.Add(prefab.name, prefab);
		}
		prefabs = Resources.LoadAll<GameObject>("Cannons/H");
		foreach(GameObject prefab in prefabs){
			ChassisPrefabs.Add(prefab.name, prefab);
		}
	}
	// Update is called once per frame
	void Update () {
	
	}
}
