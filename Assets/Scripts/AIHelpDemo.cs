using System;
using UnityEngine;
using AIHelp;
using EventType = UnityEngine.EventType;

/// <summary>
/// Unity 6 demo shell UI (scaled IMGUI + toast) wired to the 3.x / 3.4.2 API
/// (SPM tag 0.0.0). Visual layout matches main; button actions call 3.x bridges.
/// </summary>
public class AIHelpDemo : MonoBehaviour
{
    // ============ 在这里填入你自己的 AIHelp 配置 ============
    private string appKey = "THIS IS YOUR APP KEY";
    private string domain = "release.aihelp.net";
    private string androidAppId = "TryElva_platform_79453658-02b7-42fb-9384-8e8712539777";
    private string iosAppId = "TryElva_platform_79453658-02b7-42fb-9384-8e8712539777";

    private const string FAQ_SECTION_ID = "SECTION ID";
    private const string FAQ_ID = "FAQ ID";

    // 状态
    private string _notifyMessage = "Ready.";
    private Vector2 _scrollPos = Vector2.zero;
    private float _scale = 1f;
    private float _contentHeight = 0f;
    // Screen.safeArea → GUI top-left insets (notch / Dynamic Island / home indicator)
    private float _safeTop, _safeBottom, _safeLeft, _safeRight;

    // 触摸 / 拖动 / 按下态
    private bool _isDragging = false;
    private Vector2 _touchStart;
    private float _scrollStartY;
    private const float DRAG_THRESHOLD = 10f;

    private bool _hasPressed = false;
    private Rect _pressedRect = new Rect();
    private bool _pressWasDrag = false;

    // Toast
    private class Toast
    {
        public string text;
        public float appearAt;
        public float expireAt;
    }
    private Toast _toast;
    private const float TOAST_DURATION = 2.5f;
    private const float TOAST_FADE = 0.25f;

    // 布局常量
    private const float MAX_BUTTON_WIDTH_DP = 540f;
    private const float BASE_BTN_H = 60f;
    private const float BASE_BTN_SP = 8f;
    private const float BASE_SEC_H = 40f;
    private const float BASE_SEC_SP = 14f;
    private const float BASE_TITLE_H = 64f;
    private const float BASE_STATUS_H = 96f;
    private const float BASE_PAD = 18f;

    private GUIStyle _titleStyle, _sectionStyle, _buttonStyle, _statusStyle, _toastStyle;
    private Texture2D _bgTex, _sectionBgTex, _statusBgTex, _buttonTex, _buttonHoverTex, _buttonPressedTex, _toastBgTex;
    private bool _stylesInited;

    private int _baseTitleSize = 30;
    private int _baseSectionSize = 20;
    private int _baseButtonSize = 22;
    private int _baseStatusSize = 16;
    private int _baseToastSize = 22;

    private void Awake()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        Screen.autorotateToPortrait = true;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.autorotateToLandscapeLeft = false;
        Screen.autorotateToLandscapeRight = false;
        Application.targetFrameRate = 60;

        UnityMainThreadDispatcher.Initialize();

        string appId = "";
#if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android) appId = androidAppId;
#endif
#if UNITY_IOS
        if (Application.platform == RuntimePlatform.IPhonePlayer) appId = iosAppId;
#endif
#if UNITY_EDITOR
        if (string.IsNullOrEmpty(appId)) appId = androidAppId;
#endif
        if (string.IsNullOrEmpty(appId))
        {
            Debug.LogError("[AIHelpDemo] appId is empty, please configure it in AIHelpDemo.cs");
        }

        try
        {
            AIHelpSupport.enableLogging(true);
            AIHelpSupport.Init(appKey, domain, appId, "en");
            RegisterAIHelpCallbacks();
            SetNotify("AIHelp SDK init requested");
        }
        catch (Exception e)
        {
            Debug.LogError($"[AIHelpDemo] SDK init failed: {e}");
            SetNotify("SDK init failed: " + e.Message);
        }
    }

    private void OnGUI()
    {
        EnsureStyles();
        ComputeScale();
        ApplyScaleToStyles();
        RefreshSafeInsets();

        Event e = Event.current;
        UpdateDragState(e);

        DrawBackground();
        DrawTitle();
        DrawScrollableArea();
        DrawStatusBar();
        DrawToast();
    }

    private void RefreshSafeInsets()
    {
        // Screen.safeArea is bottom-left origin; OnGUI is top-left.
        Rect sa = Screen.safeArea;
        _safeTop = Mathf.Max(0f, Screen.height - (sa.y + sa.height));
        _safeBottom = Mathf.Max(0f, sa.y);
        _safeLeft = Mathf.Max(0f, sa.x);
        _safeRight = Mathf.Max(0f, Screen.width - (sa.x + sa.width));
    }

    private void UpdateDragState(Event e)
    {
        switch (e.type)
        {
            case EventType.MouseDown:
                _touchStart = e.mousePosition;
                _scrollStartY = _scrollPos.y;
                _isDragging = false;
                _pressWasDrag = false;
                _hasPressed = false;
                break;

            case EventType.MouseDrag:
                if (Vector2.Distance(e.mousePosition, _touchStart) > DRAG_THRESHOLD)
                {
                    _isDragging = true;
                    _pressWasDrag = true;
                    _hasPressed = false;
                }
                if (_isDragging)
                {
                    _scrollPos.y = _scrollStartY - (e.mousePosition.y - _touchStart.y);
                }
                break;

            case EventType.MouseUp:
                break;
        }
    }

    private void DrawBackground()
    {
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), _bgTex);
    }

    private void DrawTitle()
    {
        float pad = BASE_PAD * _scale;
        float titleH = BASE_TITLE_H * _scale;
        var titleRect = new Rect(_safeLeft, _safeTop + pad, Screen.width - _safeLeft - _safeRight, titleH);
        GUI.Label(titleRect, "AIHelp Unity Demo · 3.4.2", _titleStyle);
    }

    private void DrawScrollableArea()
    {
        float pad = BASE_PAD * _scale;
        float titleH = BASE_TITLE_H * _scale;
        float statusH = BASE_STATUS_H * _scale;
        float topY = _safeTop + pad + titleH + 12f * _scale;
        float bottomY = Screen.height - _safeBottom - statusH;

        float usableW = Screen.width - _safeLeft - _safeRight;
        float maxW = MAX_BUTTON_WIDTH_DP * _scale;
        float btnW = Mathf.Min(usableW - pad * 2, maxW);
        float btnLeft = _safeLeft + (usableW - btnW) / 2f;
        float areaH = Mathf.Max(0f, bottomY - topY);

        var viewRect = new Rect(0, 0, btnW, _contentHeight);
        var scrollRect = new Rect(btnLeft, topY, btnW, areaH);
        _scrollPos = GUI.BeginScrollView(scrollRect, _scrollPos, viewRect,
                                         false, false, GUIStyle.none, GUIStyle.none);

        float y = 0;

        y = DrawSection(y, btnW, "入口  /  Entrances");
        y = DrawButton(y, btnW, "机器人客服 (ShowConversation)", SafeShowBot);
        y = DrawButton(y, btnW, "人工客服 (HUMAN_SUPPORT)", SafeShowHuman);
        y = DrawButton(y, btnW, "全部 FAQ (ShowAllFAQSections)", SafeShowAllFAQ);
        y = DrawButton(y, btnW, "FAQ 分区 (ShowFAQSection)", SafeShowFAQSection);
        y = DrawButton(y, btnW, "单条 FAQ (ShowSingleFAQ)", SafeShowSingleFAQ);
        y = DrawButton(y, btnW, "运营 (ShowOperation)", SafeShowOperation);
        y = DrawButton(y, btnW, "打开 URL (ShowUrl)", SafeShowUrl);

        y = DrawSection(y, btnW, "用户  /  User");
        y = DrawButton(y, btnW, "UpdateUserInfo (sample)", SafeUpdateUserInfo);
        y = DrawButton(y, btnW, "ResetUserInfo", SafeResetUserInfo);

        y = DrawSection(y, btnW, "调试  /  Debug");
        y = DrawButton(y, btnW, "GetSDKVersion", SafeGetSDKVersion);
        y = DrawButton(y, btnW, "IsAIHelpShowing", SafeIsShowing);
        y = DrawButton(y, btnW, "StartUnreadMessageCountPolling", SafeUnreadPolling);
        y = DrawButton(y, btnW, "UpdateSDKLanguage (en)", SafeUpdateLangEn);
        y = DrawButton(y, btnW, "Uninstall", SafeUninstall);

        _contentHeight = y;
        GUI.EndScrollView();

        if (Event.current.type == EventType.MouseUp)
        {
            _isDragging = false;
            _pressWasDrag = false;
            _hasPressed = false;
        }
    }

    private float DrawSection(float y, float w, string title)
    {
        float h = BASE_SEC_H * _scale;
        float sp = BASE_SEC_SP * _scale;
        var rect = new Rect(0, y, w, h);
        GUI.DrawTexture(rect, _sectionBgTex);
        GUI.Label(new Rect(20f * _scale, y, w - 40f * _scale, h), title, _sectionStyle);
        return y + h + sp;
    }

    private float DrawButton(float y, float w, string label, Action onClick)
    {
        float h = BASE_BTN_H * _scale;
        float sp = BASE_BTN_SP * _scale;
        var rect = new Rect(0, y, w, h);

        Event e = Event.current;
        bool isHot = rect.Contains(e.mousePosition);
        bool isPress = _hasPressed && _pressedRect == rect;
        bool showDown = isPress && !_pressWasDrag;

        var bg = showDown ? _buttonPressedTex : (isHot ? _buttonHoverTex : _buttonTex);
        GUI.DrawTexture(rect, bg);

        var prevColor = GUI.color;
        GUI.color = showDown ? Color.white : new Color(0.161f, 0.145f, 0.141f, 1f);
        GUI.Label(new Rect(rect.x, rect.y, rect.width, rect.height), label, _buttonStyle);
        GUI.color = prevColor;

        if (e.type == EventType.MouseDown && isHot)
        {
            _hasPressed = true;
            _pressedRect = rect;
        }
        else if (e.type == EventType.MouseUp && isHot && _hasPressed && !_pressWasDrag)
        {
            SafeRun(label, onClick);
        }

        return y + h + sp;
    }

    private void DrawStatusBar()
    {
        float pad = BASE_PAD * _scale;
        float statusH = BASE_STATUS_H * _scale;
        float barH = statusH + _safeBottom;
        float y = Screen.height - barH;
        var statusRect = new Rect(0, y, Screen.width, barH);
        GUI.DrawTexture(statusRect, _statusBgTex);
        GUI.Label(new Rect(_safeLeft + pad, y + 8f * _scale,
                           Screen.width - _safeLeft - _safeRight - pad * 2,
                           statusH - 16f * _scale),
                  "Notify: " + _notifyMessage, _statusStyle);
    }

    private void DrawToast()
    {
        if (_toast == null) return;
        float now = Time.realtimeSinceStartup;
        if (now >= _toast.expireAt) { _toast = null; return; }

        float pad = BASE_PAD * _scale;
        float statusH = BASE_STATUS_H * _scale;
        float barH = statusH + _safeBottom;
        float y = Screen.height - barH;
        var bgRect = new Rect(0, y, Screen.width, barH);
        var textRect = new Rect(_safeLeft + pad, y,
                                Screen.width - _safeLeft - _safeRight - pad * 2, statusH);

        float fadeIn = Mathf.Clamp01((now - _toast.appearAt) / TOAST_FADE);
        float fadeOut = Mathf.Clamp01((_toast.expireAt - now) / TOAST_FADE);
        float a = Mathf.Min(fadeIn, fadeOut);

        var prev = GUI.color;
        GUI.color = new Color(1f, 1f, 1f, a);
        GUI.DrawTexture(bgRect, _toastBgTex);
        GUI.Label(textRect, _toast.text, _toastStyle);
        GUI.color = prev;
    }

    private void EnsureStyles()
    {
        if (_stylesInited) return;

        _bgTex = MakeTex(new Color(0.980f, 0.965f, 0.941f, 1f));
        _sectionBgTex = MakeTex(new Color(1.000f, 0.988f, 0.969f, 1f));
        _statusBgTex = MakeTex(new Color(1.000f, 0.988f, 0.969f, 0.96f));
        _toastBgTex = MakeTex(new Color(0.627f, 0.322f, 0.176f, 1f));
        _buttonTex = MakeRoundedTex(new Color(1.000f, 0.945f, 0.902f, 1f));
        _buttonHoverTex = MakeRoundedTex(new Color(1.000f, 0.878f, 0.800f, 1f));
        _buttonPressedTex = MakeRoundedTex(new Color(0.627f, 0.322f, 0.176f, 1f));

        _titleStyle = new GUIStyle(GUI.skin.label)
        {
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter,
        };
        _titleStyle.normal.textColor = new Color(0.161f, 0.145f, 0.141f, 1f);

        _sectionStyle = new GUIStyle(GUI.skin.label)
        {
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleLeft,
        };
        _sectionStyle.normal.textColor = new Color(0.471f, 0.443f, 0.424f, 1f);

        _buttonStyle = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter,
            wordWrap = false,
            clipping = TextClipping.Clip,
        };
        _buttonStyle.normal.textColor = Color.white;

        _statusStyle = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.UpperLeft,
            wordWrap = true,
        };
        _statusStyle.normal.textColor = new Color(0.471f, 0.443f, 0.424f, 1f);

        _toastStyle = new GUIStyle(GUI.skin.label)
        {
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter,
            wordWrap = true,
        };
        _toastStyle.normal.textColor = Color.white;

        _stylesInited = true;
    }

    private static Texture2D MakeTex(Color c)
    {
        var t = new Texture2D(1, 1, TextureFormat.RGBA32, false);
        t.SetPixel(0, 0, c);
        t.Apply();
        t.hideFlags = HideFlags.HideAndDontSave;
        return t;
    }

    private static Texture2D MakeRoundedTex(Color c)
    {
        int s = 8;
        var t = new Texture2D(s, s, TextureFormat.RGBA32, true);
        for (int y = 0; y < s; y++)
        for (int x = 0; x < s; x++)
        {
            float fx = (x + 0.5f) / s * 2f - 1f;
            float fy = (y + 0.5f) / s * 2f - 1f;
            float r = Mathf.Sqrt(fx * fx + fy * fy);
            float a = Mathf.Clamp01(1.4f - r);
            t.SetPixel(x, y, new Color(c.r, c.g, c.b, c.a * a));
        }
        t.Apply();
        t.hideFlags = HideFlags.HideAndDontSave;
        return t;
    }

    private void ComputeScale()
    {
        float dpi = Screen.dpi > 0 ? Screen.dpi : 160f;
        float s = dpi / 160f;
        if (s < 1.2f) s = 1.2f;
        if (s > 2.4f) s = 2.4f;
        _scale = s;
    }

    private void ApplyScaleToStyles()
    {
        _titleStyle.fontSize = Mathf.RoundToInt(_baseTitleSize * _scale);
        _sectionStyle.fontSize = Mathf.RoundToInt(_baseSectionSize * _scale);
        _buttonStyle.fontSize = Mathf.RoundToInt(_baseButtonSize * _scale);
        _statusStyle.fontSize = Mathf.RoundToInt(_baseStatusSize * _scale);
        _toastStyle.fontSize = Mathf.RoundToInt(_baseToastSize * _scale);
    }

    private void SafeRun(string label, Action a)
    {
        try { a(); }
        catch (Exception e)
        {
            Debug.LogError($"[AIHelpDemo] {label} failed: {e}");
            SetNotify($"{label} failed: {e.Message}");
        }
    }

    private void SafeShowBot()
    {
        var cfg = new ConversationConfig.Builder()
            .SetAlwaysShowHumanSupportButtonInBotPage(true)
            .build();
        AIHelpSupport.ShowConversation(cfg);
        SetNotify("ShowConversation (bot)");
    }

    private void SafeShowHuman()
    {
        var cfg = new ConversationConfig.Builder()
            .SetConversationIntent(ConversationIntent.HUMAN_SUPPORT)
            .build();
        AIHelpSupport.ShowConversation(cfg);
        SetNotify("ShowConversation (human)");
    }

    private void SafeShowAllFAQ()
    {
        AIHelpSupport.ShowAllFAQSections();
        SetNotify("ShowAllFAQSections");
    }

    private void SafeShowFAQSection()
    {
        AIHelpSupport.ShowFAQSection(FAQ_SECTION_ID);
        SetNotify("ShowFAQSection");
    }

    private void SafeShowSingleFAQ()
    {
        AIHelpSupport.ShowSingleFAQ(FAQ_ID);
        SetNotify("ShowSingleFAQ");
    }

    private void SafeShowOperation()
    {
        AIHelpSupport.ShowOperation();
        SetNotify("ShowOperation");
    }

    private void SafeShowUrl() => AIHelpSupport.ShowUrl("https://www.aihelp.net");

    private void SafeUpdateUserInfo()
    {
        var cfg = new UserConfig.Builder()
            .SetUserId(GetRandomNumber())
            .SetUserName("AIHelp")
            .SetUserTags("VIP1,beta")
            .SetCustomData("{\"level\":42}")
            .SetSyncCrmInfo(true)
            .build();
        AIHelpSupport.UpdateUserInfo(cfg);
        SetNotify("UpdateUserInfo called");
    }

    private void SafeResetUserInfo()
    {
        AIHelpSupport.ResetUserInfo();
        SetNotify("ResetUserInfo");
    }

    private void SafeGetSDKVersion() => SetNotify("SDK version: " + AIHelpSupport.GetSDKVersion());

    private void SafeIsShowing() => SetNotify("IsAIHelpShowing: " + AIHelpSupport.IsAIHelpShowing());

    private void SafeUnreadPolling()
    {
        AIHelpSupport.StartUnreadMessageCountPolling(count =>
            SetNotify("Unread messages: " + count));
        SetNotify("StartUnreadMessageCountPolling");
    }

    private void SafeUpdateLangEn()
    {
        AIHelpSupport.UpdateSDKLanguage("en");
        SetNotify("UpdateSDKLanguage en");
    }

    private void SafeUninstall()
    {
        AIHelpSupport.Uninstall();
        SetNotify("Uninstall");
    }

    private void RegisterAIHelpCallbacks()
    {
        AIHelpSupport.SetOnAIHelpInitializedCallback(() => SetNotify("AIHelp init success"));
        AIHelpSupport.SetOnAIHelpSessionOpenCallback(() => SetNotify("Session open"));
        AIHelpSupport.SetOnAIHelpSessionCloseCallback(() => SetNotify("Session close"));
        AIHelpSupport.SetOnSpecificUrlClickedCallback(url => SetNotify("Url click: " + url));
    }

    private void SetNotify(string msg)
    {
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            _notifyMessage = msg;
            _toast = new Toast
            {
                text = msg,
                appearAt = Time.realtimeSinceStartup,
                expireAt = Time.realtimeSinceStartup + TOAST_DURATION,
            };
        });
        Debug.Log("[AIHelpDemo] " + msg);
    }

    private string GetRandomNumber() => new System.Random().NextDouble() + "";
}
