using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Plot : MonoBehaviour
{
    public enum State
    {
        Empty,
        Hole,
        Plant,
        Crop
    }

    [SerializeField]
    private State currentState = State.Empty;

    [SerializeField]
    private SubPlot[] models;

    private bool isOccupied = false;

    private int highlightCounter = 0;

    [SerializeField]
    private UnityEvent PlantEvent;
    [SerializeField]
    private UnityEvent HarvestEvent;
    [SerializeField]
    private UnityEvent EatEvent;
    

    private void Awake()
    {
        for (int i = 0; i < models.Length; i++)
        {
            models[i].Initialize();
        }
        OnStateChange();    
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangeState();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (Action(false) != ActionType.None)
            {
                ChangeState();
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (Action(true) != ActionType.None)
            {
                ChangeState();
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Highlight(true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Highlight(false);
        }
    }

    public ActionType Action(bool _isCritter)
    {
        if (_isCritter)
        {
            if (currentState == State.Plant)
            {
                return ActionType.Harvest;
            }
            else if (currentState == State.Crop)
            {
                return ActionType.Eat;
            }
        }
        else
        {
            if (currentState == State.Empty)
            {
                return ActionType.Dig;
            }
            else if (currentState == State.Hole)
            {
                return ActionType.Plant;
            }
        }

        return ActionType.None;
    }

    public void ChangeState()
    {
        currentState++;
        if (currentState > State.Crop)
        {
            currentState = State.Empty;
            EatEvent.Invoke();
        }
        OnStateChange();
    }

    public void Highlight(bool isOn)
    {
        if (isOn)
        {
            highlightCounter++;

        }
        else
        {
            highlightCounter--;
            if (highlightCounter <= 0)
            {
                highlightCounter = 0;
            }

        }

        UpdateHighlight();
    }

    private void UpdateHighlight()
    {
        models[(int)currentState].SetHighlight(highlightCounter > 0);
    }

    public void OnStateChange()
    {
        for (int i = 0; i < models.Length; i++)
        {
            models[i].gameObject.SetActive(false);
        }

        models[(int)currentState].gameObject.SetActive(true);

        UpdateHighlight();

        if (currentState == State.Plant)
        {
            PlantEvent.Invoke();
        }
        else if (currentState == State.Crop)
        {
            HarvestEvent.Invoke();
        }
    }
}
