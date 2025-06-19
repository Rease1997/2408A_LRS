using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EditorTestWindow : EditorWindow
{
    public string thisName;
    private void OnGUI()
    {
        if (GUILayout.Button("按钮", GUILayout.Width(500)))
        {
            Debug.Log("你输入的名字是" + thisName);
        }
    }
}
