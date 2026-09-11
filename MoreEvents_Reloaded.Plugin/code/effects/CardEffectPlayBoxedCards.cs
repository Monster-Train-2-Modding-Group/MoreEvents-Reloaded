using ShinyShoe;
using System.Collections;
using static CardManager;
using static UnityEngine.GraphicsBuffer;

namespace MoreEvents_Reloaded.Plugin.code.effects
{
    public sealed class CardEffectPlayBoxedCards : CardEffectBase
    {
        public override PropDescriptions CreateEditorInspectorDescriptions()
        {
            return PropDescriptions.DisplayNone;
        }

        public override IEnumerator ApplyEffect(CardEffectState cardEffectState, CardEffectParams cardEffectParams, ICoreGameManagers coreGameManagers, ISystemManagers sysManagers)
        {
            CardManager cardManager = coreGameManagers.GetCardManager();
            AllGameData allGameData = coreGameManagers.GetAllGameData();
            SaveManager saveManager = coreGameManagers.GetSaveManager();
            RelicManager relicManager = coreGameManagers.GetRelicManager();
            RoomManager roomManager = coreGameManagers.GetRoomManager();
            CombatManager combatManager = coreGameManagers.GetCombatManager();
            CardStatistics cardStatistics = coreGameManagers.GetCardStatistics();
            PlayerManager playerManager = coreGameManagers.GetPlayerManager();
            CardStateModifiers? cardStateModifiers = cardEffectParams.playedCard?.GetCardStateModifiers();
            if (cardStateModifiers == null)
            {
                yield break;
            }

            var xCostEnergy = playerManager.GetEnergy() + (saveManager.PreviewMode ? 0 : cardEffectParams.playedCard!.GetLastPlayedCost());
            foreach (var cardState in cardStateModifiers.BoxedCardStates)
            {
                
                foreach (CardTraitState traitState in cardState.GetTraitStates())
                {
                    traitState.OnCardDrawn(cardState, coreGameManagers);
                }
                if (!combatManager.TestEffects(cardState.GetEffectStates(), cardEffectParams.selectedRoom, cardState, cardEffectParams.selfTarget, cardEffectParams.dropLocation,
                                               cardEffectParams.characterThatActivatedAbility, cardEffectParams.roomThatActivatedAbility, out var _, cardEffectParams.disableLastSpawnedCharacterTargeting,
                                               cardEffectParams.overrideLastSpawnedCharacter))
                {
                    continue;
                }
                // TODO consider handling Extract here.
                // Handle inner X costs by setting its played cost.
                cardStatistics.UpdateCardPlayedCost(cardState, xCostEnergy);
                yield return combatManager.ApplyEffects(cardState.GetEffectStates(), cardEffectParams.selectedRoom, cardState, true, cardEffectParams.sourceRelic,
                    cardEffectParams.selfTarget, cardEffectParams.dropLocation, cardEffectParams.ignoreDeadInTargeting, null, null,
                    null, true, cardEffectParams.cardTriggeredCharacter, cardEffectParams.dyingCharacter, cardEffectParams.fireCount,
                    cardEffectParams.overrideTargetCharacter, cardEffectParams.fromTrigger, cardEffectParams.triggerType, cardEffectParams.sourceCharacterTriggerState, cardEffectParams.characterThatActivatedAbility,
                    cardEffectParams.roomThatActivatedAbility, cardEffectParams.isFromHiddenTrigger, cardEffectParams.disableLastSpawnedCharacterTargeting, cardEffectParams.overrideLastSpawnedCharacter);
                yield return combatManager.ApplyCardTriggers(CardTriggerType.OnCast, cardState, fireAllMonsterTriggersInRoom: false, cardEffectParams.selectedRoom, ignoreDeadInTargeting: true,
                    cardEffectParams.cardTriggeredCharacter, cardEffectParams.dyingCharacter, null, cardEffectParams.dropLocation);
                CardManager.DiscardCardParams discardCardParams = new()
                {
                    discardCard = cardState,
                    isRecastCopy = true,
                    wasPlayed = !saveManager.PreviewMode
                };
                foreach (CardTraitState traitState in cardState.GetTraitStates())
                {
                    // Don't want to trigger Purge/Ephemeral/Holdover, but do want to trigger Spellchain
                    // So a hack.
                    discardCardParams.isRecastCopy = HackRecastCopy(traitState);
                    yield return traitState.OnCardDiscarded(discardCardParams, coreGameManagers);
                }
            }
        }

        private bool HackRecastCopy(CardTraitState cardTraitState)
        {
            return cardTraitState.GetType().Name switch
            {
                "CardTraitCopyOnPlay" => false,
                "CardTraitAddCardUpgradeOnMoonPhase" => false,
                "CardTraitScalingReturnConsumedCards" => false,
                _ => true
            };
        }

        public override bool CreateAdditionalTooltips(CardEffectData cardEffectData, List<TooltipContent> content, SaveManager saveManager, CardState? sourceCardState = null)
        {
            if (sourceCardState == null)
            {
                return false;
            }
            CardStateModifiers cardStateModifiers = sourceCardState.GetCardStateModifiers();
            if (cardStateModifiers != null)
            {
                List<CardState> list;
                using (GenericPools.GetList(out list, cardStateModifiers.BoxedCardStates))
                {
                    foreach (CardState item in list)
                    {
                        if (CardTooltipHelper.TryGetAddedCardTooltipContent(saveManager.GetAllGameData().FindCardData(item.GetCardDataID()), out var value, item, saveManager))
                        {
                            content.Add(value);
                        }
                    }
                }
            }
            return content.Count > 0;
        }
    }

}
