using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EditorTestWindow : EditorWindow
{
    public string thisName;
    private void OnGUI()
    {
        if (GUILayout.Button("��ť", GUILayout.Width(500)))
        {
            Debug.Log("�������������" + thisName);
        }
    }
}
