# S501 - ScrumBags

| Nom du jeu                  | CRADLEFALL      |
|:----------------------------|:----------------|
| Version du document         | 0.2             |
| Date de la version courante | 2023-10-01      |

## Sommaire

TODO : Sommaire généré automatiquement

## Auteurs

| Nom              | Rôle(s)                     | Spécialisation(s) ?   |
|:-----------------|:----------------------------|:----------------------|
| Nicolas V.       | Scrum Master                |                       |
| Alexandre C.     | MOe (Chef Dev)              |                       |
| Elouann D.       | Dev                         |                       |
| Martin M.        | Dev                         | Son                   |
| Alain D.         | Dev                         | Assets, Blender       |
| Raida O.         | Dev                         |                       |
| Ash M.           | Dev                         | GitLab Issues, normes |

## Concept principal

Une escouade de 3–5 joueurs, dans le cadre d'une expédition spatiale, défend un vaisseau contre des aliens, dans un FPS en Coop de type PvE.

## Présentation du jeu

### Genre, univers, thème

> Présentez l’univers et le ton général du jeu.

Robots, espace, mission spatiale, aliens, survie, tension, coopération.

### Histoire

> Décrivez le contexte et les personnages clés du jeu.

Des aliens infiltrent un vaisseau spatial.

Des robots à bord (les joueurs) sont réveillés afin de défendre l'expédition.

Ils s'arment afin de défendre en priorité les humains membres de l'équipage, qui sont cryogénisés.

### Fonctionnalités clés

> Listez les fonctionnalités clés du jeu.

- PvE Coopératif asymétrique (3-5 joueurs).
- Défense d’objectifs (caissons de cryogénisation).
- Vagues d’ennemis avec difficulté croissante.
- Environnement interactif : réparations, events, modules du vaisseau.

### Conception multijoueur

> Expliquez le côté multijoueur asymétrique de votre jeu.

#### Rôles

Rôles différents (DPS, Tank, Soutien, Support) avec des capacités et objectifs variés.

La coordination est nécessaire pour gérer les menaces et protéger les objectifs.

| Rôle       | Capacité                       | Passif(s)                                |
|:----------:|:-------------------------------|:-----------------------------------------|
| DPS        | Dégats autour + Knockback      | Vitesse de déplacement                   |
| Tank       | Bouclier                       | Plus de HP                               |
| Technicien | Tourelle                       | Répare les modules du vaisseau plus vite |
| Support    | Grenade de heal                | Heal de zone (rayon)                     |

| Rôle       | Arme               | Type de projectile  | Débit            | Taille de projectile  | Vitesse du projectile | Dégâts par projectile | 
|:----------:|:-------------------|:--------------------|:----------------:|:----------------------|:---------------------:|:---------------------:|
| DPS        | Railgun            | Faisceau fin        | Haute fréquence  | Petit, visée précise  | ++                    | ++                    |
| Tank       | Shotgun            | Plusieurs faisceaux | Basse fréquance  | Petit, visée large    | -                     | +                     |
| Soutien    | Plasma gun         | Plasma orb          | Basse fréquence  | Grand, visée moyenne  | --                    | -                     |
| Support    | SMG                | Petites orbes       | Haute fréquence  | Petit, visée précise  | +                     | --                    |

### Boucle de gameplay

> Objectifs : quels sont les objectifs à court/long terme des joueurs ?
>
> Boucle de base : que font les différents joueurs en permanence ?
>
> Progression : est-ce qu’un joueur peut s’améliorer ? Si oui, comment peut-il s’améliorer et comment cette progression affecte la boucle de base ?
> 
> Expérience voulue pour les joueurs : quelles expériences voulez-vous offrir aux joueurs ?

- Boucle de base : vagues
- L'objectif central est de protéger les caissons de cryogénisation du vaisseau.
- En découle la protection et réparation de modules du vaisseau affectant les joueurs (énergie, soin, lumière...).
- Pour cela on a une complémentarité des rôles (DPS, Tank, Soutien, Support), afin de survivre le plus longtemps possible.
- Atmosphère tendue : on veut faire sentir la fragilité de la position tout en valorisant la coordination.
- Inspirations :
  - Halo Firefight / Destiny 2 modes Horde (défense d’objectifs),
  - Helldivers (pression cumulative et priorisation des menaces).

## Gameplay

> Décrivez les mécaniques principales : déplacement, combat, objets, IA, physique, etc.

## Niveaux

> Présentez la structure du/des niveau(x) de jeu : layout, spawn points, dangers, éléments dynamiques, etc.

## Interface

> Indiquez les éléments affichés à l’écran pendant le jeu et la structure des menus.

## Contrôles

> Expliquez le schéma des entrées (commandes clavier, souris).

| Action                     | Entrée (AZERTY) | Entrée (QWERTY) |
|:---------------------------|:---------------:|:---------------:|
| Déplacement                | `ZQSD`          | `WASD`          |
| Caméra                     | Souris          | Souris          |
| Capacité                   | `A`             | `Q`             |
| Interagir                  | `E`             | `E`             |
| Recharger l'arme           | `R`             | `R`             |

## Ressources artistique et sonore

> Définissez le style visuel et le design sonore. Vous devez avoir les droits aux assets (gratuits, payants ou créés par vous). Donnez des liens vers les assets utilisés si possible.

## Roadmap

> Décrivez sommairement un découpage en 4 Sprints. Les livrables attendus à la fin de chaque Sprint ainsi que les risques anticipés.

## Annexes (si nécessaire)

> Tout élément intéressant pour le design du jeu qui ne rentre pas dans les catégories ci-dessus.
