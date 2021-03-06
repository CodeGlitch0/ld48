using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineGrabber : MonoBehaviour
{
    private static string VINE_TAG = "Vine";

    [SerializeField] private float releaseVineExtraForce = 50f;
    [SerializeField] private float verticalClimbSpeed = 1f;

    private VineSegment currentTouchingVineSegment = null;
    private bool isHoldingVine = false;
    private bool justReleasedVine = false;

    private HingeJoint2D hinge = null;
    private Rigidbody2D playerRigidbody;

    private Vector2 rawInputVerticalMovement = Vector2.zero;

    public void OnMovement(Vector2 value)
    {
        rawInputVerticalMovement = new Vector2(0, value.y);
    }

    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
    }

    public void OnJumpAction()
    {
        if (currentTouchingVineSegment == null)
        {
            return;
        }

        if (isHoldingVine)
        {
            DropFromVine();
        }
        else
        {
            isHoldingVine = true;
            hinge = playerRigidbody.gameObject.AddComponent<HingeJoint2D>();
            hinge.connectedBody = currentTouchingVineSegment.GetComponent<Rigidbody2D>();
            //hinge.autoConfigureConnectedAnchor = false;

            currentTouchingVineSegment.Vine.OnSwung();
        }
    }

    private void DropFromVine()
    {
        isHoldingVine = false;
        if (hinge != null)
        {
            Destroy(hinge);
            hinge = null;
            currentTouchingVineSegment = null;
            justReleasedVine = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var target = collision.gameObject;
        if (!target.CompareTag(VINE_TAG) || isHoldingVine)
        {
            return;
        }

        if (currentTouchingVineSegment == null)
        {
            currentTouchingVineSegment = target.GetComponent<VineSegment>();
        }
        else if (Vector2.Distance(transform.position, target.transform.position) < Vector2.Distance(transform.position, currentTouchingVineSegment.transform.position))
        {
            currentTouchingVineSegment = target.GetComponent<VineSegment>();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        var target = collision.gameObject;
        if (!target.CompareTag(VINE_TAG) || isHoldingVine)
        {
            return;
        }

        var targetVineSegment = target.GetComponent<VineSegment>();

        if (currentTouchingVineSegment == null)
        {
            currentTouchingVineSegment = targetVineSegment;
        }
        else if (currentTouchingVineSegment != targetVineSegment)
        {
            if (Vector2.Distance(transform.position, target.transform.position) < Vector2.Distance(transform.position, currentTouchingVineSegment.transform.position))
            {
                currentTouchingVineSegment = targetVineSegment;
            }
        } 
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var target = collision.gameObject;
        if (!target.CompareTag(VINE_TAG) || isHoldingVine)
        {
            return;
        }

        var targetVineSegment = target.GetComponent<VineSegment>();
        if (targetVineSegment == currentTouchingVineSegment)
        {
            currentTouchingVineSegment = null;
        }
    }

    private void Update()
    {
        if (isHoldingVine && hinge != null && rawInputVerticalMovement.sqrMagnitude > Mathf.Epsilon)
        {
            var connectedVineUp = hinge.connectedBody.transform.up;
            playerRigidbody.transform.Translate(connectedVineUp * rawInputVerticalMovement.y * verticalClimbSpeed * Time.deltaTime);

            var playerRelativeLocation = Vector3.Project(hinge.connectedBody.transform.position - playerRigidbody.transform.position, connectedVineUp);
            if (playerRelativeLocation.sqrMagnitude > 0.25)
            {
                if (hinge.connectedAnchor.y > 0.5)
                {
                    hinge.connectedBody = currentTouchingVineSegment.VineAbove.GetComponent<Rigidbody2D>();
                    currentTouchingVineSegment = currentTouchingVineSegment.VineAbove;
                }
                else if (hinge.connectedAnchor.y < -0.5)
                {
                    if (currentTouchingVineSegment.VineBelow != null)
                    {
                        hinge.connectedBody = currentTouchingVineSegment.VineBelow.GetComponent<Rigidbody2D>();
                        currentTouchingVineSegment = currentTouchingVineSegment.VineBelow;
                    }
                    else
                    {
                        DropFromVine();
                    }
                }
            }
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
