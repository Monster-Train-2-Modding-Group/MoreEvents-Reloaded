=== UnitQuest ===
<style="Body"><color="red">Heph the Blacksmith</color> huddles near the train. She is clearly unaccustomed to the <shake>cold</shake>.
“I trust the <style="Keyword">Rail</style> has been smooth. Well, as smooth as it can be in times like these...”
“Errr, right. Well, I've found something of use - a couple of relics left behind by my father. I wasn't sure when you'd come by the <style="Keyword">Forge</style>, so I trekked out into this <smallcaps><b><shake>damned cold</shake></b></smallcaps> to find you. I don't have room for them both, but I figure you may have need for one. <i>Hell, I might even be able to upgrade it later if it suits you.</i>”
<style="Prompt">“So, uh, do you want one of these?”</style>
+ [Spell]
    <style="Body">Heph gives a nod of approval as she hands over the <style="Keyword">Railspike</style>.
    “Careful with that. It looks ancient, but my old man made sure it would work for a good while.”
    “And if you can get a few more of 'em - <b>4 or more will do</b> - I'll make it worth your while.”</style>
    >>>GIVE_REWARD: @ReplicatingSpellReward
	-> END
+ [Unit]
    <style="Body">Heph hands over the strange machine.
    “Odd thing that invention. But if my old man made it, it's sure to work better than any junk you'd get from the Crucible.”
    “And if you can get a few more of those guys - <b>4 or more will do</b> - I'll make it worth your while.”</style>
    >>>GIVE_REWARD: @ReplicatingExtinguishUnitReward
	-> END
+ [Leave]
    <style="Body">“Understood. Cramped train and all...”
    And just like that, Heph heads back out into the cold, cursing it under her breath. But loudly enough that you know she wants you to feel guilty...</style>
	-> END
