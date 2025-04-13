using System.Collections;
using com.IvanMurzak.Unity.MCP.Editor.API;
using com.IvanMurzak.Unity.MCP.Editor.Utils;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace com.IvanMurzak.Unity.MCP.Editor.Tests
{
    public partial class TestToolGameObject
    {
        [UnitySetUp]
        public IEnumerator SetUp()
        {
            Debug.Log($"[{nameof(TestToolGameObject)}] SetUp");
            yield return null;
        }
        [UnityTearDown]
        public IEnumerator TearDown()
        {
            Debug.Log($"[{nameof(TestToolGameObject)}] TearDown");
            yield return null;
        }

        [UnityTest]
        public IEnumerator FindByPath()
        {
            var parentName = "root";
            var childName = "nestedGo";
            var child = new GameObject(parentName).AddChild(childName);

            var path = new Tool_GameObject().FindByName(childName);

            Assert.IsNotNull(path, $"{childName} should not be null");
            Assert.IsTrue(path.Contains(childName), $"{childName} should be found in the path");
            Assert.IsFalse(path.ToLower().Contains("error"), $"{childName} should not contain 'error' in the path");

            yield return null;
        }
    }
}