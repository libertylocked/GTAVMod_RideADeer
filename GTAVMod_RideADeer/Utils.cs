using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using GTA;
using GTA.Native;
using GTA.Math;

namespace GTAVMod_RideADeer
{
    public static class Utils
    {

        public static Stream GetResourceAsStream(string resName)
        {
            return Assembly.GetExecutingAssembly().GetManifestResourceStream(string.Format("RideADeer.Resources.{0}", resName));
        }
        public static Model LoadPedModel(PedHash pedHash)
        {
            Model newModel = new Model(pedHash);
            newModel.Request();
            while (!newModel.IsLoaded)
                Script.Wait(0);
            return newModel;
        }
        public static int PedBoneIndex(this Ped p, int b)
        {
            return Function.Call<int>(Hash.GET_PED_BONE_INDEX, p.Handle, b);
        }
        public static void Task_GoStraightToCoord(this Ped p, Vector3 position, float unk1, float unk2, float h1, float h2)
        {
            Function.Call(Hash.TASK_GO_STRAIGHT_TO_COORD, p.Handle, position.X, position.Y, position.Z, unk1, unk2, h1, h2);

        }
        public static bool ControlJustPressed(int control)
        {
            return Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, 2, control);
        }
        public static bool ControlPressed(int control)
        {
            return Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 2, control);
        }
        public static float ControlNormal(int control)
        {
            return Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, control);
        }
        public static Vector3 GetCoordsInfrontOfEntityWithDistance(int entity, float distance, float pHeading = 0f)
        {
            Vector3 coords = Function.Call<Vector3>(Hash.GET_ENTITY_COORDS, entity, 0);
            float heading = Function.Call<float>(Hash.GET_ENTITY_HEADING, entity);
            heading = DegreesToRadians(heading + pHeading);
            return new Vector3(coords.X + distance * Sin(-1.0f * heading), coords.Y + distance * Cos(-1.0f * heading), coords.Z);
        }
        public static Func<double, float> Tan = angleR => (float)System.Math.Tan(angleR);
        public static Func<double, float> Cos = angleR => (float)System.Math.Cos(angleR);
        public static Func<double, float> Sin = angleR => (float)System.Math.Sin(angleR);
        public static Func<double, float> DegreesToRadians = angleR => (float)(angleR * Math.PI / 180f);
    }
}
