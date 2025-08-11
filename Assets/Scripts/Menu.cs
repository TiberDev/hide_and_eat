using System.Collections;
using System.Collections.Generic;
using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TMP_InputField nicknameInputField;
    [SerializeField] private TMP_InputField codeInputField;
    [SerializeField] private Button joinButton;
    [SerializeField] private Button createButton;
    
    private void Start()
    {
        // Limit the length of the code input field
        codeInputField.characterLimit = 6; // Assuming a 6-character room code
        codeInputField.contentType = TMP_InputField.ContentType.Alphanumeric; // Set to alphanumeric for room codes
        
        nicknameInputField.characterLimit = 20; // Set a reasonable limit for nicknames
        
        // Load saved nickname if it exists
        nicknameInputField.text = PlayerPrefs.HasKey("nickname") ? PlayerPrefs.GetString("nickname") : "Player"; // Default nickname

        // Set up button listeners
        joinButton.onClick.AddListener(OnJoinButtonClicked);
        createButton.onClick.AddListener(OnCreateButtonClicked);
    }

    private void OnCreateButtonClicked()
    {
        var code = nicknameInputField.text;
        if(string.IsNullOrWhiteSpace(code))
        {
            code = GenerateCodeRandom();
        }

        StartupNetworkController.Instance.RoomName = code;
        StartupNetworkController.Instance.Connect(GameMode.Host);
    }

    private void OnJoinButtonClicked()
    {
        throw new System.NotImplementedException();
    }
    
    private string GenerateCodeRandom(int length = 4)
    {
        char[] chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789".ToCharArray();

        string str = "";
        for (int i = 0; i < length; i++)
        {
            str += chars[Random.Range(0, chars.Length)];
        }
        return str;
    }
}
