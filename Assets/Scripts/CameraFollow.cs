using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private CinemachineFreeLook mainCineFreeLook;
    [SerializeField] private CinemachineVirtualCamera mainCineVirtualCamera;

    private Transform target;
    private Transform cachedTransform;
    public Transform Transform => cachedTransform;

    public static CameraFollow Instance { private set; get; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
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
        if (mainCineFreeLook != null)
        {
            mainCineFreeLook.Follow = target;
            mainCineFreeLook.LookAt = target;
        }

        if (mainCineVirtualCamera != null)
        {
            mainCineVirtualCamera.Follow = target;
            mainCineVirtualCamera.LookAt = target;
        }
    }

    private void Update()
    {
        if (target == null) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            if (mainCineFreeLook != null)
                mainCineFreeLook.enabled = false;
        }
        else if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Cursor.lockState = CursorLockMode.Locked;
            if (mainCineFreeLook != null)
                mainCineFreeLook.enabled = true;
        }
    }
}