using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages camera focus
/// </summary>

public class CameraFocus : MonoBehaviour {

	// Current object that we're focusing on
	private GameObject currentFocus;
	private Vector3 targetOffset;

	// How much to show of the object we're focusing on
	private const float viewScale = 1.05f;


	// Sets the focus of the main camera to the object specified
	static public void SetCameraFocus(GameObject newFocus)
	{
		Camera.main.GetComponent<CameraFocus>().currentFocus = newFocus;

		// Offset is dynamically determined by the fov of the camera
		// and the size of the object, so we need to recalculate it
		Camera.main.GetComponent<CameraFocus>().RecalculateOffset();
	}


	void Start()
	{
		Debug.Assert(GetComponent<Camera>() == Camera.main, "This should only be attached to the main camera!");

		// TODO: don't put this here. Have the current field call CameraAdjuster.SetCameraFocus(this) instead
		SetCameraFocus(GameObject.Find("playingfield01"));
    }

	void LateUpdate()
	{
		if (currentFocus)
		{
			// TODO: lerping

			// Look at the new object
			transform.LookAt(currentFocus.transform, new Vector3(1,0,0));
			transform.position = currentFocus.transform.position + targetOffset;
        }
    }

	void RecalculateOffset()
	{
		Camera camera = GetComponent<Camera>();
        Vector3 normal = new Vector3(0, 1, 0);  // HACK: assuming we're always looking down

		// Get the bounds of the object
		// Note that we're assuming an AABB is OK here since the normal hack above is one of those axes
		Bounds bounds = new Bounds(currentFocus.transform.position, Vector3.zero);
		foreach (Renderer r in currentFocus.GetComponentsInChildren<Renderer>())
		{
			bounds.Encapsulate(r.bounds);
		}

		Vector3 extents = bounds.extents * viewScale;

		// Calculate distance needed to fit along different axes
		// This likely depends on the normal hack too...
		float distx = extents.x / Mathf.Tan(Mathf.Deg2Rad * camera.fieldOfView / 2) + extents.y;
		float distz = extents.z / Mathf.Tan(Mathf.Deg2Rad * camera.fieldOfView * camera.aspect / 2) + extents.y;

		float offset = Mathf.Max(distx, distz, camera.nearClipPlane);
		Debug.Assert(offset < camera.farClipPlane, "Object too big to fully fit in render frustrum");

		targetOffset = normal * offset;

		// Take into account the center of the bounds too
		targetOffset += bounds.center - currentFocus.transform.position;
    }
}
