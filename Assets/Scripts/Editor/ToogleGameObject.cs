using UnityEditor;
using UnityEngine;

public class ToogleGameObject : Editor
{   
    [MenuItem("Shortcuts/ToogleObject #a")]
    static void DeactivateObject()
    {
        GameObject obj = Selection.activeGameObject;
        if (obj.activeSelf)
        {
            obj.SetActive(false);
        }
        else
        {
            obj.SetActive(true);
        }
    }
}
