using UnityEngine;
using System.Collections;



public class Scrolling : MonoBehaviour {
	
	public float scrollingSpeed = 10.0f;
	
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		transform.position += new Vector3(scrollingSpeed * Time.deltaTime, 0, 0);
	}
}
