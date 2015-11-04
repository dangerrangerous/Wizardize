using UnityEngine;
using System.Collections;

public class Rotater : MonoBehaviour
{
	// utility rotater... not in use yet but could slap it on the potions 
    void Update()
    {
        transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);
    }
}
