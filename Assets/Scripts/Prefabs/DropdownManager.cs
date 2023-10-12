using System.Collections.Generic;
using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class DropdownManager : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public bool ignoreListables;
    public List<TMP_Dropdown.OptionData> listables = new List<TMP_Dropdown.OptionData>();

    void Start()
    {
        if (ignoreListables)
            return;

        if (dropdown.options.Count > 0)
            dropdown.ClearOptions();

        dropdown.AddOptions(listables);
    }
}
