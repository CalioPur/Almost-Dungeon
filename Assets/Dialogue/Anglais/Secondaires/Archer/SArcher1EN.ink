INCLUDE ../../../GlobalVariables.ink

{not SArcher1Seen: Boss ! Watch out ! #chara:minion #minion:in } 
{SArcher1Seen : Eeeeeek ! Boss ! Someone is trying to shoot me down! #chara:minion #minion:in }
{not SArcher1Seen : ->Encounter1} 
{SArcher1Seen : ->Encounter2} 

=== Encounter1 ===
Cursed imp! You've ruined everything! #chara:archer
~SArcher1Seen = true 
Who dares to shoot at me like this? Show yourself! #chara:dragon #minion:out
Here I am, Dragon! I, Arash from the Eastern Lands, will put an end to your misdeeds. #chara:archer
Very well, I didn't expect to face an archer here, but I won't allow myself to be defeated so easily! #chara:dragon
Do not underestimate me, Dragon. Your miserable minions won't be able to defeat me. #chara:archer
We'll see about that. #chara:dragon
-> END

=== Encounter2 ===
Cursed fool! He messed up everything again! #chara:archer
Well, Arash from the Eastern Lands! You have a knack for that, don't you? #chara:dragon #minion:out
Saved by your minion once again! Misfortune follows me everywhere in this Dungeon! #chara:archer
Prepare to face the consequences! #chara:dragon
 -> END
