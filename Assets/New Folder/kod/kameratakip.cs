using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kameratakip : MonoBehaviour {

	public bool takip;
	public Transform hedef;
	public Vector3 duzeltme;

	[Range(0, 10)]
	public float position_yusakligi;
	[Range(0, 10)]
	public float rotation_yumusakligi;

	Rigidbody _rb;

	void Start () 
	{
		_rb = GetComponent<Rigidbody> ();
	}
	

	private void FixedUpdate () 
	{
		if(takip)
		{
			this._rb.velocity.Normalize();
			transform.LookAt(hedef);

			Quaternion k_rot = transform.rotation;
			Vector3 t_pos = hedef.position + hedef.TransformDirection (duzeltme);

			if(t_pos.y < hedef.position.y)
			{
				t_pos.y = hedef.position.y;
			}

			transform.position = Vector3.Lerp (transform.position, t_pos, Time.deltaTime * position_yusakligi);
			transform.rotation = Quaternion.Lerp (k_rot, transform.rotation, Time.deltaTime * rotation_yumusakligi);

			if(transform.position.y < 0.5f)
			{
				transform.position = new Vector3 (transform.position.x, 0.5f, transform.position.z);
			}
				

		}
	}
}
