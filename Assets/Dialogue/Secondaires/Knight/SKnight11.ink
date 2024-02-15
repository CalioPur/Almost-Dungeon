INCLUDE ../../GlobalVariables.ink

{not SKnight11Seen : Fuhuhu... Enfin, ma beauté rayonne jusqu'aux yeux d'un dragon ! #chara:knight } 
{SKnight11Seen : Fuhuhu... Alors Dragon, que penses-tu de la beauté de sire Edouard Le Magnifique ? #chara:knight }
{not SKnight11Seen : ->Encounter1} 
{SKnight11Seen : ->Encounter2} 

=== Encounter1 ===
Encore un aventurier fou à lier patron ! #chara:minion minion:in
~SKnight11Seen = true 
Fuhuhu... Je comprends bien que ma beauté tienne presque de l'iréel mais de là à me traiter de fou... #chara:knight
Je ne m'intéresse pas à ce genre de choses héros, mais j'ose espérer que vous aurez plus que quelques paillettes pour m'imperssioner au combat. #chara:dragon
Mais cela va de soi Dragon, vous allez comprendre que ma beauté ne s'arrête pas à la décoration ! #chara:knight
Parfait, battons-nous dans ce cas ! #chara:dragon #minion:out
    -> END

=== Encounter2 ===
J'avoue être surpris chevalier, nous avons mené une bien belle bataille ! #chara:dragon
Fuhuhu... Bien évidemment ! Après tout, je suis exceptionnel. #chara:knight
Cette fois-ci, évitez simplement de vous mettre à chanter au beau milieu du combat ! #chara:dragon
Comment ? Mais... #chara:knight
Nous ne sommes pas dans une comédie musicale Edouard. Cessez de danser et chanter dans mon donjon ! #chara:dragon
 -> END