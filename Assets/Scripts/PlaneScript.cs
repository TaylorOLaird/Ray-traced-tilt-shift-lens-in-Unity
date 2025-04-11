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
        float scale_factor = rayTracingScript._FocusDistance / 4.0f;
        _Plane.transform.localScale = new Vector3(scale_factor, scale_factor, scale_factor);

        // move the plane in the positive z direction by _FocusDistance
        transform.position += rayTracingScript._FocusDistance * rayTracingScript.transform.forward;
    }
}
