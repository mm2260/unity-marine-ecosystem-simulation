using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailRotation : MonoBehaviour {

    private void FixedUpdate()
    {
        transform.Rotate( new Vector3(0, Mathf.Sin(Time.time * 5) * 2f, 0), Space.Self);     
    }   
}
