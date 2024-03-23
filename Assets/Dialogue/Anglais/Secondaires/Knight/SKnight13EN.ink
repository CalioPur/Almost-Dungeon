INCLUDE ../../../GlobalVariables.ink

{not SKnight13Seen : Quick, quick, quick... #chara:knight } 
{SKnight13Seen : Quick, quick, quick... #chara:knight}
{not SKnight13Seen : ->Encounter1} 
{SKnight13Seen : ->Encounter2} 

=== Encounter1 ===
Well, knight... Why are you running through the corridors like that? #chara:dragon
~SKnight13Seen = true 
Late! I'm late, Dragon! #chara:knight
Well, what <color=yellow>impatience</color>, did you really think you could give me the slip, knight? #chara:dragon
I don't have time, Dragon. Don't try to stop me from leaving, or I'll have to destroy your dungeon. #chara:knight
Boss, I found this bow on the ground, do you want me to try to stop him? #minion:in #chara:minion

* [Aim for his legs ($$the hero$$ loses <color=purple>10 speed</color>)] -> Regen
* [Aim for his body ($$the hero$$ loses <color=red>5 hp</color>)] -> Damages
    
=== Regen ===
HAHA! I got him ! He's slowing down! #chara:minion #time:-10
#minion:out
-> END

=== Damages ===
HAHA! He's hit... He's not slowing down, but he won't get far injured like that... #chara:knight #damages:5
#minion:out
-> END

=== Encounter2 ===
You again, Sir Vifvent... You don't seem to be stopping anytime soon... #chara:dragon
Late! I'm even "later" now, Dragon! #chara:knight
Does it even matter? You won't escape this dungeon alive! #chara:dragon 
I don't have time, Dragon. Don't try to hide the exit or I'll destroy your walls again! #chara:knight
Boss, where should I aim this time? #chara:minion #minion:in

* [Aim for his legs ($$the hero$$ loses <color=purple>10 speed</color>)] -> Regen
* [Aim for his body ($$the hero$$ loses <color=red>5 hp</color>)] -> Damages

