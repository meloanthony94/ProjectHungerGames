using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 lookAtMe = target.position;
        transform.LookAt(target);
        //transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0);
    }
}
