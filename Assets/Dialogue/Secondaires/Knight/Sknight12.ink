INCLUDE ../../GlobalVariables.ink

{not SKnight12Seen : Enfin dans la crypte, prépare-toi Dragon ! Sire Jacques et sa lance divine vont ajouter un nouveau trophée à leur collection. #chara:knight } 
{SKnight12Seen : Graou, graou ! Je suis le patr... Dragon ! Héros, je vais te vaincre ! #chara:minion minion:in }
{not SKnight12Seen : ->Encounter1} 
{SKnight12Seen : ->Encounter2} 

=== Encounter1 ===
Bonjour monsieur de chevalier ! Vous êtes là pour la bagarre ? #chara:minion minion:in
~SKnight12Seen = true 
Ha ! Bien sûr petite créature ! Appelle ton maître pour que nous puissions nous affronter ! #chara:knight
Haha ! Le patron est parti faire des courses, aujourd'hui c'est moi le patron ! #chara:minion
Comment ? Le dragon est absent ? Je ne vais tout de même pas affronter un vulgaire diablotin ! #chara:knight
Sbire ! Pourquoi ne m'as-tu pas réveillé, pourquoi es-tu avec le héros ?!  #chara:dragon
Euh... héhé, désolé patron... Je voulais me battre avec ce héros... #chara:minion
Tss... Contente-toi de tenir ton rôle petit sbire, regarde plutôt comment je vais me débarasser de ce chevalier ! #chara:dragon #minion:out
    -> END

=== Encounter2 ===
Perfide sbire ! Je sais très bien que c'est toi derrière ce masque ! #chara:knight
Tu as recommencé sbire ? Tu sais très bien que c'est à moi d'affronter les héros ! #chara:dragon
Mais patron ! Moi aussi je veux me battre contre les héros ! #chara:minion
Allons-bon, et qu'est ce qu'il me resterait à faire dans ce cas ? #chara:dragon
Vous avez raison patron... #chara:minion
Bon... Vous avez fini ? On peut se battre Dragon ? #chara:knight #minion:out

 -> END