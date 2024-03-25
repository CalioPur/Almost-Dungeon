INCLUDE ../../../GlobalVariables.ink

{not SArcher2Seen : What a hassle, I've been in this dungeon for weeks now...... #chara:archer } 
{SArcher2Seen : What a hassle, more corridors and still no potion in sight. #chara:archer }
{not SArcher2Seen : ->Encounter1} 
{SArcher2Seen : ->Encounter2} 

=== Encounter1 ===
Boss! There's an archer on the brink of death over here! #chara:minion #minion:in
~SArcher2Seen = true 
So, archer? Tough times? Too bad you don't have that fabulous life potion. #chara:dragon
Please, Dragon, I beg you, let me have the potion, I swear I'll fight with all my might. #chara:archer #minion:out
* [Give her the potion (the heroine becomes @@courageous@@)] -> Regen
* [Drink the potion (heals $$you$$ @1 HP@)] -> Damages
    
=== Regen ===
Aaah... Thank you, Dragon... I'm back in action! #chara:archer #changepers:courageous
-> END

=== Damages ===
Tsss... Heartless creature, I'll take you down. #chara:archer #dragonHeal:1
-> END

=== Encounter2 ===
Boss! Another nearly dead archer... #chara:minion #minion:in
Haha... So, archer, still running out of potions?  #chara:dragon #minion:out
Please, Dragon, I beg you, let me have the potion this time, I promise you'll get a real fight. #chara:archer 
* [Give her the potion (the heroine becomes @@courageous@@)] -> Regen
* [Drink the potion (heals $$you$$ @1 HP@)] -> Damages
 -> END

