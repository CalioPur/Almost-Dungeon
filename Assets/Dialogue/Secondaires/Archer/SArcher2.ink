INCLUDE ../../GlobalVariables.ink

{not SArcher2Seen : Quelle galère, ça fait des semaines que je suis dans ce donjon... #chara:archer } 
{SArcher2Seen : Quel enfer... Encore des couloirs et toujours pas de potion en vue. #chara:archer }
{not SArcher2Seen : ->Encounter1} 
{SArcher2Seen : ->Encounter2} 

=== Encounter1 ===
Patron ! J'ai trouvé un archer à moitié crevé ici ! #chara:minion #minion:in
~SArcher2Seen = true 
Alors archer ? Les temps sont durs ? Dommage que vous n'ayez pas cette fabuleuse potion de vie. #chara:dragon
Pitié Dragon, je vous en prie, laissez moi la potion, je vous jure que j'irai me battre de toutes mes forces. #chara:archer #minion:out
* [Lui donner la potion (devient <color=red>courageux</color> )] -> Regen
* [Boire la potion ($$vous$$ soigne <color=red>1 pdv</color> )] -> Damages
    
=== Regen ===
Aaah... Merci Dragon... Me voilà de nouveau d'attaque ! #chara:archer #changepers:courageux
-> END

=== Damages ===
Tsss... Créature sans coeur, je vais t'abattre. #chara:archer #dragonHeal:1
-> END

=== Encounter2 ===
Patron ! Encore l'archer crevé #chara:minion #minion:in
Haha... Alors archer, toujours en rade de potions de soin ?  #chara:dragon #minion:out
Pitié Dragon, je vous en prie, laissez moi la potion cette fois ci, je vous jure que vous aurez le droit à de vrai combats. #chara:archer 
* [Lui donner la potion (devient <color=red>courageux</color> )] -> Regen
* [Boire la potion (vous soigne <color=red>1 pdv</color> )] -> Damages
 -> END
