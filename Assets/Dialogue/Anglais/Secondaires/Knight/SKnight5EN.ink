INCLUDE ../../../GlobalVariables.ink

{not SKnight5Seen : Here we are... The Dungeon... #chara:knight } 
{SKnight5Seen : It can't be! Back in the Dungeon again... #chara:knight}
{not SKnight5Seen : ->Encounter1} 
{SKnight5Seen : ->Encounter2} 

=== Encounter1 ===
Well... A depressed knight, what a peculiar encounter. #chara:dragon
~SKnight5Seen = true 
He looks all downcast, boss! Are you sure he's alive? #chara:minion #minion:in
A Dragon?? This is just my luck... What a stupid idea to come here... #chara:knight
What's the matter, hero? Pull yourself together, come on. #chara:dragon
I never asked to come here... I don't want to fight... #chara:knight

* [Push him into the dungeon ($$the hero$$ loses @@@@@@10 speed@@@@@@ )] -> Regen
* [Motivate him ($$the hero$$ becomes @courageous@ )] -> Damages
    
=== Regen ===
Listen... Just pull yourself together and go for it... I don't care about your stories, I'm hungry. #chara:dragon
Wait, what are you doing theeeeeeeeeeeeeeeeeere... #chara:knight #time:-10
-> END

=== Damages ===
Alright, human... Take a deep breath... you can do it. #chara:dragon
We're going to have a nice fight and everything will be fine...
Have a little faith in yourself, you can do it.
Really? Okay... If you say so... I'll try... #chara:knight #changepers:courageous
-> END

=== Encounter2 ===
It can't be! Back in the Dungeon again... #chara:knight
Well well... Here you are again, Karl. #chara:dragon 
And here's the Dragon again... I can't take it anymore... #chara:knight
Well, no need to ask twice this time, right? #chara:dragon
Please, let me go... #chara:knight
* [Push him into the dungeon ($$the hero$$ loses @@@@@@10 speed@@@@@@ )] -> Regen2
* [Motivate him ($$the hero$$ becomes @courageous@ )] -> Damages2
 -> END
 
 === Regen2 ===
Wait, what are you doing theeeeeeeeeeeeeeeeeere... #chara:knight #time:-10
-> END

=== Damages2 ===
Listen here, buddy... Take a deep breath... you can do it... #chara:dragon
We're going to have another nice fight and everything will be fine...
Have a little faith in yourself, you can do it.
Really? Okay... If you say so... I'll try... #chara:knight #changepers:courageous
-> END
