using UnityEngine;

public class GameController : MonoBehaviour {

	[SerializeField] float forwardSpeed = -10.0f;
	[SerializeField] float horizontalSpeed = 10.0f;
	[SerializeField] float limits = 20.0f;
	[SerializeField] GameObject player;

	void FixedUpdate() {
		transform.Translate(Vector3.forward * Time.fixedDeltaTime * forwardSpeed);

		Vector3 currentPosition = transform.position;

    currentPosition.x = Mathf.Clamp(currentPosition.x + Time.fixedDeltaTime * -Input.acceleration.x * horizontalSpeed, -limits, limits);

    if (Input.GetKey("right")) {
        currentPosition.x = Mathf.Clamp(currentPosition.x + Time.fixedDeltaTime * -horizontalSpeed, -limits, limits);
    }
    if (Input.GetKey("left")) {
      currentPosition.x = Mathf.Clamp(currentPosition.x + Time.fixedDeltaTime * horizontalSpeed, -limits, limits);
    }

		transform.position = currentPosition;

		if (Input.acceleration.y < -0.8f || Input.GetKeyDown("up")) {
			player.SendMessage("Jump");
		}
	}
}
