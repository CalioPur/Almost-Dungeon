INCLUDE ../../GlobalVariables.ink

{not SKnight5Seen : ça y est... Le Donjon... Misère de misère... #chara:knight } {SKnight5Seen : C'est pas possible ! Encore dans le Donjon... #chara:knight}
{not SKnight5Seen : ->Encounter1} 
{SKnight5Seen : ->Encounter2} 

=== Encounter1 ===
Et bien... Un chevalier déprimé, en voilà une drôle de rencontre. #chara:dragon
~SKnight5Seen = true 
Il a l'air tout raplapla patron ! Vous êtes sûr qu'il est vivant ? #chara:minion :minion:in
Un Dragon ?? C'est bien ma veine ça... Quelle idée de venir ici aussi...  #chara:knight
Quel est le problème héros ? Ressaisissez vous voyons. #chara:dragon
Je n'ai jamais demandé à venir ici moi... Je ne veux pas me battre... #chara:knight

* [Le pousser dans le donjon] -> Regen
* [Le motiver] : -> Damages
    
=== Regen ===
Bon écoutez... Prenez sur vous et allez-y d'un coup... Je me fiche de vos histoires moi, j'ai faim. #chara:dragon
Attendez qu'est ce que vous faites laaaaaaaaaaaaaaaaaaaaaaaaa... #chara:knight #time:-10
-> END

=== Damages ===
Bon écoutez humain... Respirez un bon coup... vous pouvez le faire... #chara:dragon
On va se faire une bonne bataille et tout va bien se passer...
Ayez un peu confiance en vous, vous pouvez le faire.
Vraiment ? bon.... Si vous le dites... Je vais essayer... #chara:knight #changepers:courageux
-> END

=== Encounter2 ===
C'est pas possible ! Encore dans le Donjon... #chara:knight
Allons-bon... Vous revoilà Karl. #chara:dragon 
Et revoilà le Dragon... je n'en peux plus... #chara:knight
Bon bah pas la peine de vous faire prier cette fois alors ? #chara:dragon
Pitié laissez moi partir... #chara:knight
* [Le pousser dans le donjon] -> Regen2
* [Le motiver] : -> Damages2
 -> END
 
 === Regen2 ===
Attendez qu'est ce que vous faites laaaaaaaaaaaaaaaaaaaaaaaaa... #chara:knight #time:-10
-> END

=== Damages2 ===
Bon écoutez mon gars... Respirez un bon coup... vous pouvez le faire... 
On va se faire une autre bonne bataille et tout va bien se passer...
Ayez un peu confiance en vous, vous pouvez le faire.
Vraiment ? bon.... Si vous le dites... Je vais essayer... #chara:knight #changepers:courageux
-> END