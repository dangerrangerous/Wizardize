using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : LivingEntity {

    public enum State { Idle, Chasing, Attacking, Running };
    State currentState;

    public ParticleSystem deathEffect;

	NavMeshAgent pathfinder;
	Transform target;
    Material skinMaterial;
    LivingEntity targetEntity;

    Color originalColor;

    float attackDistanceThreshold = .5f;
    float timeBetweenAttacks = 1;
    float damage = 1;


    float nextAttackTime;
    float myCollisionRadius;
    float targetCollisionRadius;

	// could modify deathSplurt to be enemySpawnSound
	public AudioClip deathSplurt;
	private AudioSource source;

	public AudioClip enemyAttackSound;
	private AudioSource attackSource;

	public AudioClip enemyLaugh1;
	public AudioClip enemyLaugh2;
	public AudioClip enemyLaugh3;

	private AudioSource enemyLaughSource;

	private float volLowRange = .7f;
	private float volHighRange = 1.0f;
	private float pitchLowRange = .8f;
	private float pitchHighRange = 1.2f;

	private bool isCoroutineExecuting = false;

	bool hasTarget;

	void Awake() 
	{
		pathfinder = GetComponent<NavMeshAgent> ();

		if (GameObject.FindGameObjectWithTag ("Player") != null) 
		{
			hasTarget = true;
			
			target = GameObject.FindGameObjectWithTag ("Player").transform;
			targetEntity = target.GetComponent<LivingEntity> ();

			myCollisionRadius = GetComponent<CapsuleCollider> ().radius;
			targetCollisionRadius = target.GetComponent<CapsuleCollider> ().radius;

		}
	}

    protected override void Start () 
	{
        base.Start();

		attackSource = GetComponent<AudioSource>();
		enemyLaughSource = GetComponent<AudioSource>();


        if (hasTarget)
        {
            currentState = State.Chasing;
   
            targetEntity.OnDeath += OnTargetDeath;

            StartCoroutine(UpdatePath());
        }
	}

	public void SetCharacteristics(float moveSpeed, int hitsToKillPlayer, float enemyHealth, Color skinColor) 
	{
		pathfinder.speed = moveSpeed;

		if (hasTarget) 
		{
			damage = Mathf.Ceil(targetEntity.startingHealth / hitsToKillPlayer);
		}
		startingHealth = enemyHealth;
		skinMaterial = GetComponent<Renderer>().material;
		skinMaterial.color = skinColor;
		originalColor = skinMaterial.color;

	}

    public override void TakeHit (float damage, Vector3 hitPoint, Vector3 hitDirection)
    {
		// On Death
        if (damage >= health)
        {
			// Destroy the object and instantiate the death effect
            Destroy(Instantiate(deathEffect.gameObject, hitPoint, Quaternion.FromToRotation(Vector3.forward, hitDirection)) as GameObject, deathEffect.startLifetime);


        }
        base.TakeHit(damage, hitPoint, hitDirection);

        // if takeHit(type ice) currentState = idle
    }

    void OnTargetDeath()
    {
        hasTarget = false;
        currentState = State.Idle;
    }
	
	void Update () {

        if (hasTarget)
        {
            // sqrRt operation is expensive

            if (Time.time > nextAttackTime)
            {
                float sqrDstToTarget = (target.position - transform.position).sqrMagnitude;

                if (sqrDstToTarget < Mathf.Pow(attackDistanceThreshold + myCollisionRadius + targetCollisionRadius, 2))
                {
                    nextAttackTime = Time.time + timeBetweenAttacks;
                    StartCoroutine(Attack());
                }
            }
        }

		StartCoroutine("EnemyLaugh");


	}

	IEnumerator EnemyLaugh() 
	{
		if (isCoroutineExecuting) 
		{
			yield break;
		}
		isCoroutineExecuting = true;

		int waitTime = Random.Range(4, 10);

		yield return new WaitForSeconds(waitTime);

		SoundManager.instance.RandomizeSfx (enemyLaugh1, enemyLaugh2);

		isCoroutineExecuting = false;
	}

    IEnumerator Attack()
    {
        currentState = State.Attacking;
        // if enemy is attacking, don't set enemies path
        pathfinder.enabled = false;

        Vector3 originalPosition = transform.position;
        Vector3 dirToTarget = (target.position - transform.position).normalized;
        Vector3 attackPosition = target.position - dirToTarget * (myCollisionRadius);

        float attackSpeed = 3;
        float percent = 0;

        skinMaterial.color = Color.red;

        bool hasAppliedDamage = false;

        while (percent <= 1)
        {
            if (percent >= .5f && !hasAppliedDamage)
            {
                hasAppliedDamage = true;
                targetEntity.TakeDamage(damage);
            }

            percent += Time.deltaTime * attackSpeed;
            float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
            transform.position = Vector3.Lerp(originalPosition, attackPosition, interpolation);


            yield return null;
        }
		// play attack sound
		float vol = Random.Range(volLowRange, volHighRange);
		float attackPitch = Random.Range(pitchLowRange, pitchHighRange);
		
		attackSource.pitch = attackPitch;
		attackSource.PlayOneShot(enemyAttackSound, vol);

        skinMaterial.color = originalColor;
        currentState = State.Chasing;
        pathfinder.enabled = true;
    }

	IEnumerator UpdatePath() {
		// navmesh pathfinder optimization
		float refreshRate = .25f;

		while (hasTarget) {
            if (currentState == State.Chasing)
            {
                Vector3 dirToTarget = (target.position - transform.position).normalized;
                Vector3 targetPosition = target.position - dirToTarget * (myCollisionRadius + targetCollisionRadius + attackDistanceThreshold / 2);
                if (!dead)
                {
                    pathfinder.SetDestination(targetPosition);

                }
            }
            yield return new WaitForSeconds(refreshRate);
		}
	}

	// deathSplurt sound is currently tied up with the deathEffect particles system... could modify this deathSplurt to be EnemySpawnSound
	// created SoundManager to manage most sounds.
	/*
	void PlayDeathSound()
	{

		// Variance in the Death Sound Effect
		float vol = Random.Range(volLowRange, volHighRange);
		float deathPitch = Random.Range(pitchLowRange, pitchHighRange);
		// Getting error that instance isn't set to an object
		source.pitch = deathPitch;
		source.PlayOneShot(deathSplurt, vol);

	}
	*/
}
