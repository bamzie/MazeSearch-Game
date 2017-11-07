using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleItem : MonoBehaviour {
	[SerializeField] private string itemName; 

	void OnTriggerEnter(Collider other) { 
		Managers.Inventory.Additem (name); 
		Destroy(this.gameObject) ;
	} 
}
