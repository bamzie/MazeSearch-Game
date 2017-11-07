using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour, IGameManager {
	public ManagerStatus status { get; private set; } 	// Property can be read from anywhere but only set within this script.

	public string equippedItem {get; private set; } 
	private Dictionary<string, int> _items; 	// Dictionary is declared with two types: key and the value. 

	public void Startup() { 
		Debug.Log ("Inventory manager starting...");	// Any long running startup tasks go here. 

		_items = new Dictionary<string, int> (); 	// Initialize the empty item list. 

		status = ManagerStatus.Started; 				// For long running tasks, use status 'Initializing' instead. 
	} 

	private void DisplayItems() { 		// Print console message of the current inventory. 
		string itemDisplay = "Items: "; 
		foreach (KeyValuePair<string, int> item in _items) { 
			itemDisplay += item.Key + "(" + item.Value + ") "; 
		} 
		Debug.Log (itemDisplay); 
	} 

	public void Additem(string name) { 		// Other scripts cant manipulate the item list directly but can call this. 
		if (_items.ContainsKey (name)) { 	// Check for existing entries before entering new data. 
			_items [name] += 1; 
		} else { 
			_items [name] = 1; 
		} 

		DisplayItems (); 
	} 

	public List<string> GetItemList() { 
		List<string> list = new List<string> (_items.Keys); 	// Returns a List of all the dictionary keys. 
		return list; 
	} 

	public int GetItCount(string name) { 		// Returns how many of that item are in inventory. 
		if (_items.ContainsKey (name)) { 
			return _items [name]; 
		} 
		return 0; 
	} 

	public bool EquipItem(string name) {
		if (_items.ContainsKey(name) && equippedItem != name) {
			equippedItem = name;
			Debug.Log("Equipped " + name);
			return true;
		}

		equippedItem = null;
		Debug.Log("Unequipped");
		return false;
	} 

	public bool ConsumeItem(string name) { 
		if (_items.ContainsKey (name)) { 
			_items [name]--; 
			if (_items [name] == 0) { 
				_items.Remove (name); 
			}
		} else { 
			Debug.Log ("cannot consume " + name); 
			return false; 
		} 

		DisplayItems (); 
		return true; 
	}

}
