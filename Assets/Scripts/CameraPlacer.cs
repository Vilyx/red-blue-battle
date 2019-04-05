using UnityEngine;

public class CameraPlacer : MonoBehaviour {
	[SerializeField]
	private float margin = 1;
	private new Camera camera;

	private void Awake()
	{
		camera = GetComponent<Camera>();
	}

	public void ScaleCamera(Bounds targetBounds)
	{
		float screenRatio = (float)Screen.width / (float)Screen.height;
		float targetRatio = targetBounds.size.x / targetBounds.size.z;

		if (screenRatio >= targetRatio)
		{
			camera.orthographicSize = targetBounds.size.z / 2 * margin;
		}
		else
		{
			float differenceInSize = targetRatio / screenRatio;
			camera.orthographicSize = targetBounds.size.z / 2 * differenceInSize * margin;
		}

		transform.position = new Vector3(targetBounds.center.x, 1f, targetBounds.center.z);
	}
}
