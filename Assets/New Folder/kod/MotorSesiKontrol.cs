using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorSesiKontrol : MonoBehaviour {

	public AudioClip acilis;
	public AudioClip calisma;
	public AudioClip kapanis;

	public float du_hiz;
	public float mi_pit;
	public float pi_hiz;

	private AudioSource _source;

	float hiz;


	void Start () 
	{
		_source = GetComponent<AudioSource>();
	}
	

	void Update () 
	{
		
		hiz = GetComponent<tuna>().speed;


		if(_source.clip == calisma)
		{
			_source.clip = kapanis;
			_source.Play();
		}
		if((_source.clip == kapanis || _source.clip == null))
		{
			_source.clip = acilis;
			_source.Play();
			_source.pitch = 1;
		}
		if(!_source.isPlaying)
		{
			_source.clip = calisma;
			_source.Play();
		}
		if (_source.clip == calisma)

		{
			_source.pitch = Mathf.Lerp (_source.pitch, mi_pit + Mathf.Abs (hiz) / du_hiz, pi_hiz);
		}
			

	}
}
