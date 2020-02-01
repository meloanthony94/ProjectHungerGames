using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubPlot : MonoBehaviour
{
    [SerializeField]
    private Material defaultMaterial;

    [SerializeField]
    private Material highlightMaterial;

    private Renderer _renderer;

    public void Initialize()
    {
        _renderer = GetComponent<Renderer>();
    }

    public void SetHighlight(bool isOn)
    {
        if(isOn)
        {
            _renderer.material = highlightMaterial;
        }
        else
        {
            _renderer.material = defaultMaterial;
        }
    }
}
