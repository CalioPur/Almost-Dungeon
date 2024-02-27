INCLUDE ../../GlobalVariables.ink

{not SKnight3Seen : Un Dragon ! Prendre sa tête m'assurera une gloire éternelle ! #chara:knight } 
{SKnight3Seen : Et revoici Sire Godric. Toujours en quête d'une tête de Dragon ? #chara:dragon }
{not SKnight3Seen : ->Encounter1} 
{SKnight3Seen : ->Encounter2} 

=== Encounter1 ===
Si seulement l'inverse était aussi vrai... #chara:dragon
~SKnight3Seen = true 
Que veux-tu dire, perfide créature ?  #chara:knight
Et bien... S'il me suffisait d'amener votre tête en ville pour revenir couvert d'argent, je pourrais arrêter d'entretenir ce donjon. #chara:dragon
J'aurai la tienne avant Dragon, Sire Godric l'invincible ne peut perdre contre toi ! #chara:knight
Vous êtes certain que je ne peux pas essayer ? Je pourrais faire la grasse matinée tous les jours ! #chara:dragon
Tu vas retourner dans l'enfer où tu es né, Dragon ! #chara:dragon
    -> END

=== Encounter2 ===
Toujours ! Tout pour la gloire cher Dragon ! #chara:knight
Vous êtes certain que vous ne voulez pas vous contenter d'un dessin ? #chara:dragon 
Ou d'une tête en papier ? 
Une tentative futile de m'amadouer Dragon, personne ne détourne sire Godric de ses objectifs #chara:knight
Bon... Eh bien voyons si vous changerez d'avis une fois dans mon assiette... #chara:dragon
 -> END