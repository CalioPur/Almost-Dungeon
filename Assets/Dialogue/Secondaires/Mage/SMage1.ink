INCLUDE ../../GlobalVariables.ink

//<font=Witch of Thebes SDF></font>

{not SMage1Seen : Patron ! Regardez celui-là, qu'est ce qu'il fait avec ses bouquins et son carnet ! #chara:minion minion:in } 
{SMage1Seen : Patron ! Encore le type terrifiant ! #chara:minion minion:in }
{not SMage1Seen : ->Encounter1} 
{SMage1Seen : ->Encounter2} 

=== Encounter1 ===
Je crois bien qu'il s'agit d'un étudiant étranger, allons voir ce qu'il veut. #chara:dragon
~SMage1Seen = true 
<font=Witch of Thebes SDF>Bonjour, je suis Melchior, étudiant en magie au Collège de l'Est</font>  #chara:mage
... #chara:dragon
Patron..  Qu'est ce qu'il dit ? J'ai rien compris ! #chara:minion
Attention ! Il approche ! #chara:dragon
<font=Witch of Thebes SDF>Vous êtes le maitre des lieux n'est ce pas, n'ayez pas peur, je voudrais simplement savoir ou sont les cabinets.</font> #chara:mage
Patron ! J'ai peur ! Il veut nous lancer un sort ! #chara:minion
* [L'attaquer] -> Regen
* [Lui montrer le chemin] -> Damages
    
=== Regen ===
AAAAAH ! Mais vous etes fou ! Je veux juste remplir mon thermos ! #chara:mage #changepers:peureux #minion:out
-> END

=== Damages ===
Ah ! Vous etes bien aimable, merci pour l'information. #chara:mage #changepers:clairvoyant #minion:out
-> END

=== Encounter2 ===
Pas de panique, je vais essayer de communiquer cette fois ci ! #chara:dragon
<font=Witch of Thebes SDF>Bonjour ! C'est encore moi Melchior, je cherche votre crypte vous pourriez m'aider ?</font> #chara:mage
Au secours ! Patron ! J'ai l'impression qu'il essaie d'aspirer mon âme ! #chara:minion
Le voilà qui se rapproche à nouveau ! #chara:dragon 
Vite patron ! Faites un truc ! #chara:minion
* [La diriger vers les oubliettes] -> Regen2
* [Accuser les monstres de fraude] -> Damages2
 -> END
 
 === Regen2 ===
AAAAAH ! Mais vous êtes fou ! Je cherche juste à finir mon rapport de stage maudit lézard ! #chara:mage #changepers:peureux #minion:out
-> END

=== Damages2 ===
Ah ! Vous êtes bien aimable, merci pour l'information. Je vous laisse alors... #chara:mage #changepers:peureux #minion:out
-> END
