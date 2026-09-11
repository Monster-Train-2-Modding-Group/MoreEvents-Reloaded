using Conductor.Extensions;
using HarmonyLib;
using Ink.Runtime;
using MoreEvents_Reloaded.Plugin.code.rewards;
using ShinyShoe.Logging;
using System.Collections;

namespace MoreEvents_Reloaded.Plugin.code
{
    public static class InkExternalFunctions
    {
        public static object CardsAvailableForFusion(object[] args)
        {
            var deck = AllGameManagers.Instance!.GetSaveManager().GetDeckState();
            int count = 0;
            foreach (var card in deck)
            { 
                if (card.IsMonsterCard() && !card.IsChampionCard())
                {
                    bool flag = false;
                    foreach (var upgrade in card.GetCardStateModifiers().GetCardUpgrades())
                    {
                        if (upgrade.IsEssenceUpgrade())
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (!flag)
                        count++;
                }
            }
            return count >= 2;
        }

        public static object? MoreEvents_SetBuildACardOption(object[] args)
        {
            var component = (int)args[0];
            var entry = (int)args[1];

            BuildACardRewardData.SetUpgradeComponentKey(component, entry);

            return null;
        }
    }
}
