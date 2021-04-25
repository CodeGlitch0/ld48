using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinTexture : MonoBehaviour
{
    [SerializeField] private float spinAmount = 90f;

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(transform.position, Vector3.forward, spinAmount * Time.deltaTime);
    }
}
