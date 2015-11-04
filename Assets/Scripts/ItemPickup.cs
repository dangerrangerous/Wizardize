using UnityEngine;
using System.Collections;

public class ItemPickup : MonoBehaviour {
    /* 


    public int distanceToItem;
    public GameObject potion;

	void Start () {
	
	}
	
	void Update () {
        Collect();
	}

    void Collect()
    {
        if (Input.GetMouseButtonUp(1))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out hit, distanceToItem))
            {
                if(hit.collider.gameObject.name == "Potion")
                {
                    Debug.DrawLine(Vector3.zero, new Vector3(1, 0, 0), Color.red, 3.0f);
                    Debug.Log("potion hit");
                    Destroy(hit.collider.gameObject);
                }
            }
        }

    }
    */
}
