using UnityEngine;
using System.Collections;

public class UnitPainter : MonoBehaviour {
	public GameObject[] parts;

	public void paint(Color color){
		foreach(GameObject part in parts){
			part.renderer.material.color = part.renderer.material.color * color;
		}
	}
}
