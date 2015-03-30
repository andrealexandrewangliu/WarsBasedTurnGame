using UnityEngine;
using System.Collections;

public class UnitPainter : MonoBehaviour {
	public GameObject[] parts;

	public void paint(Color color){
		foreach(GameObject part in parts){
			part.GetComponent<Renderer>().material.color = part.GetComponent<Renderer>().material.color * color;
		}
	}
}
