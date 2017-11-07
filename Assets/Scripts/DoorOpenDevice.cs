using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpenDevice : MonoBehaviour {
	[SerializeField] private Vector3 dPos; 		// The position to offest the door when its open. 

	private bool _open; 			// To keep track of the door open state. 

	public void Operate() { 
		if (_open) { 			// Open or the close the door depending on the open state. 
			Vector3 pos = transform.position - dPos; 
			transform.position = pos; 
		} else { 
			Vector3 pos = transform.position + dPos; 
			transform.position = pos; 
		} 
		_open = !_open; 
	} 

	public void Activate() { 
		if (!_open) { 
			Vector3 pos = transform.position + dPos; 
			transform.position = pos; 
			_open = true; 
		} 
	} 

	public void Deactivate() { 
		if (_open) { 
			Vector3 pos = transform.position - dPos; 
			transform.position = pos; 
			_open = false; 
		} 
	} 
}
