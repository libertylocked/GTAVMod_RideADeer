using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace GTAVMod_RideADeer
{
    public static class Globals
    {
        public static Keys ActivationKey { get; set; }
        public static Keys SpawnDeerKey { get; set; }
        public static Keys OnOffDeerKey { get; set; }
        public static bool ShowMoveMarker { get; set; }
        public static bool DeerInvincible { get; set; }
        public static bool DeerCanRagdoll { get; set; }
        public static float DeerWalkSpeed { get; set; }
        public static float DeerRunSpeed { get; set; }
        public static Structs.SetupFile MainSetup { get; set; }
        private static bool VSBuild = false;
        public static void InitGlobals()
        {
            LoadSettings();
        }
        public static void LoadSettings()
        {
            Stream setupIn = null;
            string settingsFileName = "scripts/rideadeer_settings.txt";
            if (VSBuild)
            {
                if (File.Exists(settingsFileName))
                    setupIn = File.Open(settingsFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                else
                {
                    setupIn = File.Create(settingsFileName);
                    Stream defaultStream = Utils.GetResourceAsStream("DefaultControlSetup.txt");
                    defaultStream.Seek(0, SeekOrigin.Begin);
                    defaultStream.CopyTo(setupIn);

                    setupIn.Flush();
                    setupIn.Seek(0, SeekOrigin.Begin);
                }

            }
            else
            {
                ////ON RELEASE BUILD ONLY!!
                //string settingsData = "IyBEZWZhdWx0IENvbnRyb2wgU2V0dXANCiMga2V5IGRvY3M6IGh0dHBzOi8vbXNkbi5taWNyb3NvZnQuY29tL2VuLXVzL2xpYnJhcnkvc3lzdGVtLndpbmRvd3MuZm9ybXMua2V5cyUyOHY9dnMuMTEwJTI5LmFzcHgNCiMgVXBkYXRlZCBieSBMaWJlcnR5bG9ja2VkDQojIE9yaWdpbmFsIHNjcmlwdCBieSBYQkxUb290aFBpaywgYXAgaWkgaW50ZW5zZSA8Mw0KIyBSaWRlQURlZXINCg0KLktleXMNCkFjdGl2YXRlID0gTnVtUGFkNw0KU3Bhd25EZWVyID0gTnVtUGFkNA0KR2V0T25PZmZEZWVyID0gRg0KDQouU2V0dGluZ3MNClNob3dNb3ZlTWFya2VyID0gZmFsc2UNCkRlZXJDYW5SYWdEb2xsID0gZmFsc2UNCkRlZXJJbnZpbmNpYmxlID0gdHJ1ZQ0KRGVlcldhbGtTcGVlZCA9IDIuMA0KRGVlclJ1blNwZWVkID0gMy4wDQo=";
                //byte[] settingsDataBytes = Convert.FromBase64String(settingsData);
                //if (File.Exists(settingsFileName))
                //    setupIn = File.Open(settingsFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                //else
                //{
                //    //Writing default settings file, base64 encoded
                //    setupIn = File.Create(settingsFileName);
                //    System.IO.BinaryWriter Writer = new System.IO.BinaryWriter(setupIn);
                //    Writer.BaseStream.Position = 0;

                //    Writer.Write(settingsDataBytes, 0, settingsDataBytes.Length);

                //    Writer.Flush();
                //    Writer.BaseStream.Seek(0, SeekOrigin.Begin);

                //}
                throw new IOException("Config file not found");
            }

            MainSetup = new Structs.SetupFile(setupIn);
            setupIn.Close();
            ActivationKey = MainSetup.GetEntryByName("Keys", "Activate").KeyValue;
            SpawnDeerKey = MainSetup.GetEntryByName("Keys", "SpawnDeer").KeyValue;
            OnOffDeerKey = MainSetup.GetEntryByName("Keys", "GetOnOffDeer").KeyValue;
            ShowMoveMarker = MainSetup.GetEntryByName("Settings", "ShowMoveMarker").BoolValue;
            DeerCanRagdoll = MainSetup.GetEntryByName("Settings", "DeerCanRagDoll").BoolValue;
            DeerInvincible = MainSetup.GetEntryByName("Settings", "DeerInvincible").BoolValue;
            DeerWalkSpeed = MainSetup.GetEntryByName("Settings", "DeerWalkSpeed").FloatValue;
            DeerRunSpeed = MainSetup.GetEntryByName("Settings", "DeerRunSpeed").FloatValue;
        }

    }
}
