using UnityEngine;
using System.Collections;

public class GrubGrub : MonoBehaviour {

	// eventually have the gloflies fly around... land on trees.... etc but for now... act like a clock.
	// so much room for activities here.

	// Update is called once per frame
	void Update ()
	{
		transform.Rotate (-Vector3.forward * Time.deltaTime * 8);
	}
}
