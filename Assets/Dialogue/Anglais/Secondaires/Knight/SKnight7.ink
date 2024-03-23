INCLUDE ../../../GlobalVariables.ink

{not SKnight7Seen : Dragon! Finally, I find you! I can fulfill my vengeance. #chara:knight } 
{SKnight7Seen : Dragon! There you are, you won't escape my vengeance once again! #chara:knight}
{not SKnight7Seen : ->Encounter1} 
{SKnight7Seen : ->Encounter2} 

=== Encounter1 ===
Hmm... You don't seem familiar to me... #chara:dragon
~SKnight7Seen = true 
The treacherous creature denies its crime! #chara:knight
But come on! At least tell me what you accuse me of! #chara:dragon
The offense is too great, Dragon, let's fight here and now! #chara:knight
But... Will you just tell me what you want from me!? #chara:dragon
    -> END

=== Encounter2 ===
You again, Dantes! I assure you, I have no idea what you're talking about! #chara:knight
Wasn't our last battle enough for you, monster?! #chara:dragon
Will you finally tell me what you accuse me of!? #chara:dragon
I'm the one asking questions, Dragon! #chara:knight
 -> END
