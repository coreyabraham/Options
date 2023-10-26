using UnityEngine;
using UnityEngine.Events;

using TMPro;

public class PromptManager : MonoBehaviour
{
    [Header("Text")]
    public TMP_Text title;
    public TMP_Text body;

    [Header("Buttons")]
    public ActionButtonManager acceptBtn;
    public ActionButtonManager denyBtn;

    // Private Definitions
    [HideInInspector] public GameObject frame;
    private UnityEvent<bool, string> passthroughEvent;
    private string currentEventName;

    private string defaultTitle;
    private string defaultBody;

    #region Public Functions

    public void HookButtons(string eventName)
    {
        currentEventName = eventName;

        acceptBtn.button.onClick.AddListener(() => { HandleInteraction(true, eventName); });
        denyBtn.button.onClick.AddListener(() => { HandleInteraction(false, eventName); });
    }

    public void UnhookButtons()
    {
        acceptBtn.button.onClick.RemoveAllListeners();
        denyBtn.button.onClick.RemoveAllListeners();

        currentEventName = string.Empty;
    }

    public void SetEvent(UnityEvent<bool, string> unityEvent)
    {
        passthroughEvent = unityEvent;
    }

    public void SetFrameVisibility(bool toggle)
    {
        frame.SetActive(toggle);
    }

    #region Easy Setup

    public void StartPrompt(string eventName, UnityEvent<bool, string> unityEvent)
    {
        HookButtons(eventName);
        SetEvent(unityEvent);
        SetFrameVisibility(true);
    }

    public void StartPrompt(string title, string body, string eventName, UnityEvent<bool, string> unityEvent)
    {
        SetTitle(title);
        SetBody(body);

        HookButtons(eventName);
        SetEvent(unityEvent);

        SetFrameVisibility(true);
    }

    #endregion

    #endregion

    #region Private Functions

    private void SetTitle(string title)
    {
        this.title.text = title;
    }

    private void SetBody(string body)
    {
        this.body.text = body;
    }

    private void RevertTitle()
    {
        title.text = defaultTitle;
    }

    private void RevertBody()
    {
        body.text = defaultBody;
    }

    private void HandleInteraction(bool toggle, string eventName)
    {
        passthroughEvent.Invoke(toggle, eventName);

        RevertTitle();
        RevertBody();
        SetFrameVisibility(false);
        
        UnhookButtons();
    }

    private void Start()
    {
        defaultTitle = title.text;
        defaultBody = body.text;
        frame = gameObject;
    }

    #endregion
}
