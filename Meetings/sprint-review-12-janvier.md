# Sprint meeting

Auteur: Alain Sandoz
## Récap

objectif :
- implémentation multijoueur
- combler les trous : map, HUD et bugs liés au gameplay


## tour de table

- Martin
    - travail sur les bruitages sonores
    - finir pour se mettre sur networking

- Raida
    - a fait le HUD rechargement (des armes)
    - va faire le HUD des stats

- Elouann
    - Remplace les salles par l'asset pack
    - question : Est-ce qu'on garde la même echelle ? -> pas nécessairement
    - HUD sur l'écran de démarrage incruté sur un écran dans une scène

- Alain
    - a fait les shaders
    - modèle du joueur fini à 80%
    - va s'occuper du modèle d'arme
    - Moins de tâches que prévu
    - Peut récupérer des tâches ou apporter de l'aide

- Ash
    - networking
    - documentation network for game object (tutos + Alexandre)
    - n'a pas commencé ses tâches

- Alexandre
    - documentation network for game object (tutos)
    - a installé network for game object
    - a fait la connexion en réseau des joueurs
    - brainstorming sur le networking du code
    - planification du networking du code
    - besoin de changer le fonctionnement du sprint : faire chaque tâche networking à 2 (créateur du code original + spécialiste du networking)
    - faire gaffe aux fixs : Tout ce q'on va ajouter va devoir être synchronisé au niveau du multi -> terminer ça plus tôt + faire une liste des issues de fix du sprint 1

- Nicolas
    - documentation (tutos)
    - planification

## Problèmes

Passage en multijoueur plus compliqué que prévu.

## Solutions

Réorganiser le fonctionnement pour ce script : pour les tâches de networking d'objet, 
avoir le concepteur de l'objet pas à côté pour aider, car il sait comment fonctionne son code. 

Le but est d'avoir 2 spécialistes en netcode (Ash et Alexandre), et de les mettre en binôme 
avec la bonne personne en fonction du contexte.

La partie graphique avançant plus vite que prévue, les membres assignés à ces tâches pourront libérer du temps pour apporter de l'aide.

## Divers

### feedback du prof

**issue sans descriptions :** oubli de l'ancien sprint
issue au poids différents : ces tâches semblen similaires mais n'ont pas la même complexité (networking, modelisation)

**Manque de feedback  dans le jeu :** va être implémenté dans le 2ème le 3ème sprint

**Networking :** faire attention, netcode ça touche à pleins d'endroit du code, voir ça avec tout le monde

**Séparation networking/nouvelles features :** Faire attention à ce que le networking ne casse pas les nouvelles features
- possibilité de faire deux démos :
    - une sur ce qui marche en multi dans une scène temporaire
    - une sur ce qui ne marche qu'en offline

Ajouter une version 1 joueur ? 2 joueurs ? 1 joueur + IA ? _(note de l'auteur : wtff ??)_