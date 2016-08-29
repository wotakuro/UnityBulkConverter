/**
 * MIT License

Copyright (c) 2016 Yusuke Kurokawa

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections;
using System.Collections.Generic;

namespace BulkConvertBatch
{
    /// <summary>
    /// Bulk Convert Utility for prefabs and GameObjects in scene.
    /// </summary>
    public class BulkConvertUtility
    {
        /// <summary>
        /// Delegate for execute
        /// </summary>
        /// <param name="gmo">Gameobject for execute</param>
        /// <returns>If you change gameobject return true to save scene or prefab.</returns>
        public delegate bool Execute(GameObject gmo);

        /// <summary>
        /// Delegate for execute
        /// </summary>
        /// <param name="gmo">Gameobject for execute</param>
        /// <param name="prefabPath">prefabPath</param>
        /// <returns>If you change gameobject return true to save scene or prefab.</returns>
        public delegate bool ExecutePrefab(GameObject gmo, string prefabPath);


        /// <summary>
        /// execute batch to all root GameObject in all scenes.
        /// </summary>
        /// <param name="execFunc">execute Function</param>
        public static void DoForAllRootGameObjectInAllScene(Execute execFunc)
        {
            var guids = AssetDatabase.FindAssets("t:Scene");
            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                EditorSceneManager.OpenScene(path);
                DoForAllRootGameObjectInCurrentScene(execFunc);
            }
        }

        /// <summary>
        /// execute batch to all root GameObject in current scene.
        /// </summary>
        /// <param name="execFunc">execute Function</param>
        public static void DoForAllRootGameObjectInCurrentScene(Execute execFunc)
        {
            var gmoList = GetAllRootObjectsInScene();
            foreach (var gmo in gmoList)
            {
                bool flag = execFunc(gmo);
                if (flag)
                {
                    EditorUtility.SetDirty(gmo);
                }
            }
            EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
        }

        
        /// <summary>
        /// execute batch to all prefabs.
        /// </summary>
        /// <param name="title">Dialog title</param>
        /// <param name="execFunc">execute Function</param>
        public static void DoForAllPrefab(string title, Execute execFunc)
        {
            DoForAllPrefab(title, (gmo, path) => { return execFunc(gmo); });
        }

        /// <summary>
        /// execute batch to all prefabs.
        /// </summary>
        /// <param name="title">Dialog title</param>
        /// <param name="execFunc">execute Function</param>
        public static void DoForAllPrefab(string title, ExecutePrefab execFunc)
        {
            try
            {
                var guids = AssetDatabase.FindAssets("t:GameObject");
                int idx = 0;
                foreach (var guid in guids)
                {

                    string path = AssetDatabase.GUIDToAssetPath(guid);
                    var obj = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                    bool isChange = execFunc(obj,path);
                    if (isChange)
                    {
                        EditorUtility.SetDirty(obj);
                    }
                    ++idx;
                    EditorUtility.DisplayProgressBar(title, path, idx / (float)guids.Length);
                }
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
        }


        /// <summary>
        /// Get all  root GameObjects in current scene
        /// </summary>
        /// <returns>all root GameObjects</returns>
        private static List<GameObject> GetAllRootObjectsInScene()
        {
            List<GameObject> list = new List<GameObject>();

            GameObject[] objs = UnityEngine.Resources.FindObjectsOfTypeAll<GameObject>();
            for (int i = 0; i < objs.Length; i++)
            {
                string path = AssetDatabase.GetAssetOrScenePath(objs[i]);
                if (path.EndsWith(".unity") && objs[i].transform.parent == null)
                {
                    list.Add(objs[i]);
                }
            }
            return list;
        }
    }

}