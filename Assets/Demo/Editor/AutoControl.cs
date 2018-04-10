using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System;

public class AutoControl
{
    public static MenuCommand Command { get; private set; }

    [InitializeOnLoadMethod]
    public static void Regist()
    {
        EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyWindow;
    }

    private static void OnHierarchyWindow(int instanceID, Rect selectionRect)
    {
        if (Event.current != null && selectionRect.Contains(Event.current.mousePosition)
           && Event.current.button == 1 && Event.current.type <= EventType.mouseUp)
        {
            Debug.Log(EditorUtility.InstanceIDToObject(instanceID));
            EditorUtility.DisplayPopupMenu(selectionRect, "Assets", Command);
        }
    }
}
