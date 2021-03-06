﻿using UnityEngine;
using System.Collections;

public class EnergyDischarge : MonoBehaviour {

	// The idea is to get a kind of 'dripping' effect from energy blasts

    public Rigidbody myRigidbody;
    public float forceMin;
    public float forceMax;

    float lifetime = 3;
    float fadeTime = 2;

	void Start () {

        
        float force = Random.Range(forceMin, forceMax);
        // myRigidbody.AddForce(transform.right * force);
        myRigidbody.AddTorque(Random.insideUnitSphere * force);

        StartCoroutine(Fade ());
	}
	
    IEnumerator Fade()
    {
        yield return new WaitForSeconds(lifetime);

        float percent = 0;
        float fadeSpeed = 1 / fadeTime;
        Material mat = GetComponent<Renderer>().material;
        Color initialColor = mat.color;

        while (percent < 1)
        {
            percent += Time.deltaTime * fadeSpeed;
            mat.color = Color.Lerp(initialColor, Color.clear, percent);
            yield return null;
        }

        Destroy(gameObject);
    }
}
