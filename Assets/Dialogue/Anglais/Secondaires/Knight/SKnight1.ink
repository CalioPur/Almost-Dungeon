INCLUDE ../../../GlobalVariables.ink

{not SKnight1Seen : Dragon! I, Sir Grandjean, demand an audience with the master of this place. #chara:knight } 
{SKnight1Seen : Look, boss, here comes Sir Grandjean again! #chara:minion #minion:in }
{not SKnight1Seen : ->Encounter1} 
{SKnight1Seen : ->Encounter2} 

=== Encounter1 ===
Well, hero, is it you causing this commotion at my door? #chara:dragon
~SKnight1Seen = true
It is I, Dragon! I am Sir Grandjean, the greatest knight of the Vale and paladin of... #chara:knight
Didn't you come just to recite your titles? #chara:dragon
Uh... #chara:knight
Then let's fight! #chara:dragon
Alright... En garde, Dragon! #chara:knight
    -> END

=== Encounter2 ===
Since you're still here, Grandjean, I imagine you know what I'm going to tell you. #chara:dragon
Tsk... I'll have your head, Dragon! #chara:knight
Aha, I look forward to it! #chara:dragon
 -> END
