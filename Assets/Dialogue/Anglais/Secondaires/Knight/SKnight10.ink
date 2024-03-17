INCLUDE ../../../GlobalVariables.ink

{not SKnight10Seen : Et voici venir un nouveau chevalier, motivé par la gloire et la richesse. #chara:dragon } 
{SKnight10Seen : Vous savez sire Lume, vous auriez plus de chances de trouver une princesse en sortant de chez moi qu'en continuant de me réveiller à une heure pareille ! #chara:dragon }
{not SKnight10Seen : ->Encounter1} 
{SKnight10Seen : ->Encounter2} 

=== Encounter1 ===
Je te demande pardon, Dragon ? Je ne suis pas motivé par un but aussi bas. #chara:knight
~SKnight10Seen = true 
Ah ? Voilà qui est intriguant... Très bien, j'admets vous avoir sous-estimé... #chara:dragon
Haha, mais tu fais bien Dragon ! Moi, Sire Lume, n'est motivé que par l'idée de sauver les jeunes femmes en détresse que tu oses capturer ! #chara:knight
Je retire ce que j'ai dit... Je ne sais pas qui vous a fait gober de telles sornettes. #chara:dragon
Allons, ne mens pas Dragon, tout le monde sait bien que les créatures dans ton genre apprécient ce genre de choses... #chara:knight
Je n'apprécie que les pièces d'or et la grasse-matinée, chevalier... Alors trève de bavardages, battons-nous. #chara:dragon
    -> END

=== Encounter2 ===
Haha, tes mensonges ne prennent pas Dragon, je sais qu'elles sont retenues prisonnières au fond de ton donjon. #chara:knight
Non ! Je ne fais pas ce genre de choses chevalier, la seule fois où c'est arrivé, la princesse m'a fait la seule cicatrice de mon vieux corps ! #chara:dragon
Ha... Effectivement, ça doit faire mal. #chara:knight
Ah, vous voyez ! Maintenant, arrêtez de raconter vos bêtises et battez-vous ! #chara:dragon
Bon... Bon, c'est d'accord... J'arrive... #chara:knight
 -> END