INCLUDE ../../GlobalVariables.ink

{not SBarbarian5Seen : Dragon ! Ma rage fait ma force et tu vas comprendre pourquoi ! #chara:barbarian } 
{SBarbarian5Seen : Ah ! Revoilà le léz... #chara:barbarian}
{not SBarbarian5Seen : ->Encounter1} 
{SBarbarian5Seen : ->Encounter2} 

=== Encounter1 ===
Ne t'enflamme pas trop vite héros, c'est à moi de m'en charger normalement. #chara:dragon
~SBarbarian4Seen = true 
Ha ! Tes flammes ne peuvent surpasser celles qui brulent dans le coeur d'un vrai guerrier !  #chara:barbarian
Il rigole pas patron ! Regardez, on dirait presque qu'il dégage des flammes ! #chara:minion #minion:in
Je suis désolé mon ami, mais voyez-vous, ma machoire me démange et je croquerai bien dans quelque chose pour me soulager... #chara:dragon
Quoi ! Non pitié noble Dragon, ne me dévorez pas, je vous jure que j'ai très mauvais goût. #chara:knight

* [Croquer dans le sac de l'aventurier ($$vous$$ soigne @2 pdv@ )] -> Regen
* [Croquer dans l'aventurier ($$le héros$$ perd @10 pdv@ )] -> Damages
    
=== Regen ===
MAIS ! MON SAC ! Mes provisions ! Tu vas payer Dragon ! #chara:knight #healDragon:2
-> END

=== Damages ===
AÏE ! Je vais me venger Dragon ! Tu vas payer ! Mon pauvre bras... #chara:knight #damages:10
-> END

=== Encounter2 ===
Attendez Sire Louis... Je suis désolé pour la dernière fois... #chara:dragon
Non mais ça va pas !? #chara:knight
Vous m'avez croqué {Regen : MON sac } {Damages : MON bras !} et vous comptez vous en sortir avec des excuses ? 
Et bien vous allez rire, mais ma machoire me démange toujours... #chara:dragon 
Oh non... #chara:knight
* [Croquer dans le sac de l'aventurier ($$vous$$ soigne @2 pdv@ )] -> Regen2
* [Croquer dans l'aventurier ($$le héros$$ perd @10 pdv@ )] -> Damages2
 -> END
 
 === Regen2 ===
MAIS ! MON SAC ! {Regen : ENCORE !} Mes provisions ! Tu vas payer Dragon ! #chara:knight #healDragon:2
-> END

=== Damages2 ===
AÏE ! Je vais me venger Dragon ! Tu vas payer ! {Damages : Encore mon bras !} Mon pauvre bras... #chara:knight #damages:10
-> END