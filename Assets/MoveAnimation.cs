using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAnimation : MonoBehaviour
{
    public GameObject pos;

    // Start is called before the first frame update
    void Update()
    {
        gameObject.transform.Translate(Vector3.forward * Time.deltaTime, Space.World);
        gameObject.transform.LookAt(pos.transform);
    }
}
