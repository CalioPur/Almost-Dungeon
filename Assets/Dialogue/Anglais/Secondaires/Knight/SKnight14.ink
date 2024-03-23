INCLUDE ../../../GlobalVariables.ink

{not SKnight14Seen : Look boss, there's still a strange guy on this floor! #chara:minion #minion:in } 
{SKnight14Seen : Father Enoch! What a pleasure to meet you again here. #chara:dragon}
{not SKnight14Seen : ->Encounter1} 
{SKnight14Seen : ->Encounter2} 

=== Encounter1 ===
But what is this creature, is this a new trial, oh God? #chara:knight
~SKnight14Seen = true 
The entire Dungeon is a trial, my father, and I am its guardian! #chara:dragon
Hm... It seems I will once again need to put this old body to the test then. #chara:knight
Wow boss! Did you see those muscles? This priest looks much stronger than expected! #chara:minion
This promises to be an interesting fight. On guard, priest! #chara:dragon #minion:out
    -> END

=== Encounter2 ===
You again, Dragon? #chara:knight
You certainly don't intend to spare this old man, do you?
This guy lies like he breathes boss! Look at him, not a scratch. #chara:minion #minion:in
Don't be ridiculous, I'm just trying to return to the place where that damned ghost snatched me, and I'll do anything for that. #chara:knight
Interesting. Know, my father, that I do not employ ghosts in the Dungeon; your situation is therefore not my concern. #chara:dragon
I'm afraid we have no choice but to fight then Dragon... #chara:knight
    -> END

