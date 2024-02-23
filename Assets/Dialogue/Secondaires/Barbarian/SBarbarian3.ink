INCLUDE ../../GlobalVariables.ink

{not SBarbarian3Seen : Hahaha, un Dragon, notre combat sera légendaire ! #chara:barbarian } 
{SBarbarian3Seen : Hahaha, un Dragon, me revoilà ! #chara:barbarian}
{not SBarbarian3Seen : ->Encounter1} 
{SBarbarian3Seen : ->Encounter2} 

=== Encounter1 ===
Si vous saviez le nombre de fois que j'entends ça par jour... #chara:dragon
~SBarbarian3Seen = true 
Hahaha ! Ne me sous-estimez pas Dragon, je ne suis pas comme les autres aventuriers ! #chara:barbarian
Ah ? Et pourquoi donc ? #chara:dragon
Et bien... Je... J'ai une grande hache ! #chara:barbarian
Pfff... Comme tous les barbares de ce Donjon #chara:dragon
Mais ! Je ne suis... #chara:barbarian
Silence ! Vous me faites perdre mon temps !#chara:dragon
Battons nous ! 
-> END

=== Encounter2 ===
Ah... Fenris, comment se fait il que vous soyez encore ici ? #chara:dragon
Hahaha ! Et bien, il s'avère que j'ai enfin trouvé ce qui me rendait unique ! #chara:barbarian
Et bien éclairez ma lanterne, dites-moi ce qui vous rend si spécial... #chara:dragon
Hahaha ! Absolument rien ! #chara:barbarian
... Finissons-en. #chara:barbarian
-> END