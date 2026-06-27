using System;
using System.Threading.Tasks;
using UnityEngine;
using AIHelp;
// IMGUI 的 EventType 和 AIHelp SDK 的 EventType 同名, 用 alias 区分
using EventType = UnityEngine.EventType;

public class AIHelpDemo : MonoBehaviour
{
    // ============ 在这里填入你自己的 AIHelp 配置 ============
    private string domain = "release.aihelp.net";
    private string androidAppId = "TryElva_platform_79453658-02b7-42fb-9384-8e8712539777";
    private string iosAppId     = "TryElva_platform_09ebf7fa-8d45-4843-bd59-cfda3d8a8dc0";

    private const string ENTRANCE_CUSTOMER_SERVICE = "test004";
    private const string ENTRANCE_HELP_CENTER      = "test001";
    private const string ENTRANCE_CUSTOM           = "THIS IS YOUR ENTRANCE ID";
    private const string FAQ_ID                    = "THIS IS YOUR FAQ ID";

    // 状态
    private string _notifyMessage = "Ready.";
    private Vector2 _scrollPos = Vector2.zero;
    private float _scale = 1f;
    private float _contentHeight = 0f;

    // 触摸 / 拖动 / 按下态
    private bool _isDragging = false;
    private Vector2 _touchStart;
    private float _scrollStartY;
    private const float DRAG_THRESHOLD = 10f; // 超过这个像素距离才算 drag, 防止误触

    private bool _hasPressed = false;
    private Rect _pressedRect = new Rect();
    private bool _pressWasDrag = false;

    // Toast (瞬时通知)
    private class Toast
    {
        public string text;
        public float appearAt;
        public float expireAt;
    }
    private Toast _toast;
    private const float TOAST_DURATION = 2.5f;
    private const float TOAST_FADE     = 0.25f;

    // 布局常量
    private const float MAX_BUTTON_WIDTH_DP = 540f; // 按钮封顶宽度, 防止横屏占满整行
    private const float BASE_BTN_H   = 60f;
    private const float BASE_BTN_SP  = 8f;
    private const float BASE_SEC_H   = 40f;
    private const float BASE_SEC_SP  = 14f;
    private const float BASE_TITLE_H = 64f;
    private const float BASE_STATUS_H = 96f;
    private const float BASE_PAD      = 18f;

    // 缓存样式 / 贴图
    private GUIStyle _titleStyle, _sectionStyle, _buttonStyle, _statusStyle, _toastStyle;
    private Texture2D _bgTex, _sectionBgTex, _statusBgTex, _buttonTex, _buttonHoverTex, _buttonPressedTex, _toastBgTex;
    private bool _stylesInited;

    // 文字基准大小 (运行时按 _scale 缩放)
    private int _baseTitleSize   = 30;
    private int _baseSectionSize = 20;
    private int _baseButtonSize  = 22;
    private int _baseStatusSize  = 16;
    private int _baseToastSize   = 22;

    private void Awake()
    {
        // 强制竖屏 (运行时, 即时生效; 配合 PlayerSettings → Default Orientation = Portrait 一起)
        Screen.orientation = ScreenOrientation.Portrait;
        Screen.autorotateToPortrait = true;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.autorotateToLandscapeLeft = false;
        Screen.autorotateToLandscapeRight = false;

        // Android 默认 targetFrameRate=-1 → 30 FPS, 滚动明显顿; 显式提到 60
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
            AIHelpSupport.Initialize(domain, appId);
            RegisterAIHelpEventListener();
            SetNotify("AIHelp SDK initialized");
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

        Event e = Event.current;
        UpdateDragState(e);

        DrawBackground();
        DrawTitle();
        DrawScrollableArea();
        DrawStatusBar();
        DrawToast();
    }

    // ---------- 触摸 / 滚动 ----------

    private void UpdateDragState(Event e)
    {
        switch (e.type)
        {
            case EventType.MouseDown:
                _touchStart   = e.mousePosition;
                _scrollStartY = _scrollPos.y;
                _isDragging   = false;
                _pressWasDrag = false;
                _hasPressed   = false; // 由 DrawButton 内部根据 hit-test 决定是否真的按下
                break;

            case EventType.MouseDrag:
                if (Vector2.Distance(e.mousePosition, _touchStart) > DRAG_THRESHOLD)
                {
                    _isDragging   = true;
                    _pressWasDrag = true;
                    _hasPressed   = false; // 拖动取消按钮按下态
                }
                if (_isDragging)
                {
                    _scrollPos.y = _scrollStartY - (e.mousePosition.y - _touchStart.y);
                }
                break;

            case EventType.MouseUp:
                // 留着 _isDragging / _pressWasDrag 给 DrawButton 读取, 绘制完再清
                break;
        }
    }

    // ---------- 绘制 ----------

    private void DrawBackground()
    {
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), _bgTex);
    }

    private void DrawTitle()
    {
        var titleRect = new Rect(0, BASE_PAD * _scale, Screen.width, BASE_TITLE_H * _scale);
        GUI.Label(titleRect, "AIHelp Unity Demo", _titleStyle);
    }

    private void DrawScrollableArea()
    {
        float pad         = BASE_PAD     * _scale;
        float titleH      = BASE_TITLE_H * _scale;
        float statusH     = BASE_STATUS_H * _scale;
        float topY        = pad + titleH + 12f * _scale;
        float bottomY     = Screen.height - statusH;

        // 按钮宽度封顶 + 居中
        float maxW   = MAX_BUTTON_WIDTH_DP * _scale;
        float btnW   = Mathf.Min(Screen.width - pad * 2, maxW);
        float btnLeft = (Screen.width - btnW) / 2f;
        float areaH  = bottomY - topY;

        // ScrollView, 显式传 GUIStyle.none 给两个滚动条, 完全禁用视觉上的滚动条;
// 触摸拖拽还是能用 (scrollRect/viewRect 内部的 hit-test 不受影响)
        var viewRect   = new Rect(0, 0, btnW, _contentHeight);
        var scrollRect = new Rect(btnLeft, topY, btnW, areaH);
        _scrollPos = GUI.BeginScrollView(scrollRect, _scrollPos, viewRect,
                                         false, false, GUIStyle.none, GUIStyle.none);

        float y = 0;

        y = DrawSection(y, btnW, "入口  /  Entrances");
        y = DrawButton(y, btnW, "客服 (Show CustomerService)",  SafeShowCustomerService);
        y = DrawButton(y, btnW, "帮助中心 (Show HelpCenter)",   SafeShowHelpCenter);
        y = DrawButton(y, btnW, "自定义入口 (Show Custom)",     SafeShowCustomEntrance);
        y = DrawButton(y, btnW, "单条 FAQ (ShowSingleFAQ)",     SafeShowSingleFAQ);
        y = DrawButton(y, btnW, "打开 URL (ShowUrl)",           SafeShowUrl);

        y = DrawSection(y, btnW, "用户  /  User");
        y = DrawButton(y, btnW, "Login",                        SafeDoLogin);
        y = DrawButton(y, btnW, "Logout (ResetUserInfo)",      SafeLogout);
        y = DrawButton(y, btnW, "UpdateUserInfo (sample)",     SafeUpdateUserInfo);

        y = DrawSection(y, btnW, "调试  /  Debug");
        y = DrawButton(y, btnW, "GetSDKVersion",                SafeGetSDKVersion);
        y = DrawButton(y, btnW, "IsAIHelpShowing + FetchUnread", SafeFetchUnread);
        y = DrawButton(y, btnW, "UpdateSDKLanguage (en)",      SafeUpdateLangEn);

        _contentHeight = y;
        GUI.EndScrollView();

        // 鼠标抬起时, 一次性清掉按下 / 拖动态
        if (Event.current.type == EventType.MouseUp)
        {
            _isDragging   = false;
            _pressWasDrag = false;
            _hasPressed   = false;
        }
    }

    private float DrawSection(float y, float w, string title)
    {
        float h  = BASE_SEC_H  * _scale;
        float sp = BASE_SEC_SP * _scale;
        var rect = new Rect(0, y, w, h);
        GUI.DrawTexture(rect, _sectionBgTex);
        GUI.Label(new Rect(20f * _scale, y, w - 40f * _scale, h), title, _sectionStyle);
        return y + h + sp;
    }

    private float DrawButton(float y, float w, string label, Action onClick)
    {
        float h  = BASE_BTN_H  * _scale;
        float sp = BASE_BTN_SP * _scale;
        var rect = new Rect(0, y, w, h);

        Event e = Event.current;
        bool isHot    = rect.Contains(e.mousePosition);
        bool isPress  = _hasPressed && _pressedRect == rect;
        bool showDown = isPress && !_pressWasDrag;

        // 背景
        var bg = showDown ? _buttonPressedTex : (isHot ? _buttonHoverTex : _buttonTex);
        GUI.DrawTexture(rect, bg);

        // 文字颜色按状态切: 按下时白字 (深橙底), 其他深暖色 (浅桃底)
        var prevColor = GUI.color;
        GUI.color = showDown ? Color.white : new Color(0.161f, 0.145f, 0.141f, 1f);
        GUI.Label(new Rect(rect.x, rect.y, rect.width, rect.height), label, _buttonStyle);
        GUI.color = prevColor;

        // 命中检测
        if (e.type == EventType.MouseDown && isHot)
        {
            _hasPressed  = true;
            _pressedRect = rect;
        }
        else if (e.type == EventType.MouseUp && isHot && _hasPressed && !_pressWasDrag)
        {
            // 真点击: 在按钮上, 没有拖过
            SafeRun(label, onClick);
        }

        return y + h + sp;
    }

    private void DrawStatusBar()
    {
        float pad      = BASE_PAD      * _scale;
        float statusH  = BASE_STATUS_H * _scale;
        float y        = Screen.height - statusH;
        var statusRect = new Rect(0, y, Screen.width, statusH);
        GUI.DrawTexture(statusRect, _statusBgTex);
        GUI.Label(new Rect(pad, y + 8f * _scale, Screen.width - pad * 2, statusH - 16f * _scale),
                  "Notify: " + _notifyMessage, _statusStyle);
    }

    private void DrawToast()
    {
        if (_toast == null) return;
        float now = Time.realtimeSinceStartup;
        if (now >= _toast.expireAt) { _toast = null; return; }

        // 和 status bar 占同一块区域: 全宽, 贴在屏幕底部
        // toast 浮在上面, 淡出后 status bar 的内容自然露出来
        float pad     = BASE_PAD      * _scale;
        float statusH = BASE_STATUS_H * _scale;
        float y       = Screen.height - statusH;
        var bgRect   = new Rect(0, y, Screen.width, statusH);
        var textRect = new Rect(pad, y, Screen.width - pad * 2, statusH);

        // 淡入 / 淡出
        float fadeIn  = Mathf.Clamp01((now - _toast.appearAt) / TOAST_FADE);
        float fadeOut = Mathf.Clamp01((_toast.expireAt - now) / TOAST_FADE);
        float a       = Mathf.Min(fadeIn, fadeOut);

        var prev = GUI.color;
        GUI.color = new Color(1f, 1f, 1f, a);
        GUI.DrawTexture(bgRect, _toastBgTex);
        GUI.Label(textRect, _toast.text, _toastStyle);
        GUI.color = prev;
    }

    // ---------- 样式 / 资源 ----------

    private void EnsureStyles()
    {
        if (_stylesInited) return;

        // Warm Cream 配色 (同色系递进, 避免霓虹感):
        //   bg #FAF6F0 cream / card #FFFCF7 / button #FFF1E6 peach /
        //   hover #FFE0CC / pressed & toast #A0522D sienna (再降饱和)
        //   text #292524 / dim #78716C
        _bgTex           = MakeTex(new Color(0.980f, 0.965f, 0.941f, 1f));         // #FAF6F0
        _sectionBgTex    = MakeTex(new Color(1.000f, 0.988f, 0.969f, 1f));         // #FFFCF7
        _statusBgTex     = MakeTex(new Color(1.000f, 0.988f, 0.969f, 0.96f));      // #FFFCF7
        _toastBgTex      = MakeTex(new Color(0.627f, 0.322f, 0.176f, 1f));         // #A0522D
        _buttonTex       = MakeRoundedTex(new Color(1.000f, 0.945f, 0.902f, 1f));  // #FFF1E6
        _buttonHoverTex  = MakeRoundedTex(new Color(1.000f, 0.878f, 0.800f, 1f));  // #FFE0CC
        _buttonPressedTex = MakeRoundedTex(new Color(0.627f, 0.322f, 0.176f, 1f)); // #A0522D

        _titleStyle = new GUIStyle(GUI.skin.label)
        {
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter,
        };
        _titleStyle.normal.textColor = new Color(0.161f, 0.145f, 0.141f, 1f);    // #292524 warm dark

        _sectionStyle = new GUIStyle(GUI.skin.label)
        {
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleLeft,
        };
        _sectionStyle.normal.textColor = new Color(0.471f, 0.443f, 0.424f, 1f);  // #78716C warm gray

        _buttonStyle = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter,
            wordWrap  = false,
            clipping = TextClipping.Clip,
        };
        // 基础色 white, 实际显示色由 DrawButton 用 GUI.color 按状态切换:
        //   默认 / hover: 深暖色 (浅桃底)  /  按下: 白色 (深橙底)
        _buttonStyle.normal.textColor = Color.white;

        _statusStyle = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.UpperLeft,
            wordWrap  = true,
        };
        _statusStyle.normal.textColor = new Color(0.471f, 0.443f, 0.424f, 1f);   // #78716C warm gray

        _toastStyle = new GUIStyle(GUI.skin.label)
        {
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter,
            wordWrap  = true,
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
        // 4x4 圆角近似 (用羽化边), 拉伸后看不出锯齿
        int s = 8;
        var t = new Texture2D(s, s, TextureFormat.RGBA32, true);
        for (int y = 0; y < s; y++)
        for (int x = 0; x < s; x++)
        {
            float fx = (x + 0.5f) / s * 2f - 1f;
            float fy = (y + 0.5f) / s * 2f - 1f;
            float r  = Mathf.Sqrt(fx * fx + fy * fy);
            // 1 在圆内, 0 在圆外; 角上做 2 像素羽化
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
        float s   = dpi / 160f;
        if (s < 1.2f) s = 1.2f;
        if (s > 2.4f) s = 2.4f;
        _scale = s;
    }

    private void ApplyScaleToStyles()
    {
        // 每帧把 _scale 应用到所有 style 的 fontSize
        _titleStyle.fontSize   = Mathf.RoundToInt(_baseTitleSize   * _scale);
        _sectionStyle.fontSize = Mathf.RoundToInt(_baseSectionSize * _scale);
        _buttonStyle.fontSize  = Mathf.RoundToInt(_baseButtonSize  * _scale);
        _statusStyle.fontSize  = Mathf.RoundToInt(_baseStatusSize  * _scale);
        _toastStyle.fontSize   = Mathf.RoundToInt(_baseToastSize   * _scale);
    }

    // ---------- Safe 包装 ----------

    private void SafeRun(string label, Action a)
    {
        try { a(); }
        catch (Exception e) { Debug.LogError($"[AIHelpDemo] {label} failed: {e}"); SetNotify($"{label} failed: {e.Message}"); }
    }

    private void SafeShowCustomerService() => ShowCustomerService();
    private void SafeShowHelpCenter()      => ShowHelpCenter();
    private void SafeShowCustomEntrance()  => ShowCustomEntrance();
    private void SafeShowSingleFAQ()       => ShowSingleFAQ();
    private void SafeDoLogin()             => DoLogin();
    private void SafeLogout()              => AIHelpSupport.ResetUserInfo();
    private void SafeUpdateLangEn()        => AIHelpSupport.UpdateSDKLanguage("en");
    private void SafeUpdateUserInfo()
    {
        var cfg = new UserConfig.Builder()
            .SetUserName("AIHelp")
            .SetUserTags("VIP1,beta")
            .SetCustomData("{\"level\":42}")
            .Build();
        AIHelpSupport.UpdateUserInfo(cfg);
        SetNotify("UpdateUserInfo called");
    }
    private void SafeGetSDKVersion()       => SetNotify("SDK version: " + AIHelpSupport.GetSDKVersion());
    private void SafeFetchUnread()
    {
        AIHelpSupport.IsAIHelpShowing();
        AIHelpSupport.FetchUnreadMessageCount();
        AIHelpSupport.FetchUnreadTaskCount();
        SetNotify("IsAIHelpShowing + FetchUnread called");
    }
    private void SafeShowUrl()             => AIHelpSupport.ShowUrl("https://www.aihelp.net");

    // ---------- SDK 业务逻辑 ----------

    private void ShowCustomerService() => AIHelpSupport.Show(ENTRANCE_CUSTOMER_SERVICE);
    private void ShowHelpCenter()      => AIHelpSupport.Show(ENTRANCE_HELP_CENTER);
    private void ShowCustomEntrance()  => AIHelpSupport.Show(ENTRANCE_CUSTOM);
    private void ShowSingleFAQ()       => AIHelpSupport.ShowSingleFAQ(FAQ_ID, ConversationMoment.AFTER_MARKING_UNHELPFUL);

    private void DoLogin()
    {
        var cfg = new LoginConfig.Builder()
            .SetUserId(GetRandomNumber())
            .SetUserConfig(new UserConfig.Builder()
                .SetUserName("AIHelp")
                .SetUserTags("VIP1")
                .SetCustomData("{}")
                .Build())
            .Build();
        AIHelpSupport.Login(cfg);
        SetNotify("Login called");
    }

    private void RegisterAIHelpEventListener()
    {
        AIHelpSupport.RegisterAsyncEventListener(AIHelp.EventType.Initialization, (json, ack) => SetNotify("Initialization " + json));
        AIHelpSupport.RegisterAsyncEventListener(AIHelp.EventType.UserLogin,     (json, ack) => SetNotify("UserLogin " + json));
        AIHelpSupport.RegisterAsyncEventListener(AIHelp.EventType.EnterpriseAuth, async (json, ack) =>
        {
            SetNotify("EnterpriseAuth " + json);
            await Task.Delay(2000);
            ack("{\"token\":\"this is your async token\"}");
        });
        AIHelpSupport.RegisterAsyncEventListener(AIHelp.EventType.SessionOpen,    (json, ack) => SetNotify("SessionOpen " + json));
        AIHelpSupport.RegisterAsyncEventListener(AIHelp.EventType.SessionClose,   (json, ack) =>
        {
            SetNotify("SessionClose " + json);
            AIHelpSupport.UnregisterAsyncEventListener(AIHelp.EventType.SessionOpen);
            AIHelpSupport.UnregisterAsyncEventListener(AIHelp.EventType.SessionClose);
        });
        AIHelpSupport.RegisterAsyncEventListener(AIHelp.EventType.MessageArrival,  (json, ack) => SetNotify("MessageArrival " + json));
        AIHelpSupport.RegisterAsyncEventListener(AIHelp.EventType.LogUpload,       (json, ack) =>
        {
            SetNotify("LogUpload " + json);
            ack("{\"content\":\"this is your synchronous log\"}");
        });
        AIHelpSupport.RegisterAsyncEventListener(AIHelp.EventType.UrlClick,        (json, ack) => SetNotify("UrlClick " + json));
        AIHelpSupport.RegisterAsyncEventListener(AIHelp.EventType.UnreadTaskCount, (json, ack) => SetNotify("UnreadTaskCount " + json));
        AIHelpSupport.RegisterAsyncEventListener(AIHelp.EventType.ConversationStart, (json, ack) => SetNotify("ConversationStart " + json));
    }

    private void SetNotify(string msg)
    {
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            _notifyMessage = msg;
            _toast = new Toast
            {
                text     = msg,
                appearAt = Time.realtimeSinceStartup,
                expireAt = Time.realtimeSinceStartup + TOAST_DURATION,
            };
        });
        Debug.Log("[AIHelpDemo] " + msg);
    }

    private string GetRandomNumber() => new System.Random().NextDouble() + "";
}
