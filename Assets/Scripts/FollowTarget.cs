using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] private Transform target;

    [SerializeField] private bool lockX = false;
    [SerializeField] private bool lockY = false;
    [SerializeField] private bool lockZ = true;

    void LateUpdate()
    {
        var currentPosition = transform.position;
        var targetPosition = target.position;

        transform.position = new Vector3(
            lockX ? currentPosition.x : targetPosition.x,
            lockY ? currentPosition.y : targetPosition.y,
            lockZ ? currentPosition.z : targetPosition.z
        );
    }
}
