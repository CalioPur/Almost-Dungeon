INCLUDE ../../../GlobalVariables.ink

{not SKnight12Seen : Finally in the crypt, prepare yourself, Dragon! Sir Jacques and his divine lance will add a new trophy to their collection. #chara:knight } 
{SKnight12Seen : Gawr, gawr! I am the bos... Dragon! Hero, I will defeat you! #chara:minion minion:in }
{not SKnight12Seen : ->Encounter1} 
{SKnight12Seen : ->Encounter2} 

=== Encounter1 ===
Hello, Mr. Knight! Are you here for a fight? #chara:minion minion:in
~SKnight12Seen = true 
Ha! Of course, little creature! Call your master so we can face off! #chara:knight
Haha! The boss went shopping, today I'm the boss! #chara:minion
What? The dragon is absent? I refuse to face a mere imp! #chara:knight
Minion! Why didn't you wake me up, why are you with the hero?! #chara:dragon
Uh... hehe, sorry boss... I wanted to fight this hero... #chara:minion
Tss... Stick to your role, little minion, and watch how I get rid of this knight! #chara:dragon #minion:out
    -> END

=== Encounter2 ===
Deceitful minion! I know it's you behind that mask! #chara:knight
You've done it again, minion? You know very well that it's my job to face the heroes! #chara:dragon
But boss! I want to fight heroes too! #chara:minion
Well, then, what would be left for me to do? #chara:dragon
You're right, boss... #chara:minion
Alright... Are you done? Can we fight now, Dragon? #chara:knight #minion:out
 -> END
