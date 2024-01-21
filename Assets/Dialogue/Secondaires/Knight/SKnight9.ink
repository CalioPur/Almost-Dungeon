INCLUDE ../../GlobalVariables.ink

{not SKnight9Seen : Patron ! J'ai envoyé des araignées effrayer le héros dans les escalier ! #chara:minion #minion:in} 
{SKnight9Seen : Patron ! Revoilà l'arcano... l'arphobe ! Euh... L'arachnophobe ! #chara:minion #minion:in}
{not SKnight9Seen : ->Encounter1} 
{SKnight9Seen : ->Encounter2} 

=== Encounter1 ===
Parfait nous allons pouvoir en profiter #chara:dragon
~SKnight9Seen = true 
L'embuscade est prête, que voulez-vous que les araignées fassent ? #chara:minion

* [Voler ses bottes (perd <color=lime>10 de vitesse</color> )] -> Regen
* [Voler ses potions ($$vous$$ soigne <color=red>1 pdv</color> )] -> Damages
    
=== Regen ===
Noooon ! Au secours ! Des araignées géantes ! Elles ont pris mes bottes ! #chara:knight 
Haha ! Le voilà qui détale patron ! #chara:minion 
Tout ça pour quelques petites araignées géantes ! #time:-10
-> END

=== Damages ===
Noooon ! Au secours ! Des araignées géantes ! Elles ont pris mes potions ! #chara:knight 
Haha ! Le voilà qui détale patron ! #chara:minion 
Tout ça pour quelques petites araignées géantes ! #healDragon:1
-> END

=== Encounter2 ===
Ne va pas me l'effrayer cette fois-ci, il risque d'être plus dur à attraper ! #chara:dragon 
Vous inquiétez pas patron, j'ai envoyé des spécialistes cette fois-ci ! Qu'est ce qu'on lui vole ? #chara:minion
* [Voler ses bottes (perd <color=lime>10 de vitesse</color> )] -> Regen2
* [Voler ses potions ($$vous$$ soigne <color=red>1 pdv</color> )] -> Damages2

=== Regen2 ===
Noooon ! Au secours ! Des rats géants ! Ils ont pris mes bottes ! #chara:knight 
Haha ! Le voilà qui détale patron ! #chara:minion 
Tout ça pour quelques gros rats !  #minion:out #time:-10
-> END

=== Damages2 ===
Noooon ! Au secours ! Des rats géants ! Ils ont pris mes potions ! #chara:knight 
Haha ! Le voilà qui détale patron ! #chara:minion 
Tout ça pour quelques gros rats !  #minion:out #healDragon:1
-> END