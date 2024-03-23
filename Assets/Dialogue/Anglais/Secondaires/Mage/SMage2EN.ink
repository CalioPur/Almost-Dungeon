INCLUDE ../../../GlobalVariables.ink

//<font=Witch of Thebes SDF></font>

{not SMage2Seen : A Dragon? Finally an infinite source of magic... My research are coming to an end! #chara:mage } 
{SMage2Seen : Dragon! I sense your magic, come out of hiding! #chara:mage }
{not SMage2Seen : ->Encounter1} 
{SMage2Seen : ->Encounter2} 

=== Encounter1 ===
I'm afraid I must indeed put an end to it... Don't take it personally. #chara:dragon
~SMage2Seen = true 
I won't let all these years of study be reduced to ashes! #chara:mage
Whatever motivations drive you to explore the Dungeon, it will take more than words to impress me, mage. #chara:dragon
Very well... Let the magic speak then, Dragon! #chara:mage
Perfect, let's see what your little tricks can do. #chara:minion
-> END

=== Encounter2 ===
Hm... Why are you so interested in my magic, Mephisto? #chara:dragon
It's simple, I've been pursuing my research for decades to achieve the most powerful of magics. #chara:mage
Interesting, but what magic is important enough for you to challenge a dragon? #chara:dragon
The one that can bring my family back from the dead! And I will set the world on fire and blood to achieve it! #chara:dragon 
Well... That's a commendable goal, too bad I can't afford to let you win. En garde, mage! #chara:dragon
-> END

