INCLUDE ../../GlobalVariables.ink

//<font=Witch of Thebes SDF></font>

{not SMage1Seen : Patron ! Regardez celui-là, qu'est ce qu'il fait avec ses bouquins et son carnet ! #chara:minion #minion:in } 
{SMage1Seen : Patron ! Encore le type terrifiant ! #chara:minion #minion:in }
{not SMage1Seen : ->Encounter1} 
{SMage1Seen : ->Encounter2} 

=== Encounter1 ===
Je crois bien qu'il s'agit d'un étudiant étranger, allons voir ce qu'il veut. #chara:dragon
~SMage1Seen = true 
°°Bonjour, je suis Melchior, étudiant en magie au Collège de l'Est°°  #chara:mage
... #chara:dragon
Patron..  Qu'est ce qu'il dit ? J'ai rien compris ! #chara:minion
Attention ! Il approche ! #chara:dragon
°°Vous êtes le maître des lieux, n'est ce pas ? N'ayez pas peur, je voudrais simplement savoir où sont les cabinets.°° #chara:mage
Patron ! J'ai peur ! Il veut nous lancer un sort ! #chara:minion
* [ °°L'attaquer (perd @@preureux@@ )] -> Regen
* [ °°Lui montrer le chemin (perd @@@@clairvoyant@@@@ )] -> Damages
    
=== Regen ===
°°AAAAAH ! Mais vous êtes fou ! Je veux juste remplir mon thermos !°° #chara:mage #changepers:peureux #minion:out
-> END

=== Damages ===
°°Ah ! Vous êtes bien aimable, merci pour l'information.°° #chara:mage #changepers:clairvoyant #minion:out
-> END

=== Encounter2 ===
Pas de panique, je vais essayer de communiquer cette fois ci ! #chara:dragon
°°Bonjour ! C'est encore moi Melchior, je cherche votre crypte, vous pourriez m'aider ?°° #chara:mage
Au secours ! Patron ! J'ai l'impression qu'il essaie d'aspirer mon âme ! #chara:minion
Le voilà qui se rapproche à nouveau ! #chara:dragon 
Vite patron ! Faites un truc ! #chara:minion
* [ °°L'attaquer (perd @@preureux@@ )] -> Regen2
* [ °°Lui montrer le chemin (perd @@@@clairvoyant@@@@ )] -> Damages2
 -> END
 
 === Regen2 ===
°°AAAAAH ! Mais vous êtes fou ! Je cherche juste à finir mon rapport de stage maudit lézard !°° #chara:mage #changepers:peureux #minion:out
-> END

=== Damages2 ===
°°Ah ! Vous êtes bien aimable, merci pour l'information. Je vous laisse alors...°° #chara:mage #changepers:peureux #minion:out
-> END
