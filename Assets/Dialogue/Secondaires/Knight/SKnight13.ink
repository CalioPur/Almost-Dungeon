INCLUDE ../../GlobalVariables.ink

{not SKnight13Seen : Vite, vite vite... #chara:knight } 
{SKnight13Seen : Vite, vite vite... #chara:knight}
{not SKnight13Seen : ->Encounter1} 
{SKnight13Seen : ->Encounter2} 

=== Encounter1 ===
Eh bien chevalier... Pourquoi courez-vous ainsi dans les couloirs ? #chara:dragon
~SKnight13Seen = true 
En retard ! Je suis en retard Dragon !  #chara:knight
Eh bien, quelle <color=yellow>impatience</color>, vous ne pensiez tout de même pas pouvoir me fausser compagnie, chevalier. #chara:dragon
Je n'ai pas le temps, Dragon. N'essayez pas de m'empêcher de sortir, ou je devrai détruire votre donjon. #chara:knight
Patron, j'ai trouvé cet arc par terre, vous voulez que je tente de l'arrêter ? #minion:in #chara:minion

* [Viser ses jambes ($$le héros$$ perd @@@@@@10 de vitesse@@@@@@ )] -> Regen
* [Viser son corps ($$le héros$$ perd @5 pdv@ )] -> Damages
    
=== Regen ===
HAHA ! Touché ! Le voilà qui ralentit ! #chara:minion #time:-10
#minion:out
-> END

=== Damages ===
HAHA ! Touché... Il ne ralentit pas mais il va pas aller bien loin blessé comme ça... #chara:knight #damages:5
#minion:out
-> END

=== Encounter2 ===
Encore vous Vifvent... Vous ne semblez pas prêt de vous arrêter de courir... #chara:dragon
En retard ! Je suis encore plus en retard Dragon ! #chara:knight
Quelle importance, vous ne réchapperez pas de ce Donjon vivant ! #chara:dragon 
Je n'ai pas le temps, Dragon. N'essayez plus de cacher la sortie où je vais à nouveau détruire vos murs ! #chara:knight
Patron, où dois-je tirer cette fois-ci ? #chara:minion #minion:in

* [Viser ses jambes ($$le héros$$ perd @@@@@@10 de vitesse@@@@@@ )] -> Regen
* [Viser son corps ($$le héros$$ perd @5 pdv@ )] -> Damages
