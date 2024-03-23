INCLUDE ../../../GlobalVariables.ink

//<font=Witch of Thebes SDF></font>

{not SMage3Seen : A small surge of energy and... But... I'm no longer on the ship! This is impossible! #chara:mage } 
{SMage3Seen : Ah! Doctor Seltis! I see you haven't found a way back home yet! #chara:dragon }
{not SMage3Seen : ->Encounter1} 
{SMage3Seen : ->Encounter2} 

=== Encounter1 ===
Teleported into my dungeon? Now that's rare magic! #chara:dragon
~SMage3Seen = true 
A dragon! But where has this cursed artifact sent me! #chara:mage
I believe introductions are in order. Welcome to the Dungeon, sorceress. I am both its master and guardian. #chara:dragon
How am I supposed to go back... I don't have time to linger here, Dragon, send me back from where I came! #chara:mage
Such impatience! I have no means to send you back home, sorceress, however... #chara:dragon
* [ Offer her your knowledge (loses <color=orange>impatient</color> )] -> Regen
* [ Steal her glasses (gains <color=yellow>shortsighted</color> )] -> Damages
    
=== Regen ===
I invite you to explore the Dungeon : its artifacts and magical scrolls might help you find your way back home... #chara:dragon
Really? You have such objects here... As a scientist, it's my duty to shed light on this knowledge. #chara:mage #changepers:clairvoyant
-> END

=== Damages ===
You're here in my domain and I set the rules. If you wish to return home, try at least to survive here... #chara:mage #changepers:shortsighted
WHAT! My glasses! Dragon! Give me back my glasses! #chara:mage
-> END

=== Encounter2 ===
That's still my goal, but the artifact is reacting strangely in this place. It's my duty to try to unravel this mystery. #chara:mage
And what's the next step in your studies? Do you think you'll manage to understand this object? #chara:dragon
Rejoice, Dragon, you'll be able to participate in scientific progress. I've realized that to activate a new aspect of its powers I'll need your gold! #chara:mage
I beg your pardon?! You'll never have my gold! #chara:dragon 
* [ Give her a false lead (loses <color=orange>impatient</color> )]  -> Regen2
* [ Steal her glasses (gains <color=yellow>shortsighted</color> )] -> Damages2
 -> END
 
 === Regen2 ===
But... proceed to the end of this floor, you might find my treasure there. If you survive... #chara:dragon #changepers:clairvoyant
-> END

=== Damages2 ===
Ha! Try to survive this floor first, you're not worthy of laying your hands on a single coin of mine. #chara:dragon #changepers:shortsighted
-> END
