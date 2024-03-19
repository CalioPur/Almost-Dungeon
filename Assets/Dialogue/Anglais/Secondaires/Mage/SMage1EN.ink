INCLUDE ../../../GlobalVariables.ink

{not SMage1Seen : Boss! Look at that one, what's he doing with his books and his notebook! #chara:minion #minion:in } 
{SMage1Seen : Boss! It's the terrifying guy again! #chara:minion #minion:in }
{not SMage1Seen : ->Encounter1} 
{SMage1Seen : ->Encounter2} 

=== Encounter1 ===
I believe he's a foreign student, let's see what he wants. #chara:dragon
~SMage1Seen = true 
<font=Witch of Thebes SDF>Hello, I'm Melchior, a magic student at the College of the East</font>  #chara:mage
... #chara:dragon
Boss.. What did he say? I didn't understand anything! #chara:minion
Watch out! He's approaching! #chara:dragon
<font=Witch of Thebes SDF>You're the master of this place, aren't you? Don't be afraid, I just want to know where the toilets are.</font> #chara:mage
Boss! I'm scared! He wants to cast a spell on us! #chara:minion
* [ <font=Witch of Thebes SDF>Attack him (loses <color=blue>fearful</color> )] -> Regen
* [ <font=Witch of Thebes SDF>Show him the way (loses <color=yellow>clairvoyant</color> )] -> Damages
    
=== Regen ===
<font=Witch of Thebes SDF>AAAAAH! Are you crazy! I just want to fill my thermos!</font> #chara:mage #changepers:fearful #minion:out
-> END

=== Damages ===
<font=Witch of Thebes SDF>Ah! You're very kind, thank you for the information.</font> #chara:mage #changepers:clairvoyant #minion:out
-> END

=== Encounter2 ===
Don't panic, I'll try to communicate this time! #chara:dragon
<font=Witch of Thebes SDF>Hello! It's me again, Melchior, I'm looking for your crypt, could you help me?</font> #chara:mage
Help! Boss! I feel like he's trying to suck my soul! #chara:minion
Here he comes again! #chara:dragon 
Quick boss! Do something! #chara:minion
* [ <font=Witch of Thebes SDF>Attack him (loses <color=blue>fearful</color> )] -> Regen2
* [ <font=Witch of Thebes SDF>Show him the way (loses <color=yellow>clairvoyant</color> )] -> Damages2
 -> END
 
 === Regen2 ===
<font=Witch of Thebes SDF>AAAAAH! Are you crazy! I'm just trying to finish my darn internship report, you lizard!</font> #chara:mage #changepers:fearful #minion:out
-> END

=== Damages2 ===
<font=Witch of Thebes SDF>Ah! You're very kind, thank you for the information. I'll leave you then...</font> #chara:mage #changepers:fearful #minion:out
-> END

