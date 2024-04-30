INCLUDE ../../../GlobalVariables.ink

{not SKnight2Seen : Just one scale from the Dragon, just one to heal the villagers. #chara:knight } 
{SKnight2Seen : Sir Artorius, still determined to steal one of my scales? #chara:dragon }
{not SKnight2Seen : ->Encounter1} 
{SKnight2Seen : ->Encounter2} 

=== Encounter1 ===
One must first manage to tear it from me, knight. #chara:dragon
~SKnight2Seen = true 
Ah! Dragon, you were here! #chara:knight
Hard not to hear you coming given the noise your armor makes, Knight. #chara:dragon
Boss! Boss! This one doesn't seem too bright, can I bite him? #chara:minion #minion:in
No, we'll show him what it costs to play the hero! #chara:dragon #minion:out
For my people, I shall prevail! #chara:knight
    -> END

=== Encounter2 ===
I would give my life to save my people, Dragon! #chara:knight
Good, I've been wanting to get my hands on your ritual armor, it will make an excellent trophy. #chara:dragon
You won't have me, Dragon, I'll defeat you. #chara:knight
Very well, knight, entertain me enough and I might give you what you seek. #chara:dragon
 -> END

=== Rencontre3 ===
Sire Artorius... #chara:dragon
For once, I admit defeat. You have indeed exceeded my expectations.
So, honor your deal, Dragon! Save my village! #chara:knight
You're not really in a position to force my hand, Artorius. #chara:dragon
But very well!
Here, take this scale.
And now I'll take pleasure in escorting you out of here myself. #unlockAchievement:REDEMPTION
 -> END