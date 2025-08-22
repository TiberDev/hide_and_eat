using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

// Thêm property Instance vào Menu nếu muốn dễ gọi ở nơi khác (Singleton).
public class Menu : MonoBehaviour
{
    public static Menu Instance { get; private set; }

    public enum ScreenType
    {
        GamePlay,
        JoinRoom,
        Loading,
        Escape
    }

    [Header("UI Elements")] [SerializeField]
    private TMP_InputField nicknameInputField;

    [SerializeField] private TMP_InputField codeInputField;
    [SerializeField] private TMP_Text roomCodeText;
    [SerializeField] private TextMeshProUGUI loadingText;
    [SerializeField] private Button joinButton;
    [SerializeField] private Button createButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button leaveButton;
    [SerializeField] private CanvasGroup joinRoomScreenCGr;
    [SerializeField] private CanvasGroup loadingScreenCGr;
    [SerializeField] private CanvasGroup escapeScreenGr;
    private bool isLoadingTextRunning = false;
    private CancellationTokenSource loadingCts;

    private const string nicknameKey = "nickname";

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
    }

    private void Start()
    {
        SetScreen(ScreenType.JoinRoom);
        // Limit the length of the code input field
        codeInputField.characterLimit = 6; // Assuming a 6-character room code
        codeInputField.contentType = TMP_InputField.ContentType.Alphanumeric; // Set to alphanumeric for room codes

        nicknameInputField.characterLimit = 20; // Set a reasonable limit for nicknames

        // Load saved nickname if it exists
        nicknameInputField.text =
            PlayerPrefs.HasKey(nicknameKey) ? PlayerPrefs.GetString(nicknameKey) : "Player"; // Default nickname

        // Set up button listeners
        joinButton.onClick.AddListener(OnJoinButtonClicked);
        createButton.onClick.AddListener(OnCreateButtonClicked);
        continueButton.onClick.AddListener(OnContinueButtonClicked);
        leaveButton.onClick.AddListener(OnLeaveButtonClicked);
    }

    public ScreenType CurrentUIType { get; private set; }

    private void OnCreateButtonClicked()
    {
        var code = codeInputField.text;
        if (string.IsNullOrWhiteSpace(code))
        {
            code = GenerateCodeRandom();
        }

        roomCodeText.text = $"Room code: <color=green>{code}</color>";
        var nickname = nicknameInputField.text;
        PlayerPrefs.SetString(nicknameKey, nickname);
        StartupNetworkController.Instance.NickName = nickname;
        StartupNetworkController.Instance.RoomName = code;
        StartupNetworkController.Instance.StartConnecting(GameMode.Host);
    }

    private void OnJoinButtonClicked()
    {
        var code = codeInputField.text;
        if (string.IsNullOrWhiteSpace(code))
        {
            code = GenerateCodeRandom();
        }

        roomCodeText.text = $"Room code: <color=green>{code}</color>";
        var nickname = nicknameInputField.text;
        PlayerPrefs.SetString(nicknameKey, nickname);
        StartupNetworkController.Instance.NickName = nickname;
        StartupNetworkController.Instance.RoomName = code;
        StartupNetworkController.Instance.StartConnecting(GameMode.Client);
    }

    private void OnContinueButtonClicked()
    {
        Cursor.lockState = CursorLockMode.Locked;
        CameraFollow.Instance.SetTargetView(true);
        SetScreen(ScreenType.GamePlay);
    }

    private void OnLeaveButtonClicked()
    {
        StartupNetworkController.Instance.Shutdown();
        SetScreen(ScreenType.JoinRoom);
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

    public void SetScreen(ScreenType screen)
    {
        CurrentUIType = screen;
        joinRoomScreenCGr.alpha = screen == ScreenType.JoinRoom ? 1 : 0;
        loadingScreenCGr.alpha = screen == ScreenType.Loading ? 1 : 0;
        escapeScreenGr.alpha = screen == ScreenType.Escape ? 1 : 0;
        joinRoomScreenCGr.blocksRaycasts = screen == ScreenType.JoinRoom;
        loadingScreenCGr.blocksRaycasts = screen == ScreenType.Loading;
        escapeScreenGr.blocksRaycasts = screen == ScreenType.Escape;

        if (screen == ScreenType.Loading)
            StartLoadingText();
        else
            StopLoadingText();
    }

    private void StartLoadingText()
    {
        if (isLoadingTextRunning) return;
        isLoadingTextRunning = true;
        loadingCts = new CancellationTokenSource();
        AnimateLoadingTextAsync(loadingCts.Token).Forget();
    }

    private void StopLoadingText()
    {
        if (!isLoadingTextRunning) return;
        isLoadingTextRunning = false;
        loadingCts?.Cancel();
        if (loadingText != null)
            loadingText.text = "Loading...";
    }

    private async UniTaskVoid AnimateLoadingTextAsync(CancellationToken token)
    {
        string baseText = "Loading";
        int dotCount = 0;

        while (!token.IsCancellationRequested)
        {
            dotCount = (dotCount + 1) % 4; // 0,1,2,3 then back to 0
            loadingText.text = baseText + new string('.', dotCount);
            await UniTask.Delay(500, cancellationToken: token);
        }
    }
}