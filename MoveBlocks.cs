using UnityEngine;
using System.Collections;

public class MoveBlocks : MonoBehaviour {

	public float Speed;
	public Transform targetPosition;
	void Start () {
	Destroy(gameObject,10);
	}
	
	
	void Update () {
	transform.Translate(Vector3.forward*Time.deltaTime*Speed);
	}
}
