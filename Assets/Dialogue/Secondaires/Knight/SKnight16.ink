INCLUDE ../../GlobalVariables.ink

{not SKnight15Seen : Enfin, un dragon ! Je vais enfin obtenir ce qui me revient de droit ! #chara:knight} 
{SKnight15Seen : Père Enoch ! Quel plaisir de vous recroiser ici. #chara:dragon}
{not SKnight15Seen : ->Encounter1} 
{SKnight15Seen : ->Encounter2} 

=== Encounter1 ===
Hmm... Pour une fois qu'un héros semble heureux de me voir... #chara:dragon
~SKnight14Seen = true 
Tu ne crois pas si bien dire Dragon ! #chara:knight
Tes os me permettront de lever une armée et de prendre le Val.
Je ne pense pas que les chevaliers seront très heureux d'apprendre ça... #chara:dragon
Oh mais je ne compte pas demander leur avis, ces imbéciles seront bientôt de l'histoire ancienne ! #chara:knight
Voilà qui ne m'arrange pas vraiment ! #chara:dragon
Vraiment ? Les dragons ne sont-ils pas les ennemis jurés des Chevaliers ? #chara:knight
Vous n'y êtes pas, ce sont mes proies... #chara:dragon
Ils viennent en quête d'or et je les dépouille sans sortir de chez moi ! 
Et vous allez vous-aussi en faire les frais !

    -> END

=== Encounter2 ===
Ha... haaaaaa... #chara:knight
Dragon, cette fois-ci j'aurai l'un de tes os !  
Encore vous Duncan ? Décidément, vous avez une fixette sur mes os ! #chara:dragon
Bien sûr que non ! #chara:knight
L'armée de morts-vivants que je dois lever ne s'éveillera que grâce à vos os ! 
Votre objectif ne m'intéresse, petit nécromancien. 
Je ne peux pas vous laisser les atteindre. #chara:dragon
Mais ne vous inquiétez pas Dragon, je ne comptais pas vous laisser vivre non plus... #chara:knight
    -> END