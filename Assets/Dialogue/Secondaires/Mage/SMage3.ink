INCLUDE ../../GlobalVariables.ink

//<font=Witch of Thebes SDF></font>

{not SMage3Seen : Une petite décharge d'énergie et... Mais... Je ne suis plus dans le vaisseau ! C'est impossible ! #chara:mage } 
{SMage3Seen : Ah ! Docteure Seltis ! Je constate que vous n'avez pas trouvé de moyen de retourner chez vous ! #chara:dragon }
{not SMage3Seen : ->Encounter1} 
{SMage3Seen : ->Encounter2} 

=== Encounter1 ===
Téléportée dans mon donjon ? En voilà une magie rare ! #chara:dragon
~SMage3Seen = true 
Un dragon ! Mais où ce maudit artefact m'a-t-il envoyé !  #chara:mage
Je crois que des présentations s'imposent. Bienvenue dans le Donjon, magicienne. J'en suis à la fois le maître et le gardien. #chara:dragon
Comment vais-je faire pour retourner en arrière... Je n'ai pas le temps de m'éterniser ici, Dragon, renvoie-moi d'où je viens ! #chara:mage
Quelle impatience ! Je n'ai aucun moyen de vous renvoyer chez vous, magicienne, par contre... #chara:dragon
* [ Lui offrir votre savoir (perd <color=orange>impatient</color> )] -> Regen
* [ Lui voler ses lunettes (gagne <color=yellow>bigleux</color> )] -> Damages
    
=== Regen ===
Je vous invite à explorer le Donjon, ses artefacts et parchemins magiques pourraient peut être vous aider à retourner chez vous... #chara:dragon
Vraiment ? Vous disposez de tels objets ici... En tant que scientifique, je me dois de faire la lumière sur ce savoir. #chara:mage #changepers:clairvoyant
-> END

=== Damages ===
Vous êtes ici chez moi et je pose les règles. Si vous souhaitez retourner chez vous, essayez au moins de survivre ici... #chara:mage #changepers:bigleux
MAIS ! Mes lunettes ! Dragon ! Rendez-moi mes lunettes ! #chara:mage
-> END

=== Encounter2 ===
C'est toujours mon objectif, mais l'artefact réagit étrangment à cet endroit. Il en va de mon devoir de chercher à élucider ce mystère. #chara:mage
Et quelle est la suite de vos études ? Vous pensez pouvoir réussir à comprendre cet objet ? #chara:dragon
Réjoussez-vous Dragon, vous allez pouvoir participer au progrès scientifique. J'ai compris que pour activer un nouveau pan de ses pouvoirs j'allais avoir besoin de votre or ! #chara:mage
Je vous demande pardon ?! Vous n'aurez jamais mon or ! #chara:dragon 
* [ Lui donner une fausse piste (perd <color=orange>impatient</color> )]  -> Regen2
* [ Lui voler ses lunettes (gagne <color=yellow>bigleux</color> )] -> Damages2
 -> END
 
 === Regen2 ===
Mais... avancez à la fin de cet étage, vous trouverez peut être mon trésor. Si vous survivez... #chara:dragon #changepers:clairvoyant
-> END

=== Damages2 ===
Ha ! Essayez déjà de survivre à cet étage, vous n'êtes en aucun cas digne de mettre la main sur une seule pièce d'or. #chara:dragon #changepers:bigleux
-> END
