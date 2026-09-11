using ShinyShoe.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace MoreEvents_Reloaded.Plugin.code.rewards
{ 
    public class NexusSpellMergeRewardData : GrantableRewardData
    {
        public readonly int numCardsToMerge = 2;

        public CardUpgradeMaskData? firstFilterData;
        public CardUpgradeMaskData? secondFilterData;

        public CardData? targetlessCard;
        public CardData? targetedCard;
        public CardData? targetlessXCard;
        public CardData? targetedXCard;

        public CardUpgradeData? reservePositive;
        public CardUpgradeData? reserveNegative;

        private const string _firstPassInstructionKey = "MergeSpell_FirstInstruction";

        private const string _nextPassInstructionKey = "MergeSpell_NextInstruction";

        private List<CardState> cardsChosen = [];

        public override InteractionType interactionType => InteractionType.Targeted;

        public override void GrantReward(GrantParams grantParams)
        {
            grantParams.systemManagers.GetScreenManager().StartCoroutine(ShowDeckSelection(grantParams));
        }

        private IEnumerator ShowDeckSelection(GrantParams grantParams)
        {
            cardsChosen.Clear();
            for (int i = 0; i < numCardsToMerge; i++)
            {
                CardUpgradeMaskData? filter = firstFilterData;
                string instructionContent = "MergeSpell_FirstInstruction".Localize();
                if (i > 0)
                {
                    instructionContent = string.Format("MergeSpell_NextInstruction".Localize(), cardsChosen[i - 1].GetTitle());
                    filter = secondFilterData;
                }
                bool cardChosen = false;
                bool interactionCompleted = false;
                ref Action interactionCompleteCallback = ref grantParams.interactionCompleteCallback;
                interactionCompleteCallback = (Action)Delegate.Combine(interactionCompleteCallback, (Action)delegate
                {
                    interactionCompleted = true;
                });
                DoUIScreenConfirmationFlow(ScreenName.Deck, grantParams, delegate (IScreen screen, ScreenSetupConfirmationDelegate screenSetupConfirmation)
                {
                    DeckScreen obj = (screen as DeckScreen)!;
                    obj.Setup(new DeckScreen.Params
                    {
                        mode = DeckScreen.Mode.SpellMergeSelection,
                        showCancel = true,
                        rewardSource = grantParams.source,
                        cardUpgradeMaskData = filter,
                        totalUses = grantParams.correspondingReward.TotalUses,
                        sourceRewardState = grantParams.correspondingReward,
                        forceExcludeCard = ((i > 0) ? cardsChosen[i - 1] : null),
                        numDeckScreensNeededAfterThis = numCardsToMerge - i - 1,
                        titleKey = _rewardTitleKey,
                        overrideInstructionContent = instructionContent,
                        excludeFilteredOutCards = true
                    });
                    obj.AddDeckScreenCardStateChosenDelegate(delegate (CardState returnCardState)
                    {
                        if (returnCardState != null)
                        {
                            cardsChosen.Add(returnCardState);
                            cardChosen = true;
                        }
                    });
                });
                yield return new WaitWhile(() => !interactionCompleted);
                if (!cardChosen)
                {
                    break;
                }
            }
            if (cardsChosen.Count == numCardsToMerge)
            {
                MergeCards();
                grantParams.rewardGrantedCallback?.Invoke(default(GrantResult));
                grantParams.correspondingReward.ClaimReward(increment: false);
                Log.Info(LogGroups.Gameplay, "Granted SpellMergeRewardData reward");
            }
            else
            {
                Log.Info(LogGroups.Gameplay, "Cancelled SpellMergeRewardData");
            }
        }

        private void MergeCards()
        {
            var cardData = targetlessCard;
            bool requireXCostCard = false;
            CardStateModifiers cardStateModifiers = new();
            bool requireReserveNegative = false;
            bool requireReservePositive = false;
            foreach (CardState item in cardsChosen)
            {
                CardData setCardData = saveManager.GetAllGameData().FindCardData(item.GetCardDataID())!;
                CardState cardState = new(setCardData, saveManager, true, false);
                cardState.CopyPermanentUpgrades(item.GetCardStateModifiers(), saveManager.GetAllGameData(), saveManager);

                List<CardEffectState> effectStates = cardState.GetEffectStates();
                for (int i = 0; i < effectStates.Count; i++)
                {
                    CardEffectState cardEffectState = effectStates[i];
                    ICardEffect cardEffect = cardEffectState.GetCardEffect();
                    if (cardEffect is CardEffectSacrifice)
                    {
                        if (cardEffectState.GetTargetCharacterSubtype().IsNone && !cardEffectState.GetParamSubtype().IsNone)
                        {
                            cardEffectState.SetTargetCharacterSubtype(cardEffectState.GetParamSubtype());
                        }
                    }
                }

                if (item.GetCardTargetMode() == CardTargetMode.SingleTarget)
                {
                    cardData = targetedCard;
                }

                if (item.IsConsumeRemainingEnergyCostType())
                    requireXCostCard = true;

                foreach (var trigger in item.GetCardTriggers())
                {
                    if (trigger.GetTrigger() == CardTriggerType.OnUnplayedNegative)
                        requireReserveNegative = true;
                    if (trigger.GetTrigger() == CardTriggerType.OnUnplayedPositive)
                        requireReservePositive = true;
                }

                int boxedCardStateId = saveManager.AddBoxedCardState(cardState);
                cardStateModifiers.BoxCardState(boxedCardStateId, saveManager);
            }

            if (requireXCostCard)
            {
                cardData = (cardData == targetedCard) ? targetedXCard : targetlessXCard;
            }

            var nexus = saveManager.AddCardToDeck(cardData, cardStateModifiers, applyExistingRelicModifiers: true, 0, applyExtraCopiesMutator: false, ShowRewardAnimationInEvent);
            if (requireReserveNegative)
            {
                var upgradeState = new CardUpgradeState();
                upgradeState.Setup(reserveNegative);
                nexus.ApplyPermanentUpgrade(upgradeState, saveManager, true);
            }
            if (requireReservePositive)
            {
                var upgradeState = new CardUpgradeState();
                upgradeState.Setup(reservePositive);
                nexus.ApplyPermanentUpgrade(upgradeState, saveManager, true);
            }
        }
    }
}
