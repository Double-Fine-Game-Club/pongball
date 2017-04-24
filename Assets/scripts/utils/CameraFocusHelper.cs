using UnityEngine;
using System.Collections;

public class CameraFocusHelper : MonoBehaviour
{

    private void LateUpdate()
    {
        CameraFocus.SetCameraFocus(this.gameObject);
    }

}
