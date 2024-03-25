INCLUDE ../../../GlobalVariables.ink

{not SMage1Seen : Boss, Look at that one! What's he doing with books and a notebook? #chara:minion #minion:in } 
{SMage1Seen : Boss! It's the terrifying guy again! #chara:minion #minion:in }
{not SMage1Seen : ->Encounter1} 
{SMage1Seen : ->Encounter2} 

=== Encounter1 ===
I believe he's a foreign student, let's see what he wants. #chara:dragon
~SMage1Seen = true 
°°Hello, I'm Melchior, a magic student from the College of the East°°  #chara:mage
... #chara:dragon
Boss.. What did he say? I didn't understand anything! #chara:minion
Watch out! He's approaching! #chara:dragon
°°You're the master of this place, aren't you? Don't be afraid, I just want to know where the toilets are.°° #chara:mage
Boss! I'm scared! He wants to cast a spell on us! #chara:minion
* [ °°Attack him (loses @@fearful@@ )] -> Regen
* [ °°Show him the way (loses @@@@clairvoyant@@@@ )] -> Damages
    
=== Regen ===
°°AAAAAH! Are you crazy! I just want to fill my thermos!°°#chara:mage #changepers:fearful #minion:out
-> END

=== Damages ===
°°Ah! You're very kind, thank you for the information.°° #chara:mage #changepers:clairvoyant #minion:out
-> END

=== Encounter2 ===
Don't panic, I'll try to communicate this time! #chara:dragon
°°Hello! It's me again, Melchior, I'm looking for your crypt, could you help me?°° #chara:mage
Help! Boss! I feel like he's trying to suck my soul out! #chara:minion
Here he comes again! #chara:dragon 
Quick, boss! Do something! #chara:minion
* [ °°Attack him (loses @@fearful@@ )] -> Regen2
* [ °°Show him the way (loses @@@@clairvoyant@@@@ )] -> Damages2
 -> END
 
 === Regen2 ===
°°AAAAAH! Are you crazy! I'm just trying to finish my darn internship report, you lizard!°° #chara:mage #changepers:fearful #minion:out
-> END

=== Damages2 ===
°°Ah! You're very kind, thank you for the information. I'll leave you then...°° #chara:mage #changepers:fearful #minion:out
-> END

