using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private float mouseSensitivity = 2.0f;
    [SerializeField] private float minPitch = -30f;
    [SerializeField] private float maxPitch = 60f;

    private float yaw = 0f;
    private float pitch = 20f;

    
    private Transform target;
    private Transform cachedTransform;
    public Transform Transform => cachedTransform;
    
    public static CameraFollow Instance { private set; get; }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        
        cachedTransform = transform;
    }
    
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private void Update()
    {
        if (target == null)
            return;
        
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        RotateCamera(mouseDelta);
    }

    private void LateUpdate()
    {
        if (target != null)
        {
            // var targetPosition = target.position + offset;
            // cachedTransform.position = targetPosition;
            // cachedTransform.LookAt(target);
            
            Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
            Vector3 desiredPosition = target.position + rotation * offset;
            cachedTransform.position = desiredPosition;
            cachedTransform.LookAt(target.position);

        }
    }
    
    public void RotateCamera(Vector2 mouseDelta)
    {
        yaw += mouseDelta.x * mouseSensitivity;
        pitch -= mouseDelta.y * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
    }
}
