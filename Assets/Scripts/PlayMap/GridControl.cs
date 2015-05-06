using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridControl : MonoBehaviour {
	
	private static GameObject[] TilesPrefab;
	private static int[] TilesIndex;
	private static bool PrefabsLoaded = false;
	private byte[,] MapTerrain;
	private GameObject[,] MapTiles;
	
	// Recommended max size = 64 * 64
	// Stretched  max size = 128 * 128 NO GUARANTEES ABOVE THIS LIMIT!
	public Vector2 MapSize;

	public float getFloorHeight(int x, int y){
		TileSelect tile = (TileSelect)MapTiles [x, y].GetComponent<TileSelect> ();
		return tile.FloorHeight;
	}

	public void ChangeTile(int x, int y, byte id){
		if (x >= 0 && x < (int)MapSize.x && 
			y >= 0 && y < (int)MapSize.y) {
			Destroy (MapTiles [x, y]);
			MapTerrain[x,y] = id;

			var tile = (GameObject)Instantiate (GetTile(MapTerrain[x,y]), new Vector3((x + 0.5f), 0, (y + 0.5f)), Quaternion.identity);
			tile.transform.parent = this.transform.FindChild("Tiles").transform;
			TileSelect tileSpec = (tile.GetComponent<TileSelect>());
			RoadDraw thisroadDrawer = (tile.GetComponent<RoadDraw>());
			tileSpec.coordinate = new Vector2(x, y);
			MapTiles[x,y] = tile;
			if (x > 0){
				hideBorderY(y, x, x - 1);
				RoadDraw neighborsroadDrawer = (MapTiles[x - 1, y].GetComponent<RoadDraw>());
				if (thisroadDrawer != null)
					thisroadDrawer.UpdateConnect (MapTiles[x - 1, y].GetComponent<TileSelect>());
				if (neighborsroadDrawer != null){
					neighborsroadDrawer.UpdateConnect (tileSpec);
					neighborsroadDrawer.DrawConnections();
				}
			}
			if (y > 0){
				hideBorderX(y, y - 1, x);
				RoadDraw neighborsroadDrawer = (MapTiles[x, y - 1].GetComponent<RoadDraw>());
				if (thisroadDrawer != null)
					thisroadDrawer.UpdateConnect (MapTiles[x, y - 1].GetComponent<TileSelect>());
				if (neighborsroadDrawer != null){
					neighborsroadDrawer.UpdateConnect (tileSpec);
					neighborsroadDrawer.DrawConnections();
				}
			}
			if (x + 1 < (int)MapSize.x){
				hideBorderY(y, x, x + 1);
				RoadDraw neighborsroadDrawer = (MapTiles[x + 1, y].GetComponent<RoadDraw>());
				if (thisroadDrawer != null)
					thisroadDrawer.UpdateConnect (MapTiles[x + 1, y].GetComponent<TileSelect>());
				if (neighborsroadDrawer != null){
					neighborsroadDrawer.UpdateConnect (tileSpec);
					neighborsroadDrawer.DrawConnections();
				}
			}
			if (y + 1 < (int)MapSize.y){
				hideBorderX(y, y + 1, x);
				RoadDraw neighborsroadDrawer = (MapTiles[x, y + 1].GetComponent<RoadDraw>());
				if (thisroadDrawer != null)
					thisroadDrawer.UpdateConnect (MapTiles[x, y + 1].GetComponent<TileSelect>());
				if (neighborsroadDrawer != null){
					neighborsroadDrawer.UpdateConnect (tileSpec);
					neighborsroadDrawer.DrawConnections();
				}
			}
		}
	}

	// Use this for initialization
	void Start () {
		int x = Mathf.RoundToInt (MapSize.x);
		int y = Mathf.RoundToInt (MapSize.y);
		MapTerrain = new byte[x,y];
		MapTiles = new GameObject[x,y];


		SetPrefabs ();
		CreateGrid ();
		
		PlayMap.Grid = this;
		PlayMap.MapTerrain = MapTerrain;
		PlayMap.MapTiles = MapTiles;
	}

	void SetPrefabs(){
		if (!PrefabsLoaded) {
			SortedDictionary<string, int> dictionary = TileDictionary ();
			TilesPrefab = Resources.LoadAll<GameObject> ("Prefabs/Tiles");

			for (int index = 0; index < TilesPrefab.Length; index++) {
				TilesIndex [dictionary [TilesPrefab [index].name]] = index;
			}
			PrefabsLoaded = true;
		}
	}

	SortedDictionary<string, int> TileDictionary(){
		SortedDictionary<string, int> dictionary = new SortedDictionary<string, int>();
		dictionary.Add ("Concrete", 0);
		dictionary.Add ("Grass", 1);
		dictionary.Add ("Plains", 2);
		dictionary.Add ("Sand", 3);
		dictionary.Add ("ShallowWater", 4);
		dictionary.Add ("DeepWater", 5);
		dictionary.Add ("Forest", 6);
		dictionary.Add ("Hill", 7);
		dictionary.Add ("Mountain", 8);
		dictionary.Add ("SunkenConcrete", 9);
		dictionary.Add ("City", 10);
		dictionary.Add ("ConcreteRoad", 11);
		dictionary.Add ("GrassRoad", 12);
		dictionary.Add ("PlainsRoad", 13);
		dictionary.Add ("ShallowBridge", 14);
		dictionary.Add ("UrbanBridge", 15);
		dictionary.Add ("DeepBridge", 16);
		TilesIndex = new int[dictionary.Count];
		return dictionary;
	}

	GameObject GetTile(int index){
		return TilesPrefab[TilesIndex[index]];
	}

	void CreateGrid(){
		for (int x = 0; x < MapSize.x; x++) {
			for (int y = 0; y < MapSize.y; y++) {
				var tile = (GameObject)Instantiate (GetTile(MapTerrain[x,y]), new Vector3((x + 0.5f), 0, (y + 0.5f)), Quaternion.identity);
				tile.transform.parent = this.transform.FindChild("Tiles").transform;
				TileSelect tileSpec = (tile.GetComponent<TileSelect>());
				RoadDraw thisroadDrawer = (tile.GetComponent<RoadDraw>());
				tileSpec.coordinate = new Vector2(x, y);
				MapTiles[x,y] = tile;
				if (x > 0){
					hideBorderY(y, x, x - 1);
					RoadDraw neighborsroadDrawer = (MapTiles[x - 1, y].GetComponent<RoadDraw>());
					if (thisroadDrawer != null)
						thisroadDrawer.UpdateConnect (MapTiles[x - 1, y].GetComponent<TileSelect>());
					if (neighborsroadDrawer != null)
						neighborsroadDrawer.UpdateConnect (tileSpec);
				}
				if (y > 0){
					hideBorderX(y, y - 1, x);
					RoadDraw neighborsroadDrawer = (MapTiles[x, y - 1].GetComponent<RoadDraw>());
					if (thisroadDrawer != null)
						thisroadDrawer.UpdateConnect (MapTiles[x, y - 1].GetComponent<TileSelect>());
					if (neighborsroadDrawer != null)
						neighborsroadDrawer.UpdateConnect (tileSpec);
				}
			}
		}
	}
	
	void hideBorderY(int y,int x1, int x2){
		int xlow, xhigh;
		if (x1 < x2) {
			xlow = x2;
			xhigh = x1;
		}
		else  {
			xlow = x1;
			xhigh = x2;
		}
		TileSelect east = (TileSelect) MapTiles[xlow,y].GetComponent<TileSelect>();
		TileSelect west = (TileSelect) MapTiles[xhigh,y].GetComponent<TileSelect>();
		if (east.borderOcclusion && west.borderOcclusion) {
			east.HideWest();
			west.HideEast();
		}
	}
	
	void hideBorderX(int y1,int y2, int x){
		int ylow, yhigh;
		if (y1 < y2) {
			ylow = y1;
			yhigh = y2;
		}
		else  {
			ylow = y2;
			yhigh = y1;
		}
		TileSelect north = (TileSelect) MapTiles[x,yhigh].GetComponent<TileSelect>();
		TileSelect south = (TileSelect) MapTiles[x,ylow].GetComponent<TileSelect>();
		if (north.borderOcclusion && south.borderOcclusion) {
			north.HideSouth();
			south.HideNorth();
		}
	}

	void OnDrawGizmos(){
		for (int x = 0; x <= MapSize.x; x++) {
			Gizmos.DrawLine(new Vector3(0, 0.21f, x), new Vector3(MapSize.y, 0.21f, x));
		}
		for (int y = 0; y <= MapSize.y; y++) {
			Gizmos.DrawLine(new Vector3(y, 0.21f, 0), new Vector3(y, 0.21f, MapSize.x));
		}
	}

	public TileSelect getTileSpec(int x, int y){
		return (TileSelect) MapTiles[x,y].GetComponent<TileSelect>();
	}

}
