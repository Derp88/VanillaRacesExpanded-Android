﻿
using System.Collections.Generic;
using VEF.AnimalBehaviours;
using RimWorld;
using RimWorld.Planet;
using RimWorld.QuestGen;
using Verse;
namespace VREAndroids
{
    public class QuestNode_GenerateAndroid : QuestNode
    {
        [NoTranslate]
        public SlateRef<string> storeAs;

        [NoTranslate]
        public SlateRef<string> addToList;

        [NoTranslate]
        public SlateRef<IEnumerable<string>> addToLists;

        public SlateRef<PawnKindDef> kindDef;

        public SlateRef<Faction> faction;

        public SlateRef<bool> forbidAnyTitle;



        public SlateRef<bool> ensureNonNumericName;

        public SlateRef<IEnumerable<TraitDef>> forcedTraits;

        public SlateRef<IEnumerable<TraitDef>> prohibitedTraits;

        public SlateRef<Pawn> extraPawnForExtraRelationChance;

        public SlateRef<float> relationWithExtraPawnChanceFactor;

        public SlateRef<bool?> allowAddictions;

        public SlateRef<float> biocodeWeaponChance;

        public SlateRef<float> biocodeApparelChance;

        public SlateRef<bool> mustBeCapableOfViolence;

        public SlateRef<bool> isChild;

        public SlateRef<bool> allowPregnant;

        public SlateRef<Gender?> fixedGender;

        private const int MinExpertSkill = 11;

        public override bool TestRunInt(Slate slate)
        {
            return true;
        }

        protected virtual DevelopmentalStage GetDevelopmentalStage(Slate slate)
        {
            if (!Find.Storyteller.difficulty.ChildrenAllowed || !isChild.GetValue(slate))
            {
                return DevelopmentalStage.Adult;
            }
            return DevelopmentalStage.Child;
        }

        public  override void RunInt()
        {

           

            Slate slate = QuestGen.slate;
            PawnKindDef value = kindDef.GetValue(slate);
            Faction value2 = Faction.OfPlayer;
            bool flag = allowAddictions.GetValue(slate) ?? true;
            bool value3 = allowPregnant.GetValue(slate);
            IEnumerable<TraitDef> value4 = forcedTraits.GetValue(slate);
            IEnumerable<TraitDef> value5 = prohibitedTraits.GetValue(slate);
            float value6 = biocodeWeaponChance.GetValue(slate);
            PawnGenerationRequest request = new PawnGenerationRequest(mustBeCapableOfViolence: mustBeCapableOfViolence.GetValue(slate), colonistRelationChanceFactor: 1f, forceAddFreeWarmLayerIfNeeded: false, allowGay: true, allowPregnant: value3, allowFood: true, allowAddictions: flag, inhabitant: false, certainlyBeenInCryptosleep: false, forceRedressWorldPawnIfFormerColonist: false, worldPawnFactionDoesntMatter: false, biocodeWeaponChance: value6, extraPawnForExtraRelationChance: extraPawnForExtraRelationChance.GetValue(slate), relationWithExtraPawnChanceFactor: relationWithExtraPawnChanceFactor.GetValue(slate), fixedGender: fixedGender.GetValue(slate), kind: value, faction: value2, context: PawnGenerationContext.NonPlayer, tile: -1, forceGenerateNewPawn: false, allowDead: false, allowDowned: false, canGeneratePawnRelations: true, biocodeApparelChance: biocodeApparelChance.GetValue(slate), validatorPreGear: null, validatorPostGear: null, forcedTraits: value4, prohibitedTraits: value5, minChanceToRedressWorldPawn: null, fixedBiologicalAge: null, fixedChronologicalAge: null, fixedLastName: null, fixedBirthName: null, fixedTitle: null, fixedIdeo: null, forceNoIdeo: false, forceNoBackstory: false, forbidAnyTitle: false, forceDead: false, forcedXenogenes: null, forcedEndogenes: null, forcedXenotype: null, forcedCustomXenotype: null, allowedXenotypes: null, forceBaselinerChance: 0f, developmentalStages: GetDevelopmentalStage(slate));
            request.BiocodeApparelChance = biocodeApparelChance.GetValue(slate);
            request.ForbidAnyTitle = forbidAnyTitle.GetValue(slate);
            request.ForcedXenotype = VREA_DefOf.VREA_AndroidAwakened;
            Pawn pawn = PawnGenerator.GeneratePawn(request);


            if (ensureNonNumericName.GetValue(slate) && (pawn.Name == null || pawn.Name.Numerical))
            {
                pawn.Name = PawnBioAndNameGenerator.GeneratePawnName(pawn);
            }
            if (storeAs.GetValue(slate) != null)
            {
                QuestGen.slate.Set(storeAs.GetValue(slate), pawn);
            }
            if (addToList.GetValue(slate) != null)
            {
                QuestGenUtility.AddToOrMakeList(QuestGen.slate, addToList.GetValue(slate), pawn);
            }
            if (addToLists.GetValue(slate) != null)
            {
                foreach (string item in addToLists.GetValue(slate))
                {
                    QuestGenUtility.AddToOrMakeList(QuestGen.slate, item, pawn);
                }
            }
            QuestGen.AddToGeneratedPawns(pawn);
            if (!pawn.IsWorldPawn())
            {
                Find.WorldPawns.PassToWorld(pawn);
            }
        }
    }
}