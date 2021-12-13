using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour {

    //Core Components:
    private Rigidbody rb;

    //Age
    public float age;
    private float initAge;
    public float ageFactor = 10;

    //Growth
    public float AgeOfMaturity = 10;
    private Vector3 initScale;
    public Vector3 currScale;

    public CreatureManager creatureManager;
    
    //----------------------------------------------------------------
    
    //Start and OnEnable:

    private void Start()
    {
        InitializeVariables();

    }

    private void OnEnable()
    {
        InitializeVariables();

        InitializeCreatureList();
    }

    //Update Functions:

    private void Update()
    {
        //Age
        age = (Time.time - initAge) / ageFactor;

    }

    private void FixedUpdate()
    {
        Growth();

        transform.rotation = Quaternion.Euler(transform.rotation.x, Mathf.Sin(Time.time) * 2.5f, Mathf.Sin(Time.time) * 5f);
    }

    //----------------------------------------------------------------

    //User Defined Functions:

    public void Growth()
    {
        if (transform.localScale.magnitude <= initScale.magnitude)
        {
            transform.localScale = (initScale * (age / AgeOfMaturity));
        }
    }

    private void InitializeVariables()
    {
        initAge = Time.time;
        initScale = transform.localScale;

        if (GetComponent<Rigidbody>() == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.useGravity = true ;
            rb.constraints = RigidbodyConstraints.FreezePositionX;
        
            rb.constraints = RigidbodyConstraints.FreezePositionZ;
        
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
        
    }

    private void InitializeCreatureList()
    {
        //Initialize List of Creatures:
        creatureManager = FindObjectOfType<CreatureManager>();
        creatureManager.PlantList.Add(this);
    }

    //----------------------------------------------------------------

    //OnDisable:

    private void OnDisable()
    {
        creatureManager.PlantList.Remove(this);
    }
}
