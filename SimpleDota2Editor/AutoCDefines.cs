namespace SimpleDota2Editor
{
    public static class AutoCDefines
    {
        private static readonly string[] MainKeyValueDefs_Abils =
        {
            //General
            "ID", "MaxLevel", "AbilityBehavior", "AbilityCastRange", "AbilityCastPoint", "AbilityCooldown", "AbilityChannelTime", "AbilityManaCost",
            "AbilityUnitDamageType", "AbilitySpecial", "AbilityUnitTargetTeam", "AbilityUnitTargetType", "AbilityUnitDamageType", 
        };

        private static readonly string[] MainKey_Events =
        { //Events
            "OnChannelFinish", "OnChannelInterrupted", "OnChannelSucceeded", "OnOwnerDied", "OnOwnerSpawned", "OnProjectileFinish", "OnProjectileHitUnit", "OnSpellStart",
            "OnToggleOff", "OnToggleOn", "OnUpgrade", "OnAbilityEndChannel", "OnAbilityStart", "OnAttack", "OnAttackAllied", "OnAttackFailed", "OnCreated", "OnEquip",
            "OnHealReceived", "OnHealthGained", "OnHeroKilled", "OnManaGained", "OnOrder", "OnProjectileDodge", "OnRespawn", "OnSpentMana", "OnStateChanged",
            "OnTeleported", "OnTeleporting", "OnUnitMoved",
        };

        private static readonly string[] MainKeyValueDefs_Heroes =
        {
            //General
            "Model", "Portrait", "IdleExpression", "SoundSet", "PickSound", "BanSound", "Enabled", "HeroUnlockOrder", "Role", "Rolelevels", "Team", "HeroID", "ModelScale",
            "HeroGlowColor", "CMEnabled", "url", "LastHitChallengeRival", "HeroSelectSoundEffect", "ArmorPhysical", "Ability", "AbilityLayout",

            //Armor
            "ArmorPhysical",

            //Attack
            "AttackCapabilities", "AttackDamageMin", "AttackDamageMax", "AttackRate", "AttackAnimationPoint", "AttackAcquisitionRange", "AttackRange", "ProjectileModel",
            "ProjectileSpeed",

            //Status
            "StatusHealthRegen",

            //Attributes
            "AttributePrimary", "AttributeBaseStrength", "AttributeStrengthGain", "AttributeBaseIntelligence", "AttributeIntelligenceGain", "AttributeBaseAgility",
            "AttributeAgilityGain",

            //Movement
            "MovementSpeed", "MovementTurnRate",

            //Vision
            "VisionNighttimeRange",

            //Bounds
            "BoundsHullName", "HealthBarOffset",

            //Files
            "ParticleFile", "GameSoundsFile", "VoiceFile",

            //KeyBlocks
            "AbilityPreview", "RenderablePortrait", "ItemSlots", "Bot",
        };

        private static readonly string[] MainKeyValueDefs_Items =
        {
            //General
            "ItemInitialCharges", "ItemPermanent", "ItemQuality", "ItemDroppable", "ItemShareability", "ItemDeclarations", "MaxUpgradeLevel", "ItemBaseLevel",
        };

        public static string MakeList_MainKey_Abils(string word)
        {
            string list = "";

            foreach (var def in MainKeyValueDefs_Abils)
            {
                if (def.Contains(word))
                    list += def + " ";
            }

            foreach (var def in MainKey_Events)
            {
                if (def.Contains(word))
                    list += def + " ";
            }

            if (list.Length != 0)
                list = list.Substring(0, list.Length - 1);

            return list;
        }

        public static string MakeList_MainKey_Items(string word)
        {
            string list = "";

            foreach (var def in MainKeyValueDefs_Items)
            {
                if (def.Contains(word))
                    list += def + " ";
            }

            foreach (var def in MainKeyValueDefs_Abils)
            {
                if (def.Contains(word))
                    list += def + " ";
            }

            foreach (var def in MainKey_Events)
            {
                if (def.Contains(word))
                    list += def + " ";
            }

            if (list.Length != 0)
                list = list.Substring(0, list.Length - 1);

            return list;
        }

        public static string MakeList_MainKey_Heros(string word)
        {
            string list = "";

            foreach (var def in MainKeyValueDefs_Heroes)
            {
                if (def.Contains(word))
                    list += def + " ";
            }

            if (list.Length != 0)
                list = list.Substring(0, list.Length - 1);

            return list;
        }


    }
}