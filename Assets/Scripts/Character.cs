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
    public ProgressBar myProgressBar;
    public ActionUI actionUI;
    GameSetting.TeamSetting currentTeam;
    bool isPerformingAction = false;


    [Header("Character Variables")]
    [SerializeField]
    bool isCritter = false;
    [SerializeField]
    bool isFastCharacter = false;
    [SerializeField]
    [Range(0.0f, 1.0f)]
    float walkSpeed = 0.1f;

    float currentMovementSpeed = 0.1f;
    float actionCompleteTime = 0;
    float currentActionTime = 0;
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
        layerMask = 1 << 9;
        //layerMask = ~layerMask;

        //Set Team
        if (isCritter)
            currentTeam = gameSettings.Critter;
        else
            currentTeam = gameSettings.Farmer;
        //
    }

    // Update is called once per frame
    void Update()
    {
        //Movement
        if (!isDashing && !isPerformingAction)
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



        //Raycast
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

        if (Input.GetButtonDown("Action" + playerId) && !isPerformingAction)
        {
            if (CurrentPlotReference != null)
            {
                if (CurrentPlotReference.Action(isCritter) != ActionType.None)
                {
                    isPerformingAction = true;
                    currentActionTime = 0;
                    SetActionTime(CurrentPlotReference.Action(isCritter));
                    myProgressBar.gameObject.SetActive(true);
                    myRigidBody.isKinematic = true;
                }
            }
        }

        if (isPerformingAction)
        {
            if(CurrentPlotReference != null)
                PerformPlotAction(CurrentPlotReference.Action(isCritter));
        }
    }

    void SetActionTime(ActionType action)
    {
        switch (action)
        {
            case ActionType.Dig:
                if (isFastCharacter)
                    actionCompleteTime = currentTeam.DigSpeedFast;
                else
                    actionCompleteTime = currentTeam.DigSpeedFast;
                break;
            case ActionType.Plant:
                if (isFastCharacter)
                    actionCompleteTime = currentTeam.PlantSpeedFast;
                else
                    actionCompleteTime = currentTeam.PlantSpeedSlow;
                break;
            case ActionType.Harvest:
                if (isFastCharacter)
                    actionCompleteTime = currentTeam.HarvestSpeedFast;
                else
                    actionCompleteTime = currentTeam.HarvestSpeedSlow;
                break;
            case ActionType.Eat:
                if (isFastCharacter)
                    actionCompleteTime = currentTeam.EatSpeedFast;
                else
                    actionCompleteTime = currentTeam.EatSpeedSlow;
                break;
            case ActionType.None:
                break;

            default:
                break;
        }
    }

    void PerformPlotAction(ActionType action)
    {
        currentActionTime += Time.deltaTime;
        myProgressBar.BarValue = (currentActionTime / actionCompleteTime) * 100;
        actionUI.SetAction(action);

        if (currentActionTime >= actionCompleteTime)
        {
            isPerformingAction = false;
            CurrentPlotReference.ChangeState();
            myProgressBar.gameObject.SetActive(false);
            myRigidBody.isKinematic = false;
        }
    }

    IEnumerator DashCooldownTimer()
    {
        isDashingLocked = true;

        yield return new WaitForSeconds(dashCooldownTime);

        isDashingLocked = false;
    }
}
