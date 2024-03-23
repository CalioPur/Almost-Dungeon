INCLUDE ../../../GlobalVariables.ink

{not SKnight10Seen : And here comes a new knight, motivated by glory and wealth. #chara:dragon } 
{SKnight10Seen : You know, Sir Lume, you'd have better chances of finding a princess by leaving my place at this hour than by continuing to wake me up at this time! #chara:dragon }
{not SKnight10Seen : ->Encounter1} 
{SKnight10Seen : ->Encounter2} 

=== Encounter1 ===
I beg your pardon, Dragon? I'm not motivated by such a lowly goal. #chara:knight
~SKnight10Seen = true 
That's intriguing... Very well, I admit I underestimated you... #chara:dragon
Haha, but you're right, Dragon! I, Sir Lume, am only motivated by the idea of saving the damsels in distress you dare to capture! #chara:knight
I take back what I said... I don't know who made you believe such nonsense. #chara:dragon
Come on, don't lie, Dragon, everyone knows that creatures like you enjoy that kind of thing... #chara:knight
I only enjoy gold coins and sleeping in, knight... So enough chit-chat, let's fight. #chara:dragon
    -> END

=== Encounter2 ===
Haha, your lies don't fool me, Dragon, I know they're being held prisoner deep in your dungeon. #chara:knight
No! I don't do that kind of thing, knight, the only time it happened, the princess gave me the only scar on my old body! #chara:dragon
Wow... That must hurt indeed. #chara:knight
See? Now stop telling your nonsense and let's fight! #chara:dragon
Alright... Alright, I'm coming... #chara:knight
 -> END
