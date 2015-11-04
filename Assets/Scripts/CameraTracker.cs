using UnityEngine;
using System.Collections;

public class CameraTracker : MonoBehaviour {

    /*
    
    private const float _distance_threshold = -5.0f;
    

	void Start () {
	    if (this.Player != null)
        {
            this._endpoint = this.Player.transform.position; 
            
        }
	}
	
	/// <summary>
    /// 
    /// </summary>
    void Update () {
	    if (this.Player != null)
        {
            if (Vector3.Distance(this.Player.transform.position, this.transform.position) > _distance_threshold)
            {
                // Slerp is like lerp but rather than being linear slerp has a "spherical" rate of change 
                // move toward player
                this.transform.position = Vector3.Slerp(this.transform.position, this._endpoint, Time.deltaTime);
            }
            // Check if tracker has reached its endpoint
            if (this.transform.position != this._endpoint)
            {
                this.transform.position = Vector3.Slerp(this.transform.position, this._endpoint, Time.deltaTime);
            }
            // update the last frame's player position
            this._endpoint = this.Player.transform.position;

        }
	}
    */
}
