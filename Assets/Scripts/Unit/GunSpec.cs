using UnityEngine;
using System.Collections;

public class GunSpec : MonoBehaviour {
	public string name = "Unnamed Weapon";
	public float damage = 0;
	public float pierce = 0;
	public float absolute = 0;
	public int burstsize = 1;
	public bool counter = true;
	public bool direct = true;
	public int minrange = 0;
	public int maxrange = 0;
	public float weigth = 0;
	public int ammo = -1;  // Ammo Limit
	public int supply = 0; // Supply Cost
	public GunType type = GunType.Direct;
	public TurretSpec Turret;

	public UnitSpec.UnitSize turretSize;

	public enum EquipType{
		Personal,
		Cannon,
		Launch
	}

	public enum GunType{
		Direct,     //Machine guns, lazers (Can AA with high depression turret, opposite for aerial)
		Homing,     //Guided missiles (Target all except subs)
		Heavy,      //Artillery and cannons (Never AA), Ranged only available with high depression or fly
		Drop,       //Bombers, anti sub bombs (Never AA, can anti sub, ship, hover and bombers only, only bombers can attack above water)
		Torpedo,    //Sea only torpedos (Similar to drop but ships can use it agains't ships as well)
	}



	public float getWeight(){
		return weigth;
	}
	
	public void paint(Color color){
		((UnitPainter)this.GetComponent ("UnitPainter")).paint(color);
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}
}
