using UnityEngine;

public class PlayerController : MonoBehaviour {

  [SerializeField] float jumpForce = 5.0f;

  Rigidbody rigidbody;
  bool grounded = true;

  void Start() {
    rigidbody = GetComponent<Rigidbody>();
  }

	void Jump() {
    if (grounded) {
      rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
      grounded = false;
    }
	}

  void OnCollisionEnter(Collision collision) {
    if (collision.gameObject.tag == "Ground") {
      grounded = true;
    }
  }
}
