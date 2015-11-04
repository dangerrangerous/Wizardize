using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]

public class PlayerController : MonoBehaviour
{

    Vector3 velocity;
    // rigidbody for moving the player
    Rigidbody myRigidbody;

    int potionCount = 0;
    // startMoveSpeed should be tied to the Player... i think
    float startMoveSpeed = 5;
    public int moveSpeedDuration = 10;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();


    }

    public void Move(Vector3 _velocity)
    {
        velocity = _velocity;
    }

    public void LookAt(Vector3 lookPoint)
    {
        Vector3 heightCorrectedPoint = new Vector3(lookPoint.x, transform.position.y, lookPoint.z);
        transform.LookAt(heightCorrectedPoint);
    }
    // Move rigidbody in FixedUpdate - in small regular steps so we don't go through objects
    private void FixedUpdate()
    {
        myRigidbody.MovePosition(myRigidbody.position + velocity * Time.fixedDeltaTime);

    }
    

    // super basic potion pickup
    void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.CompareTag("Pick Up"))
        {
            other.gameObject.SetActive(false);
            potionCount = potionCount + 1;
            Debug.Log("Potion Count: " + potionCount);
            // Find player object
            GameObject thePlayer = GameObject.Find("Player");
            Player player = thePlayer.GetComponent<Player>();

            // store initial movespeed, increase current movespeed, reset to initial after set time
            
            player.moveSpeed += player.moveSpeed * 0.4f;

            StartCoroutine(moveSpeedBuffTimer());

        }
    }
    IEnumerator moveSpeedBuffTimer()
    {
        yield return new WaitForSeconds(moveSpeedDuration);
        playerMoveSpeedUndo();
    }

    void playerMoveSpeedUndo()
    {
        GameObject thePlayer = GameObject.Find("Player");
        Player player = thePlayer.GetComponent<Player>();
        player.moveSpeed = startMoveSpeed;
    }
}