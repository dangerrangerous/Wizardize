using UnityEngine;
using System.Collections;

public class WandController : MonoBehaviour
{

	public Transform weaponHold;
	public Wand startingWand;
	Wand equippedWand;
    public float mana;

	void Start()
    {

		if (startingWand != null)
        {
			EquipWand(startingWand);
		}

        
	}

	public void EquipWand(Wand wandToEquip)
    {
		if (equippedWand != null)
        {
			Destroy (equippedWand.gameObject);
		}
		equippedWand = Instantiate (wandToEquip, weaponHold.position, weaponHold.rotation) as Wand;
		equippedWand.transform.parent = weaponHold;
        equippedWand.mana = 5;
	}

   /* 
   public void ManaRecharge()
    {
        if (equippedWand != null)
        {
            equippedWand.ManaRecharge();
        }
    }
    */
	public void OnTriggerHold()
	{
		if (equippedWand != null) 
		{
			equippedWand.OnTriggerHold();
		}
	}

	public void OnTriggerRelease()
	{
		if (equippedWand != null) 
		{
			equippedWand.OnTriggerRelease();

		}
	}
	/*
	public void ChargeSecondary()
	{
		if (equippedWand != null) {
			equippedWand.ChargeSecondary();
		}
	}
	*/
	/*
	public void ShootSecondary()
	{
		if (equippedWand != null) 
		{
			equippedWand.ShootSecondary();
			
		}
	}
	*/
}
