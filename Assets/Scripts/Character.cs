using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum ActionType
{
    Dig,
    Plant,
    Harvest,
    Eat,
    None
}

public class Character : MonoBehaviour
{
    [SerializeField]
    UnityEvent EatEvent;

    Rigidbody myRigidBody;

    Plot CurrentPlotReference = null;

    [Header("Character Variables")]
    [SerializeField]
    bool isCritter = false;
    [SerializeField]
    [Range(0.0f, 1.0f)]
    float movementSpeed;
    [SerializeField]
    int dashSpeed;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //Movement
        Vector3 newPos = transform.position + ((Vector3.right  * Input.GetAxis("Horizontal")) * movementSpeed);
        newPos = newPos + ((Vector3.forward * Input.GetAxis("Vertical")) * movementSpeed);
        myRigidBody.MovePosition(newPos); 
        //
        
        if (Input.GetButtonDown("Action"))
            print("action");

        //Raycast
        //if hit a plot, grab that ref if current ref is null
        //if not, unhighlight, set to null or to new ref
        //Ternary
        //check against PlotLayer
    }
}
