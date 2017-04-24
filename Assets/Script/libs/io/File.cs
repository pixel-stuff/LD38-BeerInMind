using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Libs.IO
{
    class File
    {
        public static string ReadResource(string _filePath)
        {
            UnityEngine.TextAsset targetFile = UnityEngine.Resources.Load<UnityEngine.TextAsset>(_filePath);
            return targetFile.text;
        }
        public static string ReadLocalFile(string _filePath)
        {
            System.IO.StreamReader reader = new System.IO.StreamReader(_filePath, System.Text.Encoding.Default);
            return reader.ReadToEnd();
        }
    }
}
