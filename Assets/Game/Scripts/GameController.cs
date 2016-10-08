using UnityEngine;

public class GameController : MonoBehaviour {

	[SerializeField] float forwardSpeed = -10.0f;
	[SerializeField] float horizontalSpeed = 5.0f;
	[SerializeField] float limits = 20.0f;
	[SerializeField] GameObject player;

  GUIStyle style;

	void Start() {
		Input.gyro.enabled = true;
    style = new GUIStyle();
    style.fontSize = 100;
	}

	void FixedUpdate() {
		transform.Translate(Vector3.forward * Time.fixedDeltaTime * forwardSpeed);

		Vector3 currentPosition = transform.position;
		float roll = Input.gyro.attitude.eulerAngles.y;

		if ((roll < 90.0f && roll > 20.0f) || Input.GetKey("left")) {
			currentPosition.x += Time.fixedDeltaTime * horizontalSpeed;
		} else if ((roll > 270.0f && roll < 340.0f) || Input.GetKey("right")) {
			currentPosition.x -= Time.fixedDeltaTime * horizontalSpeed;
		}

		if (currentPosition.x > limits) {
			currentPosition.x = limits;
		} else if (currentPosition.x < -limits) {
			currentPosition.x = -limits;
		}

		transform.position = currentPosition;
	}

	void OnGUI() {
		GUI.Label(new Rect(0, 0, 120, 30), Input.gyro.attitude.eulerAngles.ToString(), style);
	}
}
