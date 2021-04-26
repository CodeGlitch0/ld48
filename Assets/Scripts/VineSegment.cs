using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineSegment : MonoBehaviour
{
    public VineSegment VineAbove;
    public VineSegment VineBelow;

    public Vine Vine;

    private void Start()
    {
        Vine = GetComponentInParent<Vine>();
    }
}
