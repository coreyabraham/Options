using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public Button button;
    public GameObject frame;
    public bool hidePreviousFrame;
    public bool silenceDebug;

    void Start()
    {
        if (button == null)
        {
            if (!silenceDebug)
                Debug.LogWarning("Variable: 'Button' with name: 'button' in Object: " + name + " needs to be specified!");
            
            return;
        }

        if (frame == null)
        {
            if (!silenceDebug)
                Debug.LogWarning("Variable: 'GameObject' with name: 'frame' in Object: " + name + " needs to be specified!");
            
            return;
        }

        FrameManager fm = GetComponentInParent<FrameManager>();

        if (fm == null)
        {
            if (!silenceDebug)
                Debug.LogWarning("Class: 'FrameManager' cannot be found in current scene, please fix!");
            
            return;
        }

        button.onClick.AddListener(delegate { fm.OnClick(frame, hidePreviousFrame ); });
    }
}
