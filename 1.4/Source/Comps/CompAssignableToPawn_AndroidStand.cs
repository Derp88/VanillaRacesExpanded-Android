﻿using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace VREAndroids
{
    public class CompAssignableToPawn_AndroidStand : CompAssignableToPawn
    {
        public override IEnumerable<Pawn> AssigningCandidates
        {
            get
            {
                if (!parent.Spawned)
                {
                    return Enumerable.Empty<Pawn>();
                }
                return parent.Map.mapPawns.FreeColonists.Where((Pawn p) => p.HasActiveGene(VREA_DefOf.VREA_MemoryProcessing));
            }
        }

        public override string GetAssignmentGizmoDesc()
        {
            return "CommandBedSetOwnerDesc".Translate(parent.def.building.bed_humanlike ? FactionDefOf.PlayerColony.pawnSingular : "Animal".Translate().ToString());
        }

        public override bool AssignedAnything(Pawn pawn)
        {
            foreach (var other in Building_AndroidStand.stands)
            {
                if (other.CompAssignableToPawn.AssignedPawns.Contains(pawn))
                {
                    return true;
                }
            }
            return false;
        }

        public override void TryAssignPawn(Pawn pawn)
        {
            Building_AndroidStand stand = (Building_AndroidStand)parent;
            foreach (var assignedPawn in stand.CompAssignableToPawn.AssignedPawns.ToList())
            {
                stand.CompAssignableToPawn.ForceRemovePawn(assignedPawn);
            }
            foreach (var other in Building_AndroidStand.stands)
            {
                if (other.CompAssignableToPawn.AssignedPawns.Contains(pawn))
                {
                    other.CompAssignableToPawn.ForceRemovePawn(pawn);
                }
            }
            stand.CompAssignableToPawn.ForceAddPawn(pawn);
        }

        public override void TryUnassignPawn(Pawn pawn, bool sort = true, bool uninstall = false)
        {
            Building_AndroidStand stand = (Building_AndroidStand)parent;
            stand.CompAssignableToPawn.ForceRemovePawn(pawn);
        }

        public override bool ShouldShowAssignmentGizmo()
        {
            return parent.Faction == Faction.OfPlayer;
        }
    }
}
