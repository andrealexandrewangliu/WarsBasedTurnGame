using UnityEngine;
using System.Collections;

public class GunSpec : MonoBehaviour {
	public float damage = 0;
	public float pierce = 0;
	public int burstsize = 1;
	public bool counter = true;
	public bool direct = true;
	public int minrange = 0;
	public int maxrange = 0;
	public float weigth = 0;

	public UnitSpec.UnitSize turretSize;

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
