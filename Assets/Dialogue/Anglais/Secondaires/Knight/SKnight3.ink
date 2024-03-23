INCLUDE ../../../GlobalVariables.ink

{not SKnight3Seen : A Dragon! Taking its head will grant me eternal glory! #chara:knight } 
{SKnight3Seen : And here comes Sir Godric again. Still trying to chop my head off, I presume? #chara:dragon }
{not SKnight3Seen : ->Encounter1} 
{SKnight3Seen : ->Encounter2} 

=== Encounter1 ===
If only I could do this too... #chara:dragon
~SKnight3Seen = true 
What do you mean, foul creature?  #chara:knight
Well... If all it took was bringing your head to town to come back covered in gold, I could give up maintaining this dungeon. #chara:dragon
I'll have yours before, Dragon, Sir Godric the invincible cannot lose against you! #chara:knight
Are you sure I can't give it a try? I could sleep in every day! #chara:dragon
You will return to the hell you belong in! #chara:knight
    -> END

=== Encounter2 ===
Always! All for glory, dear Dragon! #chara:knight
Are you sure you wouldn't settle for a drawing? #chara:dragon 
Or a paper head?
A futile attempt to sway me, Dragon. Nothing steers Sir Godric away from his objectives. #chara:knight
Well... Let's see if you change your mind once you're on my plate... #chara:dragon
 -> END
