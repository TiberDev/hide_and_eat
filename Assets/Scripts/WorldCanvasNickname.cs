using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// Shows the player names in world view.
/// </summary>
public class WorldCanvasNickname : MonoBehaviour
{
    /// <summary>
    /// Make this always point towards the main camera and follow the target if there is one.
    /// If there is no target to follow, we destroy this object to clean up.
    /// </summary>
    ///
    [SerializeField] TMP_Text worldNicknameText;

    [SerializeField] private Color playerCtrlColor;
    [SerializeField] private Color otherClientsColor;

    public Vector3 offset;

    private Transform target;

    bool destroying = false;

    private void LateUpdate()
    {
        // If the player was despawned or no longer exist, 
        if (target)
        {
            transform.position = target.position + offset;
            transform.rotation = Camera.main.transform.rotation;
        }
        else if (!destroying)
        {
            destroying = true;
            StartCoroutine(WaitAndDestroy());
        }
    }

    private IEnumerator WaitAndDestroy()
    {
        yield return new WaitForSeconds(3);
        if (target != null && !target.Equals(null))
        {
            //continue following the target
            yield return null;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void SetNickname(string nickname, bool inputAuthority)
    {
        if (worldNicknameText != null)
        {
            worldNicknameText.text = nickname;
        }
        else
        {
            Debug.LogError("WorldNicknameText is not assigned.");
        }

        worldNicknameText.color = inputAuthority ? playerCtrlColor : otherClientsColor;
    }
}