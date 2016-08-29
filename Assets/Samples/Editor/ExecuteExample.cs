using UnityEngine;
using UnityEditor;
using System.Collections;

/// <summary>
/// Execute all Recttransfrom sample
/// </summary>
public class ExecuteExample : EditorWindow
{
    /// <summary>
    /// position
    /// </summary>
    private Vector2 scrollPosition;

    /// <summary>
    /// Create Window
    /// </summary>
    [MenuItem("Tools/BulkConvertSample")]
    public static void CreateWindow()
    {
        EditorWindow.GetWindow<ExecuteExample>();
    }


    void OnGUI()
    {
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        GUILayout.Label( "BulkConvertBatch Sample" );

        GUILayout.Label("RectTransform");
        // smaller
        if (GUILayout.Button("Make rectTransform in all scene smaller "))
        {
            BulkConvertBatch.BulkConvertUtility.DoAllComponentsInAllScene<RectTransform>(RectToSmall);
        }
        if (GUILayout.Button("Make rectTransform in all prefab smaller"))
        {
            BulkConvertBatch.BulkConvertUtility.DoAllComponentsInPrefab<RectTransform>( RectToSmall,"RectTransform Smaller");
        }

        // bigger
        if (GUILayout.Button("Make rectTransform in all scene bigger "))
        {
            BulkConvertBatch.BulkConvertUtility.DoAllComponentsInAllScene<RectTransform>(RectToBig);
        }
        if (GUILayout.Button("Make rectTransform in all prefab bigger"))
        {
            BulkConvertBatch.BulkConvertUtility.DoAllComponentsInPrefab<RectTransform>(RectToBig, "RectTransform Bigger");
        }
        GUILayout.Label("");
        GUILayout.Label("Add or Remove Collider");
        if (GUILayout.Button("Add Collider"))
        {
            BulkConvertBatch.BulkConvertUtility.DoAllPrefab(AddPrefabToBoxCollider, "AddCollider");
        }
        if (GUILayout.Button("Remove Collider"))
        {
            BulkConvertBatch.BulkConvertUtility.DoAllPrefab(RemovePrefabToBoxCollider, "Remove Collider");
        }
        // 
        GUILayout.Label("");
        GUILayout.Label("Font Position");
        if (GUILayout.Button("UIFontPositionUp"))
        {
            BulkConvertBatch.BulkConvertUtility.DoAllComponentsInAllScene<UnityEngine.UI.Text>(UIFontPositionUp);
            BulkConvertBatch.BulkConvertUtility.DoAllComponentsInPrefab<UnityEngine.UI.Text>(UIFontPositionUp);
        }
        if (GUILayout.Button("UIFontPositionDown"))
        {
            BulkConvertBatch.BulkConvertUtility.DoAllComponentsInAllScene<UnityEngine.UI.Text>(UIFontPositionDown);
            BulkConvertBatch.BulkConvertUtility.DoAllComponentsInPrefab<UnityEngine.UI.Text>(UIFontPositionDown);
        }
        EditorGUILayout.EndScrollView();
    }


    /// <summary>
    /// execute Rectransform making smaller
    /// </summary>
    /// <param name="rectTransform"> target RectTransform</param>
    /// <returns>true if changed</returns>
    private static bool RectToSmall(RectTransform rectTransform)
    {
        if (rectTransform == null) { return false; }
        rectTransform.localScale = rectTransform.localScale * 0.5f;

        return true;
    }
    /// <summary>
    /// execute Rectransform making bigger
    /// </summary>
    /// <param name="rectTransform"> target RectTransform</param>
    /// <returns>true if changed</returns>
    private static bool RectToBig(RectTransform rectTransform)
    {
        if (rectTransform == null) { return false; }

        rectTransform.localScale = rectTransform.localScale * 2.0f;
        return true;
    }


    /// <summary>
    /// Add BoxCollider to prefab
    /// </summary>
    /// <param name="gmo">prefab GameObject</param>
    /// <param name="prefabPath">prefab path</param>
    /// <returns> true if Changed</returns>
    private static bool AddPrefabToBoxCollider(GameObject gmo,string prefabPath)
    {
        if (gmo.GetComponent<BoxCollider>() != null)
        {
            return false;
        }
        gmo.AddComponent<BoxCollider>();
        return true;
    }
    /// <summary>
    /// Remove BoxCollider from prefab.
    /// </summary>
    /// <param name="gmo">prefab GameObject</param>
    /// <param name="prefabPath">prefab path</param>
    /// <returns>true if Changed</returns>
    private static bool RemovePrefabToBoxCollider( GameObject gmo , string prefatPath)
    {
        var boxCollider = gmo.GetComponent<BoxCollider>();
        if (boxCollider == null)
        {
            return false;
        }
        Object.DestroyImmediate(boxCollider, true);
        return true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="textComponent"></param>
    /// <returns></returns>
    private static bool UIFontPositionUp(UnityEngine.UI.Text textComponent,GameObject gmo)
    {
        textComponent.rectTransform.localPosition += Vector3.up * 5.0f;
        return true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="textComponent"></param>
    /// <returns></returns>
    private static bool UIFontPositionDown(UnityEngine.UI.Text textComponent, GameObject gmo)
    {
        textComponent.rectTransform.localPosition -= Vector3.up * 5.0f;
        return true;
    }
}
