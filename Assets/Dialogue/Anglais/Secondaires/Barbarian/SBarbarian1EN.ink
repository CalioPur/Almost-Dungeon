INCLUDE ../../../GlobalVariables.ink

{not SBarbarian1Seen : Haha, after weeks of walking from the Northern Realms, here I am finally in the Dungeon! #chara:barbarian } 
{SBarbarian1Seen : Here's that good explorer again... #chara:dragon}
{not SBarbarian1Seen : ->Encounter1} 
{SBarbarian1Seen : ->Encounter2} 

=== Encounter1 ===
Oh? A Northern explorer in my Barracks. That's an unexpected encounter. #chara:dragon
~SBarbarian1Seen = true 
A Dragon, just like in the legends! Quickly, my map! #chara:barbarian
Pardon me? A map? What map? #chara:dragon
My map! Look, during my journey I found a magical map from your land! #chara:barbarian
A magical map! Hmm, that could be useful... #chara:dragon

* [Steal the map (loses @@@@@explorer@@@@@ )] -> Regen
* [Add false information to it (loses @@@@@@10 speed@@@@@@ )] -> Damages
    
=== Regen ===
What? MY MAP! Give it back, Dragon! I warn you, this won't end well for you! #chara:barbarian #changepers:none
-> END

=== Damages ===
What? You say there are secret chambers in the dungeon? #chara:barbarian
I should take my time then!
Exactly, take your time... Move at your own pace. #chara:dragon #time:-10
-> END

=== Encounter2 ===
Here you are again Dragon! What luck, I just got my hands on a second map... #chara:barbarian
Again!? How is that even possible... Who gave him another map? #chara:dragon 
Boss, it was you who filled the chests last time... #chara:minion #minion:in
Ah! No need to try to {Regen : steal,} {Damages : taint,} it this time! You won't get me! #chara:barbarian #minion:out
* [Burn the map! (hero loses @@@@@explorer@@@@@ )] -> Regen2
* [Make the map unreadable! (hero loses @@@@@@10 speed@@@@@@ )] -> Damages2
 -> END
 
 === Regen2 ===
What? MY MAP! YOU BURNED IT! #chara:barbarian 
This time you've gone too far! I'll have your head, Dragon! #changepers:none
-> END

=== Damages2 ===
Wait, what did you throw at me there? #chara:barbarian
Uuuuu... Troll drool... My map is practically unreadable, great job with that! #time:-10
-> END
