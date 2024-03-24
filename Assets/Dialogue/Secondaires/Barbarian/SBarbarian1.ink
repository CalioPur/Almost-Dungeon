INCLUDE ../../GlobalVariables.ink

{not SBarbarian1Seen : Haha, après des semaines de marche depuis les Contrées du Nord, me voici enfin dans le Donjon ! #chara:barbarian } 
{SBarbarian1Seen : Revoici ce bon explorateur... #chara:dragon}
{not SBarbarian1Seen : ->Encounter1} 
{SBarbarian1Seen : ->Encounter2} 

=== Encounter1 ===
Oh ? Un explorateur du Nord, dans ma Caserne. Voilà une rencontre inattendue. #chara:dragon
~SBarbarian1Seen = true 
Un Dragon, comme dans les légendes ! Vite, ma carte ! #chara:barbarian
Je vous demande pardon ? Une carte ? Quelle carte ? #chara:dragon
Ben ma carte ! Regardez, pendant mon voyage j'ai trouvé une carte magique de chez vous ! #chara:barbarian
Une carte magique ! Hmm, voilà qui pourrait être utile... #chara:dragon

* [Voler la carte (perd @@@@@explorateur@@@@@ )] -> Regen
* [Lui ajouter de fausses informations (perd @@@@@@10 de vitesse@@@@@@ )] -> Damages
    
=== Regen ===
Mais ! MA CARTE ! Rendez-la moi, Dragon ! Je vous préviens, ça va mal se finir pour vous ! #chara:barbarian #changepers:none
-> END

=== Damages ===
Comment ? Vous dites que vous avez des pièces secrètes dans le donjon ? #chara:barbarian
Je dois prendre mon temps alors !
Exactement, prenez votre temps... Avancez à votre rythme. #chara:dragon #time:-10
-> END

=== Encounter2 ===
Ah vous revoilà Dragon ! Quelle chance, je viens de mettre la main sur une deuxième carte... #chara:barbarian
Encore !? Ce n'est pas possible... Qui lui a donné une autre carte ? #chara:dragon 
Patron, c'est vous qui avez rempli les coffres la dernière fois... #chara:minion #minion:in
Ah ! Pas la peine d'essayer de me la {Regen : voler,} {Damages : salir,} cette fois-ci ! Vous ne m'aurez pas ! #chara:barbarian #minion:out
* [Bruler la carte ! (le héros perd @@@@@explorateur@@@@@ )] -> Regen2
* [Rendre la carte illisible ! (le héros perd @@@@@@10 de vitesse@@@@@@ )] -> Damages2
 -> END
 
 === Regen2 ===
Mais ! MA CARTE ! VOUS L'AVEZ BRULEE ! #chara:barbarian 
Cette fois-ci c'en est trop ! J'aurai ta tête, Dragon ! #changepers:none
-> END

=== Damages2 ===
Attendez, qu'est ce que vous m'avez jeté dessus là ? #chara:barbarian
Beuargh... De la bave de troll... Ma carte est pratiquement illisible, c'est malin ça ! #time:-10
-> END