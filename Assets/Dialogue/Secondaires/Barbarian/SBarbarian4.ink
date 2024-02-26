INCLUDE ../../GlobalVariables.ink

{not SBarbarian4Seen : Hahaha, un Dragon, notre combat sera légendaire ! #chara:barbarian } 
{SBarbarian4Seen : Ha Dragon ! Je n'ai pas oublié cette vilaine balle que tu as osé me lancé la dernière fois ! #chara:barbarian}
{not SBarbarian4Seen : ->Encounter1} 
{SBarbarian4Seen : ->Encounter2} 

=== Encounter1 ===
Ne t'enflamme pas trop vite héros, c'est à moi de m'en charger normalement. #chara:dragon
~SBarbarian4Seen = true 
Ha ! Tes flammes ne peuvent surpasser celles qui brulent dans le coeur d'un vrai guerrier !  #chara:barbarian
Il rigole pas patron ! Regardez, on dirait presque qu'il dégage des flammes ! #chara:minion #minion:in
Enfer, il faut l'arrêter avant qu'il ne risque de mettre le feu à toute la forêt ! #chara:dragon
J'ai exactement ce qu'il nous faut patron, j'ai trouvé ces sacs de poudres, on peut les lui jeter dessus ! #chara:minion minion:in
* [Utiliser la grenade ($$le héros$$ perd <color=red>5 pdv</color> )] -> Regen
* [Utiliser le fumigène ($$le héros$$ gagne <color=blue>bigleux</color> )] -> Damages
    
=== Regen ===
Haha petite balle, pas la peine de rouler vers moi, tu ne pourras pas non plus atténuer ma rage ! #chara:barbarian
Aaaaaaaaaaaaah ! 
Méchante balle ! 
Dragon, je vais me venger !
-> END

=== Damages ===
Haha petite balle, pas la peine de rouler vers moi, tu ne pourras pas non plus atténuer ma rage ! #chara:barbarian
Qu'est ce que ! Quelle est cette fumée je ne vois plus rien !
-> END

=== Encounter2 ===
Ce cher Fenris, votre rage est-elle toujours aussi bouillonante ? #chara:dragon
Bien évidemment, et je ne fais que m'échauffer ! J'aurai ta peau Dragon ! #chara:barbarian
Bien... Heureusement que j'ai pris la liberté de me procurer plus de poudres... #chara:dragon 
* [Utiliser la grenade ($$le héros$$ perd <color=red>5 pdv</color> )] -> Regen2
* [Utiliser le fumigène ($$le héros$$ gagne <color=blue>bigleux</color> )] -> Damages2
 -> END
 
 === Regen2 ===
Aaaah ! Pas encore une de ces balles ! Tu vas le payer Dragon ! #chara:barbarian
-> END

=== Damages2 ===
Aaaah ! Pas encore une de ces balles ! Tu vas le payer Dragon ! #chara:barbarian
-> END