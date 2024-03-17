INCLUDE ../../../GlobalVariables.ink

{not SKnight15Seen : Enfin la forêt souterraine ! Si les légendes disent vrai, je ne suis qu'à quelques pas de l'artefact ! #chara:knight} 
{SKnight15Seen : Je... Je dois obtenir cet artefact à tout prix ! #chara:dragon}
{not SKnight15Seen : ->Encounter1} 
{SKnight15Seen : ->Encounter2} 

=== Encounter1 ===
Eh bien chevalier... On tente de piller mon Donjon ? #chara:dragon
~SKnight15Seen = true 
Mais bien sûr Dragon, c'est mon objectif et tu n'y pourras rien. #chara:knight
Et qu'est ce qui vaut un tel intérêt pour mes trésors ? #chara:dragon
Il y a dans vos trésors un livre magique connu pour renfermer un savoir divin... Je veux m'en emparer... #chara:knight
Hmm... Intéressant. Voyons si vous en êtes digne, chevalier ! #chara:dragon
Battons-nous et vous m'expliquerez ce que vous souhaitez en faire. 
Je vous attends Dragon ! #chara:knight
    -> END

=== Encounter2 ===
Je... Je dois obtenir cet artefact à tout prix !  #chara:knight
Toujours motivé à piller mon Donjon à ce que je vois...#chara:dragon
Je n'ai pas le droit à l'erreur Dragon, j'ai trop sacrifié pour arriver jusqu'ici, je dois récupérer ce livre ! #chara:knight
Vous ne m'avez toujours pas dit pourquoi... #chara:dragon
C'est le seul moyen de revoir ma famille. Les livres de dieux doivent parler d'un moyen d'échapper à la mort ! #chara:knight
Hmm... Je vois... Mais quel que soit le contenu de ce livre, vous allez devoir me passer sur le corps avant de le savoir ! #chara:dragon
Alors je vous tuerai, Dragon ! #chara:knight
    -> END