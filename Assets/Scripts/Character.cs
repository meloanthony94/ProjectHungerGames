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

    [SerializeField]
    private int playerId = 0;

    Rigidbody myRigidBody;

    Plot CurrentPlotReference = null;

    public GameSetting gameSettings;

    [Header("Character Variables")]
    [SerializeField]
    bool isCritter = false;
    [SerializeField]
    [Range(0.0f, 1.0f)]
    float walkSpeed = 0.1f;

    float currentMovementSpeed = 0.1f;
    int layerMask;

    [Header("Dash Variables")]
    [SerializeField]
    [Range(0.0f, 2000.0f)]
    float dashForce;

    [SerializeField]
    ForceMode DashType;

    bool isDashing = false;
    bool isDashingLocked = false;
    [Range(0.0f, 10.0f)]
    public float dashCooldownTime = 3;
    [Range(0.0f, 5.0f)]
    //The variable controls when the user can start moving again during dash. 0 = very end of dash
    public float minimumDashVelocity = 0.5f;


    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody>();
        layerMask = 1 << 8;
        layerMask = ~layerMask;
    }

    // Update is called once per frame
    void Update()
    {
        //Movement
        if (!isDashing)
        {
            Vector3 input = new Vector3(Input.GetAxis("Horizontal" + playerId), 0, -Input.GetAxis("Vertical" + playerId));
            if (input != Vector3.zero)
            {
                transform.forward = input;
                myRigidBody.MovePosition(transform.position + (transform.forward * walkSpeed));
                //transform.Translate(transform.forward * movementSpeed, Space.World);
            }
        }
        else
        {
            if (myRigidBody.velocity.magnitude < minimumDashVelocity)
            {
                isDashing = false;
                StartCoroutine(DashCooldownTimer());
            }
        }
        //

        //Controls
        if (Input.GetButtonDown("Dash" + playerId) && !isDashing && !isDashingLocked)
        {
            isDashing = true;
            myRigidBody.AddForce(transform.forward * dashForce, DashType);
        }

        if (Input.GetButtonDown("Action" + playerId))
        {
            if (CurrentPlotReference != null)
            {
                if (CurrentPlotReference.Action(isCritter) != ActionType.None)
                {
                    CurrentPlotReference.ChangeState();
                }
            }
        }

        //Raycast
        //if hit a plot, grab that ref if current ref is null
        //if not, unhighlight, set to null or to new ref
        //Ternary
        //check against PlotLayer

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, gameSettings.PlayerRaycastLength, layerMask))
        {
            if (hit.transform.GetComponent<Plot>() != CurrentPlotReference)
            {
                CurrentPlotReference?.Highlight(false);
                
                CurrentPlotReference = hit.transform.GetComponent<Plot>();
                CurrentPlotReference?.Highlight(true);
            }
        }
        else
        {
            if (CurrentPlotReference != null)
            {
                CurrentPlotReference.Highlight(false);
                CurrentPlotReference = null;
            }
        }

        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * gameSettings.PlayerRaycastLength, Color.white);
    }

    IEnumerator DashCooldownTimer()
    {
        isDashingLocked = true;

        yield return new WaitForSeconds(dashCooldownTime);

        isDashingLocked = false;
    }
}
