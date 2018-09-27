using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour {

    public GameObject camera;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = camera.transform.position;
        transform.rotation = Quaternion.Euler(90 + camera.transform.rotation.eulerAngles.x,
            camera.transform.rotation.eulerAngles.y,
            camera.transform.rotation.eulerAngles.z);
	}
}
