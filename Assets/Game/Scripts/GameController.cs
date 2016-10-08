using UnityEngine;

public class GameController : MonoBehaviour {

	public float forwardSpeed = -10.0f;
	public float horizontalSpeed = 5.0f;
	public float limits = 20.0f;

	void Start() {
		Input.gyro.enabled = true;
	}

	void Update() {
		transform.Translate(Vector3.forward * Time.deltaTime * forwardSpeed);

		Vector3 currentPosition = transform.position;
		float roll = Input.gyro.attitude.eulerAngles.y;

		if (roll < 90.0f) {
			currentPosition.x += Time.deltaTime * horizontalSpeed;
		} else if (roll > 270.0f) {
			currentPosition.x -= Time.deltaTime * horizontalSpeed;
		}

		if (currentPosition.x > limits) {
			currentPosition.x = limits;
		} else if (currentPosition.x < -limits) {
			currentPosition.x = -limits;
		}

		transform.position = currentPosition;
	}
}
