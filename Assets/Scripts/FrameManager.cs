using UnityEngine;

public class FrameManager : MonoBehaviour
{
    public GameObject mainFrame;

    [HideInInspector] public GameObject currentFrame;
    [HideInInspector] public GameObject previousFrame;

    private void Start()
    {
        if (mainFrame == null)
        {
            Debug.LogWarning("MainFrame is equal to null, please fix.");
            return;
        }

        if (previousFrame == null)
            previousFrame = mainFrame;

        Debug.Log("Initalization Success!");
    }

    public void OnClick(GameObject frame, bool hidePreviousFrame)
    {
        if (frame == mainFrame && currentFrame != null)
        {
            mainFrame.SetActive(true);

            currentFrame.SetActive(false);
            currentFrame = null;
            
            return;
        }

        if (currentFrame != null)
            return;

        frame.SetActive(true);

        if (currentFrame != null)
            previousFrame = currentFrame;
        
        currentFrame = frame;

        if (previousFrame.activeSelf && hidePreviousFrame)
            previousFrame.SetActive(false);
    }
}
