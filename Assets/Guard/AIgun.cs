using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIgun : MonoBehaviour {

	public int damage;
	public float range;
	public float fireRate;

	public float reloadTime;
	public int maxAmmo;
	private int currentAmmo;
	private bool isReloading = false;

	public ParticleSystem muzzleFlash;
	public GameObject impactEffect;
	public AudioClip gunShot;

	private AudioSource audioSource;
	private Animator anim;

	private float nextTimeToFire = 0;

	public bool engage = false;

	private void Start() {
		audioSource = GetComponent<AudioSource>();
		anim = GetComponent<Animator>();
		audioSource.clip = gunShot;
		currentAmmo = maxAmmo;
	}

	private void Update() {

		if (isReloading) {
			return;
		}

		if (currentAmmo <= 0) {
			StartCoroutine(Reload());
			return;
		}

		if (engage && Time.time >= nextTimeToFire) {
			nextTimeToFire = Time.time + 1f / fireRate;
			Shoot();
		}

	}

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

		if (Physics.Raycast(transform.position, transform.forward, out _hit, range, -1, QueryTriggerInteraction.Ignore)) {
			GameObject impact = Instantiate(impactEffect, _hit.point, Quaternion.LookRotation(_hit.normal));
			Destroy(impact, 1f);
			PlayerController target = _hit.transform.GetComponent<PlayerController>();
			if (target != null) {
				target.TakeDamage(damage);
			}
		}

	}
}
