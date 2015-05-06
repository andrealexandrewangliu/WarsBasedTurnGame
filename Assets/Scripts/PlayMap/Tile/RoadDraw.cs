using UnityEngine;
using System.Collections;

// REQUIRES TILESELECT ON SAME GAMEOBJECT
public class RoadDraw : MonoBehaviour {
	public string[] TagConnect;
	public GameObject NorthEntry;
	public GameObject SouthEntry;
	public GameObject EastEntry;
	public GameObject WestEntry;

	public GameObject NWIntersect;
	public GameObject NEIntersect;
	public GameObject SWIntersect;
	public GameObject SEIntersect;
	
	public bool North;
	public bool South;
	public bool East;
	public bool West;

	public void Start(){
		DrawConnections ();
	}

	public void UpdateConnect(TileSelect neighborspecs){
		TileSelect thisspecs = (TileSelect) this.GetComponent ("TileSelect");
		Vector2 diff = thisspecs.coordinate - neighborspecs.coordinate;
		if (diff.x > 0)
			West = false;
		else if (diff.x < 0)
			East = false;
		else if (diff.y > 0)
			South = false;
		else if (diff.y < 0)
			North = false;

		foreach (string tagString in TagConnect) {
			if (tagString.Equals (neighborspecs.gameObject.tag)) {
				if (diff.x > 0)
					West = true;
				else if (diff.x < 0)
					East = true;
				else if (diff.y > 0)
					South = true;
				else if (diff.y < 0)
					North = true;
				break;
			}
		}
	}

	public void DrawConnections(){
		int numConnections = (North? 1 : 0) + (South? 1 : 0) + (East? 1 : 0) + (West? 1 : 0);

		NorthEntry.SetActive (North);
		SouthEntry.SetActive (South);
		EastEntry.SetActive (East);
		WestEntry.SetActive (West);

		NWIntersect.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0, 0);
		NEIntersect.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0, 0);
		SWIntersect.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0, 0);
		SEIntersect.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0, 0);

		switch (numConnections) {
		case 1:
			if (North)
				SouthEntry.SetActive (true);
			if (South)
				NorthEntry.SetActive (true);
			if (East)
				WestEntry.SetActive (true);
			if (West)
				EastEntry.SetActive (true);
			
			if (North || South){
				NWIntersect.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0.5f, 0.5f);
				NEIntersect.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0.5f, 0.0f);
				SWIntersect.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0.5f, 0.0f);
				SEIntersect.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0.5f, 0.5f);
			}
			else if (East || West){
				NWIntersect.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0.5f, 0.0f);
				NEIntersect.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0.5f, 0.5f);
				SWIntersect.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0.5f, 0.5f);
				SEIntersect.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0.5f, 0.0f);
			}

			break;
		default:
			if (North){
				if (West)
					NWIntersect.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0.0f, 0.5f);
				else
					NWIntersect.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0.5f, 0.5f);
				if (East)
					NEIntersect.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0.0f, 0.5f);
				else 
					NEIntersect.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0.5f, 0.0f);

			}

			if (South){
				if (West)
					SWIntersect.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0.0f, 0.5f);
				else
					SWIntersect.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0.5f, 0.0f);
				
				if (East)
					SEIntersect.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0.0f, 0.5f);
				else
					SEIntersect.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0.5f, 0.5f);
			}

			if (East){
				if (!North)
					NEIntersect.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0.5f, 0.5f);
				if(!South)
					SEIntersect.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0.5f, 0.0f);
			}
			if (West){
				if(!North)
					NWIntersect.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0.5f, 0.0f);
				if(!South)
					SWIntersect.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0.5f, 0.5f);
			}

			break;
		}
	}

}
