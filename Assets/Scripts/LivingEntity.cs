using UnityEngine;
using System.Collections;

public class LivingEntity : MonoBehaviour, IDamageable {


    public float startingHealth;
    protected float health;
    protected bool dead;

	public AudioClip deathSound;

	public event System.Action OnDeath;

    protected virtual void Start()
    {
        health = startingHealth;

	}

    public virtual void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection)
    {
        // saving this for later to do things with the hit variable... like instantiate particle effect on hit
        TakeDamage(damage);
    }

    public virtual void TakeDamage(float damage)
    {
        health -= damage;

		if (health <= 0 && !dead)
        {
            Die();
        }
    }
    [ContextMenu("Self Destruct")]
    protected virtual void Die()
    {
        // todo: if entity is player, start death animation

		
        dead = true;

        if (OnDeath != null)
        {
            OnDeath();
			Debug.Log("OnDeath called, said thanks for all the sockgnomes");
			// need to be able to pass this through RandomizeSfx
			// AudioSource.PlayClipAtPoint(deathSound, transform.position);
        }
		 
        GameObject.Destroy(gameObject);
		Debug.Log ("Game Object Destroyed");

        // camera shake on death
        // CameraShake();

        
    }
}
