using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private CinemachineFreeLook mainCineFreeLook;

    private Transform target;
    private Transform cachedTransform;

    private float originFreeLookMaxSpeedX; // Original max speed for X axis
    private float originFreeLookMaxSpeedY; // Original max speed for Y axis

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

        if (mainCineFreeLook != null)
        {
            originFreeLookMaxSpeedX = mainCineFreeLook.m_XAxis.m_MaxSpeed;
            originFreeLookMaxSpeedY = mainCineFreeLook.m_YAxis.m_MaxSpeed;
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        if (mainCineFreeLook != null)
        {
            mainCineFreeLook.Follow = target;
            mainCineFreeLook.LookAt = target;
        }
    }

    public void SetTargetView(bool canFollow)
    {
        if (mainCineFreeLook == null) return;
        
        // mainCineFreeLook.enabled = canFollow;
        
        if (canFollow)
        {
            mainCineFreeLook.m_XAxis.m_MaxSpeed = originFreeLookMaxSpeedX;
            mainCineFreeLook.m_YAxis.m_MaxSpeed = originFreeLookMaxSpeedY;
        }
        else
        {
            mainCineFreeLook.m_XAxis.m_MaxSpeed = 0;
            mainCineFreeLook.m_YAxis.m_MaxSpeed = 0;
        }
        
    }
}