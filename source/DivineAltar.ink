EXTERNAL CardsAvailableForFusion()

=== DivineAltar ===
~ temp roll = RANDOM(1, 5)
~ temp roll2 = RANDOM(1, 5)

- (check_second)
{ roll2 == roll:
    ~ roll2 = RANDOM(1, 5)
    -> check_second
}

<style="Body">Beyond the celestial archway lies a shattered ivory and brass chamber. At its center, an ancient Divine Altar <bigshake>roars</bigshake> to life, its focus crystals flaring with residual temple light.

A <mystical>mysterious whisper</mystical> echoes from within the chamber:

<whisper>"When the Divine Temples fell, their secrets did not vanish-they just scattered. This altar still remembers how to weave two souls into one, or forge raw divinity into a spell. The fire awaits"</whisper></style>

<style="Prompt">Do you dare invoke its power?</style>

{ CardsAvailableForFusion():
+  [EngagePact]
    <style="Body">The two units step onto the altar. Rings of oxidized metal grind into motion, aligning focus lenses overhead until a single lance of auric light pierces the chamber.
	
	One form dissolves into brilliant, liquid embers that flow steadily across the floor conduits, pouring directly into the heart of the survivor.
	
	The remaining unit draws a sharp, resonant breath, its eyes now burning with a twin flame.</style>
	-> END
}
{ roll == 1 || roll2 == 1:
+ [PurgePact]
	-> ending
}
{ roll == 2 || roll2 == 2:
+ [SeekPact]
	-> ending
}
{ roll == 3 || roll2 == 3:
+  [ThricePact]
	-> ending
}
{ roll == 4 || roll2 == 4:
+  [TruePact]
	-> ending
}
{ roll == 5 || roll2 == 5:
+  [ValuePact]
	-> ending
}
+[Leave]
	<style="Body">You step away from the altar. The focus crystals dim to a dull embers, and the brass gears grind to a quiet halt.
	
	<whisper>"A cautious soul,"</whisper> the voice sighs, fading into the cold chamber. <whisper>"The altar waits, as it always has."</whisper></style>
	-> END
	
- (ending)
<style="Body">You place the spell upon the altar. Brass needles snap forward, etching liquid gold directly onto the spell as ancient temple power floods the parchment.

The text flares blindingly, permanently rewritten by divine law.

<whisper>"A covenant carved in fire,"</whisper> the voice hums, fading into the chamber. <whisper>"Speak the words, and let the heavens answer."</whisper></style>
-> END