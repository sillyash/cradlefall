# S501 - ScrumBags

| Nom du jeu                  | CRADLEFALL      |
|:----------------------------|:----------------|
| Version du document         | 0.4             |
| Date de la version courante | 2023-10-19      |

## Sommaire

<!-- TOC -->

- [S501 - ScrumBags](#s501---scrumbags)
  - [Sommaire](#sommaire)
  - [Concept principal](#concept-principal)
  - [Présentation du jeu](#présentation-du-jeu)
    - [Genre, univers, thème](#genre-univers-thème)
    - [Histoire](#histoire)
    - [Fonctionnalités clés](#fonctionnalités-clés)
    - [Conception multijoueur](#conception-multijoueur)
      - [Rôles](#rôles)
    - [Boucle de gameplay](#boucle-de-gameplay)
  - [Gameplay](#gameplay)
    - [Déplacement](#déplacement)
    - [Physique](#physique)
    - [Combat](#combat)
      - [Armes](#armes)
    - [Ennemis](#ennemis)
      - [Types](#types)
      - [Déclinaisons](#déclinaisons)
  - [Niveaux](#niveaux)
    - [Modules vaisseau](#modules-vaisseau)
      - [Modules génériques](#modules-génériques)
      - [Modules de rôle](#modules-de-rôle)
  - [Interface](#interface)
    - [Menu principal](#menu-principal)
    - [Lobby](#lobby)
    - [En jeu](#en-jeu)
      - [HUD](#hud)
      - [Menu pause](#menu-pause)
  - [Contrôles](#contrôles)
  - [Ressources artistique et sonore](#ressources-artistique-et-sonore)
    - [Style visuel](#style-visuel)
    - [Design sonore](#design-sonore)
      - [Actif](#actif)
      - [Passif](#passif)
  - [Roadmap](#roadmap)
  - [Annexes (si nécessaire)](#annexes-si-nécessaire)

<!-- /TOC -->

## Concept principal

Une escouade de 4 joueurs, dans le cadre d'une expédition spatiale, défend un vaisseau 
contre des aliens, dans un FPS en Coop de type PvE.

## Présentation du jeu

### Genre, univers, thème

> Présentez l’univers et le ton général du jeu.

Robots, espace, mission spatiale, aliens, survie, tension, coopération.

Nous sommes dans un univers de science-fiction, à bord d'un vaisseau spatial.

Le jeu se déroule dans le futur, dans un environnement fermé (vaisseau spatial).

Les joueurs incarnent des robots, avec des rôles et capacités différentes.

Leur but est de défendre des objectifs contre des aliens.

### Histoire

> Décrivez le contexte et les personnages clés du jeu.

Une expédition humaine traverse l’espace à bord du vaisseau **CRADLE IV**, en route vers une colonie lointaine.

L’équipage humain, placé en cryogénisation pour le long voyage, est surveillé par une équipe de robots 
spécialisés, chargés de la maintenance et de la sécurité du vaisseau.

Au cours du trajet, un astéroïde percute le vaisseau, endommageant plusieurs modules critiques et
ouvrant une brèche dans la coque. Profitant de la confusion, des formes de vie extraterrestres
hostiles s’infiltrent à bord. Les systèmes d’alerte déclenchent alors le protocole **A.E.G.I.S** des robots.

Les joueurs incarnent ces robots, chacun doté de capacités uniques, qui doivent s’organiser pour repousser les vagues d’aliens, réparer les systèmes endommagés et surtout protéger les caissons de cryogénisation contenant les humains. Leur mission : assurer la survie de l’expédition jusqu’à l’arrivée des secours ou la restauration complète du vaisseau.

### Fonctionnalités clés

> Listez les fonctionnalités clés du jeu.

- PvE Coopératif asymétrique (3-5 joueurs).
- Défense d’objectifs (caissons de cryogénisation).
- Vagues d’ennemis avec difficulté croissante.
- Environnement interactif : réparation de modules affectant le gameplay, events.
- Rôles complémentaires avec capacités uniques.

### Conception multijoueur

> Expliquez le côté multijoueur asymétrique de votre jeu.

#### Rôles

Rôles différents (DPS, Tank, Soutien, Support) avec des capacités et objectifs variés.

La coordination est nécessaire pour gérer les menaces et protéger les objectifs.

| Rôle       | Capacité                       | Passif(s)                                | Arme               | 
|:----------:|:-------------------------------|:-----------------------------------------|:-------------------|
| DPS        | Dégâts de zone + Knockback     | Vitesse de déplacement                   | Railgun            |
| Tank       | Bouclier                       | Plus de HP                               | Shotgun            |
| Technicien | Tourelle                       | Répare les modules du vaisseau plus vite | Plasma gun         |
| Support    | Revive de coéquipier           | Heal de zone (rayon)                     | SMG                |

### Boucle de gameplay

> Objectifs : quels sont les objectifs à court/long terme des joueurs ?
>
> Boucle de base : que font les différents joueurs en permanence ?
>
> Progression : est-ce qu’un joueur peut s’améliorer ? Si oui, comment peut-il
> s’améliorer et comment cette progression affecte la boucle de base ?
> 
> Expérience voulue pour les joueurs : quelles expériences voulez-vous offrir aux joueurs ?

- Boucle de base : vagues
- L'objectif central est de protéger les caissons de cryogénisation du vaisseau.
- En découle la protection et réparation de modules du vaisseau affectant les joueurs (énergie, soin, lumière...).
- Pour cela on a une complémentarité des rôles (DPS, Tank, Soutien, Support), afin de survivre le plus longtemps possible.
- Atmosphère tendue : on veut faire sentir la fragilité de la position tout en valorisant la coordination.
- Entre les vagues, les joueurs pourront se soigner, se provisioner et les joueurs précédemment décédés se verront respawn.
- Inspirations :
  - Halo Firefight / Destiny 2 modes Horde (défense d’objectifs),
  - Helldivers (pression cumulative et priorisation des menaces).
  - Among Us (coopération, modules vaisseau).

## Gameplay

> Décrivez les mécaniques principales : déplacement, combat, objets, IA, physique, etc.

### Déplacement

On veut éviter un déplacement trop rapide / libre (Apex Legends, The Finals):
- Pas de sauts trop hauts
- Pas beaucoup de conservation ou gain de vitesse
- Pas de mécaniques style 'bunny hop' ou 'jetpack', etc.
- Pas de 'crouch'

On propose cependant des déplacements assez dynamiques mais en accentuant la notion de fragilité d'une position:
- Il est difficile de fuir facilement les ennemis (sauf éventuellement DPS plus rapide)
- On pourra cependant se déplacer à une vitesse plutôt rapide, pour assurer un dynamisme des déplacements dans le vaisseau
- Déplacement 'normal'
- Sprint → plus rapide mais limité (stamina)
- Saut → utile pour échapper à de petits ennemis

### Physique

Les robots, donc les joueurs, sont assez lourds, surtout pour le tank.

Il y a des colissions avec les autres joueurs ainsi qu'avec les ennemis et l'environnement : on
ne pourra pas passez au travers d'objets.

Il y a donc un système de pesanteur, aussi bien pour les joueurs que pour les ennemis.

Les joueurs et ennemis ont une vitesse dite 'fixe' : leur vitesse maximale est déterminée par leur état
et la vitesse ne se cumule pas : (i.e. pas de mécanique de saut en boucle pour gagner en vitesse).

Les projectiles des armes ne sont pas soumis à la gravité, puique à base d'énergie électrique,
mais les grenades et explosifs le sont.

### Combat

Le combat s'articule en gramde majorité autour des armes des joueurs et leurs capacité:

On s'inspire du FPS classique avec une arme seulement par joueur.

Chaque joueur dispose d'une arme différente, en fonction de son [rôle](#rôles),
ainsi que d'une capacité.

Les capacités se rechargent avec le temps et lors d'une action spécifique à chaque rôle:
- DPS → faire des kills
- Tank → absorber des dégâts
- Technicien → réparer un module
- Support → soigner un allié jusqu'à ses PV max

#### Armes

| Arme               | Projectile          | Débit            | Taille de projectile  | Vitesse du proj. | Dégâts du proj. | 
|:-------------------|:--------------------|:----------------:|:----------------------|:----------------:|:---------------:|
| Railgun            | Faisceau fin        | Haute fréquence  | Petit, visée précise  | ++               | ++              |
| Shotgun            | Plusieurs faisceaux | Basse fréquance  | Petit, visée large    | -                | +               |
| Plasma gun         | Orbe de plasma      | Basse fréquence  | Grand, visée moyenne  | --               | -               |
| SMG                | Petites orbes       | Haute fréquence  | Petit, visée précise  | +                | --              |

### Ennemis

Les ennemis sont tous des aliens.

Pour un gameplay intéressant et évolutif, on propose plusieurs types d'ennemis:

#### Types

**Maxilliarus inferiarus**

Alien à grande bouche humanoide

- PV bas
- Vitesse normale
- DPS via corps a corps

**Maxilliarus superiarus**

Alien ???

- PV normaux
- Vitesse lente
- DPS via projectiles

**Petitus rapidus**

Alien ???

- PV bas
- Vitesse rapide
- DPS via corps à corps

**Bigus bouclidus**

Alien ???

- PV hauts
- Vitesse lente
- DPS via corps à corps

#### Déclinaisons

Avec l'augmentation de la difficulté, les ennemis apparaîtront dans de nouvelles déclinaisons:

- Couleur représentant un niveau de déclinaison (le même pour tous les types d'ennemis)
- Statistiques augmentées à chaque niveau (vitesse, dégâts, défense)

Couleurs:
- Vert
- Violet
- Rouge
- Bleu

## Niveaux

> Présentez la structure du/des niveau(x) de jeu : layout, spawn points, dangers, éléments dynamiques, etc.

Le niveau (la 'map') est unique.

C'est un vaisseau spatial divisé en plusieurs zones:
- pont
  - Zone de spawn / départ
  - Taille moyenne
  - Connectée aux quartiers
- quartiers / cafétériat
  - Zone centrale
  - Grande taille
  - Plusieurs sorties / connexions à des pièces annexes
- salle cryotechnique
  - Connectée aux quartiers
  - Taille moyenne
  - Objectif de défense (les caissons de cryogénisation)
- armurerie
  - Module de munitions (DPS)
  - Petite taille
- infirmerie
  - Module de soin (Support)
- salle des machines
  - Module d'oxygène
  - Module électrique

### Modules vaisseau

- Chaque zone contient des modules (ex : générateur d’énergie, infirmerie, tourelle de défense).
- Les modules ont des fonctions (ex : recharger énergie, soigner, défense automatique).
- Les modules peuvent être endommagés par les ennemis et doivent être réparés par les joueurs (notamment le technicien).
- La gestion des modules est cruciale pour la survie de l’équipe.

> [!IMPORTANT]
> 
> Il y a deux types de modules :
> - Les modules génériques, affectant tous les joueurs.
> - Les modules spécifiques à un rôle, offrant des buffs/debuffs particuliers.
>
> Les modules spécifiques à un rôle sont réparables uniquement par le joueur ayant ce rôle.
> Sinon, le module peut être réparé par n’importe quel joueur, mais le technicien le répare le plus vite.

#### Modules génériques

| Module                | Fonction                             | Impact sur les joueurs                   |
|:----------------------|:-------------------------------------|:-----------------------------------------|
| Générateur d’énergie  | Recharge l’énergie des joueurs       | Permet l’utilisation des capacités       |
| Électricité           | Alimente les lumières                | Améliore la visibilité                   |
| Oxygène               | Alimente les humains en oxygène      | S'il n'est pas réparé a temps, Game Over |
| Azote liquide         | Refroidit les caissons de cryo.      | S'il n'est pas réparé a temps, Game Over |

#### Modules de rôle

| Module                 | Rôle         | Fonction                             | Impact sur les joueurs              |
|:-----------------------|:-------------|:-------------------------------------|:------------------------------------|
| Station de soin        | Soutien      | Soigne les alliés                    | Régénération de santé accrue        |
| Station réseau         | Technicien   | Donne la minimap et les PV alliés    | Permet la coordination d'équipe     |
| Station de défense     | Tank         | Renforce les défenses                | Réduit les dégâts subis             |
| Station de combat      | DPS          | Augmente les dégâts infligés         | Améliore les dégâts des armes       |

## Interface

### Menu principal

- "Rejoindre une partie"
  - Entrer l'adresse IP du serveur
- "Héberger une partie"
  - Choisir un port
  - → Voir Lobby
- "Options"
  - Modifier les paramètres graphiques
  - Ajuster les contrôles & mappings
  - Régler le volume sonore
- "Crédits"
- "Quitter"

### Lobby

- Liste des joueurs connectés
- Choisir son rôle (DPS, Tank, Soutien, Support)
- Prêt / Non prêt
- Démarrer la partie (hôte uniquement)

### En jeu

#### HUD

On veut un HUD minimaliste, pour ne pas trop obstruer la vue.

- En bas
  - Barre de santé
  - Indicateur de capacité (cooldown)
  - Mini-carte (position des alliés & ennemis, objectifs)
  - Indicateur de munitions
- En haut
  - Indicateur de vague (No. de vague, nombre d'ennemis restants, temps avant la prochaine vague)
  - Score / statistiques (nombre de kills, assistances, etc.)
  - HP des objectifs (caissons de cryogénisation, modules du vaisseau)
  - HP des alliés
- Autres
  - Notifications contextuelles (ex : "Appuyez sur E pour interagir")

#### Menu pause

- Reprendre
- Options
- Quitter vers le menu principal

## Contrôles

> Expliquez le schéma des entrées (commandes clavier, souris).

On veut garder un schéma classique de FPS, pour rerster intuitif.

On utilisera plusieurs mappings clavier (AZERTY et QWERTY) afin
d'une majorité de joueurs.

| Action                     | Entrée (AZERTY) | Entrée (QWERTY) |
|:---------------------------|:---------------:|:---------------:|
| Déplacement                | `ZQSD`          | `WASD`          |
| Caméra                     | Souris          | Souris          |
| Capacité                   | `A`             | `Q`             |
| Interagir                  | `E`             | `E`             |
| Recharger l'arme           | `R`             | `R`             |

## Ressources artistique et sonore

> Définissez le style visuel et le design sonore. Vous devez avoir les droits aux assets
> (gratuits, payants ou créés par vous). Donnez des liens vers les assets utilisés si possible.

### Style visuel

Le style visuel sera low-poly, dans une ambiance assez sombre, afin de créer 
une ambiance immersive et pour éviter la fatigue visuelle.

Les robots joueurs auront un design légèrement différent en fonction du rôle,
avec des formes géométriques simples et des couleurs différentes pour faciliter
leur identification.

Le vaisseau spatial adoptera également un aspect low-poly, avec des [modules](#modules-vaisseau)
distincts et des éléments interactifs facilement repérables.

On cherche à faire en sorte que des formes et couleurs soient associées à de l'information
utile en jeu:

- <span style="color: #d15c5cff">rouge = danger</span>
- <span style="color: #58b058ff">vert = soin</span>
- <span style="color: #5b5bd7ff">bleu = énergie & bouclier</span>
- <span style="color: #c6c65bff">bouton = interaction</span>

On partira sur des couleurs pas trop saturées pour éviter la fatigue visuelle, surtout
sur les textures du vaisseau.

### Design sonore

Le son est spatialisé, et olus ou moins intense en fonction de la distance.

#### Actif

> HUD, objets, armes, ennemis...

- Menus
  - Clicks
- Ennemis
  - Cris (attaques, dégâts...)
- Armes
  - Laser → "Zap"
  - Blaster → "Piou piou"
  - Explosions → ???
  - Rechargement → bruits mécaniques & électriques
- Capacités
  - Bruit du HUD quand la capacité est disponible
  - Son de la capacité
  - Voicelines
- Déplacements
  - "Clank clonk"
- Interactions
  - Modules
    - Réparation → ???
  - Soin
  - Dégâts

#### Passif

> Musique, environnement...

La musique doit refléter l'intensité du gameplay : des moments intenses, et des moments de repos
(menus, entre-deux vagues).

Nous aurons donc deux musiques:
- La musique des menus et entre les vagues:
  - Planante, reposante
  - Synthwave
  - Inspirations SF et Risk of Rain
- La musique pendant les vagues
  - Tension
  - Rapide
  - Épique
  - Synthwave
  - Rythmée

Et des bruits d'ambiance:
- Vaisseau
  - Bruit de ventilation et de courant électrique faible
- Modules
  - Fonctionnels → bruit de moteur A/C, voiture électrique
  - Endommagés → "bzzt"
- Début de vague → bruit sombre, stressant
- Fin de vague → bruit 'clair', récompense, reposant

Inspirations:
- [Risk of Rain 2](https://open.spotify.com/album/3eFWXLdogrCHW11f8ZQLoa?si=qcOLxpROQHWQiAe3Er4ocw)
- [Furi](https://open.spotify.com/album/1NCt4W6d8fNGkBQAjXDVrA?si=QWs6100-QWqb-LQ3UCWhDA)

## Roadmap

> Décrivez sommairement un découpage en 4 Sprints. Les livrables attendus à 
> la fin de chaque Sprint ainsi que les risques anticipés.

TODO : Roadmap

TODO : Link roadmap to GitLab Milestones Description

## Annexes (si nécessaire)

> Tout élément intéressant pour le design du jeu qui ne rentre pas 
> dans les catégories ci-dessus.
