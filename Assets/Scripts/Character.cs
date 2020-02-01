using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum ActionType
{
    Dig,
    Plant,
    Harvest,
    Eat
}

public class Character : MonoBehaviour
{
    [SerializeField]
    UnityEvent EatEvent;

    Plot CurrentPlotReference = null;

    [Header("Character Variables")]
    [SerializeField]
    bool isCritter = false;
    [SerializeField]
    int movementSpeed;
    [SerializeField]
    int dashSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Movement

        //Raycast
        //if hit a plot, grab that ref if current ref is null
        //if not, unhighlight, set to null or to new ref
        //Ternary
        //check against PlotLayer
    }
}
