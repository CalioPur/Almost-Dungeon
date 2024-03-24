INCLUDE ../../GlobalVariables.ink

{not SArcher4Seen : Une forêt ? Ici ? C'est incroyable ! #chara:archer } 
{SArcher4Seen : Alors étranger, toujours à la recherche d'un moyen de retourner dans votre <color=blue>maison</color> ? #chara:dragon }
{not SArcher4Seen : ->Encounter1} 
{SArcher4Seen : ->Encounter2} 

=== Encounter1 ===
Salutations étrange archer... Bienvenue dans ma forêt souterraine. #chara:dragon
~SArcher4Seen = true 
Un... un... UN DRAGON ! #chara:archer
Impossible, comment ce labyrinthe peut-il mener à un Dragon !  
Allons, quoi de plus normal pour un dragon que de protéger son Donjon ? #chara:dragon
Un Donjon ? Alors la @@maison@@ menait à un Donjon ? Est ce là la fin de cette énigme ? #chara:archer
Votre @@maison@@ mènerait à chez moi ? Haha ! Vous rêvez, archer. #chara:dragon
Alors comment ? Comment une porte de la <color=blue>maison</color> peut elle mener ici ? #chara:archer
Je dois comprendre !
* [Lui donner des réponses (perd @@@@@explorateur@@@@@ )] -> Regen
* [Le calmer (perd @@peureux@@ )] -> Damages
    
=== Regen ===
Vous avez surement été victime d'un objet magique... Si votre @@maison@@ peut mener à des endroits que vous ne connaissez pas, alors peut être vous a-t-elle mené jusque dans ce monde ? #chara:dragon 
Mais je crains que dans ce cas vous ne puissiez revenir en arrière. 
Mais alors je suis bloqué dans ce Donjon ? Que vais-je devenir... Je dois m'enfuir, survivre, vite ! #chara:archer
-> END

=== Damages ===
Calmez vous, mon ami. Pensez de manière rationelle, explorez ce donjon, vous trouverez bien un moyen de rentrer chez vous... #chara:dragon
Vous êtes certain ? Vous avez raison, si une porte de la @@maison@@ peut mener ici, alors une autre peut bien me faire revenir en arrière ! #chara:archer #changepers:none
Je dois en avoir le coeur net ! 
-> END

=== Encounter2 ===
Encore cette maudite forêt !? Ce donjon va-t-il enfin me laisser un peu de répit ? #chara:archer
Allons monsieur Navidson, vous vous doutez bien que tout ceci n'a rien de personnel, je ne fais que mon travail. #minion:dragon
Mais je n'ai jamais demandé à être enfermé ici ! Je ne veux même pas votre trésor ! Je veux juste rentrer chez moi ! #chara:archer
* [Lui donner des réponses (perd @@@@@explorateur@@@@@ )] -> Regen2
* [Le motiver (perd @@peureux@@ )] -> Damages2
 -> END
 
 === Regen2 ===
Ecoutez Navidson... Quelle que soit la manière dont vous avez été transporté ici, il y a forcément un moyen de revenir chez vous. #chara:dragon 
Votre étrange @@maison@@ doit être magique. Il n'y a peut être aucun moyen de revenir en arrière s'il s'agissait de magie.
Mais alors je suis bloqué dans ce Donjon ? Que vais-je devenir... Je dois m'enfuir, survivre, vite ! #chara:archer
-> END

=== Damages2 ===
N'êtes vous pas un grand aventurier ? Vous n'allez pas vous laisser abattre par tout cela voyons. Pensez à la gloire lorsque vous serez de retour ! #chara:dragon
La... La gloire ! Oui... #chara:archer #changepers:none
Vous avez raison ! Je suis le premier humain à mettre les pieds ici ! Je vais être célèbre !
$Si vous survivez...$ #chara:dragon
-> END