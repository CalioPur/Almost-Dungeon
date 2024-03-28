INCLUDE ../../GlobalVariables.ink

{not SKnight2Seen && not SKnight2SeenTwice : Juste une écaille du Dragon, juste une pour soigner les villageois. #chara:knight } 
{SKnight2Seen && not SKnight2SeenTwice : Tiens, sire Artorius, toujours décidé à voler mes écailles ? #chara:dragon }
{SKnight2SeenTwice: Dragon ! J'ai tenu ma part du marché, malgré tous les camarades qui sont tombés, me voici encore devant toi ! #chara:knight }
{not SKnight2Seen && not SKnight2SeenTwice : ->Encounter1} 
{SKnight2Seen && not SKnight2SeenTwice : ->Encounter2}
{SKnight2SeenTwice : ->Encounter3}

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
~SKnight2SeenTwice = true
Tant mieux, je mettrais bien la main sur votre armure ritulante, elle fera un excellent trophée. #chara:dragon
Vous ne m'aurez pas Dragon, je vais vous vaincre. #chara:knight
Très bien chevalier, amusez-moi suffisament et je vous donnerai peut-être ce que vous cherchez. #chara:dragon
 -> END
 
 === Encounter3 ===
 Sire Artorius... #chara:dragon
 Pour une fois je m'avoue vaincu. Vous avez effectivement dépassé mes attentes.
 Alors honorez votre marché Dragon ! Sauvez-mon village ! #chara:knight
 Vous n'êtes pas vraiment en position de me forcer la main Artorius. #chara:dragon
 Mais soit ! 
 Tenez, prenez cette écaille. 
 Et maintenant je vais me faire un plaisir de vous faire sortir d'ici moi-même. #unlockAchievement:REDEMPTION
 -> END