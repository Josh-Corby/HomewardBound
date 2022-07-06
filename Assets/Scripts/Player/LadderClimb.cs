using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderClimb : GameBehaviour

{

	public Transform player;
	bool inside = false;
	public float speedUpDown = 3.2f;

	void Start()
	{

		inside = false;
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "Ladder")
		{
			IM.enabled = false;
			inside = !inside;
		}
	}

	void OnTriggerExit(Collider col)
	{
		if (col.gameObject.tag == "Ladder")
		{
			IM.enabled = true;
			inside = !inside;
		}
	}

	void Update()
	{
		if (inside == true && Input.GetKey("w"))
		{
			player.transform.position += Vector3.up / speedUpDown;
		}

		if (inside == true && Input.GetKey("s"))
		{
			player.transform.position += Vector3.down / speedUpDown;
		}
	}

}