using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreSystem : MonoBehaviour {

    //Core Components
    private Rigidbody rb;
    private ConstantForce constForce;

    //Materials and Fish GameObjects:
    public GameObject tail;
    public GameObject body;

    //Age
    public float age;
    private float initAge;
    public float ageFactor = 10;
    
    //Growth
    public float AgeOfMaturity = 10;
    private Vector3 initScale;
    public Vector3 currScale;

    //Hunger System:
    public float HungerStatus = 100;
    public float HungerThreshold = 75;
    public bool isHungry = false;

    public float HungerReductionFactor = 0.5f;
    public float InitialHungerReductionFactor;
    public float HrfIncrementLimit = 5f;

    //Heirarchy & Hunting:

    public bool isCarnivore;

    public int Heirarchy = 1;
    public float DistanceThreshold = 30f;

    private Vector3 feedingGrowth = Vector3.zero;

    public CreatureManager creatureManager;
    [SerializeField]  private List<CoreSystem> Creatures;

    //Movement
    public float MovementSpeed_Hunt = 2f;
    public float MovementSpeed = 1.5f;
    public int MovementChangeInterval = 2;
    [SerializeField]private float MovementChangeIterator;

    private float rotTimeInstant;
    [SerializeField]private Vector3 currentMovementDirection;

    //Plant System OR Herbivore System:

    List<Plant> Plants;

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
        //Update the creature list.
        Creatures = creatureManager.list;
        //Update the plant list.
        Plants = creatureManager.PlantList;

        //Age
        age = (Time.time - initAge) / ageFactor;

        //Hunger
        if (age < AgeOfMaturity && HungerReductionFactor <= (InitialHungerReductionFactor + InitialHungerReductionFactor*HrfIncrementLimit))
            HungerReductionFactor = InitialHungerReductionFactor + ((age / AgeOfMaturity) * (HrfIncrementLimit * InitialHungerReductionFactor)) ;

        isHungry = (HungerStatus < HungerThreshold);
        HungerStatus -= Time.deltaTime * HungerReductionFactor;
        
        if (HungerStatus <= 0)
            Destroy(this.gameObject);
    }

    private void FixedUpdate()
    {

        //Feeding system.

        Movement();

        if (isCarnivore)
        {
            foreach (CoreSystem creature in Creatures)
            {
                if (Heirarchy > creature.Heirarchy)
                {
                    var posVector = (creature.gameObject.transform.position - this.gameObject.transform.position);
                    var distance = posVector.magnitude;
                    //precaution.
                    distance = (distance < 0) ? (-1 * distance) : distance;

                    if (isHungry && (distance < DistanceThreshold))
                    {
                        rb.AddForce(posVector.normalized * MovementSpeed_Hunt);
                    }
                }


            }
        }

        if (!isCarnivore || (HungerStatus < 25) )
        {
            Plant nearestPlant = null;
            float tempDist = 100;

            foreach (Plant plant in Plants)
            {
                float distance = (plant.gameObject.transform.position - transform.position).magnitude;

                if(distance < tempDist)
                {
                    tempDist = distance;
                    nearestPlant = plant;
                }
                
            }

            Vector3 posVector;
            if (nearestPlant != null)
            {
                posVector = (nearestPlant.gameObject.transform.position - this.gameObject.transform.position);

                if (isHungry)
                    rb.AddForce(posVector.normalized * MovementSpeed_Hunt);
            }
        }

        Growth();
    }

    //----------------------------------------------------------------

    //User Defined Functions:

    public void Growth()
    {
        if (transform.localScale.magnitude <= initScale.magnitude)
        {
            transform.localScale = ( initScale * (age / AgeOfMaturity) + ( ( transform.localScale.magnitude > 2*initScale.magnitude ) ? Vector3.zero : feedingGrowth ) );
        }
    }

    private void Movement()
    {

        if (MovementChangeIterator >= MovementChangeInterval)
        {
            MovementChangeIterator = 0;
            currentMovementDirection = new Vector3(Random.Range(0, 1.5f), Random.Range(0, 1.5f), Random.Range(0, 1.5f));
            rb.AddForce(currentMovementDirection * MovementSpeed);
            rotTimeInstant = Time.time;
        }

        //transform.rotation = Quaternion.Lerp( transform.rotation, Quaternion.Euler(new Vector3(currentMovementDirection.normalized.x, 0, currentMovementDirection.normalized.z) * 180), (Time.time - rotTimeInstant)); 
        transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.position.normalized, ( new Vector3(currentMovementDirection.normalized.x, 0, currentMovementDirection.normalized.z) ), 5 * Time.deltaTime,0.5f));

        MovementChangeIterator += Time.fixedDeltaTime;
    }

    private void InitializeVariables()
    {
        MovementChangeIterator = MovementChangeInterval;

        Heirarchy = Random.Range(1, 5);

        if (Heirarchy > 2)
            isCarnivore = true;
        
        tail.GetComponent<Renderer>().material.color = Color.Lerp(Color.green, Color.red, (Heirarchy / 5f));
        body.GetComponent<Renderer>().material.color = Color.Lerp(Color.green, Color.red, (Heirarchy / 5f));

        if (rb == null)
        rb = gameObject.AddComponent<Rigidbody>();
        rb.useGravity = false;
        //rb.freezeRotation = true;

        if(constForce == null)
        constForce = gameObject.AddComponent<ConstantForce>();

        initAge = Time.time;
        initScale = transform.localScale;
        InitialHungerReductionFactor = HungerReductionFactor;

        currentMovementDirection = new Vector3(Random.Range(0, 1.5f), Random.Range(0, 1.5f), Random.Range(0, 1.5f));
    }

    private void InitializeCreatureList()
    {
        //Initialize List of Creatures:
        creatureManager = FindObjectOfType<CreatureManager>();
        creatureManager.list.Add(this);

        //Initialize Plant list:
        Plants = creatureManager.PlantList;
    }

    //----------------------------------------------------------------

    //OnDisable:

    private void OnDisable()
    {
        creatureManager.list.Remove(this);
    }

    //----------------------------------------------------------------

    //OnCollisionEnter:

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.GetComponent<CoreSystem>() != null) 
        {
            var go = collision.collider.gameObject;

            if (go.GetComponent<CoreSystem>().Heirarchy < Heirarchy && isCarnivore)
            {
                feedingGrowth += initScale * 0.05f;
                HungerStatus = 100;
                rb.velocity = Vector3.zero;
                Destroy(go);
            }
        }

        if ( (collision.collider.gameObject.GetComponent<Plant>() != null) && !isCarnivore)
        {
            var go = collision.collider.gameObject;

            feedingGrowth += initScale * 0.02f;
            HungerStatus += 50;
            rb.velocity = Vector3.zero;
            Destroy(go);
        }
    }
}
