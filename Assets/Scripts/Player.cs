using UnityEngine;
using System.Collections;
// forces our controller to get component playercontroller
[RequireComponent (typeof (PlayerController))]
[RequireComponent (typeof (WandController))]

public class Player : LivingEntity
{

    public float moveSpeed = 5.0f;
    public float sideStrafeSpeed = 4.0f;
    public float horizontalMouseSpeed = 100f;
    public float verticalMouseSpeed = 3.0f;

    public float throwSpeed = 2000f;

	Camera viewCamera;
	PlayerController controller;
	WandController wandController;


    float rayDistance = 5f;
	// initialize
	protected override void Start ()
    {
        base.Start();
		controller = GetComponent<PlayerController> ();
		wandController = GetComponent<WandController> ();
		viewCamera = Camera.main;

 	}

    void Update()
    {
        float forwardTranslation = Input.GetAxis("Vertical") * moveSpeed;
        float sidewaysTranslation = Input.GetAxis("Horizontal") * sideStrafeSpeed;
        float horizontalRotation = Input.GetAxisRaw("Mouse X") * horizontalMouseSpeed;
        float verticalRotation = Input.GetAxis("Mouse Y") * verticalMouseSpeed;
        forwardTranslation *= Time.deltaTime;
        sidewaysTranslation *= Time.deltaTime;
        // horizontalRotation *= Time.deltaTime;

        transform.Translate(sidewaysTranslation, 0, forwardTranslation);
        transform.Rotate(0, horizontalRotation, 0);

		/*
		if (TakeDamage) 
		{
			Camera.main.transform.localPosition
		}
		*/
        // the frustrations

        /*
        float hitDist = 0.0f;
        Plane playerPlane = new Plane(Vector3.up, transform.position);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (playerPlane.Raycast (ray, out hitDist))
            {
            Vector3 targetPoint = ray.GetPoint(hitDist);
            Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, horizontalMouseSpeed * Time.deltaTime);
        }



        */
        // try some geometry tricks




        // for vertical rotation, move camera and crosshairs not player

        // cast ray forward from player (origin)
        // cast ray to mouse position
        // lerp the difference

        // might need to do some trigonometry haha
        // float hitBoxDistanceThreshold = 1.0f;

        /*
        RaycastHit playerHit;
        if (Physics.Raycast(transform.position, transform.forward, out playerHit))
        {
            Debug.DrawLine(transform.position, playerHit.point, Color.green);
        }

        */

        /*
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit mouseHit;
        if (Physics.Raycast(ray, out mouseHit, 15))
        {
            Debug.DrawLine(ray.origin, mouseHit.point, Color.red);

            if (mouseHit.distance > 10.0f)
            {
                Vector3 targetPoint = mouseHit.point;
                // Determine target rotation. This is the rotation if the player looks at the mouseHit point
                Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
                // smoothly rotate towards the target point
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, horizontalMouseSpeed * Time.deltaTime);
            }
                       
        }
        */


        /* if (Vector3.Distance(mouseHit.point, playerHit.point) > hitBoxDistanceThreshold)
         {
             // Slerp is like lerp but rather than being linear slerp has a "spherical" rate of change 
             // 
             //playerHit.point = Vector3.Slerp(playerHit.point, mouseHit.point, Time.deltaTime);
         }
         */



        // get distance between green raycast hitpoint and red raycast hit point



        // Weapon input
        if (Input.GetMouseButton(0))
        {
            wandController.OnTriggerHold();
        }

		if (Input.GetMouseButtonUp (0)) 
		{
			wandController.OnTriggerRelease();
		}

		/*
        if (Input.GetMouseButton(1))
        {
            // secondary attack
			wandController.ChargeSecondary();

        }
        */
		/*
		if (Input.GetMouseButtonUp (1)) 
		{
			wandController.ShootSecondary();
		}
		*/
        if (Input.GetMouseButton(0) && Input.GetMouseButton(1))
        {
            // special attack
        }
    }
}
