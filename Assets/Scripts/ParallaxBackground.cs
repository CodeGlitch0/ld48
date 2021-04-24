using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField, Range(0, 1)] private float parallaxMultiplier = 0.5f;
    [SerializeField] private bool lockX = false;
    [SerializeField] private bool lockY = false;

    private Transform cameraTransform;
    private Vector3 lastCameraPosition;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;
    }

    void FixedUpdate()
    {
        var deltaMovement = cameraTransform.position - lastCameraPosition;
        var currentPosition = transform.position;

        transform.position = new Vector3(
            lockX ? currentPosition.x : currentPosition.x + deltaMovement.x * parallaxMultiplier,
            lockY ? currentPosition.y : currentPosition.y + deltaMovement.y * parallaxMultiplier,
            currentPosition.z
        );
        lastCameraPosition = cameraTransform.position;
    }
}
