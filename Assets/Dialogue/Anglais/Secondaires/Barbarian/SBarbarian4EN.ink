INCLUDE ../../../GlobalVariables.ink

{not SBarbarian4Seen : Hahaha, a Dragon, our battle will be legendary! #chara:barbarian } 
{SBarbarian4Seen : Ha Dragon! I haven't forgotten that nasty ball you dared to throw at me last time! #chara:barbarian}
{not SBarbarian4Seen : ->Encounter1} 
{SBarbarian4Seen : ->Encounter2} 

=== Encounter1 ===
Don't get fired up too quickly, hero, it's usually my job to handle that. #chara:dragon
~SBarbarian4Seen = true 
Ha! Your flames cannot surpass the ones burning in the heart of a true warrior! #chara:barbarian
He's not joking, boss! Look, it's almost like he's emitting flames! #chara:minion #minion:in
Hell, we need to stop him before he sets the whole forest on fire! #chara:dragon
I have just what we need, boss, I found these bags of powder, we can throw them at him! #chara:minion #minion:in
* [Use the grenade ($$the hero$$ loses @5 hp@ )] -> Regen
* [Use the smoke bomb ($$the hero$$ gains @@@@blind@@@@ )] -> Damages
    
=== Regen ===
Haha, little ball, no need to roll towards me, you won't be able to quench my rage either! #chara:barbarian
Aaaaaaaaaaaaah!
Mean ball!
Dragon, I will get my revenge!
-> END

=== Damages ===
Haha, little ball, no need to roll towards me, you won't be able to quench my rage either! #chara:barbarian
What! What is this smoke, I can't see anything anymore!
-> END

=== Encounter2 ===
Ah, dear Fenris, is your rage still as boiling as ever? #chara:dragon
Of course, and I'm just warming up! I'll have your skin, Dragon! #chara:barbarian
Well... Luckily, I bought more powder this morning... #chara:dragon 
* [Use the grenade ($$the hero$$ loses@5 hp@ )] -> Regen2
* [Use the smoke bomb ($$the hero$$ gains @@@@blind@@@@ )] -> Damages2
 -> END
 
 === Regen2 ===
Aaaah! Not another one of those balls! You'll pay for this, Dragon! #chara:barbarian
-> END

=== Damages2 ===
Aaaah! Not another one of those balls! You'll pay for this, Dragon! #chara:barbarian
-> END
