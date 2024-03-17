INCLUDE ../../../GlobalVariables.ink

{not SKnight2Seen : Juste une écaille du Dragon, juste une pour soigner les villageois. #chara:knight } 
{SKnight2Seen : Tiens, sire Artorius, toujours décidé à voler mes écailles ? #chara:dragon }
{not SKnight2Seen : ->Encounter1} 
{SKnight2Seen : ->Encounter2} 

=== Encounter1 ===
Encore faut-il parvenir à me l'arracher chevalier. #chara:dragon
~SKnight2Seen = true 
Ah ! Dragon, vous étiez là ! #chara:knight
Difficile de ne pas vous entendre arriver avec le bruit de votre armure Chevalier. #chara:dragon
Patron ! Patron ! Celui-là n'a pas l'air bien dégourdi, je peux le mordre ? #chara:minion #minion:in
Non, nous allons lui montrer ce qu'il en coûte de jouer aux héros ! #chara:dragon #minion:out
Pour mon peuple, je vaincrai !#chara:knight
    -> END

=== Encounter2 ===
Je donnerais ma vie pour sauver mon peuple Dragon ! #chara:knight
Tant mieux, je mettrais bien la main sur votre armure ritulante, elle fera un excellent trophée. #chara:dragon
Vous ne m'aurez pas Dragon, je vais vous vaincre. #chara:knight
Très bien chevalier, amusez-moi suffisament et je vous donnerai peut-être ce que vous cherchez. #chara:dragon
 -> END