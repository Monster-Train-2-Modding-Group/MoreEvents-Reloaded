EXTERNAL CardsAvailableWithFilterMinCount(string, number)
EXTERNAL CardsAvailableWithFilter(string)

=== SpellMerge ===
<style="Body">A <color="blue"><shake>pillar of light</shake></color> spews strange fragments into the air making the way ahead dangerous. The Boneshaker grinds to a halt on its own, perhaps recognizing the odd power emanating from the pillar.
The air is stale, imperceptible even. No longer stinging with the outer chill, but also devoid of life entirely. You feel the urge to hold your hand to the light, unsure of what may occur, but something in the back of your mind urges caution.
<align="center">Oh, but the light does beckon.</style></align>
<style="Prompt">And do you listen?</style>

+ { CardsAvailableWithFilterMinCount("OnlySpells", 2) } [Accept]
    <style="Body">Entranced, you reach into the unknown power and the light consumes you. Flashes of Hellborne and Humans and those of Heaven flicker past in this strange pocket realm. As if refocusing it, their lifeforce can be felt like molten brands in your head.
    And looming beyond it all, a booming darkness unlike anything you've ever felt. A void of life and death and goodness. The Divinity.
    When you pull your hand out, a remnant of the all-powerful Divinity comes with you.
    <align="center">You know now, you are not alone.</align></style>
	-> END
+ { CardsAvailableWithFilter("OnlySpells") } [Duplicate]
    <style="Body">Some part of your subconscious wills you away from the light. It is powerful but unknown.
    Tossing some of your own power into the pillar fragments it into smaller pieces, imbuing each one with some of the light within.</style>
	-> END
+ [Leave]
    -> END