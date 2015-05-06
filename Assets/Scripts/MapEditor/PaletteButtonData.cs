using UnityEngine;
using System.Collections;

public class PaletteButtonData : MonoBehaviour {
	public int TilePaint = -1;
	// Use this for initialization
	public void Trigger () {
		((MapEditor) PlayMap.GridController).setTilePaint(TilePaint);
	}
}
