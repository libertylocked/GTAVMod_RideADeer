using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Math;
using GTA.Native;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace GTAVMod_RideADeer
{
	public class Main : Script
    {
        bool Activated = false;
        Ped mainDeerHandle;
        
        public Main()
        {
            Tick += this.UpdateThis;
            KeyDown += this.KeyPress;
            mainDeerHandle = new Ped(-1);
            Globals.InitGlobals();
        }

        void UpdateThis(object sender, EventArgs e)
        {
            
            Ped myPed = Game.Player.Character;

            if (Utils.ControlJustPressed(204))
                EnterExitDeer();
            int attachedTo = Function.Call<int>(Hash.GET_ENTITY_ATTACHED_TO, myPed);
            if ((!myPed.IsInVehicle() || !myPed.IsGettingIntoAVehicle) && myPed.IsAttached() && attachedTo.Equals(mainDeerHandle.Handle) && Activated)
            {
                if (mainDeerHandle.Exists())
                {
                    float leftAxisXNormal = Utils.ControlNormal(218);
                    float leftAxisYNormal = Utils.ControlNormal(219);


                    float speed = Globals.DeerWalkSpeed;
                    float range = 4.0f;

                    if (Utils.ControlPressed(201))
                    {
                        range = 8.0f;
                        speed = Globals.DeerRunSpeed;
                    }

                    Vector3 goToOffset = mainDeerHandle.GetOffsetInWorldCoords(new Vector3(leftAxisXNormal * range, leftAxisYNormal * -1.0f * range, 0f));
                    mainDeerHandle.Task.LookAt(goToOffset);

                    mainDeerHandle.Task_GoStraightToCoord(goToOffset, speed, 20000, 40000f, 0.5f);

                    if (Globals.ShowMoveMarker)
                        Function.Call(Hash.DRAW_MARKER, 6, goToOffset.X, goToOffset.Y, goToOffset.Z, 0f, 0f, 0f, 0f, 0f, 0f, 1.0f, 1.0f, 1.0f, 255, 255, 255, 255, false, false, 2, false, false, false, false);

                }
            }
        }
        void KeyPress(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Globals.ActivationKey)
                GTA.UI.ShowSubtitle((Activated = !Activated) ? "You can now ~g~ride ~w~deer!" : "Riding deer ~r~disabled");
            if (Activated && e.KeyCode == Globals.SpawnDeerKey)
            {
                if (mainDeerHandle.Exists())
                    DestroryDeer();
                else
                    CreateDeer();
            }
            if (Activated && e.KeyCode == Globals.OnOffDeerKey)
                EnterExitDeer();
        }
        void DestroryDeer()
        {
            Ped myPed = Game.Player.Character;
            myPed.Detach();
            myPed.Task.ClearAllImmediately();
            mainDeerHandle.Delete();
            GTA.UI.ShowSubtitle("Deer removed");
        }
        void CreateDeer()
        {
            Model deer = Utils.LoadPedModel(PedHash.Deer);
            Ped myPed = Game.Player.Character;
            mainDeerHandle = World.CreatePed(deer, myPed.Position);
            myPed.Task.ClearAllImmediately();
            AttachToDeer(mainDeerHandle);
            
            mainDeerHandle.CanRagdoll = Globals.DeerCanRagdoll;

            mainDeerHandle.IsInvincible = Globals.DeerInvincible;
            deer.MarkAsNoLongerNeeded();

            GTA.UI.ShowSubtitle("You are now riding a deer!");
        }
        void AttachToDeer(Ped deerPed)
        {
            Ped myPed = Game.Player.Character;
            deerPed.FreezePosition = true;
            myPed.FreezePosition = true;
            myPed.Position = deerPed.Position;
            myPed.AttachTo(deerPed, deerPed.PedBoneIndex(24816), new Vector3(-0.3f, 0.0f, 0.3f), new Vector3(180f, 0.0f, 90f));
            myPed.Task.PlayAnimation("rcmjosh2", "josh_sitting_loop", 8f, -1, true, -1);
            deerPed.FreezePosition = false;
            myPed.FreezePosition = false;
        }
        void EnterExitDeer()
        {
            
            Ped myPed = Game.Player.Character;
            if (myPed.IsInVehicle() || myPed.IsDead || myPed.IsGettingIntoAVehicle)
                return;

            int attachedTo = Function.Call<int>(Hash.GET_ENTITY_ATTACHED_TO, myPed.Handle);
            Entity attachedToEntity = (Entity)new Ped(attachedTo);
            if (myPed.IsAttached() && attachedToEntity.Model.IsPed && attachedToEntity.Model.Hash.Equals(Function.Call<int>(Hash.GET_HASH_KEY, "a_c_deer")))
            {
                Vector3 sideCoords = Utils.GetCoordsInfrontOfEntityWithDistance(attachedToEntity.Handle, 1f, 90f);
                sideCoords.Z = World.GetGroundHeight(sideCoords);
                float sideHeading = attachedToEntity.Heading;
                mainDeerHandle = new Ped(-1);
                myPed.Detach();
                myPed.Task.ClearAllImmediately();
                myPed.Position = sideCoords;
                myPed.Heading = sideHeading;
            }
            else
            {
                Ped[] nearByPeds = World.GetNearbyPeds(myPed, 2f);
                foreach (Ped p in nearByPeds)
                {
                    if (p.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "a_c_deer"))
                    {
                        mainDeerHandle = p;
                        AttachToDeer(p);
                        break;
                    }
                }
            }
        }
    }
}