EXTERNAL MoreEvents_SetBuildACardOption(number, number)

=== BuildACard ===

<style="Body">An <color="yellow">Arcane Machine</color> peeks through the snow, mechanical arms and controls in various states of disrepair. The large harness in the center looks to have held something vaguely familiar, but you can't place what.
Near the opening of the train, a metal control board stands relatively untouched. Clean, even. Strange.
When you move towards it, the board whirs to life and a single button with a diamond icon flashes at you.</style>
<style="Prompt">Do you press the button?</style>

+ [Accept]
    {MoreEvents_SetBuildACardOption(-1, -1)}
    -> FirstComponent
+ [Leave]
    -> END
    
=FirstComponent
~ temp roll = RANDOM(1, 4)

<style="Body">A row of levers rises from the metal board, each one adorned with symbols you can only assume refer to various health-related effects.</style>
<style="Prompt">What do you pull?</style>
{ roll != 1:
+ [Primary_Upgrade0]
    {MoreEvents_SetBuildACardOption(0, 0)}
    <style="Keyword"><align="center">You selected Jagged Lines.</align></style>
    -> SecondComponent
}
{ roll != 2:
+ [Primary_Upgrade1]
    {MoreEvents_SetBuildACardOption(0, 1)}
    <style="Keyword"><align="center">You selected Smooth Waves.</align></style>
    -> SecondComponent
}
{ roll != 3:
+ [Primary_Upgrade2]
    {MoreEvents_SetBuildACardOption(0, 2)}
    <style="Keyword"><align="center">You selected Upward Red Arrows.</align></style>
    -> SecondComponent
}
{ roll != 4:
+ [Primary_Upgrade3]
    {MoreEvents_SetBuildACardOption(0, 3)}
    <style="Keyword"><align="center">You selected Upward Green Arrows.</align></style>
    -> SecondComponent
}
+ [nothing]
    -> SecondComponent

=SecondComponent
~ temp first = RANDOM(1, 13)
~ temp second = RANDOM(1, 13)
~ temp third = RANDOM(1, 13)

- (check_second)
{ second == first:
    ~ second = RANDOM(1, 13)
    -> check_second
}

- (check_third)
{ third == first || third == second:
    ~ third = RANDOM(1, 13)
    -> check_third
}

<style="Body">A row of levers rises from the metal board, each one adorned with symbols you can only assume refer to various elemental effects.</style>
<style="Prompt">What do you press?</style>

{ first == 1 || second == 1 || third == 1:
+ [Status_Upgrade0]
    {MoreEvents_SetBuildACardOption(1, 0)}
    <style="Keyword"><align="center">You pressed the shield icon.</align></style>
    -> ThirdComponent
}
{ first == 2 || second == 2 || third == 2:
+ [Status_Upgrade1]
    {MoreEvents_SetBuildACardOption(1, 1)}
    <style="Keyword"><align="center">You pressed the life icon.</align></style>
    -> ThirdComponent
}
{ first == 3 || second == 3 || third == 3:
+ [Status_Upgrade2]
    {MoreEvents_SetBuildACardOption(1, 2)}
    <style="Keyword"><align="center">You pressed the sludge icon.</align></style>
    -> ThirdComponent
}
{ first == 4 || second == 4 || third == 4:
+ [Status_Upgrade3]
    {MoreEvents_SetBuildACardOption(1, 3)}
    <style="Keyword"><align="center">You pressed the snow icon.</align></style>
    -> ThirdComponent
}
{ first == 5 || second == 5 || third == 5:
+ [Status_Upgrade4]
    {MoreEvents_SetBuildACardOption(1, 4)}
    <style="Keyword"><align="center">You pressed the shards icons.</align></style>
    -> ThirdComponent
}
{ first == 6 || second == 6 || third == 6:
+ [Status_Upgrade5]
    {MoreEvents_SetBuildACardOption(1, 5)}
    <style="Keyword"><align="center">You pressed the flame icon.</align></style>
    -> ThirdComponent
}
{ first == 7 || second == 7 || third == 7:
+ [Status_Upgrade6]
    {MoreEvents_SetBuildACardOption(1, 6)}
    <style="Keyword"><align="center">You pressed the gear icon.</align></style>
    -> ThirdComponent
}
{ first == 8 || second == 8 || third == 8:
+ [Status_Upgrade7]
    {MoreEvents_SetBuildACardOption(1, 7)}
    <style="Keyword"><align="center">You pressed the star icon.</align></style>
    -> ThirdComponent
}
{ first == 9 || second == 9 || third == 9:
+ [Status_Upgrade8]
    {MoreEvents_SetBuildACardOption(1, 8)}
    <style="Keyword"><align="center">You pressed the mushroom icon.</align></style>
    -> ThirdComponent
}
{ first == 10 || second == 10 || third == 10:
+ [Status_Upgrade9]
    {MoreEvents_SetBuildACardOption(1, 9)}
    <style="Keyword"><align="center">You pressed the dragon icon.</align></style>
    -> ThirdComponent
}
{ first == 11 || second == 11 || third == 11:
+ [Status_Upgrade10]
    {MoreEvents_SetBuildACardOption(1, 10)}
    <style="Keyword"><align="center">You pressed the explosion icon.</align></style>
    -> ThirdComponent
}
{ first == 12 || second == 12 || third == 12:
+ [Status_Upgrade11]
    {MoreEvents_SetBuildACardOption(1, 11)}
    <style="Keyword"><align="center">You pressed the angel icon.</align></style>
    -> ThirdComponent
}
{ first == 13 || second == 13 || third == 13:
+ [Status_Upgrade12]
    {MoreEvents_SetBuildACardOption(1, 12)}
    <style="Keyword"><align="center">You pressed the greed icon.</align></style>
    -> ThirdComponent
}
+ [nothing]
    -> ThirdComponent

=ThirdComponent
~ temp roll = RANDOM(1, 5)
~ temp roll2 = RANDOM(1, 5)
~ temp shuffle_opt = RANDOM(1, 2)

- (check_second_in_third)
{ roll2 == roll:
    ~ roll2 = RANDOM(1, 5)
    -> check_second_in_third
}

<style="Body">A small rod rises from a space in the board. It can be moved in 4 directions, though one direction is blocked entirely.</style>
<style="Prompt">Which direction do you shift the rod to?</style>

{ not (roll == 1 || roll2 == 1):
+ [Movement_Upgrade0]
    {MoreEvents_SetBuildACardOption(2, 0)}
    <style="Keyword"><align="center">You chose Forward.</align></style>
    -> FourthComponent
}
{ not(roll == 2 || roll2 == 2):
+ [Movement_Upgrade1]
    {MoreEvents_SetBuildACardOption(2, 1)}
    <style="Keyword"><align="center">You chose Backward.</align></style>
    -> FourthComponent
}
{ not(roll == 3 || roll2 == 3):
+ [Movement_Upgrade2]
    {MoreEvents_SetBuildACardOption(2, 2)}
    <style="Keyword"><align="center">You chose Down.</align></style>
    -> FourthComponent
}
{ not(roll == 4 || roll2 == 4):
+ [Movement_Upgrade3]
    {MoreEvents_SetBuildACardOption(2, 3)}
    <style="Keyword"><align="center">You chose Up.</align></style>
    -> FourthComponent
}
{ not(roll == 5 || roll2 == 5) && shuffle_opt == 1:
+ [Movement_Upgrade4]
    {MoreEvents_SetBuildACardOption(2, 4)}
    <style="Keyword"><align="center">You spun the Joystick clockwise.</align></style>
    -> FourthComponent
}
{ not(roll == 5 || roll2 == 5) && shuffle_opt == 2:
+ [Movement_Upgrade5]
    {MoreEvents_SetBuildACardOption(2, 5)}
    <style="Keyword"><align="center">You spun the Joystick counter-clockwise.</align></style>
    -> FourthComponent
}
+ [nothing]
    -> FourthComponent

=FourthComponent
<style="Body">Several switches appear on the board, each one decorated with a complex series of symbols you can't decipher.</style>
<style="Prompt">Do you switch any of them on?</style>

+ [Bonus_Upgrade0]
    {MoreEvents_SetBuildACardOption(3, 0)}
    <style="Keyword"><align="center">You switched on the space-related switch.</align></style>
    -> Final
+ [Bonus_Upgrade1]
    {MoreEvents_SetBuildACardOption(3, 1)}
    <style="Keyword"><align="center">You switched on the power-related toggle.</align></style>
    -> Final
+ [Bonus_Upgrade2]
    {MoreEvents_SetBuildACardOption(3, 2)}
    <style="Keyword"><align="center">You switched on the gold-related toggle.</align></style>
    -> Final
+ [nothing]
    -> Final
    
=Final
<style="Body">The buttons and levers of the control panel recede into the metal box as the entire device sinks into the ground. The metal harness clamps shut, likely trying to embrace whatever once existed in its grasp.
But as the metal arms link, the train starts to shudder. No, your <i><style="Keyword">Pyre</style></i> starts to rumble. Perhaps in recognition?
Before any logical thought can form, a blinding flash of light knocks you back. What's left behind is a polished, black orb, not unlike the Pyre itself.</style>
<style="Prompt">Do you take the orb?</style>
+ [TakeCard]
    >>>GIVE_REWARD: @BuildCardReward
    ->END
+ [LeaveCard]
    <style="Body">Right. Best to leave the relics of the arcane untouched. Especially those of the Pyre-shaking variety.</style>
    ->END