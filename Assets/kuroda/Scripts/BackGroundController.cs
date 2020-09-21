using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundController : MonoBehaviour
{
    float rotation_speed = 1;

    private void FixedUpdate()
    {
        transform.Rotate(0, 0, this.rotation_speed);
    }
}
