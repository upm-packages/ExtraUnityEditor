using NUnit.Framework;
using UnityEngine;

namespace ExtraUnityEditor.Tests.Editor.AssetDatabase
{
    public class FindAndLoadAsset
    {
        [Test]
        public void アセットが見付かる()
        {
            var material = ExtraUnityEditor.AssetDatabase.FindAndLoadAsset<Material>();
            Assert.That(material == null, Is.False);
        }

        [Test]
        public void アセットが見付からない()
        {
            var physicMaterial = ExtraUnityEditor.AssetDatabase.FindAndLoadAsset<PhysicMaterial>();
            Assert.That(physicMaterial == null, Is.True);
        }

        [Test]
        public void ディレクトリを指定してアセットが見付かる()
        {
            var material = ExtraUnityEditor.AssetDatabase.FindAndLoadAsset<Material>(new[] {"Assets/Tests/Editor/Materials"});
            Assert.That(material == null, Is.False);
        }

        [Test]
        public void ディレクトリを指定してアセットが見付からない()
        {
            var material = ExtraUnityEditor.AssetDatabase.FindAndLoadAsset<Material>(new[] {"Assets/Tests/Editor/Scripts"});
            Assert.That(material == null, Is.True);
        }

        [Test]
        public void パスフィルタによりアセットが見付かる()
        {
            var material = ExtraUnityEditor.AssetDatabase.FindAndLoadAsset<Material>(x => x.Contains("Second"));
            Assert.That(material == null, Is.False);
        }

        [Test]
        public void パスフィルタによりアセットが見付からない()
        {
            var material = ExtraUnityEditor.AssetDatabase.FindAndLoadAsset<Material>(x => x.Contains("Third"));
            Assert.That(material == null, Is.True);
        }

        [Test]
        public void ディレクトリを指定しつつパスフィルタによりアセットが見付かる()
        {
            var material = ExtraUnityEditor.AssetDatabase.FindAndLoadAsset<Material>(x => x.Contains("Second"), new[] {"Assets/Tests/Editor/Materials"});
            Assert.That(material == null, Is.False);
        }

        [Test]
        public void ディレクトリを指定しつつパスフィルタによりアセットが見付からない()
        {
            {
                var material = ExtraUnityEditor.AssetDatabase.FindAndLoadAsset<Material>(x => x.Contains("Second"), new[] {"Assets/Tests/Editor/Scripts"});
                Assert.That(material == null, Is.True);
            }
            {
                var material = ExtraUnityEditor.AssetDatabase.FindAndLoadAsset<Material>(x => x.Contains("Third"), new[] {"Assets/Tests/Editor/Materials"});
                Assert.That(material == null, Is.True);
            }
        }
    }
}