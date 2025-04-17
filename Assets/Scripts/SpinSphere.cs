using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinSphere : MonoBehaviour
{
    public GameObject shpere;
    public float speed = 10f;
    private Transform originalPosition;
    // Start is called before the first frame update
    void Start()
    {
        originalPosition = shpere.transform;
    }

    // Update is called once per frame
    void Update()
    {
        // move sphere up and down based on time
        float sin_offset = Mathf.Sin(Time.time * speed) * 0.02f; // Adjust the multiplier for height
        float cos_offset = Mathf.Cos(Time.time * speed) * 0.02f; // Adjust the multiplier for height
        // move sphere in a circular motion
        shpere.transform.position = new Vector3(originalPosition.position.x + cos_offset, originalPosition.position.y + sin_offset, originalPosition.position.z + sin_offset);


    }
}
