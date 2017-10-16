using UnityEngine;

public class Enemy : MonoBehaviour {

	public float health = 100f;
	
	public void TakeDamage (float amount) {
		health -= amount;
		if (health <= 0) {
			Die();
		}
	}

	private void Die() {
		Destroy(gameObject);
	}

}
