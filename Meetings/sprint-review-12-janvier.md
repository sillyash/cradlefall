# Sprint meeting

Auteur: Alain Sandoz
## Récap

objectif :
- multi
- fix les trous : map hud bug gameplay


## tour de table

- Martin
    - bruitages sonores
    - finir pour se mettre sur networking

- Raida
    - HUD rechargement (des armes)
    - après HUD des stats

- Elouann
    - Remplace les salles par l'asset pack
    - Est-ce qu'on garde la même echelle ?
    - HUD sur l'écran de démarrage incruté sur un écran dans une scène

- Alain
    - shader
    - modèle du joueur
    - va s'occuper du modèle d'arme
    - Moins de tâches que prévu
    - Peut récupérer des tâches ou apporter de l'aide

- Ash
    - networking
    - documentation (tutos)
    - pas facile (casse-tête x2)
    - S'inspire d'Alexandre
    - pas commencé

- Alexandre
    - documentation (tutos)
    - installer network for game object
    - connexion réseau
    - brainstorming
    - planification
    - besoin de changer le fonctionnement du sprint : faire chaque tâche networking à 2 (créateur du code original + spécialiste du networking)
    - faire gaffe aux fixs : Tout ce q'on va ajouter va devoir être synchronisé au niveau du multi -> terminer ça plus tôt + faire une liste des issues de fix du sprint 1

- Nicolas
    - 

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