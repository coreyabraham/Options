using UnityEngine;

public class FrameManager : MonoBehaviour
{
    [Header("Main")]
    public GameObject mainFrame;

    [Header("Debug")]
    public bool silenceDebug;
    
    private GameObject currentFrame;
    private GameObject previousFrame;

    public void OnClick(GameObject frame, bool hidePreviousFrame)
    {
        frame.SetActive(true);

        if (hidePreviousFrame && previousFrame != null)
            previousFrame.SetActive(false);

        currentFrame = frame;
        previousFrame = currentFrame;
    }

    private void Start()
    {
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
}
