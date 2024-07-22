using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace u22_strikeneck.Init
{
    internal class InitDirectoryAccessor
    {
        private static string rootDirectoryPath = Path.Combine(FileSystem.CacheDirectory, "pic", "train");
        private static string correctDirectoryPath = Path.Combine(rootDirectoryPath, "correct");
        private static string forwardDirectoryPath = Path.Combine(rootDirectoryPath, "forward");

        internal DirectoryInfo CorrectDirectoryInfo
        {
            get
            {
                var correctDirectoryInfo = new DirectoryInfo(correctDirectoryPath);
                if (!correctDirectoryInfo.Exists) correctDirectoryInfo.Create();
                return correctDirectoryInfo;
            }
        }

        internal DirectoryInfo ForwardDirectoryInfo
        {
            get
            {
                var forwardDirectoryInfo = new DirectoryInfo(forwardDirectoryPath);
                if (!forwardDirectoryInfo.Exists) forwardDirectoryInfo.Create();
                return new DirectoryInfo(forwardDirectoryPath);
            }
        }
        internal void ClearCorrectDirectory()
        {
            var correctDirectoryInfo = this.CorrectDirectoryInfo;
            correctDirectoryInfo.Delete(true);
            correctDirectoryInfo.Create();
        }

        internal void ClearForwardDirectory()
        {
            var forwardDirectoryInfo = this.ForwardDirectoryInfo;
            if (forwardDirectoryInfo.Exists) forwardDirectoryInfo.Delete(true);
            forwardDirectoryInfo.Create();
        }

    }
}
