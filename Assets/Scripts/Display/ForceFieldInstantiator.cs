using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ForceFieldInstantiator : MonoBehaviour
{
    private enum InvincibleForceFieldType
    {
        Top,
        Right,
        Bottom,
        Left
    }

    [SerializeField] private GameObject invincibleForceFieldSample;

    [SerializeField] private Vector3 leftInvincibleForceFieldPosition   = new Vector3(0, 0.5f, 1f);
    [SerializeField] private Vector3 rightInvincibleForceFieldPosition  = new Vector3(1f, 0.5f, 1f);
    [SerializeField] private Vector3 topInvincibleForceFieldPosition    = new Vector3(0.5f, 1, 1f);
    [SerializeField] private Vector3 bottomInvincibleForceFieldPosition = new Vector3(0.5f, 0, 1f);


    private GameObject leftInvincibleForceField;
    private Renderer leftinvincibleForceFieldRenderer;

    private GameObject rightInvincibleForceField;
    private Renderer rightInvincibleForceFieldRenderer;

    private GameObject topInvincibleForceField;
    private Renderer topInvincibleForceFieldRenderer;
    
    private GameObject bottomInvincibleForceField;
    private Renderer bottomInvincibleForceFieldRenderer;

    private Camera mainCamera;

    private Dictionary<InvincibleForceFieldType, Vector3> invincibleForceFieldPositions;

    void Start()
    {
        leftInvincibleForceField    = Instantiate(invincibleForceFieldSample);
        rightInvincibleForceField   = Instantiate(invincibleForceFieldSample);
        topInvincibleForceField     = Instantiate(invincibleForceFieldSample);
        bottomInvincibleForceField  = Instantiate(invincibleForceFieldSample);

        leftinvincibleForceFieldRenderer    = leftInvincibleForceField.GetComponent<Renderer>();
        rightInvincibleForceFieldRenderer   = rightInvincibleForceField.GetComponent<Renderer>();
        topInvincibleForceFieldRenderer     = topInvincibleForceField.GetComponent<Renderer>();
        bottomInvincibleForceFieldRenderer  = bottomInvincibleForceField.GetComponent<Renderer>();

        mainCamera = Camera.main;

        calculateInvincibleForceFieldPostions();
    }

    // Update is called once per frame
    void Update()
    {
        calculateInvincibleForceFieldPostions();
    }

    private void calculateInvincibleForceFieldPostions()
    {
        /** 
         * Remove this part pre production 
         * -------
        **/
        invincibleForceFieldPositions = new Dictionary<InvincibleForceFieldType, Vector3>
        {
            [InvincibleForceFieldType.Top] = topInvincibleForceFieldPosition,
            [InvincibleForceFieldType.Right] = rightInvincibleForceFieldPosition,
            [InvincibleForceFieldType.Bottom] = bottomInvincibleForceFieldPosition,
            [InvincibleForceFieldType.Left] = leftInvincibleForceFieldPosition
        };
        /** 
         * -------
         * Remove this part pre production 
        **/

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
        }

        if (type == InvincibleForceFieldType.Top || type == InvincibleForceFieldType.Bottom)
        {
            if (newPosition.y >= 0)
            {
                newPosition.y -= halfHeight;
            }
            else
            {
                newPosition.y += halfHeight;
            }
        }


        invincibleForceField.transform.position = newPosition;
        invincibleForceField.SetActive(true);
    }
}
