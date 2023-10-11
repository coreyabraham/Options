using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public Button button;
    public GameObject frame;
    public bool hidePreviousFrame;

    void Start()
    {
        if (button == null)
        {
            Debug.LogWarning("Variable: 'Button' with name: 'button' in Object: " + name + " needs to be specified!");
            return;
        }

        if (frame == null)
        {
            Debug.LogWarning("Variable: 'GameObject' with name: 'frame' in Object: " + name + " needs to be specified!");
            return;
        }

        FrameManager fm = GetComponentInParent<FrameManager>();
        Debug.Log(fm);
        button.onClick.AddListener(delegate { fm.OnClick(frame, hidePreviousFrame ); });
    }
}
