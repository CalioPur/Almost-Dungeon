INCLUDE ../../GlobalVariables.ink

{not SKnight1Seen : Dragon ! Moi, Sire Grandjean exige une audience avec le maître des lieux. #chara:knight } 
{SKnight1Seen : Tiens regardez patron, revoilà messire Grosjean ! #chara:minion #minion:in }
{not SKnight1Seen : ->Encounter1} 
{SKnight1Seen : ->Encounter2} 

=== Encounter1 ===
Et bien héros, est-ce vous qui provoquez ce raffut à ma porte ? #chara:dragon
~SKnight1Seen = true
C'est bien moi Dragon ! Je suis Sire Grandjean, plus grand chevalier du Val et paladin de... #chara:knight
Vous n'êtes pas venu m'énoncer vos titre avant de repartir n'est ce pas ? #chara:dragon
euh... #chara:knight
Alors battons-nous ! #chara:dragon
Bon... Et bien en garde Dragon ! #chara:knight
    -> END

=== Encounter2 ===
Perfide créature, je suis Messire Grandjean ! #chara:knight
C'est presque pareil... #chara:minion #minion:out
Silence ! Puisque vous êtes encore là Grandjean, j'imagine que vous savez ce que je vais vous dire. #chara:dragon
Tss... J'aurai votre peau Dragon ! #chara:knight
Je vous attends Chevalier ! #chara:dragon
 -> END