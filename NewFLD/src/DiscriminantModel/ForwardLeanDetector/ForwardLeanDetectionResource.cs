using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscriminantModel.ForwardLeanDetector
{
    internal class ForwardLeanDetectionResource
    {
        internal float fldBias = 0.0F;
        internal static string TRAIN_CSV_FILE = FileSystem.AppDataDirectory + "\\train.csv";
    }

    internal enum InputType
    {
        N_LEar_X, //Nose to Left Ear X
        N_LEar_Y, //Nose to Left Ear Y
        REar_REye_X, //Right Ear to Right Eye X
        REar_REye_Y, //Right Ear to Right Eye Y
        LEye_LEar_X, //Left Eye to Left Ear X
        LEye_LEar_Y, //Left Eye to Left Ear Y
        REye_N_X, //Right Eye to Nose X
        REye_N_Y, //Right Eye to Nose Y
        N_LEye_X, //Nose to Left Eye X
        N_LEye_Y //Nose to Left Eye Y
    }
}