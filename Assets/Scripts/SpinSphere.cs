using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinSphere : MonoBehaviour
{
    public GameObject shpere;
    public float speed = 10f;
    public float radius = 1f;
    public Transform plane;
    private Transform originalPosition;
    private Vector3 centerOfRotation;
    // Start is called before the first frame update
    void Start()
    {
        originalPosition = shpere.transform;
        centerOfRotation = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        // move sphere up and down based on time
        float sin_offset = Mathf.Sin(Time.time * speed);
        float cos_offset = Mathf.Cos(Time.time * speed);


        // move sphere in a circular motion
        // shpere.transform.position = new Vector3(originalPosition.position.x + cos_offset, originalPosition.position.y + sin_offset, originalPosition.position.z + sin_offset);

        // get right and up vectors of the plane
        Vector3 right = plane.TransformDirection(Vector3.right);
        Vector3 up = plane.TransformDirection(Vector3.up);
        Vector3 forward = plane.TransformDirection(Vector3.forward);

        shpere.transform.position = plane.position + (right * radius * cos_offset) + (forward * radius * sin_offset);

    }
}
