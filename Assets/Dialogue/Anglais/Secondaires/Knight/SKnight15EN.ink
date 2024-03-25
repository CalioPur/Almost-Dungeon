INCLUDE ../../../GlobalVariables.ink

{not SKnight15Seen : Finally, the underground forest! If the legends are true, I'm just steps away from the artifact! #chara:knight} 
{SKnight15Seen : I... I must obtain this artifact at all costs! #chara:dragon}
{not SKnight15Seen : ->Encounter1} 
{SKnight15Seen : ->Encounter2} 

=== Encounter1 ===
Well, knight... Are you attempting to plunder my Dungeon? #chara:dragon
~SKnight15Seen = true 
Of course, Dragon. It's my objective, and you can't stop me. #chara:knight
And what makes my treasure so valuable to you? #chara:dragon
Among your treasure there is a magical book known to contain divine knowledge... I want to seize it. #chara:knight
Hmm... Interesting. Let's see if you are worthy of it, knight! #chara:dragon
Let's fight, and you'll explain to me what you intend to do with it.
I'm waiting for you, Dragon! #chara:knight
    -> END

=== Encounter2 ===
I... I must obtain this artifact at all costs! #chara:knight
Always motivated to plunder my Dungeon, I see... #chara:dragon
I can't afford to make mistakes, Dragon. I've sacrificed too much to get here; I must retrieve this book! #chara:knight
You still haven't told me why... #chara:dragon
It's the only way to see my family again. The gods' books must contain a way to escape death! #chara:knight
Hmm... I see... But whatever the contents of this book, you'll have to get through me before to read it! #chara:dragon
Then I'll kill you, Dragon! #chara:knight 
    -> END
