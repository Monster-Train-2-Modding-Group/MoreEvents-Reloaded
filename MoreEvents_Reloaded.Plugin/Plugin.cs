using BepInEx;
using BepInEx.Logging;
using Conductor.Data.Registers;
using MoreEvents_Reloaded.Plugin.code;
using MoreEvents_Reloaded.Plugin.code.rewards;
using TrainworksReloaded.Base;
using TrainworksReloaded.Base.CardUpgrade;
using TrainworksReloaded.Base.Extensions;
using TrainworksReloaded.Core;
using TrainworksReloaded.Core.Extensions;
using TrainworksReloaded.Core.Interfaces;
using UnityEngine;

namespace MoreEvents_Reloaded.Plugin
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        internal static new ManualLogSource Logger = new(MyPluginInfo.PLUGIN_GUID);
        
        public void Awake()
        {
            Logger = base.Logger;

            var builder = Railhead.GetBuilder();
            builder.Configure(
                MyPluginInfo.PLUGIN_GUID,
                c =>
                {
                    c.AddMergedJsonFile(
                        "json/plugin.json",
                        "json/cards/SpikedriverColony.json",
                        "json/cards/AutomaticRailspikes.json",
                        "json/cards/NexusSpike.json",
                        "json/cards/OldeMagic.json",
                        "json/events/UnitQuest.json",
                        "json/events/SpellMerge.json",
                        "json/events/BuildACard.json",
                        "json/events/DivineAltar.json",
                        "json/events/UnitQuest_UnitFollowup.json",
                        "json/events/UnitQuest_SpellFollowup.json",
                        "json/enhancers/Purgepact.json",
                        "json/enhancers/Seekpact.json",
                        "json/enhancers/Thricepact.json",
                        "json/enhancers/Truepact.json",
                        "json/enhancers/Valuepact.json",
                        "json/enhancers/UnhingedPower.json",
                        "json/upgrades/BuildCard_Bonus_AddCapacity.json",
                        "json/upgrades/BuildCard_Bonus_CardDraw.json",
                        "json/upgrades/BuildCard_Bonus_GainGold.json",
                        "json/upgrades/BuildCard_Movement_Advance.json",
                        "json/upgrades/BuildCard_Movement_Ascend.json",
                        "json/upgrades/BuildCard_Movement_Descend.json",
                        "json/upgrades/BuildCard_Movement_Retreat.json",
                        "json/upgrades/BuildCard_Movement_ShuffleE.json",
                        "json/upgrades/BuildCard_Movement_ShuffleF.json",
                        "json/upgrades/BuildCard_Primary_Buff.json",
                        "json/upgrades/BuildCard_Primary_Damage.json",
                        "json/upgrades/BuildCard_Primary_Heal.json",
                        "json/upgrades/BuildCard_Primary_HealthUp.json",
                        "json/upgrades/BuildCard_Status_AddArmor.json",
                        "json/upgrades/BuildCard_Status_AddAvarice.json",
                        "json/upgrades/BuildCard_Status_AddBurst.json",
                        "json/upgrades/BuildCard_Status_AddConduit.json",
                        "json/upgrades/BuildCard_Status_AddDecay.json",
                        "json/upgrades/BuildCard_Status_AddFrostbite.json",
                        "json/upgrades/BuildCard_Status_AddPyregel.json",
                        "json/upgrades/BuildCard_Status_AddRage.json",
                        "json/upgrades/BuildCard_Status_AddRegen.json",
                        "json/upgrades/BuildCard_Status_AddSap.json",
                        "json/upgrades/BuildCard_Status_AddSpikes.json",
                        "json/upgrades/BuildCard_Status_AddUnstable.json",
                        "json/upgrades/BuildCard_Status_AddValor.json"
                    );
                }
            );

            Conductor.Utilities.AddInkExternalFunction("CardsAvailableForFusion", InkExternalFunctions.CardsAvailableForFusion);
            Conductor.Utilities.AddInkExternalFunction("MoreEvents_SetBuildACardOption", InkExternalFunctions.MoreEvents_SetBuildACardOption);

            Railend.ConfigurePostAction(c =>
            {
                var rewardRegister = c.GetInstance<IRegister<RewardData>>();
                var cardRegister = c.GetInstance<IRegister<CardData>>();
                var upgradeRegister = c.GetInstance<IRegister<CardUpgradeData>>();
                var filterRegister = c.GetInstance<IRegister<CardUpgradeMaskData>>();

                T? GetReward<T>(string id) where T : RewardData
                {
                    return rewardRegister.GetValueOrDefault(MyPluginInfo.PLUGIN_GUID.GetId(TemplateConstants.RewardData, id)) as T;
                }

                CardData GetCard(string id)
                {
                    return cardRegister.GetValueOrDefault(MyPluginInfo.PLUGIN_GUID.GetId(TemplateConstants.Card, id))!;
                }

                CardUpgradeData GetUpgrade(string id)
                {
                    return upgradeRegister.GetValueOrDefault(MyPluginInfo.PLUGIN_GUID.GetId(TemplateConstants.Upgrade, id))!;
                }

                CardUpgradeMaskData GetFilter(string id)
                {
                    return filterRegister.GetValueOrDefault(MyPluginInfo.PLUGIN_GUID.GetId(TemplateConstants.UpgradeMask, id))!;
                }

                var nexusSpikeReward = GetReward<NexusSpellMergeRewardData>("NexusSpikeSpellMergeReward")!;
                nexusSpikeReward.firstFilterData = GetFilter("BannedFromNexusSpikeStartingCard");
                nexusSpikeReward.secondFilterData = GetFilter("BannedFromNexusSpike");
                nexusSpikeReward.targetedCard = GetCard("NexusSpikeTarget");
                nexusSpikeReward.targetlessCard = GetCard("NexusSpikeTargetless");
                nexusSpikeReward.targetedXCard = GetCard("NexusSpikeXTarget");
                nexusSpikeReward.targetlessXCard = GetCard("NexusSpikeXTargetless");
                nexusSpikeReward.reservePositive = GetUpgrade("PlayReservePositiveMergeCardsUpgrade");
                nexusSpikeReward.reserveNegative = GetUpgrade("PlayReserveNegativeMergeCardsUpgrade");

                var oldeMagicReward = GetReward<BuildACardRewardData>("BuildACardReward")!;
                oldeMagicReward.cardData = GetCard("OldeMagic");
                oldeMagicReward.componentUpgrades = 
                [
                    [GetUpgrade("BuildCard_Damage"), GetUpgrade("BuildCard_Heal"), GetUpgrade("BuildCard_Buff"), GetUpgrade("BuildCard_HealthUp")],
                    [
                         GetUpgrade("BuildCard_AddArmor"), GetUpgrade("BuildCard_AddRegen"), GetUpgrade("BuildCard_AddSap"), 
                         GetUpgrade("BuildCard_AddFrostbite"), GetUpgrade("BuildCard_AddSpikes"), GetUpgrade("BuildCard_AddRage"),
                         GetUpgrade("BuildCard_AddBurst"), GetUpgrade("BuildCard_AddConduit"), GetUpgrade("BuildCard_AddDecay"),
                         GetUpgrade("BuildCard_AddPyregel"), GetUpgrade("BuildCard_AddUnstable"), GetUpgrade("BuildCard_AddValor"), 
                         GetUpgrade("BuildCard_AddAvarice")
                    ],
                    [
                        GetUpgrade("BuildCard_Advance"), GetUpgrade("BuildCard_Retreat"), GetUpgrade("BuildCard_Descend"),
                        GetUpgrade("BuildCard_Ascend"), GetUpgrade("BuildCard_ShuffleF"), GetUpgrade("BuildCard_ShuffleE")],
                    [GetUpgrade("BuildCard_AddCapacity"), GetUpgrade("BuildCard_CardDraw"), GetUpgrade("BuildCard_GainGold")]
                ];
   
            });

            Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        }
    }
}
