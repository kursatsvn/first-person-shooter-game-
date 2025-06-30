using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class FollowAI : MonoBehaviour {

	public NavMeshAgent agent; 
	public Transform target;
	Animator enemyanim;
	public float mesafe;
 
	// Use this for initialization
	void Start () {
		
		enemyanim = GetComponent<Animator> ();

		if (agent == null)
		{
			agent = GetComponent<NavMeshAgent> ();
		}
	}
	// Update is called once per frame
	void Update ()
	{
		mesafe = Vector3.Distance (transform.position, target.position);



		if (target != null)
		{	
			agent.SetDestination (target.position);
		}

		if (mesafe <= 10) 
		{
			agent.enabled = true;
		} 

		else
		
		{
			agent.enabled = false;
		}
	}
   
}
