INCLUDE ../../GlobalVariables.ink

{not SKnight6Seen : Grands Dieux... Enfin dans le Donjon, après toutes ces années de recherche ! #chara:knight } 
{SKnight6Seen : Tiens... Gabriel, toujours en quête de mon amitié ? #chara:dragon}
{not SKnight6Seen : ->Encounter1} 
{SKnight6Seen : ->Encounter2} 

=== Encounter1 ===
Laissez-moi deviner la suite, ces années de recherches vont enfin aboutir en gloire et montagnes d'or ? #chara:dragon
~SKnight6Seen = true 
Oh, mais non mon bon Dragon. Je ne cherche pas votre argent mais votre amitié ! #chara:knight
Je vous demande pardon ? #chara:dragon
Ecoutez, un slime érudit m'a un jour raconté s'être lié d'amitié avec un dragon, je souhaite faire comme lui. #chara:dragon
Balivernes ! Vous n'échapperez pas au combat avec ces belles paroles #chara:dragon
    -> END

=== Encounter2 ===
Patron ! C'est quoi l'amitié ? Est ce que ça se mange ? #chara:minion #minion:in
Ne l'écoute pas mon sbire, tout ce qui sort de sa bouche ne sont que des idioties. #chara:dragon #minion:out
Vous avez peut être raison Dragon... Mais je n'abandonne pas... #chara:knight
Alors en garde ! #chara:dragon
 -> END