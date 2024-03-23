INCLUDE ../../../GlobalVariables.ink

{not SBarbarian2Seen : A huge lizard, I've never seen one this big! #chara:barbarian } 
{SBarbarian2Seen : Ah! Here's the liza... #chara:barbarian}
{not SBarbarian2Seen : ->Encounter1} 
{SBarbarian2Seen : ->Encounter2} 

=== Encounter1 ===
Well... I think there's been a misunderstanding... #chara:dragon
~SBarbarian2Seen = true 
Oh really? You're not a giant lizard? #chara:barbarian
Of course not, little barbarian, I'm the Dragon, guardian of the Dungeon! #chara:dragon
The Dungeon!? But the sign said "Don Juan"! #chara:barbarian
Goodness gracious, you're completely blind, my poor friend! #chara:dragon
-> END

=== Encounter2 ===
Dragon! I am a Dragon! #chara:dragon
Oh... #chara:barbarian
I assure you, barbarian, you're as blind as a mole. #chara:dragon
Boss? Are you sure you don't want me to give him magical glasses? #chara:minion #minion:in
He's kind of pitiful right now...
A TALKING FROG! #chara:barbarian #minion:out
-> END