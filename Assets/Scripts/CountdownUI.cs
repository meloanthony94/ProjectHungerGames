using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownUI : MonoBehaviour
{

    public Sprite[] imageSprites;
    public Image image;

    // Start is called before the first frame update
    void Awake()
    {
        image = GetComponent<Image>();
        image.sprite = imageSprites[3];
    }

    // Update is called once per frame
    public void UpdateValue(float value)
    {
        if (value > 3)
        {
            image.sprite = imageSprites[3];
        }
        else if (value > 2)
        {
            image.sprite = imageSprites[2];
        }
        else if (value > 1)
        {
            image.sprite = imageSprites[1];
        }
        else
        {
            image.sprite = imageSprites[0];
        }
    }
}
