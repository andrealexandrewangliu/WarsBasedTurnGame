using UnityEngine;
using System.Collections;

public class PlayerFaction : MonoBehaviour {
	public Color FactionColor;
	private GameObject BaseFactionNation;
	private BaseFaction BaseFactionSpecs;
	public string Name;
	public ArrayList ActiveUnits = new ArrayList();
	public GameObject[] Units = new GameObject[5];

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
