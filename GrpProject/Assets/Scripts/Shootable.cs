using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Shootable : MonoBehaviour
{
    [SerializeField] private int health = 10;
    //[SerializeField] private Transform resetPoint;
    private int hitCount = 0;
    private Vector3 originalPosition;

    private Quaternion originalRotation;
    private Color originalColor;
    private Renderer objectRenderer;
    private void Start()
    {
        originalPosition = transform.position;

        originalRotation = transform.rotation;
        objectRenderer = GetComponent<Renderer>();

        // Check if the object has a Renderer component
        if (objectRenderer != null)
        {
            originalColor = objectRenderer.material.color;
        }
        else
        {
            Debug.LogWarning("No Renderer found on " + gameObject.name);
        }
    }

    public void TakeDamage(int damage)
    { 
        health -= damage;
        hitCount++;

        if (health <= 0 || hitCount >= 10)
        {
            ResetPosition();
        }
    }

    private void ResetPosition()
    {
        transform.position = originalPosition;
        transform.rotation = originalRotation;
        health = 10;
        hitCount = 0;

        if (objectRenderer != null)
        {
            objectRenderer.material.color = originalColor;
        }
    }
}