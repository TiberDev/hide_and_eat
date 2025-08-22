using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInputController : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Menu.Instance.CurrentUIType == Menu.ScreenType.GamePlay)
            {
                Menu.Instance.SetScreen(Menu.ScreenType.Escape);
                Cursor.lockState = CursorLockMode.None;
                CameraFollow.Instance.SetTargetView(false);
            }
            else if (Menu.Instance.CurrentUIType == Menu.ScreenType.Escape)
            {
                Cursor.lockState = CursorLockMode.Locked;
                CameraFollow.Instance.SetTargetView(true);
                Menu.Instance.SetScreen(Menu.ScreenType.GamePlay);
            }
        }
    }
}