INCLUDE ../../GlobalVariables.ink

{not SKnight7Seen : Ah, Dragon ! Enfin je te retrouve ! Je vais pouvoir accomplir ma vengeance. #chara:knight } 
{SKnight7Seen : Ah, Dragon ! Te revoilà, tu n'échapperas pas à ma vengeance une fois de plus ! #chara:knight}
{not SKnight7Seen : ->Encounter1} 
{SKnight7Seen : ->Encounter2} 

=== Encounter1 ===
Hmm... Vous ne me dites pourtant rien... #chara:dragon
~SKnight7Seen = true 
La perfide créature nie son crime !  #chara:knight
Mais enfin ! Dites-moi au moins ce que vous me reprochez ! #chara:dragon
L'affront est trop grand Dragon, battons-nous sur le champ ! #chara:knight
Mais ! Vous allez me dire ce que vous me voulez oui !? #chara:dragon
    -> END

=== Encounter2 ===
Encore vous Dantes ! Je vous assure que je ne vois pas de quoi vous parlez ! #chara:knight
Notre dernier combat ne t'a pas suffi, monstre ?! #chara:dragon
Mais allez-vous enfin me dire ce que vous me reprochez !? #chara:dragon
C'est moi qui pose les questions, Dragon ! #chara:knight
 -> END