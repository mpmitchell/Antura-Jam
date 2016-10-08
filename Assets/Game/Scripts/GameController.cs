using UnityEngine;

public class GameController : MonoBehaviour {

	public float speed = -10.0f;

	void Start() {
		Input.gyro.enabled = true;
	}

	void Update() {
		transform.Translate(Vector3.forward * Time.deltaTime * speed);
		// Move ground left/right based on gyroscope
	}
}
