using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private Vector2 parallaxMultiplier = new Vector2(0.2f, 1);
    [SerializeField] private bool lockX = false;
    [SerializeField] private bool lockY = false;

    private Transform cameraTransform;
    private Vector3 lastCameraPosition;
    private float textureUnitSizeX;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        textureUnitSizeX = texture.width / sprite.pixelsPerUnit;
    }

    void FixedUpdate()
    {
        var deltaMovement = cameraTransform.position - lastCameraPosition;
        var currentPosition = transform.position;

        transform.position = new Vector3(
            lockX ? currentPosition.x : currentPosition.x + deltaMovement.x * parallaxMultiplier.x,
            lockY ? currentPosition.y : currentPosition.y + deltaMovement.y * parallaxMultiplier.y,
            currentPosition.z
        );
        lastCameraPosition = cameraTransform.position;

        if (Mathf.Abs(cameraTransform.position.x - transform.position.x) >= textureUnitSizeX)
        {
            float offsetPositionX = (cameraTransform.position.x - transform.position.x) % textureUnitSizeX;
            transform.position = new Vector3(cameraTransform.position.x + offsetPositionX, transform.position.y);
        }
    }
}
