using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionUI : MonoBehaviour
{
    [SerializeField]
    private Sprite[] actionSprites;

    [SerializeField]
    private Image image;

    public void SetAction(ActionType action)
    {
        image.sprite = actionSprites[(int)action];
    }
}
