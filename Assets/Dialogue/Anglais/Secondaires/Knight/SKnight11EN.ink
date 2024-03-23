INCLUDE ../../../GlobalVariables.ink

{not SKnight11Seen : Fuhuhu... At last, my beauty shines even in the eyes of a dragon! #chara:knight } 
{SKnight11Seen : Fuhuhu... So, Dragon, what do you think of the beauty of Sir Edward the Magnificent? #chara:knight }
{not SKnight11Seen : ->Encounter1} 
{SKnight11Seen : ->Encounter2} 

=== Encounter1 ===
Another mad adventurer, boss! #chara:minion minion:in
~SKnight11Seen = true 
Fuhuhu... I understand that my beauty is almost unreal, but to call me mad... #chara:knight
I'm not interested in such things, hero, but I hope you have more than just looks to impress me in battle. #chara:dragon
But of course, Dragon, you'll soon realize that my beauty goes beyond mere decoration! #chara:knight
Perfect, let's fight then! #chara:dragon #minion:out
    -> END

=== Encounter2 ===
I must admit, knight, I'm surprised; we've had quite a good battle! #chara:dragon
Fuhuhu... Naturally! After all, I am exceptional. #chara:knight
Just this time, avoid bursting into song in the middle of the fight! #chara:dragon
What? But... #chara:knight
We're not in a musical, Edward. Stop dancing and singing in my dungeon! #chara:dragon
 -> END
