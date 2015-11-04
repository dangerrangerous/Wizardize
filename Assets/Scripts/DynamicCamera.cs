using UnityEngine;
using System.Collections;

public class DynamicCamera : MonoBehaviour
{
	// sorry for the mess

    public Transform Target;
    public Transform Player;
    public Vector3 _offset;
    float rayDistance = 5.0f;

    //public GameObject Player;
    private Vector3 cameraAnchorEndpoint;

    private const float cameraDistanceThreshold = -3.0f;
    //private const float _rotation_threshold = 50.0f;

    void Start()
    {
        _offset = new Vector3(Player.position.x - 0.5f, Player.position.y + 1.0f, Player.position.z - 0.5f);

    }

    void LateUpdate()
    {
		// some experiments with dynamic camera ideas
        /*
        Vector3 cameraAnchorRayOrigin;
        cameraAnchorRayOrigin = Player.transform.position;
        // rotate raycast to match player rotation
        // wtf 


        RaycastHit cameraAnchorRayHitInfo;
        if (Physics.Raycast(cameraAnchorRayOrigin, -transform.forward + -transform.up, out cameraAnchorRayHitInfo, rayDistance))
        {
            Debug.DrawLine(cameraAnchorRayOrigin, cameraAnchorRayHitInfo.point, Color.red);

            cameraAnchorEndpoint = cameraAnchorRayHitInfo.point + _offset;

            if (Vector3.Distance(Camera.main.transform.position, cameraAnchorRayHitInfo.point + _offset) > cameraDistanceThreshold)
            {
                Camera.main.transform.position = Vector3.Slerp(this.transform.position, cameraAnchorRayHitInfo.point + _offset, Time.deltaTime);
            }
        }

    */
    }

	// Camera shake has not been implemented yet... code still needs work
    public void CameraShake()
    {
        StartCoroutine(CoCameraShake());
    }

    IEnumerator CoCameraShake()
    {
        // have duration and magnitude be parameters of the function

        float elapsedTime = 0.0f;
        float shakeDuration = .5f;
        // magnitude should be modifiable by distance and size... but use const for now
        float magnitude = 1.0f;

        Vector3 originPosition = Camera.main.transform.position;

        while (elapsedTime < shakeDuration)
        {
            elapsedTime += Time.deltaTime;

            float percentComplete = elapsedTime / shakeDuration;
            float damper = 1.0f - Mathf.Clamp(4.0f * percentComplete - 3.0f, 0.0f, 1.0f);
            // map value to [-1,1]
            float x = Random.value * 2.0f - 1.0f;
            float y = Random.value * 2.0f - 1.0f;

            x *= magnitude * damper;
            y *= magnitude * damper;

            Camera.main.transform.position = new Vector3(x, y, originPosition.z);

            yield return null;
        }

        Camera.main.transform.position = originPosition;

    }
}

