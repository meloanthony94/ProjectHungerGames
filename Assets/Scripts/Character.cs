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

    #region Audio
    [Header("Audio Clips")]
    AudioSource myAudioSource;

    [SerializeField]
    AudioClip actionAStartSFXclip;

    [SerializeField]
    AudioClip actionBStartSFXclip;

    [SerializeField]
    AudioClip actionEndSFXclip;

    [SerializeField]
    AudioClip dashSFXclip;

    [SerializeField]
    AudioClip bumpSFXclip;
    #endregion

    #region Character Variables
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
    #endregion

    #region Dash Variables
    [Header("Dash Variables")]
    [SerializeField]
    [Range(0.0f, 2000.0f)]
    float dashForce;

    [SerializeField]
    [Range(0.0f, 10.0f)]
    float attackForce;

    [SerializeField]
    ForceMode DashType;

    [SerializeField]
    ForceMode AttackType;

    bool isDashing = false;
    bool isDashingLocked = false;
    [Range(0.0f, 10.0f)]
    public float dashCooldownTime = 3;
    [Range(0.0f, 5.0f)]
    //The variable controls when the user can start moving again during dash. 0 = very end of dash
    public float minimumDashVelocity = 0.5f;
    #endregion

    Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        myAudioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();

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
                animator.SetFloat("DashMulti", 1);
                animator.SetBool("dash", true);
                transform.forward = input;
                myRigidBody.MovePosition(transform.position + (transform.forward * walkSpeed));
                //transform.Translate(transform.forward * movementSpeed, Space.World);
            }
            else
                animator.SetBool("dash", false);
        }
        else
        {
            if (myRigidBody.velocity.magnitude < minimumDashVelocity)
            {
                //animator.SetTrigger("idle");
                isDashing = false;
                StartCoroutine(DashCooldownTimer());
            }
        }
        //

        //Controls
        if (Input.GetButtonDown("Dash" + playerId) && !isDashing && !isDashingLocked /*|| Input.GetKeyDown(KeyCode.D)*/)
        {
            isDashing = true;

            if (gameSettings.State == GameSetting.GameState.Play)
            {
                animator.SetBool("dash", true);
                animator.SetFloat("DashMulti", 2);
                PlaySFX(dashSFXclip);
            }

            myRigidBody.AddForce(transform.forward * dashForce, DashType);
        }

        PerformRaycast();

       //if(Input.GetKeyDown(KeyCode.A))
       //
       //   animator.SetTrigger("action");
       //

        if (Input.GetButtonDown("Action" + playerId) && !isPerformingAction && gameSettings.State == GameSetting.GameState.Play)
        {
            if (CurrentPlotReference != null)
            {
                if (CurrentPlotReference.Action(isCritter) != ActionType.None)
                {
                    animator.SetBool("action", true);
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
            if (CurrentPlotReference != null)
            {
                PerformPlotAction(CurrentPlotReference.Action(isCritter));
            }
        }

        if(gameSettings.State == GameSetting.GameState.End)
            myAudioSource.Stop();
    }

    void PlaySFX(AudioClip clip, bool isOneShot = true, bool isLooped = false)
    {
        myAudioSource.loop = isLooped;
        myAudioSource.pitch = Random.Range(0.95f, 1.05f);

        if (isOneShot)
            myAudioSource.PlayOneShot(clip);
        else
        {
            myAudioSource.clip = clip;
            myAudioSource.Play();
        }
    }

    void PerformRaycast()
    {
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
    }

    void SetActionTime(ActionType action)
    {
        switch (action)
        {
            case ActionType.Dig:
                PlaySFX(actionAStartSFXclip);
                if (isFastCharacter)
                    actionCompleteTime = currentTeam.DigSpeedFast;
                else
                    actionCompleteTime = currentTeam.DigSpeedSlow;
                break;
            case ActionType.Plant:
                PlaySFX(actionBStartSFXclip);
                if (isFastCharacter)
                    actionCompleteTime = currentTeam.PlantSpeedFast;
                else
                    actionCompleteTime = currentTeam.PlantSpeedSlow;
                break;
            case ActionType.Harvest:
                PlaySFX(actionAStartSFXclip);
                if (isFastCharacter)
                    actionCompleteTime = currentTeam.HarvestSpeedFast;
                else
                    actionCompleteTime = currentTeam.HarvestSpeedSlow;
                break;
            case ActionType.Eat:
                PlaySFX(actionBStartSFXclip);
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

    public void OnAttacked()
    {
        PlaySFX(bumpSFXclip);
        isPerformingAction = false;
        myProgressBar.gameObject.SetActive(false);
        myRigidBody.isKinematic = false;
    }

    void PerformPlotAction(ActionType action)
    {
        currentActionTime += Time.deltaTime;
        myProgressBar.BarValue = (currentActionTime / actionCompleteTime) * 100;
        actionUI.SetAction(action);

        if (currentActionTime >= actionCompleteTime)
        {
            animator.SetTrigger("idle");
            PlaySFX(actionEndSFXclip);
            isPerformingAction = false;
            CurrentPlotReference.ChangeState();
            myProgressBar.gameObject.SetActive(false);
            myRigidBody.isKinematic = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("OnCollision");
            if (isDashing)
            {
                Character other = collision.gameObject.GetComponent<Character>();
                other?.OnAttacked();
                collision.rigidbody.AddForce(transform.forward * attackForce, AttackType);
                  
            }
        }
    }

    IEnumerator DashCooldownTimer()
    {
        isDashingLocked = true;

        yield return new WaitForSeconds(dashCooldownTime);

        isDashingLocked = false;
    }
}
