using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Growth : MonoBehaviour {

    public Vector3 initScale;
    public float initAge;
    public float age;
    public float ageFactor = 10;
    
    [SerializeField] private float AgeOfMaturity = 50;

    private void FixedUpdate()
    {
        //Age
        age = (Time.time - initAge) / ageFactor;

        growth();
    }

    private void Start()
    {
        initAge = Time.time;
        initScale = transform.localScale;

    }
    public void growth()
    {
        if (transform.localScale.magnitude <= initScale.magnitude)
        {
            transform.localScale = (initScale * (age / AgeOfMaturity) );
        }
    }

}
