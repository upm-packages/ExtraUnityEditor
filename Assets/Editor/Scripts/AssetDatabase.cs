using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using Object = UnityEngine.Object;

namespace ExtraUnityEditor
{
    [PublicAPI]
    // ReSharper disable once PartialTypeWithSinglePart
    public static partial class AssetDatabase
    {
        public static T FindAndLoadAsset<T>(string[] searchInFolders = default) where T : Object
        {
            return FindAndLoadAsset<T>(_ => true, searchInFolders);
        }

        public static T FindAndLoadAsset<T>(Func<string, bool> pathFilter, string[] searchInFolders = default) where T : Object
        {
            return FindAndLoadAssets<T>(pathFilter, searchInFolders)?.FirstOrDefault();
        }

        public static IEnumerable<T> FindAndLoadAssets<T>(string[] searchInFolders = default) where T : Object
        {
            return FindAndLoadAssets<T>(_ => true, searchInFolders);
        }

        public static IEnumerable<T> FindAndLoadAssets<T>(Func<string, bool> pathFilter, string[] searchInFolders = default) where T : Object
        {
            var guidList = UnityEditor.AssetDatabase.FindAssets($"t:{typeof(T).Name}", searchInFolders);
            return guidList.Length == 0
                ? new T[0]
                : guidList
                    .Select(UnityEditor.AssetDatabase.GUIDToAssetPath)
                    .Where(pathFilter)
                    .Select(UnityEditor.AssetDatabase.LoadAssetAtPath<T>);
        }

        public static void CreateFolderRecursive(string path)
        {
            var parentFolder = string.Empty;
            foreach (var item in path.Split('/', '\\'))
            {
                var folder = Path.Combine(parentFolder, item);
                if (!UnityEditor.AssetDatabase.IsValidFolder(folder))
                {
                    UnityEditor.AssetDatabase.CreateFolder(parentFolder, item);
                }

                parentFolder = folder;
            }
        }
    }
}
