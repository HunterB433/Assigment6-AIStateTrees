Welcome to the README - This assignment was very hard, but it is Done? YES!

NOTE on INSTRUCTIONS - A few parts of the doc confused me / didnt make sence to mw
Or parts where they requested EXACT structures like "CollisionShape2D (CircleShape2D, radius: 16)"
Sometimes something didnt work for my game so I made it slightly different to try and fit the GOAL of what was wanted
Just not 100% specific

Ex: Used Draw() to make the Health bar Instead making it its own thing

ALSO - I change the Sprites after making this, so it will look more polished in game : )


Seen Below is my testing
When nothing else is triggered enemies patrol in a circle
<img width="1136" height="631" alt="Screenshot 2025-11-20 215112" src="https://github.com/user-attachments/assets/96f7e983-2e92-43a0-87cf-5b238a04ebda" />

When within chase range (yellow circle) enemy moves towards player
<img width="499" height="648" alt="Screenshot 2025-11-20 215258" src="https://github.com/user-attachments/assets/17a760ad-ea65-46e5-ab30-9875f304fd15" />

When within attack range (red circle) enemy lowers players HP bar
<img width="543" height="448" alt="Screenshot 2025-11-20 215312" src="https://github.com/user-attachments/assets/ffaaac3e-12b5-45a9-8981-54c74ab46246" />

When attacked to <50 hp, enemy summons a ally
HERE IS ONE OF THE PLACES I HAD AN ISSUE
The instructions docs never explain what "Are Allies Availbe" means
I first thought it meant "are they nearby" and worked with that for a while
but later the doc said
"You must Configure Ally scene as PackedScene on Enemy"
Meaning Summon means something more akin to "Instantiate"
So I figured it meant there is a limit of summonable enemies? (I set to 3)
REGARDLESS - You can see the ally summoned here, they are small and yellow and dont move, but they do attack
<img width="520" height="439" alt="Screenshot 2025-11-20 215335" src="https://github.com/user-attachments/assets/2cc00596-7b28-437d-bd0b-549c1cf55544" />

When attacked to <20 hp, enemy flees
<img width="1078" height="565" alt="Screenshot 2025-11-20 215502" src="https://github.com/user-attachments/assets/890d5154-941e-4743-bf78-fe92119620d9" />

For the extra 2 things I did 
1: Draw detection range circles in debug mode
2: Add sound effects for each behavior

Hope ya like em

