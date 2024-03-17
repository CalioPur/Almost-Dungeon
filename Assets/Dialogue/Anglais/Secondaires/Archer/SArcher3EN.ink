INCLUDE ../../../GlobalVariables.ink

{not SArcher3Seen : Good day, I'm from the Dungeon Inspection Office! #chara:archer } 
{SArcher3Seen : Boss! Here comes the inspector again! #chara:minion #minion:in }
{not SArcher3Seen : ->Encounter1} 
{SArcher3Seen : ->Encounter2} 

=== Encounter1 ===
Curse! The DIO, quick, my minion! They must not find our payroll! #chara:dragon
~SArcher3Seen = true 
Don't worry, Boss I'm gonna hide it! Find a way to delay her!  #chara:minion #minion:in
Ah, Mr. Dragon, we can begin the inspection now. #chara:archer #minion:out
* [Give her a false direction (loses <color=orange>explorer</color>)] -> Regen
* [Accuse the monsters of fraud (becomes <color=red>courageous</color>)] -> Damages
    
=== Regen ===
Thank you, I'll meet you in your office in a few hours, we can discuss what I'll find... #chara:archer #changepers:none
-> END

=== Damages ===
What? Your monsters didn't declare their taxes? That's very serious, sir! #chara:archer
You must stop them immediately! #chara:dragon
They won't come out alive! #chara:archer #changepers:courageous
-> END

=== Encounter2 ===
Hell! The DIO again. Quick, my minion! We need to hide our payrolls again! #chara:dragon
I'm on it, Boss! #chara:minion
Ah, Mr. Dragon, this time I'll need your payrolls. #minion:out
* [Direct her to the dungeons (loses <color=orange>explorer</color>)] -> Regen2
* [Accuse the monsters of fraud (becomes <color=red>courageous</color>)] -> Damages2
 -> END
 
 === Regen2 ===
Thank you, I'll meet you in your office in a few hours, we can discuss what I'll find... #chara:archer #changepers:none
-> END

=== Damages2 ===
What? Your monsters didn't declare their taxes? That's very serious, sir! #chara:archer #changepers:courageous
You must stop them immediately! #chara:dragon
They won't come out alive! #chara:archer
-> END