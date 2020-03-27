using System.IO;

namespace ExtraUnityEngine
{
    // ReSharper disable once PartialTypeWithSinglePart
    public static partial class Application
    {
        // ReSharper disable once InconsistentNaming 他の UnityEngine.Application.xxxPath との命名ルールを揃えるために警告を無視する
        public static string projectPath { get; } = Path.GetDirectoryName(UnityEngine.Application.dataPath);
    }
}