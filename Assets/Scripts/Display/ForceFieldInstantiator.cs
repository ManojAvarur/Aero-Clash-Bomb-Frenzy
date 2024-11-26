using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ForceFieldInstantiator : MonoBehaviour
{
    public enum InvincibleForceFieldType
    {
        Top,
        Right,
        Bottom,
        Left,
        Middle
    }

    [SerializeField] private GameObject invincibleForceFieldSample;

    [SerializeField] private Vector3 leftInvincibleForceFieldPosition   = new Vector3(0, 0.5f, 1f);
    [SerializeField] private Vector3 rightInvincibleForceFieldPosition  = new Vector3(1f, 0.5f, 1f);
    [SerializeField] private Vector3 topInvincibleForceFieldPosition    = new Vector3(0.5f, 1, 1f);
    [SerializeField] private Vector3 bottomInvincibleForceFieldPosition = new Vector3(0.5f, 0, 1f);
    [SerializeField] private Vector3 middleInvincibleForceFieldPosition = new Vector3(0.5f, 0.5f, 1f);

    private GameObject leftInvincibleForceField;
    private Renderer leftinvincibleForceFieldRenderer;

    private GameObject rightInvincibleForceField;
    private Renderer rightInvincibleForceFieldRenderer;

    private GameObject topInvincibleForceField;
    private Renderer topInvincibleForceFieldRenderer;
    
    private GameObject bottomInvincibleForceField;
    private Renderer bottomInvincibleForceFieldRenderer;

    private GameObject middleInvincibleForceField;
    private Renderer middleInvincibleForceFieldRenderer;

    private Camera mainCamera;

    private Dictionary<InvincibleForceFieldType, Vector3> invincibleForceFieldPositions;

    private int lastScreenWidth;

    void Start()
    {
        leftInvincibleForceField        = Instantiate(invincibleForceFieldSample);
        leftInvincibleForceField.name   = "Left Invincible Force Field";

        rightInvincibleForceField       = Instantiate(invincibleForceFieldSample);
        rightInvincibleForceField.name  = "Right Invincible Force Field";

        topInvincibleForceField         = Instantiate(invincibleForceFieldSample);
        topInvincibleForceField.name    = "Top Invincible Force Field";

        bottomInvincibleForceField      = Instantiate(invincibleForceFieldSample);
        bottomInvincibleForceField.name = "Bottom Invincible Force Field";

        middleInvincibleForceField      = Instantiate(invincibleForceFieldSample);
        middleInvincibleForceField.name = "Middle Invincible Force Field";

        leftinvincibleForceFieldRenderer    = leftInvincibleForceField.GetComponent<Renderer>();
        rightInvincibleForceFieldRenderer   = rightInvincibleForceField.GetComponent<Renderer>();
        topInvincibleForceFieldRenderer     = topInvincibleForceField.GetComponent<Renderer>();
        bottomInvincibleForceFieldRenderer  = bottomInvincibleForceField.GetComponent<Renderer>();
        middleInvincibleForceFieldRenderer  = middleInvincibleForceField.GetComponent<Renderer>();

        mainCamera = Camera.main;

        calculateInvincibleForceFieldPostions();
    }

    // Update is called once per frame
    void Update()
    {
        calculateInvincibleForceFieldPostions();
    }

    public void calculateInvincibleForceFieldPostions()
    {
        invincibleForceFieldPositions = new Dictionary<InvincibleForceFieldType, Vector3>
        {
            [InvincibleForceFieldType.Top] = topInvincibleForceFieldPosition,
            [InvincibleForceFieldType.Right] = rightInvincibleForceFieldPosition,
            [InvincibleForceFieldType.Bottom] = bottomInvincibleForceFieldPosition,
            [InvincibleForceFieldType.Left] = leftInvincibleForceFieldPosition,
            [InvincibleForceFieldType.Middle] = middleInvincibleForceFieldPosition
        };

        setupInvincibleForceField(
            topInvincibleForceField,
            topInvincibleForceFieldRenderer,
            InvincibleForceFieldType.Top
        );

        setupInvincibleForceField(
            rightInvincibleForceField,
            rightInvincibleForceFieldRenderer,
            InvincibleForceFieldType.Right
        );

        setupInvincibleForceField(
            bottomInvincibleForceField,
            bottomInvincibleForceFieldRenderer,
            InvincibleForceFieldType.Bottom
        );

        setupInvincibleForceField(
            leftInvincibleForceField, 
            leftinvincibleForceFieldRenderer, 
            InvincibleForceFieldType.Left
        );

        setupInvincibleForceField(
            middleInvincibleForceField,
            middleInvincibleForceFieldRenderer,
            InvincibleForceFieldType.Middle
        );

        lastScreenWidth = Screen.width;
    }

    private void setupInvincibleForceField(GameObject invincibleForceField, Renderer invincibleForceFieldRenderer, InvincibleForceFieldType type)
    { 
        // Calculate the left edge of the screen in world coordinates for 2D
        Vector3 edge = mainCamera.ViewportToWorldPoint(invincibleForceFieldPositions[type]);

        // Adjust the x, y position to the left, right & top, bottom edge of screen
        Vector3 newPosition = transform.position;
        newPosition.x = edge.x;
        newPosition.y = edge.y;
        newPosition.z = edge.z;

        float halfWidth = invincibleForceFieldRenderer.bounds.extents.x;
        float halfHeight = invincibleForceFieldRenderer.bounds.extents.y;

        if(type == InvincibleForceFieldType.Left || type == InvincibleForceFieldType.Right)
        {
            if(newPosition.x >= 0)
            {
                newPosition.x -= halfWidth;
            }
            else
            {
                newPosition.x += halfWidth;
            }

            setHeightForLeftAndReightForces(invincibleForceField, invincibleForceFieldRenderer, .01f);
        }

        if (type == InvincibleForceFieldType.Top || type == InvincibleForceFieldType.Bottom || type == InvincibleForceFieldType.Middle)
        {
            if (newPosition.y >= 0)
            {
                newPosition.y -= halfHeight;
            }
            else
            {
                newPosition.y += halfHeight;
            }

            setWidthForTopAndBottomForces(invincibleForceField, invincibleForceFieldRenderer, .01f);
        }

        invincibleForceField.transform.position = newPosition;
        invincibleForceField.SetActive(true);
    }

    public void setHeightForLeftAndReightForces(GameObject invincibleForceField, Renderer invincibleForceFieldRenderer, float width){
        float cameraHeight = mainCamera.orthographicSize * 2f;

        Vector2 spriteSize = invincibleForceFieldRenderer.bounds.size;

        //float heightScale = cameraHeight / spriteSize.y;
        //float widthScale = width / spriteSize.x;
        float heightScale = cameraHeight;
        float widthScale = width;

        invincibleForceField.transform.localScale = new Vector2(widthScale, heightScale);
    }

    public void setWidthForTopAndBottomForces(GameObject invincibleForceField, Renderer invincibleForceFieldRenderer, float height)
    {
        // Get the orthographic size of the camera
        float orthographicSize = mainCamera.orthographicSize;

        // Calculate the aspect ratio (screen width / screen height)
        float aspectRatio = (float) Screen.width / Screen.height;

        // Calculate the camera width
        float cameraWidth = 2 * orthographicSize * aspectRatio;
        float heightScale = height;

        invincibleForceField.transform.localScale = new Vector2(cameraWidth, heightScale);
    }
}
