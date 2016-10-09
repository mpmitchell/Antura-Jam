using UnityEngine;

public class PlayerController : MonoBehaviour {

	[SerializeField] float forwardSpeed = 10.0f;
	[SerializeField] float horizontalSpeed = 10.0f;
	[SerializeField] float limits = 20.0f;
  [SerializeField] float jumpForce = 5.0f;

  new Rigidbody rigidbody;
  new Collider collider;
  float timer = 0.0f;

  void Start() {
    rigidbody = GetComponent<Rigidbody>();
    collider = GetComponent<Collider>();
  }

  void FixedUpdate() {
    // Move Forward if not pressed against wall
    if (!Physics.BoxCast(transform.position, collider.bounds.extents, Vector3.forward, Quaternion.identity, 0.2f, LayerMask.GetMask("Wall"))) {
      transform.Translate(Vector3.forward * Time.fixedDeltaTime * forwardSpeed);
    }

    // Get Tilt Movement, clamped to limits
		Vector3 currentPosition = transform.position;
    currentPosition.x = Mathf.Clamp(currentPosition.x + Time.fixedDeltaTime * Input.acceleration.x * horizontalSpeed, -limits, limits);

    // Get Movement from keyboard
    if (Input.GetKey("right")) {
      currentPosition.x = Mathf.Clamp(currentPosition.x + Time.fixedDeltaTime * horizontalSpeed, -limits, limits);
    }
    if (Input.GetKey("left")) {
      currentPosition.x = Mathf.Clamp(currentPosition.x + Time.fixedDeltaTime * -horizontalSpeed, -limits, limits);
    }

    // Apply Horizontal Movement
		transform.position = currentPosition;

    // Check if grounded
    if (timer <= 0.0f) {
      if (Physics.Raycast(transform.position, Vector3.down, 0.4f, LayerMask.GetMask("Ground"))) {
        // If tilted up or key pressed, jump
        if (Input.acceleration.y < -0.8f || Input.GetKeyDown("up")) {
          rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
          timer = 0.3f;
        }
      }
    } else {
      timer -= Time.fixedDeltaTime;
    }
  }
}
