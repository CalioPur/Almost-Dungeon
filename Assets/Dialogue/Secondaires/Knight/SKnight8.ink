INCLUDE ../../GlobalVariables.ink

{not SKnight8Seen : Ha ! Dragon ! Prépare-toi pour un combat de légendes ! #chara:knight } 
{SKnight8Seen : Dragon ! J'espère que tu es réveillé cette fois-ci ! #chara:knight}
{not SKnight8Seen : ->Encounter1} 
{SKnight8Seen : ->Encounter2} 

=== Encounter1 ===
$zzzzz...zzzz...zzzz...zzz...$ #chara:dragon
~SKnight8Seen = true 
Ma ma parole ! Ce faquin dort !?  #chara:knight
$zzzzz...zzzz...zzzz...zzz...$ #chara:dragon
Sacrebleu ! Attention, ses bras bougent ! Il attaque ! #chara:knight
$zzzz...zzz....construire...donjon....zzzz...$ #chara:dragon
    -> END

=== Encounter2 ===
$zzzzz...zzzz...zzzz...zzz...$ #chara:dragon
MAIS ! Ce n'est pas vrai ! Le voilà qui dort encore ? #chara:knight
$zzzz...zzz....construire...donjon....zzzz...$ #chara:dragon
Le revoilà qui attaque en dormant ! #chara:knight
 -> END