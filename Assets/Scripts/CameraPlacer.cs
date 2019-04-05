using UnityEngine;

public class CameraPlacer : MonoBehaviour {


	/// <summary>
	/// Насколько больше будет Bounds при вычислении размера камеры
	/// </summary>
	[SerializeField]
	private float margin = 1;
	private new Camera camera;

	private void Awake()
	{
		camera = GetComponent<Camera>();
	}


	/// <summary>
	/// Установить размер камеры таким чтобы Bounds поместился целиком
	/// </summary>
	/// <param name="targetBounds"></param>
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
