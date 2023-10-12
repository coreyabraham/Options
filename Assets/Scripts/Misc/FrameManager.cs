using System;
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

        if (currentFrame == null)
            currentFrame = mainFrame;

        if (previousFrame == null)
            previousFrame = mainFrame;

        Debug.Log("Initalization Success!");
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
