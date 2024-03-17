INCLUDE ../../../GlobalVariables.ink

{not SArcher4Seen : Hello there, strange archer... Welcome to my underground forest. #chara:archer } 
{SArcher4Seen : Boss! Here comes the inspector again! #chara:minion #minion:in }
{not SArcher4Seen : ->Encounter1} 
{SArcher4Seen : ->Encounter2} 

=== Encounter1 ===
Greetings, peculiar archer... Welcome to my underground forest. #chara:dragon
~SArcher4Seen = true 
A... a... A DRAGON! #chara:archer
Impossible, how can this maze lead to a Dragon!  
Well, what's more normal for a dragon than to protect his Dungeon? #chara:dragon
A Dungeon? So the <color=blue>house</color> led to a Dungeon? Is this the end of this puzzle? #chara:archer
Your <color=blue>house</color> would lead to my place? Haha! You're dreaming, archer. #chara:dragon
Then how? How can a door from the <color=blue>house</color> lead here? #chara:archer
I must understand!
* [Give him answers (loses <color=orange>explorer</color>)] -> Regen
* [Calm him down (loses <color=blue>fearful</color>)] -> Damages
    
=== Regen ===
You must have been the victim of a magical item... If your <color=blue>house</color> can lead to places you don't know, then perhaps it led you to this world? #chara:dragon 
But I'm afraid that in this case, you can't go back.
But then I'm stuck in this Dungeon? What will become of me... I must escape, survive, quickly! #chara:archer
-> END

=== Damages ===
Calm down, my friend. Think rationally, explore this dungeon, you'll surely find a way back home... #chara:dragon
Are you sure? You're right, if a door from the <color=blue>house</color> can lead here, then another one can surely take me back! #chara:archer #changepers:none
I have to be sure! 
-> END

=== Encounter2 ===
So, stranger, still looking for a way back to your <color=blue>house</color>? #chara:dragon
This cursed forest again!? Will this dungeon finally give me a break? #chara:archer
Come on, Mr. Navidson, you must realize that all this is not personal, I'm just doing my job. #minion:dragon
But I never asked to be trapped here! I don't even want your treasure! I just want to go home! #chara:archer
* [Give him answers (loses <color=orange>explorer</color>)] -> Regen2
* [Motivate him (loses <color=blue>fearful</color>)] -> Damages2
 -> END
 
 === Regen2 ===
Listen, Navidson... However you were transported here, there must be a way to get back home. #chara:dragon 
Your strange <color=blue>house</color> must be magical. There may be no way back if it was magic.
But then I'm stuck in this Dungeon? What will become of me... I must escape, survive, quickly! #chara:archer
-> END

=== Damages2 ===
Aren't you a great adventurer? You won't let this get you down, come on. Think of the glory when you're back! #chara:dragon
Th... The glory! Yes... #chara:archer #changepers:none
You're right! I'm the first human to set foot here! I'm going to be famous!
$If you survive...$ #chara:dragon
-> END
-> END