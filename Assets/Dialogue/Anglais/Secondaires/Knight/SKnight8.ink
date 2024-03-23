INCLUDE ../../../GlobalVariables.ink

{not SKnight8Seen : Ha! Dragon! Get ready for a legendary battle! #chara:knight } 
{SKnight8Seen : Dragon! I hope you're awake this time! #chara:knight}
{not SKnight8Seen : ->Encounter1} 
{SKnight8Seen : ->Encounter2} 

=== Encounter1 ===
$zzzzz...zzzz...zzzz...zzz...$ #chara:dragon
~SKnight8Seen = true 
But, goodness! Is this scoundrel sleeping!? #chara:knight
$zzzzz...zzzz...zzzz...zzz...$ #chara:dragon
Good heavens! Watch out, his arms are moving! He's attacking! #chara:knight
$zzzz...zzz....build...dungeon....zzzz...$ #chara:dragon
    -> END

=== Encounter2 ===
$zzzzz...zzzz...zzzz...zzz...$ #chara:dragon
BUT! This can't be true! He's still sleeping? #chara:knight
$zzzz...zzz....build...dungeon....zzzz...$ #chara:dragon
There he goes attacking while sleeping again! #chara:knight
 -> END
