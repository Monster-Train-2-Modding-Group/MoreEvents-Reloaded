using ShinyShoe;
using System.Collections;

namespace MoreEvents_Reloaded.Plugin.code.effects
{
    public sealed class CardEffectPlayBoxedCardTriggers : CardEffectBase
    {
        public override PropDescriptions CreateEditorInspectorDescriptions()
        {
            return PropDescriptions.DisplayNone;
        }

        public override IEnumerator ApplyEffect(CardEffectState cardEffectState, CardEffectParams cardEffectParams, ICoreGameManagers coreGameManagers, ISystemManagers sysManagers)
        {
            CombatManager combatManager = coreGameManagers.GetCombatManager();

            CardStateModifiers? cardStateModifiers = cardEffectParams.playedCard?.GetCardStateModifiers();
            if (cardStateModifiers == null)
            {
                yield break;
            }

            foreach (var cardState in cardStateModifiers.BoxedCardStates)
            {
                yield return combatManager.ApplyCardTriggers(cardEffectParams.triggerType, cardState, false, cardEffectParams.selectedRoom, cardEffectParams.ignoreDeadInTargeting,
                    cardEffectParams.cardTriggeredCharacter, cardEffectParams.dyingCharacter, null, cardEffectParams.dropLocation, null);
            }
        }
    }
}
