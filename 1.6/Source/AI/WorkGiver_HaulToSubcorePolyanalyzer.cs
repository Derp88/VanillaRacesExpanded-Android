﻿using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI;

namespace VREAndroids
{
    public class WorkGiver_HaulToSubcorePolyanalyzer : WorkGiver_Scanner
    {
        public override ThingRequest PotentialWorkThingRequest => ThingRequest.ForDef(VREA_DefOf.VREA_SubcorePolyanalyzer);
        public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            if (!ModLister.CheckBiotech("Haul to subcore scanner"))
            {
                return false;
            }
            if (t.IsForbidden(pawn))
            {
                return false;
            }
            Building_SubcorePolyanalyzer building_SubcoreScanner;
            if ((building_SubcoreScanner = t as Building_SubcorePolyanalyzer) == null 
                || building_SubcoreScanner.State != SubcoreScannerState.WaitingForIngredients)
            {
                return false;
            }
            if (!pawn.CanReserve(t, 1, -1, null, forced))
            {
                return false;
            }
            return FindIngredients(pawn, building_SubcoreScanner).Thing != null;
        }

        public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            Building_SubcorePolyanalyzer building_SubcoreScanner;
            if ((building_SubcoreScanner = t as Building_SubcorePolyanalyzer) == null 
                || building_SubcoreScanner.State != SubcoreScannerState.WaitingForIngredients)
            {
                return null;
            }
            ThingCount thingCount = FindIngredients(pawn, building_SubcoreScanner);
            if (thingCount.Thing != null)
            {
                Job job = HaulAIUtility.HaulToContainerJob(pawn, thingCount.Thing, t);
                job.count = Mathf.Min(job.count, thingCount.Count);
                return job;
            }
            return null;
        }

        private ThingCount FindIngredients(Pawn pawn, Building_SubcorePolyanalyzer scanner)
        {
            Thing thing = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, 
                ThingRequest.ForGroup(ThingRequestGroup.HaulableEver), 
                PathEndMode.ClosestTouch, TraverseParms.For(pawn), 9999f, Validator);
            if (thing == null)
            {
                return default(ThingCount);
            }
            int requiredCountOf = scanner.GetRequiredCountOf(thing.def);
            return new ThingCount(thing, Mathf.Min(thing.stackCount, requiredCountOf));
            bool Validator(Thing x)
            {
                if (x.IsForbidden(pawn) || !pawn.CanReserve(x))
                {
                    return false;
                }
                return scanner.CanAcceptIngredient(x);
            }
        }
    }
}
