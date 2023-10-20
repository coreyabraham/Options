using UnityEngine;
using TMPro;

public class FrameManager : MonoBehaviour
{
    [Header("Main")]
    public GameObject mainFrame;
    public TMP_Text closeLabel;

    [Header("Debug")]
    public bool silenceDebug;
    public KeyCode debugClose;
    
    [HideInInspector] public GameObject currentFrame;
    [HideInInspector] public GameObject previousFrame;

    private void Start()
    {
        if (closeLabel != null)
        {
            closeLabel.text = "To Quit, Press the '" + debugClose.ToString() + "' Key!";
        }

        if (mainFrame == null)
        {
            if (!silenceDebug)
                Debug.LogWarning("MainFrame is equal to null, please fix.");

            return;
        }

        if (currentFrame == null)
            currentFrame = mainFrame;

        if (previousFrame == null)
            previousFrame = mainFrame;

        if (!silenceDebug)
            Debug.Log("Initalization Success!");
    }

    private void Update()
    {
        if (!Input.GetKeyDown(debugClose))
            return;

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif

        Application.Quit();
    }

    public void OnClick(GameObject frame, bool hidePreviousFrame)
    {
        frame.SetActive(true);

        if (hidePreviousFrame && previousFrame != null)
            previousFrame.SetActive(false);

        currentFrame = frame;
        previousFrame = currentFrame;
    }
}
