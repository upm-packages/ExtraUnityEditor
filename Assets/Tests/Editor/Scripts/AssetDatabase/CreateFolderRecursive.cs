using System.IO;
using NUnit.Framework;

namespace ExtraUnityEditor.Tests.Editor.AssetDatabase
{
    public class CreateFolderRecursive
    {
        private static (string full, string relative) BasePath { get; } =
            (
                Path.Combine(UnityEngine.Application.dataPath, "Tests", "Temp"),
                Path.Combine("Assets", "Tests", "Temp")
            );

        [SetUp]
        public void SetUp()
        {
            Directory.CreateDirectory(BasePath.full);
            // 作成が反映されないコトがあるので、AssetDatabase を再読込する
            UnityEditor.AssetDatabase.Refresh();
        }

        [TearDown]
        public void TearDown()
        {
            UnityEditor.AssetDatabase.Refresh();
            if (Directory.Exists(BasePath.full))
            {
                Directory.Delete(BasePath.full, true);
            }
            // 削除結果が反映されないことがあるので、ちゃんと AssetDatabase を再読込する
            UnityEditor.AssetDatabase.Refresh();
        }

        [Test]
        public void 直下にディレクトリを作れる()
        {
            ExtraUnityEditor.AssetDatabase.CreateFolderRecursive(Path.Combine(BasePath.relative, "Direct"));
            // 作成が反映されないコトがあるので、AssetDatabase を再読込する
            UnityEditor.AssetDatabase.Refresh();
            Assert.That(Directory.Exists(Path.Combine(BasePath.full, "Direct")), Is.True);
        }

        [Test]
        public void 再帰的にディレクトリを作れる()
        {
            ExtraUnityEditor.AssetDatabase.CreateFolderRecursive(Path.Combine(BasePath.relative, "Deep", "Level", "Directory"));
            // 作成が反映されないコトがあるので、AssetDatabase を再読込する
            UnityEditor.AssetDatabase.Refresh();
            Assert.That(Directory.Exists(Path.Combine(BasePath.full, "Deep", "Level", "Directory")), Is.True);
        }

        [Test]
        public void 既に存在していてもエラーにならない()
        {
            Directory.CreateDirectory(Path.Combine(BasePath.full, "Direct"));
            Directory.CreateDirectory(Path.Combine(BasePath.full, "Deep", "Level", "Directory"));
            // 作成が反映されないコトがあるので、AssetDatabase を再読込する
            UnityEditor.AssetDatabase.Refresh();
            Assert.That(() => ExtraUnityEditor.AssetDatabase.CreateFolderRecursive(Path.Combine(BasePath.relative, "Direct")), Throws.Nothing);
            Assert.That(() => ExtraUnityEditor.AssetDatabase.CreateFolderRecursive(Path.Combine(BasePath.relative, "Deep", "Level", "Directory")), Throws.Nothing);
        }
    }
}