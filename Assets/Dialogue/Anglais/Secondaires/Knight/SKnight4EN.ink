INCLUDE ../../../GlobalVariables.ink

{not SKnight4Seen : But where am I? This wasn't the path to the Vale... #chara:knight } 
{SKnight4Seen : OH NO, NOT YOU AGAIN! #chara:knight}
{not SKnight4Seen : ->Encounter1} 
{SKnight4Seen : ->Encounter2} 

=== Encounter1 ===
What do I see, an Adventurer lost in the Dungeon? #chara:dragon
~SKnight4Seen = true 
A DRAGON! #chara:knight
Yes, yes, I know... it's impressive. #chara:dragon
I'm sorry, my friend, but you see, my jaw is itching, and I would like to bite into something to relieve it... #chara:dragon
What? No, please noble Dragon, don't devour me, I swear I taste very bad. #chara:knight

* [Bite into the adventurer's bag (heals $$you$$ @2 hp@ )] -> Regen
* [Bite into the adventurer ($$the hero$$ loses @10 hp@ )] -> Damages
    
=== Regen ===
BUT! MY BAG! My food! You'll pay for this, Dragon! #chara:knight #healDragon:2
-> END

=== Damages ===
OUCH! I'll get revenge, Dragon! You'll pay! My poor arm... #chara:knight #damages:10
-> END

=== Encounter2 ===
Wait, Sir Louis... I'm sorry for last time... #chara:dragon
Are you out of your mind!? #chara:knight
You bit {Regen: MY bag} {Damages: MY arm!} last time and you think you can get away with apologies? 
Well, you'll laugh, but my jaw is still itching... #chara:dragon 
Oh no... #chara:knight
* [Bite into the adventurer's bag ($$you$$ heal @2 hp@ )] -> Regen2
* [Bite into the adventurer ($$the hero$$ loses @10 hp@ )] -> Damages2
 -> END
 
 === Regen2 ===
BUT! MY BAG! {Regen: AGAIN!} My food! You'll pay for this, Dragon! #chara:knight #healDragon:2
-> END

=== Damages2 ===
OUCH! I'll get revenge, Dragon! You'll pay! {Damages: Again, my arm!} My poor arm... #chara:knight #damages:10
-> END
