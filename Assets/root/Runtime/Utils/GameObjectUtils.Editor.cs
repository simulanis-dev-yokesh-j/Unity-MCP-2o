#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.IvanMurzak.Unity.MCP.Common;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace com.IvanMurzak.Unity.MCP.Utils
{
    public static partial class GameObjectUtils
    {
        public static GameObject[] FindRootGameObjects(Scene? scene = null)
        {
            if (scene == null)
            {
                var rootGos = UnityEditor.SceneManagement.EditorSceneManager
                    .GetActiveScene()
                    .GetRootGameObjects();

                return rootGos;
            }
            else
            {
                return scene.Value.GetRootGameObjects();
            }
        }
        public static GameObject FindByInstanceID(int instanceID)
        {
            if (instanceID == 0)
                return null;

            var obj = UnityEditor.EditorUtility.InstanceIDToObject(instanceID);
            if (obj is not GameObject go)
                return null;

            return go;
        }
    }
}
#endif