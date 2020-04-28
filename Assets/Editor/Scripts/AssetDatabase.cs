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

        public static void CreateOrOverwriteAsset(Object asset, string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (!File.Exists(path))
            {
                UnityEditor.AssetDatabase.CreateAsset(asset, path);
                return;
            }

            var tmpFilePath = UnityEditor.FileUtil.GetUniqueTempPathInProject();
            UnityEditor.AssetDatabase.CreateAsset(asset, tmpFilePath);
            UnityEditor.FileUtil.ReplaceFile(tmpFilePath, path);
            UnityEditor.AssetDatabase.ImportAsset(path);
        }

        public static bool CopyOrOverwriteAsset(string path, string newPath)
        {
            if (newPath == null)
            {
                throw new ArgumentNullException(nameof(newPath));
            }

            if (!File.Exists(newPath))
            {
                return UnityEditor.AssetDatabase.CopyAsset(path, newPath);
            }

            var tmpFilePath = UnityEditor.FileUtil.GetUniqueTempPathInProject();
            if (!UnityEditor.AssetDatabase.CopyAsset(path, tmpFilePath))
            {
                return false;
            }

            UnityEditor.FileUtil.ReplaceFile(tmpFilePath, newPath);
            UnityEditor.AssetDatabase.ImportAsset(newPath);

            return true;
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
