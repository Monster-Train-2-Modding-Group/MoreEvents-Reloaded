using ShinyShoe.Logging;
using UnityEngine;
using Conductor.Interfaces;

namespace MoreEvents_Reloaded.Plugin.code.rewards
{
    public sealed class BuildACardRewardData : GrantableRewardData, IRewardPreviewProvider, ICardGiverReward
    {
        [Header("GrantUpgradedCached CardReward")]
        [SerializeField]
        public CardData? cardData;

        [SerializeField]
        private static int[] upgradeKeys = [-1, -1, -1, -1];

        [SerializeField]
        public List<List<CardUpgradeData>> componentUpgrades = [];

        public override string GetRngDeterminismID()
        {
            return cardData!.GetID();
        }

        public override void GrantReward(GrantParams grantParams)
        {
            Log.Info(LogGroups.Gameplay, "Granted BuildCardRewardData reward " + cardData!.GetAssetKey());
            CardState cardState = grantParams.coreGameManagers.GetSaveManager().AddCardToDeck(cardData, null, applyExistingRelicModifiers: true, 0, applyExtraCopiesMutator: false, showAnimation: false, setupStartingUpgrades: false);
            foreach (CardUpgradeData upgradeData in GetUpgradeDatas())
            {
                CardUpgradeState cardUpgradeState = new();
                cardUpgradeState.Setup(upgradeData);
                cardState.ApplyPermanentUpgrade(cardUpgradeState, grantParams.coreGameManagers.GetSaveManager(), ignoreUpgradeAnimation: true);
            }
            grantParams.rewardGrantedCallback?.Invoke(new GrantResult(cardData.GetID(), cardData.GetAssetKey(), "CardReward"));
            grantParams.interactionCompleteCallback?.Invoke();
        }

        public static void SetUpgradeComponentKey(int component, int upgrade)
        {
            if (component == -1 && upgrade == -1)
                upgradeKeys[0] = upgradeKeys[1] = upgradeKeys[2] = upgradeKeys[3] = -1;
            else
            {
                upgradeKeys[component] = upgrade;
            }
        }

        public List<CardUpgradeData> GetUpgradeDatas()
        {
            List<CardUpgradeData> list = [];
            for (int i = 0; i < 4; i++)
            {
                int key = upgradeKeys[i];
                if (key == -1)
                    continue;
                list.Add(componentUpgrades[i][key]);
            }    
            return list;
        }

        public IRewardPreviewProvider.RewardPreviewInfo ProvidePreview(SaveManager saveManager, RelicManager relicManager)
        {
            return new IRewardPreviewProvider.RewardPreviewInfo
            {
                card = cardData,
                upgrades = GetUpgradeDatas()
            };
        }

        public string GetSpecificRewardName()
        {
            return cardData!.GetName();
        }

        public List<CardData> GetAllCardData()
        {
            return [cardData!];
        }
    }
}
