INCLUDE ../../GlobalVariables.ink

{not SArcher1Seen: Patron, attention à la flèche ! #chara:minion #minion:in } 
{SArcher1Seen : AIE ! PATRON ON ME TIRE DESSUS ! #chara:minion #minion:in }
{not SArcher1Seen : ->Encounter1} 
{SArcher1Seen : ->Encounter2} 

=== Encounter1 ===
Maudit diablotin ! Tu as tout fait rater ! #chara:archer
~SArcher1Seen = true 
Qui ose me tirer dessus ainsi ! Montrez-vous ! #chara:dragon #minion:out
Me voilà Dragon ! Moi Arash des Terres de l'Orient vais mettre fin à tes méfaits. #chara:archer
Bien, je ne pensais pas affronter d'archer ici, mais je ne compte pas me laisser faire ainsi ! #chara:dragon
Ne me sous-estime pas Dragon, tes misérables sbires ne pourront me vaincre. #chara:archer
C'est ce que nous verrons. #chara:dragon
-> END

=== Encounter2 ===
Malédiction ! Cet idiot à encore tout fait capoter ! #chara:archer
~SArcher1Seen = true 
Et bien Arash des Terres d'Orient ! Vous avez une sacrée manie dites-donc ! #chara:dragon #minion:out
Sauvé par votre sbire encore une fois ! La malchance me poursuit partout dans ce Donjon ! #chara:archer
Préparez-vous à en subir les conséquences ! #chara:dragon
 -> END