using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineGrabber : MonoBehaviour
{
    private static string VINE_TAG = "Vine";

    [SerializeField] private float releaseVineExtraForce = 50f;

    private GameObject currentTouchingVine = null;
    private bool isHoldingVine = false;
    private bool justReleasedVine = false;

    private HingeJoint2D hinge = null;
    private Rigidbody2D playerRigidbody;

    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
    }

    public void OnJumpAction()
    {
        if (currentTouchingVine == null)
        {
            return;
        }

        if (isHoldingVine)
        {
            isHoldingVine = false;
            if (hinge != null)
            {
                Destroy(hinge);
                hinge = null;
                currentTouchingVine = null;
                justReleasedVine = true;
            }
        }
        else
        {
            isHoldingVine = true;
            hinge = playerRigidbody.gameObject.AddComponent<HingeJoint2D>();
            hinge.connectedBody = currentTouchingVine.GetComponent<Rigidbody2D>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var target = collision.gameObject;
        if (target.tag != VINE_TAG || isHoldingVine)
        {
            return;
        }

        if (currentTouchingVine == null)
        {
            currentTouchingVine = target;
        }
        else if (Vector2.Distance(transform.position, target.transform.position) < Vector2.Distance(transform.position, currentTouchingVine.transform.position))
        {
            currentTouchingVine = target;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        var target = collision.gameObject;
        if (target.tag != VINE_TAG || isHoldingVine)
        {
            return;
        }

        if (currentTouchingVine == null)
        {
            currentTouchingVine = target;
        }
        else if (currentTouchingVine != target)
        {
            if (Vector2.Distance(transform.position, target.transform.position) < Vector2.Distance(transform.position, currentTouchingVine.transform.position))
            {
                currentTouchingVine = target;
            }
        } 
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var target = collision.gameObject;
        if (target.tag != VINE_TAG || isHoldingVine)
        {
            return;
        }

        if (target == currentTouchingVine)
        {
            currentTouchingVine = null;
        }
    }

    private void FixedUpdate()
    {
        if (justReleasedVine)
        {
            justReleasedVine = false;

            var boost = Vector2.up * releaseVineExtraForce;
            playerRigidbody.AddForce(boost, ForceMode2D.Impulse);
        }
    }
}
