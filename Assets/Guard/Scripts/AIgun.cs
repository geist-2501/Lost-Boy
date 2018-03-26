using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Blair Cross
This script is attached to the gun that the guards
are holding. The guard AI sets the engage variable.
 */

public class AIgun : MonoBehaviour {

	//Main variables
	[SerializeField] private int damage;
	[SerializeField] private float range;
	[SerializeField] private float fireRate;

	//Ammo related variables.
	[SerializeField] private float reloadTime;
	[SerializeField] private int maxAmmo;
	private int currentAmmo;
	private bool isReloading = false;

	//Visual effects.
	[SerializeField] private ParticleSystem muzzleFlash;
	[SerializeField] private GameObject impactEffect;
	[SerializeField] private AudioClip gunShot;

	private AudioSource audioSource;
	private Animator anim;

	private float nextTimeToFire = 0;

	//If true, gun starts firing at player, if false, nothing.
	public bool engage = false;

	private void Start() {
		//Find the components at runtime.
		audioSource = GetComponent<AudioSource>();
		anim = GetComponent<Animator>();

		//Set the guns current audio clip to gunShot.
		audioSource.clip = gunShot;

		//Set current ammo amount to the maximum.
		currentAmmo = maxAmmo;
	}

	private void Update() {

		//If reloading, don't do anything.
		if (isReloading) {
			return;
		}

		//If gun is empty, reload.
		if (currentAmmo <= 0) {
			StartCoroutine(Reload());
			return;
		}

		//If allowed to fire, and if enough time has passed 
		//since the last fire, then fire.
		if (engage && Time.time >= nextTimeToFire) {
			nextTimeToFire = Time.time + 1f / fireRate;
			Shoot();
		}

	}

	//Resets the ammo count and plays the reload animation.
	IEnumerator Reload() {
		isReloading = true;

		anim.SetTrigger("Reload");

		yield return new WaitForSeconds(reloadTime);
		currentAmmo = maxAmmo;

		isReloading = false;
	}


	private void Shoot() {
		RaycastHit _hit;

		currentAmmo--;

		muzzleFlash.Play();

		audioSource.Play();

		anim.SetTrigger("Recoil");

		//Create a ray from the gun to the player.
		Ray _gunToPlayerRay = new Ray(transform.position, transform.forward);

		//Shoot the ray. If it hits the player, its value will be true.
		bool _gunToPlayerRaycast = Physics.Raycast(_gunToPlayerRay, out _hit, range, -1, QueryTriggerInteraction.Ignore);

		if (_gunToPlayerRaycast) {	//I.e if the gun hit.
			//Create an impact spark.
			GameObject impact = Instantiate(impactEffect, _hit.point, Quaternion.LookRotation(_hit.normal));
			//Destroy it after 1 second.
			Destroy(impact, 1f);
			//Set the target to the player.
			PlayerController target = _hit.transform.GetComponent<PlayerController>();
			if (target != null) {
				//Make the player take damage.
				target.TakeDamage(damage);
			}
		}

	}
}
