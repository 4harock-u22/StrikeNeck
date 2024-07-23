using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NewFLD.src.DiscriminantModel.ForwardLeanDetector
{
    internal class ModelFileAccessor
    {
        private static String modelPath = Path.Combine(FileSystem.AppDataDirectory, "MLModel.zip");
        private FileInfo modelFile;


        internal ModelFileAccessor()
        {
            modelFile = new FileInfo(modelPath);
        }

        internal async Task<FileInfo> GetModelFileInfo()
        {
            if (!modelFile.Exists) await InitModelFile();

          return modelFile;
        }

        private async Task InitModelFile()
        {
            var assembly = Assembly.GetExecutingAssembly();
            using var assemblyStream = assembly.GetManifestResourceStream("fldmodel");
            using var fileStream = new FileStream(modelPath, FileMode.Create);

            await assemblyStream.CopyToAsync(fileStream);

            fileStream.Flush();
        }
    }
}
