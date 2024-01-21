INCLUDE ../../GlobalVariables.ink

{not SKnight4Seen : Mais où suis-je ? Ce n'était pas du tout le chemin vers le Val... #chara:knight } 
{SKnight4Seen : AH NON HEIN ! Pas encore vous ! #chara:knight}
{not SKnight4Seen : ->Encounter1} 
{SKnight4Seen : ->Encounter2} 

=== Encounter1 ===
Que vois-je, un Aventurier perdu dans le Donjon ? #chara:dragon
~SKnight4Seen = true 
AH ! UN DRAGON !  #chara:knight
Oui, oui, je sais ça fait de l'effet la première fois... #chara:dragon
Je suis désolé mon ami, mais voyez-vous, ma machoire me démange et je croquerai bien dans quelque chose pour me soulager... #chara:dragon
Quoi ! Non pitié noble Dragon, ne me dévorez pas, je vous jure que j'ai très mauvais goût. #chara:knight

* [Croquer dans le sac de l'aventurier] -> Regen
* [Croquer dans l'aventurier] -> Damages
    
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
Et bien vous allez rire mais ma machoire me dérange toujours... #chara:dragon 
Oh non... #chara:knight
* [Croquer dans le sac de l'aventurier] -> Regen2
* [Croquer dans l'aventurier] -> Damages2
 -> END
 
 === Regen2 ===
MAIS ! MON SAC ! {Regen : ENCORE !} Mes provisions ! Tu vas payer Dragon ! #chara:knight #healDragon:2
-> END

=== Damages2 ===
AÏE ! Je vais me venger Dragon ! Tu vas payer ! {Damages : Encore mon bras !} Mon pauvre bras... #chara:knight #damages:10
-> END