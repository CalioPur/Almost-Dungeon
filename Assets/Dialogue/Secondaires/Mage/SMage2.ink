INCLUDE ../../GlobalVariables.ink

//<font=Witch of Thebes SDF></font>

{not SMage2Seen : Un Dragon ? Enfin une source infinie de magie... Mes recherches arrivent à leur fin ! #chara:mage } 
{SMage2Seen : Dragon ! Je sens ta magie, sors de ta cachette ! #chara:mage }
{not SMage2Seen : ->Encounter1} 
{SMage2Seen : ->Encounter2} 

=== Encounter1 ===
Je crains devoir en effet y mettre un terme... N'y voyez rien de personnel. #chara:dragon
~SMage2Seen = true 
Je ne laisserai pas toutes ces années d'études être réduites en cendres ! #chara:mage
Quelle que soient les motivations qui vous poussent à explorer le Donjon, il faudra plus que des mots pour m'impressioner, mage. #chara:dragon
Très bien... Laissons parler la magie dans ce cas, Dragon ! #chara:mage
Parfait, voyons ce que peuvent faire vos petits tours de passe-passe. #chara:minion
-> END

=== Encounter2 ===
Hm... Pourquoi êtes-vous tant interessé par ma magie, Méphis ? #chara:dragon
C'est simple, je poursuis mes recherches depuis des décennies afin d'atteindre la plus puissante des magies.  #chara:mage
Intéressant, mais quelle magie est assez importante pour que vous ayez à vous frotter à un dragon ? #chara:dragon
Celle permettant de faire revenir ma famille d'entre les morts ! Et je mettrai le monde à feu et à sang pour y parvenir ! #chara:dragon 
Et bien... En voilà un but louable, dommage que je ne puisse me permettre de vous laisser gagner. En garde, mage ! #chara:dragon
-> END
