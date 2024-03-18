INCLUDE ../../GlobalVariables.ink

{not SArcher3Seen : Bonjour bonjour, Inspection Générale des Donjons ! #chara:archer } 
{SArcher3Seen : Patron ! Revoilà l'inspectrice ! #chara:minion #minion:in }
{not SArcher3Seen : ->Encounter1} 
{SArcher3Seen : ->Encounter2} 

=== Encounter1 ===
Malédiction ! L'IGD, vite mon sbire ! Ils ne doivent pas trouver nos fiches de paie ! #chara:dragon
~SArcher3Seen = true 
Pas de soucis Patron ! Trouvez un moyen de la retarder !  #chara:minion #minion:in
Ah monsieur le Dragon, nous allons pouvoir commencer l'inspection. #chara:archer #minion:out
* [Lui donner une fausse direction (perd <color=orange>exploratrice</color> )] -> Regen
* [Accuser les monstres de fraude (devient <color=red>courageuse</color> )] -> Damages
    
=== Regen ===
Merci bien, je vous retrouve dans quelques heures dans votre bureau, nous pourons discuter de ce que j'aurai trouvé... #chara:archer #changepers:none
-> END

=== Damages ===
Comment ? Vos monstres n'ont pas déclaré leurs impots ? C'est très grave monsieur ! #chara:archer
Il faut que vous les arrêtiez sur le champ ! #chara:dragon
Ils n'en sortiront pas vivants ! #chara:archer #changepers:courageux
-> END

=== Encounter2 ===
Enfer ! Encore l'IGD. Vite mon sbire ! Il  faut à nouveau cacher nos fiches de paie ! #chara:dragon
J'y vais patron ! #chara:minion
Ah monsieur le Dragon, cette fois ci je vais avoir besoin de vos fiches de paie. #minion:out
* [La diriger vers les oubliettes (perd <color=orange>exploratrice</color> )] -> Regen2
* [Accuser les monstres de fraude (devient <color=red>courageuse</color> )] -> Damages2
 -> END
 
 === Regen2 ===
Merci bien, je vous retrouve dans quelques heures dans votre bureau, nous pourons discuter de ce que je vais trouver... #chara:archer #changepers:none
-> END

=== Damages2 ===
Comment ? Vos monstres n'ont pas déclaré leurs impots ? C'est très grave monsieur ! #chara:archer #changepers:courageux
Il faut que vous les arrêtiez sur le champ ! #chara:dragon
Ils n'en sortiront pas vivants ! #chara:archer
-> END