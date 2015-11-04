using UnityEngine;
using System.Collections;

public class Wand : MonoBehaviour {
	public enum FireMode {Auto, Burst, Single};

	public FireMode fireMode;

	public Transform[] projectileSpawn;

	//  projectile is throwing an error
	public Projectile projectile;
	public Projectile secondary;

	public float msBetweenShots = 300;
	public float muzzleVelocity = 35;
	public int burstCount;

    public float mana = 5;
    public float manaMax = 5;
    const int manaMin = 0;

	int secondaryProjectileCount = 60;
	int spMax = 100;
	bool secondaryIsCharged = false;
	public float secondaryChargeTime = 3.0f;
	public float secondaryChargeRate = 10.0f;

    public float msBetweenManaRecharge = 30;

    float manaRechargeRate;

    public AudioClip shootSound;
    private AudioSource source;
    private float volLowRange = .5f;
    private float volHighRange = 1.0f;
    private float pitchLowRange = .9f;
    private float pitchHighRange = 1.2f;

    // MuzzleFlash muzzleFlash;

    float nextShotTime;

	bool triggerReleasedSinceLastShot;
	int shotsRemainingInBurst;
    // beginning to dislike the word discharge
    public Transform energyDischarge;
    public Transform energyDischargePoint;

    void Awake()
    {
        source = GetComponent<AudioSource>();
		shotsRemainingInBurst = burstCount;
    }
    void Update ()
     {
        if(mana < 1)
        {
            StartCoroutine(ManaRecharge());
        }
    }
     

        // mana isn't currently working... I had it set up in the wand controller and player but it's not behaving correctly


    IEnumerator ManaRecharge()
    {
        yield return new WaitForSeconds(2);
        mana = manaMax;
    }

    void Shoot()
    {
		// implement: if mana == manaMin do outOfMana shot. else continue
        if (mana > manaMin)
        {
            if (Time.time > nextShotTime)
            {
				if (fireMode == FireMode.Burst) 
				{
					if (shotsRemainingInBurst == 0) 
					{
						return;
					}
					shotsRemainingInBurst --;
				}
				else if (fireMode == FireMode.Single) 
				{
					if (!triggerReleasedSinceLastShot)
					{
						return;
					}
				}

				for(int i = 0; i  < projectileSpawn.Length; i++) 
				{


					nextShotTime = Time.time + msBetweenShots / 1000;
					Projectile newProjectile = Instantiate(projectile, projectileSpawn[i].position, projectileSpawn[i].rotation) as Projectile;
					newProjectile.SetSpeed(muzzleVelocity);
					

				}
				Instantiate(energyDischarge, energyDischargePoint.position, energyDischargePoint.rotation);
				
				// muzzleFlash.Activate();
				mana = mana - 1;
				
				float vol = Random.Range(volLowRange, volHighRange);
				float shotPitch = Random.Range(pitchLowRange, pitchHighRange);
				
				source.pitch = shotPitch;
				source.PlayOneShot(shootSound, vol);
            }

        }
    }
	/*
	void Secondary() 
	{
		// Spawn a series of projectiles in a "lotus" like fashion
		// could make this get rave-ie
			for (int i = 0; i < projectileSpawn.Length; i++) 
			{
				if (secondaryProjectileCount < 0) 
				{
					return;
				} 
				else 
				{
					for(int j = 0; j < secondaryProjectileCount; j++) {

				// divide 360 degrees by secondaryProjectileCount to get rotation degree amount
					Projectile secondaryProjectile = Instantiate (secondary, projectileSpawn [i].position, projectileSpawn [i].rotation) as Projectile;
					secondary.SetSpeed (muzzleVelocity);
					projectileSpawn [i].Rotate (Vector3.up * 360/secondaryProjectileCount, Space.World); 
					secondaryProjectileCount--;
					}
				}
			
			}
	}
*/
	public void OnTriggerHold() 
	{
		Shoot ();
		triggerReleasedSinceLastShot = false;

	}
	public void OnTriggerRelease() 
	{
		triggerReleasedSinceLastShot = true;
		shotsRemainingInBurst = burstCount;

	}
	/*
	public void ChargeSecondary()
	{
		if (secondaryIsCharged == false)
		{
			StartCoroutine ("Charging");
		}

		// call charge
		if (secondaryIsCharged == false) 
		{
			secondaryProjectileCount = (secondaryProjectileCount + (int)(secondaryChargeRate * Time.deltaTime));
		}
		secondaryIsCharged = true;

	}
	*/

	/*
	IEnumerator Charging() 
	{

		
			// secondaryProjectileCount = (secondaryProjectileCount + (int)(secondaryChargeRate * Time.deltaTime));
			secondaryProjectileCount = 60;
			yield return new WaitForSeconds (3);
			secondaryIsCharged = true;

			Secondary ();
		
	}
	*/
	/*
	public void ShootSecondary()
	{
		// if charge == full
		Secondary ();
		secondaryIsCharged = false;

	}
	*/

}