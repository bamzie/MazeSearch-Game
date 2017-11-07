using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]		// The surrounding lines are context for placing the RequireComponent() method. 
public class RelativeMovement : MonoBehaviour {
	[SerializeField] private Transform target;  // The scripts need a reference to the object to move relative to.(Camer)

	public float rotSpeed = 15.0f; 
	public float moveSpeed = 6.0f; 
	public float jumpSpeed = 15.0f; 
	public float gravity = -9.8f; 
	public float terminalVelocity = -10.0f; 
	public float minFall = -1.5f; 
	public float pushForce = 3.0f; 	// Amount of force to apply. 

	private float _vertSpeed; 

	private CharacterController _charController;
	private ControllerColliderHit _contact;  		// Needed to store collision data between functions. 
	private Animator _animator; 


	// Use this for initialization
	void Start () {
		_vertSpeed = minFall; 	// Initialize the vertical speed to the minimum falling speed at the start of the exitsting function.
		_charController = GetComponent<CharacterController> (); 	// Here's a pattern you've seen in previous chapters,
																	// used for getting access to ther components. 
		_animator = GetComponent<Animator>(); 
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 	movement = Vector3.zero; 		// Start with vector (0,0,0) and add movement components progressively

		float horInput = Input.GetAxis ("Horizontal"); 
		float vertInput = Input.GetAxis ("Vertical"); 
		if (horInput != 0 || vertInput != 0) {		// Only handle movement while arrow keys are pressed.  
			movement.x = horInput * moveSpeed; 		// MoveSpeed applies movement to the player. 
			movement.z = vertInput  * moveSpeed; 
			movement = Vector3.ClampMagnitude (movement, moveSpeed); // Limit diagonal movement to the same speed as movement along an axis. 

			Quaternion tmp = target.rotation; 		// Keep the initial rotation to restore after finshing with the target object
			target.eulerAngles = new Vector3 (0, target.eulerAngles.y, 0); 
			movement = target.TransformDirection (movement); 	// Transform movement direction from Local to Global coordinates.  
			target.rotation = tmp; 

			Quaternion direction = Quaternion.LookRotation (movement); 
			transform.rotation = Quaternion.Lerp (transform.rotation, direction, rotSpeed * Time.deltaTime);
			// With the last two lines the player rotates smoothly on the screen. (Lerp method)
		}

		_animator.SetFloat ("Speed", movement.sqrMagnitude); 

		bool hitGround = false; 
		RaycastHit hit; 

		if (_vertSpeed < 0 && Physics.Raycast (transform.position, Vector3.down, out hit)) { 	// Check if the player is falling
			float check = (_charController.height + _charController.radius) / 1.9f; 		// THe distance to check against (extend slightly beyond the bottom of the capsule. 
			hitGround = hit.distance <= check; 
		} 

		if (hitGround) { 							// CHeck the raycasting result. 
			if (Input.GetButtonDown ("Jump")) { 	// React to Jump button while on the ground. 
				_vertSpeed = jumpSpeed; 
			} else { 
				_vertSpeed = -0.1f; 
				_animator.SetBool ("Jump", false); 
			} 
		} else { 									// If not on the ground, then apply gravity until terminal velocity is reached. 
			_vertSpeed += gravity * 5 * Time.deltaTime; //Apply a realistic fall. 
			if (_vertSpeed < terminalVelocity) { 
				_vertSpeed = terminalVelocity; 
			} 
			if (_contact != null) { 
				_animator.SetBool ("Jump", true); 
			}

			if (_charController.isGrounded) { 		// Raycast didnt detect ground, but the capsule is touching the ground. 
				if (Vector3.Dot (movement, _contact.normal) < 0) { 	// Respond slightly differently depending on whether the character is facing the contact point. 
					movement = _contact.normal * moveSpeed; 
				} else { 
					movement += _contact.normal * moveSpeed; 
				} 
			} 
		}

		movement.y = _vertSpeed; 

		movement *= Time.deltaTime; 	
		_charController.Move (movement); // Remember to always multiply movements by deltaTime to make them frame rate-independent.
	}

	void OnControllerColliderHit(ControllerColliderHit hit) { 	// Store the collision data in the callback when a collision is detected. 
		_contact = hit; 

		Rigidbody body = hit.collider.attachedRigidbody; // Check if the collided object has a Rigidbody to recieve physics forces. 
		if (body != null && !body.isKinematic) { 
			body.velocity = hit.moveDirection * pushForce; //Apply velocity to the physics body. 
		}
	} 
}
