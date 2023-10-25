using UnityEngine;
using UnityEngine.Events;

public class PromptManager : MonoBehaviour
{
    [Header("Assets")]
    public ActionButtonManager acceptBtn;
    public ActionButtonManager denyBtn;

    [HideInInspector] public GameObject frame;
    private UnityEvent<bool, string> passthroughEvent;
    private string currentEventName;

    #region Public Functions

    public void HookButtons(string eventName)
    {
        currentEventName = eventName;

        acceptBtn.button.onClick.AddListener(() => { HandleInteraction(true, eventName); });
        denyBtn.button.onClick.AddListener(() => { HandleInteraction(false, eventName); });
    }

    public void UnhookButtons()
    {
        acceptBtn.button.onClick.RemoveListener(() => { HandleInteraction(true, currentEventName); });
        denyBtn.button.onClick.RemoveListener(() => { HandleInteraction(false, currentEventName); });

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

    public void HandleInteraction(bool toggle, string eventName)
    {
        passthroughEvent.Invoke(toggle, eventName);
    }
    
    #endregion

    #region Private Functions

    private void Start()
    {
        frame = gameObject;
    }

    #endregion
}
