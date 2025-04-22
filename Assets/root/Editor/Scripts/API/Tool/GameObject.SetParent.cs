#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System.ComponentModel;
using System.Text;
using com.IvanMurzak.Unity.MCP.Common;
using com.IvanMurzak.Unity.MCP.Editor.Utils;
using com.IvanMurzak.Unity.MCP.Utils;
using UnityEditor.SceneManagement;

namespace com.IvanMurzak.Unity.MCP.Editor.API
{
    public partial class Tool_GameObject
    {
        [McpPluginTool
        (
            "GameObject_SetParent",
            Title = "Set parent GameObject in opened scene",
            Description = @"Set GameObjects in opened scene by 'instanceId' (int) array."
        )]
        public string SetParent
        (
            [Description("The 'instanceId' array of the target GameObjects.")]
            int[] targetInstanceIds,
            [Description("The 'instanceId' of the parent GameObject.")]
            int parentInstanceId,
            [Description("The 'instanceId' of the parent GameObject.")]
            bool worldPositionStays = true
        )
        {
            return MainThread.Run(() =>
            {
                var stringBuilder = new StringBuilder();
                int changedCount = 0;

                for (var i = 0; i < targetInstanceIds.Length; i++)
                {
                    var targetInstanceId = targetInstanceIds[i];

                    var targetGo = GameObjectUtils.FindByInstanceId(targetInstanceId);
                    if (targetGo == null)
                    {
                        stringBuilder.AppendLine($"[Error] Target GameObject with instanceId {targetInstanceId} not found.");
                        continue;
                    }

                    var parentGo = GameObjectUtils.FindByInstanceId(parentInstanceId);
                    if (parentGo == null)
                    {
                        stringBuilder.AppendLine($"[Error] Parent GameObject with instanceId {parentInstanceId} not found.");
                        continue;
                    }

                    targetGo.transform.SetParent(parentGo.transform, worldPositionStays: worldPositionStays);
                    changedCount++;

                    stringBuilder.AppendLine(@$"[Success] Set parent of GameObject with instanceId {targetInstanceId} to GameObject with instanceId {parentInstanceId}.");
                }

                if (changedCount > 0)
                    EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());

                return stringBuilder.ToString();
            });
        }
    }
}