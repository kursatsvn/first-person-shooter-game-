using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using Photon.Pun.UtilityScripts;

public class weapon : MonoBehaviourPunCallbacks 
{

	private Animator anim;
	private AudioSource _AudioSource;

	[Header("Properties")]
	public float fireRate = 0.1f;
	public float damage = 20f;
	public float range = 100f;
	public int bulletsPerMag = 30;
	public int bulletsLeft = 200;

	public float spreadFactor = 0.1f;

	public int currentBullets;

	public enum ShootMode { Auto, Semi}
	public ShootMode shootingMode;

	[Header("UI")]
	public Text ammoText;

	[Header("setup")]
	public Transform shootPoint;
	public GameObject hitParticles;
	public GameObject bulletİmpact;

	public ParticleSystem muzzleFlash;

	[Header("Ses Efektleri")]
	public AudioClip shootSound;



	float fireTimer;
	private bool isReloading;
	private bool shootInput;
	private bool isAiming;

	private Vector3 originalPosition;
	public Vector3 aimPosition;
	public float adsSpeed = 8f;


	void OnEnable()
		{
		if (!photonView.IsMine)
			return;
		UpdateAmmoText ();
		}


	void Start () {
		anim = GetComponent<Animator>();
		_AudioSource = GetComponent<AudioSource>();

		currentBullets = bulletsPerMag;
		originalPosition = transform.localPosition;

		UpdateAmmoText ();
	}
	
	// Update is called once per frame
	void Update () {
		
		if (!photonView.IsMine) 
		{
			return;
		}
	

		switch (shootingMode) 
		{
		case ShootMode.Auto:
			shootInput = Input.GetKey (KeyCode.Mouse0);

			break;

		case ShootMode.Semi:
			shootInput = Input.GetKey (KeyCode.Mouse0);

			break;
		}

		if (shootInput) 
		{
			if (currentBullets > 0)
				Fire ();
			else if (bulletsLeft > 0)
				DoReload ();

			if (Input.GetKeyDown (KeyCode.R)) 
			{
				if (currentBullets < bulletsPerMag && bulletsLeft > 0)
					DoReload ();
			}
		}

		if (fireTimer < fireRate)
			fireTimer += Time.deltaTime;
		
		AimDownSights();
	}

	void FixedUptade()
	{
		AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo (0);


	
	}


	private void AimDownSights()
	{
		if (!photonView.IsMine) return;
		if (Input.GetKey (KeyCode.Mouse1) && !isReloading) 
		{
			transform.localPosition = Vector3.Lerp (transform.localPosition, aimPosition, Time.deltaTime * adsSpeed);
			isAiming = true;
		} 
		else 
		{
			transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, Time.deltaTime * adsSpeed);
			isAiming = false;
		}
	}



	private void Fire()
	{
		if (!photonView.IsMine) return;

		if (fireTimer < fireRate || currentBullets <= 0 || isReloading) return;

		RaycastHit hit;

		Vector3 shootDirection = shootPoint.transform.forward;
		shootDirection = shootDirection + shootPoint.TransformDirection
			(new Vector3(Random.Range(-spreadFactor, spreadFactor), Random.Range(-spreadFactor, spreadFactor)));

		//shootDirection.x += Random.Range (-spreadFactor, spreadFactor);
		//shootDirection.y += Random.Range (-spreadFactor, spreadFactor);

		if (Physics.Raycast (shootPoint.position, shootDirection, out hit, range)) 
		{
			Debug.Log (hit.transform.name + " found!");

			GameObject hitparticleEffect = Instantiate(hitParticles, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
			GameObject bulletHole = Instantiate(bulletİmpact, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));

			Destroy (hitparticleEffect, 1f);
			Destroy (bulletHole, 2f);

			if (hit.transform.GetComponent<healthController> ()) 
			{
				hit.transform.GetComponent<healthController>().ApplyDamage (damage);

			}
		}

		anim.CrossFadeInFixedTime ("Fire", 0.01f);
		muzzleFlash.Play ();
		PlayShootSound ();
	
		currentBullets--;

		UpdateAmmoText ();

		fireTimer = 0.0f;   //Reset fire timer
	}
	public void Reload()
	{
		if (!photonView.IsMine) return;


		if (bulletsLeft <= 0) return;

		int bulletsToLoad = bulletsPerMag - currentBullets;
		int bulletsToDeduct = (bulletsLeft >= bulletsToLoad) ? bulletsToLoad : bulletsLeft;

		    bulletsLeft -= bulletsToDeduct;
		    currentBullets += bulletsToDeduct;

		UpdateAmmoText ();
	}

	private void DoReload()
	{
		if (!photonView.IsMine) return;

		AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);

		if (isReloading) return;
			anim.CrossFadeInFixedTime("Reload", 0.01f);
	}

	private void PlayShootSound ()
	{
		if (!photonView.IsMine) return;

		_AudioSource.PlayOneShot (shootSound);
		// _AudioSource.clip = shootSound;
		// _AudioSource.Play ();
	}
	private void UpdateAmmoText()
	{
		if (!photonView.IsMine) return;

		ammoText.text = currentBullets + " / " + bulletsLeft;
	}

}