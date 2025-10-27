# CRADLEFALL

| Nom du jeu                  | CRADLEFALL            |
|:----------------------------|:----------------------|
| Version du document         | 1.0                   |
| Date de la version courante | 2025-10-26            |

Auteur.e.s:
- Ash M.
- Elouann D.
- Alexandre C.
- Nicolas V.
- Martin M.
- Alain S.
- Raida O.

## Sommaire

<!-- TOC -->

- [CRADLEFALL](#cradlefall)
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
    - [Zones](#zones)
      - [Pont](#pont)
      - [Salle cryotechnique](#salle-cryotechnique)
      - [Salle des machines](#salle-des-machines)
      - [Cantine](#cantine)
      - [Armurerie](#armurerie)
      - [Infirmerie](#infirmerie)
      - [Salle de communications](#salle-de-communications)
      - [Salle des commandes](#salle-des-commandes)
      - [Générateur](#générateur)
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
    - [Sprint 1 - Prototype](#sprint-1---prototype)
    - [Sprint 2 - Gameplay central](#sprint-2---gameplay-central)
    - [Sprint 3 - Gameplay extensif](#sprint-3---gameplay-extensif)
    - [Sprint 4 - Integration & équilibrage](#sprint-4---integration--équilibrage)
  - [Annexes](#annexes)
    - [Ennemis](#ennemis-1)

<!-- /TOC -->

## Concept principal

Une escouade de 4 joueurs, dans le cadre d'une expédition spatiale, défend un vaisseau 
contre des aliens, dans un FPS en Coop de type PvE.

## Présentation du jeu

### Genre, univers, thème

Keywords: Robots, mission spatiale, aliens, survie, tension, coopération.

Jeu de science‑fiction se déroulant à bord d’un vaisseau spatial fermé.

Les joueurs incarnent des robots aux rôles et capacités distincts,
dont l’objectif est de défendre des objectifs contre des créatures extraterrestres.

### Histoire

Une expédition humaine traverse l’espace à bord du vaisseau **CRADLE IV**, en route vers un système planétaire,
_Solaris_ récemment découvert avec des candidats prometteurs pour une planète habitable.

L’équipage humain, placé en cryogénisation, (_cryostasis_) pour le long voyage, est surveillé par une équipe de robots 
spécialisés, chargés de la maintenance et de la sécurité du vaisseau.

Les humains, arrivés sur une première planète de _Solaris_, repartent avec des échantillons peu prometteurs.
Ils repartent alors bredouilles et doivent aller jusqu'à la prochaine planète, à 8 mois de distance, et sont 
donc replongés en _cryostasis_.

Cependant, lors de leur précédent atterrissage, des formes de vie extraterrestres hostiles se sont infiltrées à bord: un organisme semble pondre des œufs dans la ventilation du vaisseau, libérant des créatures agressives.

Les systèmes du vaisseau déclenchent alors le protocole **A.E.G.I.S** des robots, leur donnant accès 
à la force létale pour protéger le vaisseau et son précieux équipage.

Les joueurs incarnent ces robots, chacun doté de capacités uniques, qui doivent s’organiser pour repousser les vagues d’aliens,
réparer les systèmes endommagés et surtout protéger les caissons de cryogénisation contenant l'équipage.

Leur mission : assurer la survie de l’expédition jusqu’à l’arrivée des secours ou l'élimination totale des menaces.

### Fonctionnalités clés

- PvE Coopératif asymétrique (3-5 joueurs).
- Défense d’objectifs (caissons de cryogénisation).
- Vagues d’ennemis avec difficulté croissante.
- Environnement interactif : réparation de modules affectant le gameplay, events.
- Rôles complémentaires avec capacités uniques.

### Conception multijoueur

> Expliquez le côté multijoueur asymétrique de votre jeu.

#### Rôles

Rôles distincts (DPS, Tank, Soutien, Support) — chacun disposant de capacités et d’objectifs propres.

La coordination est nécessaire pour gérer les menaces et protéger les objectifs.

| Rôle       | Capacité                       | Passif(s)                                | Arme               | 
|:----------:|:-------------------------------|:-----------------------------------------|:-------------------|
| DPS        | Dégâts de zone + Knockback     | Vitesse de déplacement                   | Railgun            |
| Tank       | Bouclier                       | Plus de HP                               | Shotgun            |
| Technicien | Tourelle                       | Répare les modules du vaisseau plus vite | Plasma gun         |
| Support    | Revive de coéquipier           | Heal de zone (rayon)                     | SMG                |

### Boucle de gameplay

Boucle de base : système de vagues

- L'objectif central est de protéger les caissons de cryogénisation du vaisseau.
- En découle les objectifs secondaires: la protection et réparation de modules du vaisseau affectant les joueurs (énergie, soin, lumière...).
- Pour cela on a une complémentarité des rôles (DPS, Tank, Soutien, Support), afin de survivre le plus longtemps possible.
- Atmosphère tendue : on veut faire sentir la fragilité de la position tout en valorisant la coordination.
- Entre les vagues, les joueurs pourront se soigner, se provisioner et les joueurs précédemment décédés se verront respawn.
- Inspirations :
  - Halo Firefight / Destiny 2 modes Horde (défense d’objectifs),
  - Helldivers (pression cumulative et priorisation des menaces).
  - Among Us (coopération, modules vaisseau).

Un motif prédéterminé d'environ 5-6 vagues serviront de bases sur laquelle on boucle. Pour éviter la répétition et améliorer la réjouabilité. Toutes les vagues contiennent une part importante d'aléatoire (Placement et nombre des montres, dysfonctionnement des modules, durée etc...).  Ainsi, même si les vagues sont basées sur le même schéma, elles ne se ressemblent pas.

Plus le nombre de vagues augmente, plus la part d'évenements aléatoires augmente et plus les alien arrivent en nombre. De plus, les aliens pourront prendre différentes couleurs selon leur puissance pour ajouter de la variété et de la difficulté entre les vagues. 

Si un module de rôle dysfonctionne, les joueurs ne bénéficierons plus de ses effets. Leur réparation est conseillée mais pas obligatoire. Pour les modules génériques en revanche (ex: panne des générateur d'oxygène), un minuteur se lance, contraignant les joueurs à le réparer dans le temps impartie, sous peine de perdre.

Au bout de 20-30 vagues, on peut affronter une vague finale plus difficile, avec un boss alien,
l'organisme pondant les œufs dans le vaisseau.

À l'issue de la partie, on peut afficher des statistiques de performance (kills, assistances, dégâts subis, objectifs réparés...).

**Bonus :** Système de progression entre les vagues (amélioration d'armes, capacités, modules vaisseau...).

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
mais les grenades et explosifs le sont. Ils cumulent cependant leur vitesse a la vitesse du 
joueur et sa direction.

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

Pour un gameplay intéressant et évolutif, on propose plusieurs types d'ennemis,
chacun avec des caractéristiques et comportements distincts.

On propose aussi pour chaque type d'ennemi plusieurs déclinaisons, avec des statistiques
augmentées (vitesse, dégâts, défense) et une couleur différente pour chaque niveau.

![Ennemis](./Mediatheque/enemies_blue.png)

#### Types

**Maxilliarus superiarus**

Alien humanoide

- PV normaux
- Vitesse lente
- DPS via projectiles

**Maxilliarus inferiarus**

Alien humanoide

- PV bas
- Vitesse normale
- DPS via corps a corps

**Petitus rapidus**

Alien insectoide

- PV bas
- Vitesse rapide
- DPS via corps à corps

**Petitus explodus**

Alien insectoide

- PV bas
- Vitesse lente
- DPS via corps à corps, explosion à la mort

**Maxilus Colossus**

Alien elephantoide

- PV élevés
- Vitesse lente
- DPS via projectiles (bouche)

#### Déclinaisons

Avec l'augmentation de la difficulté, les ennemis apparaîtront dans de nouvelles déclinaisons:

- Couleur représentant un niveau de déclinaison (le même pour tous les types d'ennemis)
- Statistiques augmentées à chaque niveau (vitesse, dégâts, défense)

Couleurs:
- Bleu
- Vert
- Violet
- Rouge

Voir [Annexes](#annexes).

## Niveaux

Le niveau est unique : un vaisseau spatial subdivisé en plusieurs zones interconnectées.

![Layout vaisseau](./Mediatheque/ship_layout.png)

### Zones

#### Pont

- Grande taille
- Zone de spawn / départ
- Plusieurs sorties / connexions à des pièces annexes
- Contient la salle cryotechnique (cf. [salle cryotechnique](#salle-cryotechnique))

#### Salle cryotechnique

- Connectée au pont
- Taille moyenne
- Objectif de défense (les caissons de cryogénisation)

#### Salle des machines

- Grande taille
- Contient le module d'azote liquide (cf. [modules génériques](#modules-génériques))
- Contient le module d'oxygène (cf. [modules génériques](#modules-génériques))

#### Cantine

- Grande taille
- Zone de repos entre les vagues

#### Armurerie

- Petite taille
- Contient la station de combat (cf. [modules](#modules-de-rôle))

#### Infirmerie

- Petite taille
- Contient la station de soin (cf. [modules](#modules-de-rôle))

#### Salle de communications

- Moyenne taille
- Contient la station réseau (cf. [modules](#modules-de-rôle))

#### Salle des commandes

- Moyenne taille
- Contient la station de défense (cf. [modules](#modules-de-rôle))

#### Générateur

- Moyenne taille
- Contient le générateur électrique (cf. [modules génériques](#modules-génériques))

### Modules vaisseau

- Chaque zone contient des modules (ex : générateur électrique, infirmerie).
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
- Choisir la difficulté (Facile, Normal, Difficile)
- Afficher les règles du jeu
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

On veut garder un schéma classique de FPS, pour rerster intuitif.

On utilisera plusieurs mappings clavier (AZERTY et QWERTY) afin
d'une majorité de joueurs.

| Action                     | Entrée (AZERTY) | Entrée (QWERTY) |
|:---------------------------|:---------------:|:---------------:|
| Déplacement                | `ZQSD`          | `WASD`          |
| Saut                       | `Espace`        | `Espace`        |
| Caméra                     | Souris          | Souris          |
| Tir                        | Clic gauche     | Clic gauche     |
| Viser                      | Clic droit      | Clic droit      |
| Capacité                   | `A`             | `Q`             |
| Interagir                  | `E`             | `E`             |
| Recharger l'arme           | `R`             | `R`             |

## Ressources artistique et sonore

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

On partira sur des couleurs pas trop saturées pour éviter la fatigue visuelle, surtout
sur les textures du vaisseau.

### Design sonore

Le son est spatialisé, et plus ou moins intense en fonction de la distance.

#### Actif

- Menus
  - Clicks
- Ennemis
  - Bruits de pas → son biologique
  - Attaque → son de cri / grondement
  - Cri d'alerte → son strident
  - Mort → son d'explosion / fracture
- Armes
  - Tir → son d'énergie électrique
  - Rechargement → son mécanique
  - Impact → son d'explosion
- Capacités
  - Bruit du HUD quand la capacité est disponible
  - Son de la capacité → spécifique à chaque rôle
  - Voicelines → spécifique à chaque rôle
- Déplacements
  - Saut → son mécanique
  - Sprint → son de moteur
  - Pas → son métallique
- Interactions
  - Modules
    - Réparation → son mécanique
    - Activation → son d'énergie
    - Endommagés → son électrique défaillant
  - Soin
    - Son de soin → son apaisant
  - Dégâts
    - Son de dégâts → son d'impact métallique
    - Alerte faible santé → son d'alerte
- Vaisseau
  - Bruit de ventilation et électricité de fond
- Gameplay
  - Bruit de pas des alliés et ennemis
  - Bruit de tir des armes des alliés et ennemis
  - Début de vague → bruit sombre, stressant
  - Fin de vague → bruit 'clair', reposant

#### Passif

> Musique, environnement...

La musique doit refléter l'intensité du gameplay : des moments intenses, et des moments de repos
(menus, entre-deux vagues).

Nous aurons donc deux musiques:
- La musique des menus et entre les vagues:
  - Planante, reposante
  - Synthwave
  - Inspirations SF
- La musique pendant les vagues
  - Tension
  - Rapide
  - Épique
  - Synthwave
  - Rythmée

Inspirations:
- [Risk of Rain 2](https://open.spotify.com/album/3eFWXLdogrCHW11f8ZQLoa?si=qcOLxpROQHWQiAe3Er4ocw)
- [Furi](https://open.spotify.com/album/1NCt4W6d8fNGkBQAjXDVrA?si=QWs6100-QWqb-LQ3UCWhDA)

## Roadmap

### Sprint 1 - Prototype

**Livrables:**
- Mouvement de base (déplacement, sprint, saut)
- Structure du vaisseau (layout principal, 3-4 zones)
- Système de vagues d'ennemis
- Système de tir basique
- 1 type d'ennemi fonctionnel
- 1 module vaisseau (générateur électrique)
- Objectif de défense (caissons cryogéniques)
- Début de l'interface (HUD basique)
- 3-4 vagues d'ennemis et fin de partie

**Risques:** Mécaniques de déplacement, IA ennemis

### Sprint 2 - Gameplay central

**Livrables:**
- Multijoueur fonctionnel (lobby, connexion)
- 2 rôles jouables avec armes spécifiques
- HUD amélioré
- Respawn des joueurs

**Risques:** Équilibrage des rôles, IA ennemis

### Sprint 3 - Gameplay extensif

**Livrables:**
- 4 rôles jouables
- Capacités des rôles
- 3 types d'ennemis supplémentaires
- Modules du vaisseau (électricité, oxygène, azote)
- Stations de rôle
- HUD complet
- Musique
- Début d'audio (armes, ennemis, interactions)
- 10-15 vagues d'ennemis
- Boss final

**Risques:** Complexité des interactions modules, temps de développement

### Sprint 4 - Integration & équilibrage

**Livrables:**
- Déclinaisons d'ennemis
- Système de difficulté
- Équilibrage gameplay
- 20-30 vagues d'ennemis
- Menus (principal, lobby, pause)
- Audio & polish final

**Risques:** Bugs multijoueur, équilibrage gameplay

## Annexes

### Ennemis

![Ennemis bleus](./Mediatheque/enemies_blue.png)
![Ennemis verts](./Mediatheque/enemies_green.png)
![Ennemis violets](./Mediatheque/enemies_purple.png)
![Ennemis rouges](./Mediatheque/enemies_red.png)

### Joueurs

![Joueurs](./Mediatheque/player.png)

### Map

![Niveau](./Mediatheque/ship_layout.png)