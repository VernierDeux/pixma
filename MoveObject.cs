using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour {

public GameObject moveobject;
public Transform targetPosition;
public float Speed;
	
	void Start () {
	
	//Debug.Log("DoStart= " + Time.time);	
	}
	
	
	void Update () {
		if(Input.GetKey(KeyCode.Space)){
		if(Vector3.Distance(moveobject.transform.position,targetPosition.position) < 0.5f)
		{
			Destroy(moveobject,2);
		}
		else
		{
	moveobject.transform.position = Vector3.MoveTowards(moveobject.transform.position,targetPosition.position,Time.deltaTime*Speed);
	moveobject.transform.LookAt(targetPosition);
	//Debug.Log("DoUpdate");
	    }
		}
	}
}
