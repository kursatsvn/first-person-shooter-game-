using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tuna : MonoBehaviour 
{

	public WheelCollider onSolCol;
	public WheelCollider onSagCol;
	public WheelCollider arkaSolCol;
	public WheelCollider arkaSagCol;

	public GameObject onSol;
	public GameObject onSag;
	public GameObject arkaSol;
	public GameObject arkasag;

	public float maxMotorGucu;
	public float maxDonusAcısı;
	public float motor;
	public float firenGucu;

	public float hiz_ayar;
	public bool kontak;
	public float speed;

	public int gasınput;

	public int Breakınput;


	private Rigidbody _rb;

	void Start () 
	{
		_rb = GetComponent<Rigidbody>();
	}

	public void gasPressed()
	{
		gasınput = 1;
	}

	public void gasReleased()
	{
		gasınput = 0;
	}

	public void BreakPressed()
	{
		Breakınput = 1;
	}

	public void BreakReleased()
	{
		Breakınput = 0;
	}
	public void buttE()
	{
		kontak = !kontak;
	}
	void FixedUpdate () 
	{
		
		{
			speed = transform.InverseTransformDirection(_rb.velocity).z * hiz_ayar;
			motor = maxMotorGucu * SimpleInput.GetAxis("Vertical");
			float donus = maxDonusAcısı*SimpleInput.GetAxis("Horizontal");
			float ElfireniTorku = firenGucu * Mathf.Abs (Input.GetAxis ("Jump"));



			onSolCol.steerAngle = onSagCol.steerAngle = donus;

			if(ElfireniTorku < 0.05)
			{	
			arkaSolCol.motorTorque = motor;
			arkaSagCol.motorTorque = motor;
		    arkaSolCol.brakeTorque = 0;
			arkaSagCol.brakeTorque = 0;
			}

			else
			{	
				arkaSolCol.brakeTorque = ElfireniTorku;
				arkaSagCol.brakeTorque = ElfireniTorku;
			}


			SanalTeker ();
		}

	}


	void SanalTeker()
	{
		Quaternion rot;
		Vector3 pos;
		onSolCol.GetWorldPose(out pos, out rot);
		onSol.transform.position = pos;
		onSol.transform.rotation = rot;

		onSagCol.GetWorldPose(out pos, out rot);
		onSag.transform.position = pos;
		onSag.transform.rotation = rot;

		arkaSolCol.GetWorldPose(out pos, out rot);
		arkaSol.transform.position = pos;
		arkaSol.transform.rotation = rot;

		arkaSagCol.GetWorldPose(out pos, out rot);
		arkasag.transform.position = pos;
		arkasag.transform.rotation = rot;
	}
}
