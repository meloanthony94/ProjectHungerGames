using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform followMe;
    public Vector3 offSet;

    // Start is called before the first frame update
    void Start()
    {
        offSet = followMe.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = followMe.position - offSet;
    }
}
