using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneScript : MonoBehaviour
{
    public NewNewMain rayTracingScript;
    private GameObject _Plane;
    // Start is called before the first frame update
    void Start()
    {
        // search children for the plane object
        _Plane = transform.Find("Plane").gameObject;

    }

    // Update is called once per frame
    void Update()
    {
        // show or hide the plane based on the _ShowPlaneDebug variable
        if (rayTracingScript._ShowPlaneDebug)
        {
            _Plane.SetActive(true);
        }
        else
        {
            _Plane.SetActive(false);
        }

        // updated the plane position to be the same as the camera
        transform.position = rayTracingScript.transform.position;
        transform.rotation = rayTracingScript.transform.rotation;

        // also adjust the plane scale based on _focusDistance
        float scale_factor = 20.0f * (rayTracingScript._FocusDistance / 4.0f) * (rayTracingScript._DefocusRadius);
        _Plane.transform.localScale = new Vector3(scale_factor, 0.002f, scale_factor);

        // move the plane in the positive z direction by _FocusDistance
        transform.position += rayTracingScript._FocusDistance * rayTracingScript.transform.forward;

        float tiltX = rayTracingScript._XTilt;
        float tiltY = rayTracingScript._YTilt;

        // add the tilts to the plane rotations
        transform.rotation *= Quaternion.Euler(tiltX, tiltY, 0.0f);

    }
}
