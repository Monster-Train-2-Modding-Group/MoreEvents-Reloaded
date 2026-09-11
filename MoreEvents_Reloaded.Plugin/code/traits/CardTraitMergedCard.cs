using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MoreEvents_Reloaded.Plugin.code.traits
{
    public class CardTraitMergedCard : CardTraitState
    {
        public override bool GetIsPlayableFromHand(CardManager cardManager, RoomManager roomManager, int sourceCharacterRoomIndex, out CommonSelectionBehavior.SelectionError selectionError)
        {
            foreach (var card in GetBoxedCards())
            {
                foreach (var trait in card.GetTraitStates())
                {
                    if (!trait.GetIsPlayableFromHand(cardManager, roomManager, sourceCharacterRoomIndex, out selectionError))
                    {
                        return false;
                    }
                }
            }
            selectionError = CommonSelectionBehavior.SelectionError.None;
            return true;
        }

        public override bool GetIsPlayableFromPlay(CardManager cardManager, RoomManager roomManager, int sourceCharacterRoomIndex, out CommonSelectionBehavior.SelectionError selectionError)
        {
            foreach (var card in GetBoxedCards())
            {
                foreach (var trait in card.GetTraitStates())
                {
                    if (!trait.GetIsPlayableFromPlay(cardManager, roomManager, sourceCharacterRoomIndex, out selectionError))
                    {
                        return false;
                    }
                }
            }
            selectionError = CommonSelectionBehavior.SelectionError.None;
            return true;
        }

        public override IEnumerator OnCardDiscarded(CardManager.DiscardCardParams discardCardParams, ICoreGameManagers coreGameManagers)
        {
            foreach (var card in GetBoxedCards())
            {
                foreach (var trait in card.GetTraitStates())
                {
                    if (trait is CardTraitTreasure && (!discardCardParams.wasPlayed || discardCardParams.triggeredByCard) && !discardCardParams.handDiscarded && !discardCardParams.isRecastCopy)
                    {
                        var newDiscardCardParams = new CardManager.DiscardCardParams
                        {
                            discardCard = card,
                            wasPlayed = discardCardParams.wasPlayed,
                            isRecastCopy = discardCardParams.isRecastCopy,
                            triggeredByCard = discardCardParams.triggeredByCard,
                            handDiscarded = discardCardParams.handDiscarded,
                        };
                        yield return trait.OnCardDiscarded(newDiscardCardParams, coreGameManagers);
                    }
                }
            }
        }

        public IEnumerable<CardState> GetBoxedCards()
        {
            return GetCard().GetCardStateModifiers().BoxedCardStates;
        }

        public override PropDescriptions CreateEditorInspectorDescriptions()
        {
            return [];
        }
    }
}
