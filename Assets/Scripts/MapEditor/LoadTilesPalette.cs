using UnityEngine;
using System.Collections;

public class LoadTilesPalette : MonoBehaviour {
	public GameObject ItemPrefab;
	public Sprite [] Images;
	public string [] Names = {
		"Concrete",
		"Grass",
		"Plains",
		"Sand",
		"Shallow Water",
		"Deep Water",
		"Forest",
		"Hill",
		"Mountain",
		"Sunken Concrete",
		"City",
		"Concrete Road",
		"Grass Road",
		"Plains Road",
		"Shallow Bridge",
		"Urban Bridge",
		"Deep Bridge"
	};
	public float offset = 85;
	public float jump = -65;


	// Use this for initialization
	void Start () {
		this.GetComponent<RectTransform> ().sizeDelta = new Vector2(0 ,(Names.Length - 1) * 70);
		for (int id = 0; id < Names.Length; id++) {
			float displacement = offset + (id * jump);
			MapEditor editor = (MapEditor) PlayMap.GridController;
			var item = (GameObject)Instantiate (ItemPrefab, new Vector3(0, 0, 0), Quaternion.identity);
			var image = item.transform.GetChild(0).GetChild(0).gameObject;
			var text = item.transform.GetChild(0).GetChild(1).gameObject;
			image.GetComponent<UnityEngine.UI.Image>().sprite = Images[id];
			text.GetComponent<UnityEngine.UI.Text>().text = Names[id];
			item.GetComponent<PaletteButtonData>().TilePaint = id ;

			item.transform.parent = this.transform;
			//Debug.Log(Names[id] + ": " + displacement);
			item.transform.localPosition = new Vector3(2, displacement, 0);
		}
	}

	
	// Update is called once per frame
	void Update () {
	
	}
}
