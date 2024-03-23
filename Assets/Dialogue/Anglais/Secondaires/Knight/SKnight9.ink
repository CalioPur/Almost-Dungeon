INCLUDE ../../../GlobalVariables.ink

{not SKnight9Seen : Boss! I sent spiders to scare the hero on the stairs! #chara:minion #minion:in} 
{SKnight9Seen : Boss! Here comes the arcano... the arphobe! Uh... The arachnophobe! #chara:minion #minion:in}
{not SKnight9Seen : ->Encounter1} 
{SKnight9Seen : ->Encounter2} 

=== Encounter1 ===
Perfect, we can take advantage of this. #chara:dragon
~SKnight9Seen = true 
The ambush is ready, what do you want the spiders to do? #chara:minion

* [Steal his boots ($$the hero$$ loses <color=lime>10 speed</color> )] -> Regen
* [Steal his potions (heals $$you$$ <color=red>1 hp</color> )] -> Damages
    
=== Regen ===
Nooo! Help meeee! Giant spiders! They took my boots! #chara:knight 
Haha! He's running away, boss! #chara:minion 
All this for a few giant spiders! #time:-10
-> END

=== Damages ===
Nooo! Help meeee! Giant spiders! They took my potions! #chara:knight 
Haha! He's running away, boss! #chara:minion 
All this for a few giant spiders! #healDragon:1
-> END

=== Encounter2 ===
Don't scare him this time, he might be harder to catch! #chara:dragon 
Don't worry boss, I sent specialists this time! What are we stealing from him? #chara:minion
* [Steal his boots ($$the hero$$ loses <color=lime>10 speed</color> )] -> Regen2
* [Steal his potions (heals $$you$$ <color=red>1 hp</color> )] -> Damages2

=== Regen2 ===
Nooo! Help meeeee! Giant rats! They took my boots! #chara:knight 
Haha! He's running away, boss! #chara:minion 
All this for a few big rats!  #minion:out #time:-10
... #chara:dragon
-> END

=== Damages2 ===
Nooo! Help meeeee! Giant rats! They took my potions! #chara:knight 
Haha! He's running away, boss! #chara:minion 
All this for a few big rats!  #minion:out #healDragon:1
... #chara:dragon
-> END
