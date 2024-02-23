INCLUDE ../../GlobalVariables.ink

{not SBarbarian2Seen : Un immense lézard, je n'en avais jamais vu d'aussi gros ! #chara:barbarian } 
{SBarbarian2Seen : Ah ! Revoilà le léz... #chara:barbarian}
{not SBarbarian2Seen : ->Encounter1} 
{SBarbarian2Seen : ->Encounter2} 

=== Encounter1 ===
Et bien... je crois qu'il y'a méprise... #chara:dragon
~SBarbarian2Seen = true 
Ah bon ? Vous n'êtes pas un lézard géant ?  #chara:barbarian
Bien sîur que non petit barbare, je suis le Dragon, gardien du Donjon ! #chara:dragon
Le Donjon !? Mais le panneau disait "maison" ! #chara:barbarian
Mais ma parole, vous êtes complètement bigleux mon pauvre ami ! #chara:dragon
-> END

=== Encounter2 ===
Dragon ! Je suis un Dragon ! #chara:dragon
Ah bon... #chara:barbarian
Je vous assure barbare, vous êtes myope comme une taupe. #chara:dragon
Patron ? Vous êtes certain que vous ne voulez pas que je lui donne des lunettes magiques ? #chara:minion #minion:in
il me fait un peu pitié là... 
UNE GRENOUILLE QUI PARLE ! #chara:barbarian #minion:out
-> END