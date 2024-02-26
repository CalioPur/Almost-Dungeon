INCLUDE ../../GlobalVariables.ink

{not SArcher4Seen : Une forêt ? Ici ? C'est incroyable ! #chara:archer } 
{SArcher4Seen : Patron ! Revoilà l'inspectrice ! #chara:minion #minion:in }
{not SArcher4Seen : ->Encounter1} 
{SArcher4Seen : ->Encounter2} 

=== Encounter1 ===
Salutations étrange archer... Bienvenue dans ma forêt souterraine. #chara:dragon
~SArcher4Seen = true 
Un... un... UN DRAGON ! #chara:archer
Impossible, comment ce labyrinthe peut-il mener à un Dragon !  
Allons, quoi de plus normal pour un dragon que de protéger son Donjon ? #chara:dragon
Un Donjon ? Alors la <color=blue>maison</color> menait à un Donjon ? Est ce là la fin de cette énigme ? #chara:archer
Votre <color=blue>maison</color> mènerait à chez moi ? Haha ! Vous rêvez debout archer. #chara:dragon
Alors comment ? Comment une porte de la <color=blue>maison</color> peut elle mener ici ? #chara:archer
Je dois comprendre !
* [Lui donner des réponses (perd <color=orange>explorateure</color> )] -> Regen
* [Le calmer (perd <color=blue>peureux</color> )] -> Damages
    
=== Regen ===
Vous avez surement été victime d'un objet magique, si votre <color=blue>maison</color> peut mener à des endroits que vous ne connaissez pas, alors peut être vous a-t-elle menée jusque dans ce monde ? #chara:dragon 
Mais je crains que dans ce cas vous ne puissiez revenir en arrière. 
Mais alors je suis bloqué dans ce Donjon ? Que vais-je devenir... Je dois m'enfuir, survivre, vite ! #chara:archer
-> END

=== Damages ===
Calmez vous mon ami, pensez de manière rationelle, explorez ce donjon, vous trouverez bien un moyen de rentrer chez vous... #chara:dragon
ous êtes certain ? Vous avez raison, si une porte de la <color=blue>maison</color> peut mener ici, alors une autre peut bien me faire revenir en arrière ! #chara:archer #changepers:none
Je dois en avoir le coeur net ! 
-> END

=== Encounter2 ===
Alors étranger, toujours à la recherche d'un moyen de retourner dans votre <color=blue>maison</color> ? #chara:dragon
Encore cette maudite forêt !? Ce donjon va-t-il enfin me laisser un peu de répit ? #chara:archer
Allons monsieur Navidson, vous vous doutez bien que tout ceci n'a rien de personnel, je ne fais que mon travail. #minion:dragon
Mais je n'ai jamais demandé à être enfermé ici ! Je ne veux même pas votre trésor ! Je veux juste rentrer chez moi ! #chara:archer
* [Lui donner des réponses (perd <color=orange>explorateur</color> )] -> Regen2
* [Le motiver (perd <color=blue>peureux</color> )] -> Damages2
 -> END
 
 === Regen2 ===
Ecoutez Navidson... Quelle que soit la manière dont vous avez été transporté ici, il y'a forcément un moyen de revenir chez vous. #chara:dragon 
Votre étrange <color=blue>maison</color> doit être magique. Il n'y a peut être aucun moyen de revenir en arrière s'il s'agissait de magie.
Mais alors je suis bloqué dans ce Donjon ? Que vais-je devenir... Je dois m'enfuir, survivre, vite ! #chara:archer
-> END

=== Damages2 ===
N'êtes vous pas un grand aventurier ? Vous n'allez pas vous laisser abattre par tout cela voyons. Pensez à la gloire lorsque vous serez de retour ! #chara:dragon
La... La gloire ! Oui... #chara:archer #changepers:none
Vous avez raison ! Je suis le premier humain à mettre les pieds ici ! Je vais être célèbre !
$Si vous survivez...$ #chara:dragon
-> END