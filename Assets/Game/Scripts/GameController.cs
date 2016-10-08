using UnityEngine;

public class GameController : MonoBehaviour {

	[SerializeField] float forwardSpeed = -10.0f;
	[SerializeField] float deadZone = 15.0f;
	[SerializeField] float minSpeed = 0.5f;
	[SerializeField] float maxSpeed = 10.0f;
	[SerializeField] float limits = 20.0f;
	[SerializeField] GameObject player;

  GUIStyle style;

	void Start() {
		Input.gyro.enabled = true;
    style = new GUIStyle();
    style.fontSize = 50;
	}

	void FixedUpdate() {
		transform.Translate(Vector3.forward * Time.fixedDeltaTime * forwardSpeed);

		Vector3 currentPosition = transform.position;
		float roll = GetAngleByDeviceAxis(Vector3.up);
		float pitch = GetAngleByDeviceAxis(Vector3.right);
		float pitchRate = Input.gyro.rotationRate.x;

		if ((roll < 90.0f && roll > deadZone) || Input.GetKey("left")) {
      float speed = maxSpeed;

      if (roll < 45.0f) {
        float oldRange = (45.0f - deadZone);
        float newRange = (maxSpeed - minSpeed);
        speed = (((roll - deadZone) * newRange) / oldRange) + minSpeed;
      }

      currentPosition.x -= Time.fixedDeltaTime * speed;
		} else if ((roll > 270.0f && roll < (360.0f - deadZone)) || Input.GetKey("right")) {
      float speed = maxSpeed;

      if (roll > 315.0f) {
        float oldRange = ((360.0f - deadZone) - 315.0f);
        float newRange = (maxSpeed - minSpeed);
        speed = (((roll - 315.0f) * newRange) / oldRange) + minSpeed;
        speed = newRange - speed;
      }

      currentPosition.x += Time.fixedDeltaTime * speed;
		}

		if (currentPosition.x > limits) {
			currentPosition.x = limits;
		} else if (currentPosition.x < -limits) {
			currentPosition.x = -limits;
		}

		transform.position = currentPosition;

		if ((Mathf.Abs(pitchRate) > 0.4 && pitch > 70.0f && pitch < 90.0f) || Input.GetKeyDown("up")) {
			player.SendMessage("Jump");
		}
	}

	void OnGUI() {
		GUI.Label(new Rect(0, 0, 120, 30), "Roll" + GetAngleByDeviceAxis(Vector3.up).ToString(), style);
		GUI.Label(new Rect(0, 50, 120, 30), "Pitch" + GetAngleByDeviceAxis(Vector3.right).ToString(), style);
	}

  // http://answers.unity3d.com/answers/1208293/view.html
  /// <summary>
  /// Returns the rotation angle of given device axis. Use Vector3.right to obtain pitch, Vector3.up for yaw and Vector3.forward for roll.
  /// This is for landscape mode. Up vector is the wide side of the phone and forward vector is where the back camera points to.
  /// </summary>
  /// <returns>A scalar value, representing the rotation amount around specified axis.</returns>
  /// <param name="axis">Should be either Vector3.right, Vector3.up or Vector3.forward. Won't work for anything else.</param>
  float GetAngleByDeviceAxis(Vector3 axis) {
    Quaternion deviceRotation = Input.gyro.attitude;
    Quaternion eliminationOfOthers = Quaternion.Inverse(
      Quaternion.FromToRotation(axis, deviceRotation * axis)
    );
    Vector3 filteredEuler = (eliminationOfOthers * deviceRotation).eulerAngles;

    float result = filteredEuler.z;
    if (axis == Vector3.up) {
      result = filteredEuler.y;
    }
    if (axis == Vector3.right) {
      // incorporate different euler representations.
      result = (filteredEuler.y > 90 && filteredEuler.y < 270) ? 180 - filteredEuler.x : filteredEuler.x;
    }
    return result;
  }
}
