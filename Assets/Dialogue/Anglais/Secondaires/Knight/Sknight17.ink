INCLUDE ../../../GlobalVariables.ink

{not SKnight17Seen : Boss! This knight here is dangerous! Impossible to fool him! #chara:minion #minion:in} 
{SKnight17Seen : Haha! My clairvoyance never fails me! You were there, Dragon! #chara:knight}
{not SKnight17Seen : ->Encounter1} 
{SKnight17Seen : ->Encounter2} 

=== Encounter1 ===
Very well, compose yourself minion, I will take care of the matter. #chara:dragon
~SKnight17Seen = true 
Ah! Finally, the main course, I was starting to get bored! #chara:knight
You took the words right out of my mouth, knight. Finally a worthy opponent. #chara:dragon
I, Sir Pierre, blessed by the supreme vision, challenge you to a duel, Dragon. You stand no chance. #chara:knight #minion:out
* [Boost his ego ($$the hero$$ gains <color=red>courageous</color>)] -> Regen
* [Exploit his boasting ($$you$$ gain <color=red>1 hp</color>)] -> Damages
    
=== Regen ===
HA! I bet you won't even be able to take down my army to reach me! #chara:dragon
Are you challenging me? #chara:knight 
Very well, you'll see, your monsters won't be enough to stop me. 
I'll take pleasure in getting rid of them. #changepers:courageous
-> END

=== Damages ===
Well...  #chara:dragon
So I guess you'll allow me to heal myself for a moment... You wouldn't refuse a fair duel, would you?
But of course, Dragon. Please, that won't be enough against me anyway. #chara:knight #healDragon:1
-> END

=== Encounter2 ===
Sir Pierre, for once, I'm glad to see an adventurer survive my traps! #chara:dragon
Our last battle was magnificent, Dragon, I look forward to facing you again. #chara:knight
Haha! Pleasure shared, knight, but you won't win! #chara:dragon
We'll see... What are the rules this time? #chara:knight
* [Make him face your minions ($$the hero$$ gains <color=red>courageous</color>)] -> Regen2
* [Drink a potion ($$you$$ gain <color=red>1 hp</color>)] -> Damages2
    
=== Regen2 ===
Perfect, I'll face your army before you in that case. They won't last long! #chara:knight #changepers:courageous
-> END

=== Damages2 ===
Very well, take your time, we're not in a hurry. #chara:knight #healDragon:1
-> END