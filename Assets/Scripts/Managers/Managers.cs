using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(PlayerManager))]	//Ensure that the various managers exist.
[RequireComponent(typeof(InventoryManager))]

public class Managers : MonoBehaviour {
	public static PlayerManager Player {get; private set;}	// Static properties that other code uses to access managers. 
	public static InventoryManager Inventory {get; private set;}

	private List<IGameManager> _startSequence;		// The list of managers to loop through during startup sequence. 

	void Awake() {
		Player = GetComponent<PlayerManager>();
		Inventory = GetComponent<InventoryManager>();

		_startSequence = new List<IGameManager>();
		_startSequence.Add(Player);
		_startSequence.Add(Inventory);

		StartCoroutine(StartupManagers());	// Launch the startup sequence asynchronously.
	}

	private IEnumerator StartupManagers() {
		foreach (IGameManager manager in _startSequence) {
			manager.Startup();
		}

		yield return null;

		int numModules = _startSequence.Count;
		int numReady = 0;

		while (numReady < numModules) {		// Keeo looping until all managers are started. 
			int lastReady = numReady;
			numReady = 0;

			foreach (IGameManager manager in _startSequence) {
				if (manager.status == ManagerStatus.Started) {
					numReady++;
				}
			}

			if (numReady > lastReady)
				Debug.Log("Progress: " + numReady + "/" + numModules);

			yield return null;		// Pause for one frame before checking again. 
		}

		Debug.Log("All managers started up");
	}
}
