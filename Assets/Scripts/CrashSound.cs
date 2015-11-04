using UnityEngine;
using System.Collections;

public class CrashSound : MonoBehaviour {

	// Crash sound isn't currently in use yet. Can be used for projectiles, falling objects, shattering objects etc.
	// This code modifies audio based upon velocity (vector magnitude) to determine which sound will be played.

    public AudioClip crashSoft;
    public AudioClip crashHard;

    private AudioSource source;
    private float lowPitchRange = .75f;
    private float highPitchRange = 1.25f;
    private float velToVol = .2f;
    private float velocityClipSplit = 10f;

    void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    void OnColliosionEnter (Collision coll)
    {
        float hitVol = coll.relativeVelocity.magnitude * velToVol;
        if(coll.relativeVelocity.magnitude < velocityClipSplit)
        {
            source.PlayOneShot(crashSoft, 1f);

        }
        else
        {
            source.PlayOneShot(crashHard, 1f);
        }

    }
}
