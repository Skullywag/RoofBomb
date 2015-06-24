using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Verse;
using Verse.AI;
using UnityEngine;
using RimWorld;
namespace RoofBomb
{
    public class Building_RoofBomb : Building
	{

        public override void SpawnSetup()
        {
            base.SpawnSetup();
        }

        public override IEnumerable<Gizmo> GetGizmos()
        {
            Command_Action com = new Command_Action();

            com.defaultLabel = "CommandDetonateLabel".Translate();
            com.icon = ContentFinder<Texture2D>.Get("UI/Commands/Detonate");
            com.defaultDesc = "CommandDetonateDesc".Translate();
            com.action = () => Command_Detonate();
           
            yield return com;
        }

        private void Command_Detonate()
        {
            base.GetComp<CompExplosive>().StartWick();
        }

        public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
        {
            base.Destroy(mode);
            MoteThrower.ThrowMicroSparks(Position.ToVector3Shifted());
            if (mode == DestroyMode.Kill)
            {
                foreach (IntVec3 current in GenAdj.CellsAdjacent8WayAndInside(this))
                {
                    if (current.GetRoof() != null)
                    {
                        if (Find.RoofGrid.RoofAt(current).isThickRoof == true)
                        {
                            RoofDef roofType = DefDatabase<RoofDef>.GetNamed("RoofRockThin");
                            Find.RoofGrid.SetRoof(current, roofType);
                        }
                        else
                        {
                            Find.RoofGrid.SetRoof(current, null);
                        }
                    }
                }
            }
        }
	}
}