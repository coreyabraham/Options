/*
 * TO DO:
 * - Updated Hooking Method to allow User to control *when* the event runs
 * - Add optional arguments into the event and to the target function when supported
 */

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ActionButtonManager : MonoBehaviour
{
    [Header("Generic")]
    public Button button;
    public UnityEvent unityEvent;

    [Header("Debugging")]
    public bool silenceOutput;

    private void PingEvent()
    {
        unityEvent.Invoke();
    }

    private void Start()
    {
        if (button == null && !silenceOutput)
        {
            Debug.LogWarning("Button Component not found, please fix!");
            return;
        }

        if (unityEvent == null && !silenceOutput)
        {
            Debug.LogWarning("UnityEvent Component not found, please fix!");
            return;
        }

        button.onClick.AddListener(PingEvent);
    }
}