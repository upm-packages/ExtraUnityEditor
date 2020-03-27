using System.Linq;
using NUnit.Framework;
using UnityEngine;

namespace ExtraUnityEditor.Tests.Editor.AssetDatabase
{
    public class FindAndLoadAssets
    {
        [Test]
        public void アセットが見付かる()
        {
            var materials = ExtraUnityEditor.AssetDatabase.FindAndLoadAssets<Material>();
            Assert.That(materials.Count(), Is.EqualTo(3));
        }

        [Test]
        public void アセットが見付からない()
        {
            var physicMaterials = ExtraUnityEditor.AssetDatabase.FindAndLoadAssets<PhysicMaterial>();
            Assert.That(physicMaterials.Count(), Is.Zero);
        }

        [Test]
        public void ディレクトリを指定してアセットが見付かる()
        {
            var materials = ExtraUnityEditor.AssetDatabase.FindAndLoadAssets<Material>(new[] {"Assets/Tests/Editor/Materials/Multiple"});
            Assert.That(materials.Count(), Is.EqualTo(2));
        }

        [Test]
        public void ディレクトリを指定してアセットが見付からない()
        {
            var materials = ExtraUnityEditor.AssetDatabase.FindAndLoadAssets<Material>(new[] {"Assets/Tests/Editor/Scripts"});
            Assert.That(materials.Count(), Is.Zero);
        }

        [Test]
        public void パスフィルタによりアセットが見付かる()
        {
            var materials = ExtraUnityEditor.AssetDatabase.FindAndLoadAssets<Material>(x => x.Contains("Multiple"));
            Assert.That(materials.Count(), Is.EqualTo(2));
        }

        [Test]
        public void パスフィルタによりアセットが見付からない()
        {
            var materials = ExtraUnityEditor.AssetDatabase.FindAndLoadAssets<Material>(x => x.Contains("Third"));
            Assert.That(materials.Count(), Is.Zero);
        }

        [Test]
        public void ディレクトリを指定しつつパスフィルタによりアセットが見付かる()
        {
            var materials = ExtraUnityEditor.AssetDatabase.FindAndLoadAssets<Material>(x => x.Contains("Multiple"), new[] {"Assets/Tests/Editor/Materials/Multiple"});
            Assert.That(materials.Count(), Is.EqualTo(2));
        }

        [Test]
        public void ディレクトリを指定しつつパスフィルタによりアセットが見付からない()
        {
            {
                var materials = ExtraUnityEditor.AssetDatabase.FindAndLoadAssets<Material>(x => x.Contains("Second"), new[] {"Assets/Tests/Editor/Scripts"});
                Assert.That(materials.Count(), Is.Zero);
            }
            {
                var materials = ExtraUnityEditor.AssetDatabase.FindAndLoadAssets<Material>(x => x.Contains("Third"), new[] {"Assets/Tests/Editor/Materials"});
                Assert.That(materials.Count(), Is.Zero);
            }
        }
    }
}