using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MoveSphere : MonoBehaviour
{
    public bool _Dance = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // float r = Mathf.PingPong(Time.time, 1.0f);
        float r = 0.008f;
        float speed = 3.5f;
        if (_Dance)
            transform.position += (new Vector3(Mathf.Cos(Time.time * speed) * r, 0.0f, Mathf.Sin(Time.time * speed) * r));
            
    }
}
