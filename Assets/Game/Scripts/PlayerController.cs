using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

	[SerializeField] float forwardSpeed = 10.0f;
	[SerializeField] float horizontalSpeed = 10.0f;
	[SerializeField] float limits = 20.0f;
  [SerializeField] float jumpForce = 5.0f;
  [SerializeField] float balanceFactor = 0.1f;
  [SerializeField] float maxBalanceSpeed = 0.5f;

  new Rigidbody rigidbody;
  new Collider collider;
  float timer = 0.0f;
  float previousDisplacement = 0.0f;
  float previousPitch = 0.0f;

  void Start() {
    rigidbody = GetComponent<Rigidbody>();
    collider = GetComponent<Collider>();
  }

  void FixedUpdate() {
    // Move Forward if not pressed against wall
    if (!Physics.BoxCast(transform.position, collider.bounds.extents, Vector3.forward, Quaternion.identity, 0.2f, LayerMask.GetMask("Wall"))) {
      transform.Translate(Vector3.forward * Time.fixedDeltaTime * forwardSpeed);
    }

    // Get Tilt Movement
    float displacement = 0.0f;
    displacement = Time.fixedDeltaTime * Input.acceleration.x * horizontalSpeed;

    // Get Movement from keyboard
    if (Input.GetKey("right")) {
      displacement = Time.fixedDeltaTime * horizontalSpeed;
    }
    if (Input.GetKey("left")) {
      displacement = Time.fixedDeltaTime * -horizontalSpeed;
    }

    // If on balance board, slow movemnt speed and maintain momentum
    if (Physics.BoxCast(transform.position, collider.bounds.extents, Vector3.down, Quaternion.identity, 0.4f, LayerMask.GetMask("Balance"))) {
      displacement *= balanceFactor;
      displacement += previousDisplacement;
      displacement = Mathf.Clamp(displacement, 0.0f, maxBalanceSpeed);
      previousDisplacement = displacement;
    } else {
      previousDisplacement = displacement * balanceFactor;
    }

    // Apply Horizontal Movement
    transform.Translate(Vector3.right * displacement);

    // Clamp movemnt to limits
    transform.position = new Vector3(Mathf.Clamp(transform.position.x, -limits, limits), transform.position.y, transform.position.z);

    // Get deltaPitch
    float pitch = Input.acceleration.y;
    float deltaPitch = pitch - previousPitch;

    // Check if grounded
    if (timer <= 0.0f) {
      if (Physics.Raycast(transform.position, Vector3.down, 0.4f, LayerMask.GetMask("Ground"))) {
        // If tilted up or key pressed, jump
        if (Mathf.Abs(deltaPitch) > 0.2f || Input.GetKeyDown("up")) {
          rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
          timer = 0.3f;
        }
      }
    } else {
      timer -= Time.fixedDeltaTime;
    }

    previousPitch = pitch;
  }

  void OnTriggerEnter(Collider collider) {
    if (collider.tag == "GameOver") {
      SceneManager.LoadScene(3);
    }

    if (collider.tag == "Finished") {
      SceneManager.LoadScene(2);
    }
  }
}
