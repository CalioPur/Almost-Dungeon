INCLUDE ../../../GlobalVariables.ink

{not SKnight17Seen : Patron ! Ce chevalier là est dangereux ! Impossible de le tromper ! #chara:minion #minion:in} 
{SKnight17Seen : Haha ! Ma clairvoyance ne me trompe jamais ! Vous étiez là Dragon ! #chara:knight}
{not SKnight17Seen : ->Encounter1} 
{SKnight17Seen : ->Encounter2} 

=== Encounter1 ===
Bien, rassure-toi sbire, je vais prendre l'affaire en main. #chara:dragon
~SKnight17Seen = true 
Ah ! Enfin le plat de résistance, je commençais justement à m'ennuyer ! #chara:knight
Vous m'ôtez les mots de la bouche chevalier, enfin un adversaire digne de ce nom. #chara:dragon
Moi, Sire Pierre, béni par la vision suprême, te provoque en duel Dragon. Tu n'as aucune chance. #chara:knight #minion:out
* [Gonfler son ego ($$le héros$$ gagne <color=red>courageux</color> )] -> Regen
* [Profiter de sa vantardise ($$vous$$ gagnez <color=red>1 point de vie</color> )] -> Damages
    
=== Regen ===
HA ! Je vous parie que vous n'arriverez même pas à abattre mon armée pour m'atteindre ! #chara:dragon
Vous me mettez au défi ? #chara:knight 
Très bien, vous allez voir, vos monstres ne suffiront pas à m'arrêter. 
Je vais me faire un plaisir de vous en débarrasser. #changepers:courageux
-> END

=== Damages ===
Bien...  #chara:dragon
Alors j'imagine que vous me permettrez de me soigner un instant... Vous ne refuseriez pas un duel à la loyale, n'est ce pas ?
Mais bien sûr, Dragon. Je vous en prie, cela ne suffira de toute façon pas contre moi. #chara:knight #healDragon:1
-> END

=== Encounter2 ===
Sire Pierre, pour une fois, je suis heureux de voir un aventurier survivre à mes pièges ! #chara:dragon
Notre dernier combat était magnifique, Dragon, je me réjouis à l'idée vous affronter à nouveau. #chara:knight
Haha ! Plaisir partagé chevalier, mais vous ne gagnerez pas ! #chara:dragon
Nous verrons bien... Quelles sont les règles cette fois ci ? #chara:knight
* [Lui faire affronter vos sbires ($$le héros$$ gagne <color=red>courageux</color> )] -> Regen2
* [Boire une potion ($$vous$$ gagnez <color=red>1 point de vie</color> )] -> Damages2
    
=== Regen2 ===
Parfait, je vais affronter votre armée avant vous dans ce cas. Ils ne vont pas faire long feu ! #chara:knight #changepers:courageux
-> END

=== Damages2 ===
Bien, prenez votre temps, nous ne sommes pas pressés. #chara:knight #healDragon:1
-> END