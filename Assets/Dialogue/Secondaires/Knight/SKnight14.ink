INCLUDE ../../GlobalVariables.ink

{not SKnight14Seen : Regardez patron, il reste encore un type étrange dans cet étage ! #chara:minion #minion:in } 
{SKnight14Seen : Père Enoch ! Quel plaisir de vous recroiser ici. #chara:dragon}
{not SKnight14Seen : ->Encounter1} 
{SKnight14Seen : ->Encounter2} 

=== Encounter1 ===
Mais quelle est cette créature, est-ce là une nouvelle épreuve ô Dieu ? #chara:knight
~SKnight14Seen = true 
Le Donjon entier est une épreuve, mon père, et j'en suis le gardien. !  #chara:dragon
Hm... On dirait que je vais à nouveau avoir besoin de mettre ce vieux corps au défi dans ce cas. #chara:knight
Wow patron ! Vous avez vu ces muscles ? Ce prêtre m'a tout l'air d'être bien plus fort que prévu ! #chara:minion
Voilà un combat qui promet d'être intéressant. En garde prêtre ! #chara:dragon #minion:out
    -> END

=== Encounter2 ===
Encore vous Dragon ?  #chara:knight
Décidément vous ne comptez pas ménager le vieil homme que je suis ?
Ce type ment comme il respire patron ! Regardez-le, il n'a pas une égratignure. #chara:minion #minion:in
Ne soyez pas ridicule, je cherche simplement revenir à l'endroit où ce maudit fantôme m'arraché et je suis prêt à tout pour ça. #chara:knight
Intéressant. Sachez, mon père, que je n'engage pas de fantômes dans le Donjon ; votre situation n'est donc pas de mon ressort. #chara:dragon
Je crains qu'il ne nous reste plus qu'à nous battre dans ce cas Dragon... #chara:knight
    -> END
