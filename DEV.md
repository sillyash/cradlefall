# Développement

Instructions et informations pour les développeurs.

---

## Structure

```tree
.
├─ Assets/
│  ├─ Art/
│  ├─ Audio/
│  ├─ Prefabs/
│  ├─ Scenes/
│  ├─ Scripts/
│  │  ├─ Gameplay/
│  │  ├─ Systems/
│  │  └─ Utilities/
│  ├─ Shaders/
│  ├─ Plugins/
│  └─ Resources/
├─ Packages/
│  └─ manifest.json
├─ ProjectSettings/
├─ .gitattributes
├─ .gitignore
├─ .gitlab-ci.yml (CI/CD configuration)
├─ CHANGELOG.md
├─ DEV.md         (This file, Development instructions)
├─ GAME.md        (Game Design Document)
├─ LICENSE
└─ README.md      (Overview of the project)
```

---

## Outils

### Git LFS

Nous utilisons Git LFS pour les fichiers volumineux (modèles 3D, textures, audio, vidéos, animations).

1. Installer : `git lfs install`
2. Vérifier : `git lfs env`
3. Suivi défini dans `.gitattributes` (ne pas modifier sans accord).
4. Ajouter normalement: `git add chemin/fichier`
5. Vérifier ce qui est en LFS: `git lfs ls-files`
6. En cas d’oubli (gros fichier déjà poussé), ouvrir une issue avant d’essayer de réécrire l’historique.

> [!CAUTION]
>
> - Ne jamais versionner `/Library`, `/Temp`, exports intermédiaires.
> - Préférer formats compressés (PNG optimisé, FBX propre sans animation inutile, audio en WAV si nécessaire sinon OGG).

### Hooks Git

#### Installation

Nous utilisons [Husky.NET](https://alirezanet.github.io/Husky.Net/) pour gérer les hooks Git.

```powershell
dotnet tool install Husky
dotnet tool install dotnet-format
dotnet tool install Versionize

dotnet husky install
```

#### Commitizen

> Aide à produire des messages [Conventional Commits](#commits).

Commit interactif:

```bash
cz commit
```

Changelog + bump de version sémantique:

```bash
cz changelog
cz bump
```

Bump applique un tag Git (ex: v1.2.0). Pousser ensuite:

```bash
git push && git push --tags
```

#### dotnet format

Uniformise le style C# selon `.editorconfig`.

Commande manuelle:

```bash
dotnet format
```

Vérification sans modifier:

```bash
dotnet format --verify-no-changes
```

### Game-CI

Nous utilisons [Game-CI](https://game.ci/) pour automatiser les builds Unity.

### Unity Hub

Utiliser [Unity Cloud](https://cloud.unity.com/) pour la gestion des projets annexes.

> [!NOTE]
> Nous avons une organisation Unity Teams :
>
> Elle nous permet de créer plusieurs projets (assets, maps, sandbox, PoC) visibles par tous, utiles au début du développement lors des phases d’expérimentation, puis de les regrouper dans un seul projet par la suite.

---

### Rappel

> [!TIP]
>
> **Avant chaque commit**
>
> ```bash
> git pull --rebase  # pour récupérer les dernières modifications
> dotnet husky run   # run hooks
> git commit
> git push --tags    # si vous avez créé un nouveau tag
> ```
>
> *Respecter Conventional Commits = changelog auto + releases propres*.

---

## Conventions

### Unity

#### Version

> [!IMPORTANT]
>
> Nous utilisons la **dernière version LTS** (Long Term Support) de Unity.
>
> [**Unity 6.0 LTS**](https://unity3d.com/unity/whats-new/6000.0.59f2)

### Nommage

Les paramètres de nommages .NET et C# sont aussi configures dans [.editorconfig](.editorconfig).

#### Variables (C#)

- Les noms des classes, méthodes, énumérations, champs publics, propriétés publiques et espaces de noms : **PascalCase**.
- Les noms des variables locales et des paramètres : **camelCase**.
- Les noms des champs et propriétés privés, protégés, internes et protégés internes : **_camelCase** (avec un underscore initial).
- La convention de nommage n’est pas affectée par les modificateurs comme `const`, `static`, `readonly`, etc.
- Pour la casse, un « mot » est tout ce qui s’écrit sans espace interne, y compris les acronymes. Par exemple, `MonRpc` au lieu de `MonRPC`.
- Les noms des interfaces commencent par un `I`, par exemple : `IInterface`.

#### Fichiers

- Les noms de fichiers et de dossiers sont en **PascalCase**, par exemple : `MonFichier.cs`.
- Le nom du fichier doit correspondre au nom de la classe principale qu’il contient, par exemple : `MaClasse.cs`.
- En général, faire un fichier par classe.

### Git

#### Branches

Stratégie de branches : Stratégie GitFlow (simplifiée) :

- `main` : Versions stables, jouables ("releases").
- `beta` : Versions de test. Contient des fonctionnalités complètes mais peut encore avoir des bugs.
- `develop` : Branche d'intégration. Version de travail la plus à jour.
- `feature/nom-de-la-feature` : Chaque membre travaille sur un.e feature/fix/refactor dans sa propre branche, puis la merge dans develop via un Merge Request (ou Pull Request).

> [!IMPORTANT]
>
> **Processus de dev**
>
> ```mermaid
> %%{ init : { "theme" : "default" } }%%
> 
> graph TD
>   subgraph Releases
>     M[main]
>   end
> 
>   subgraph Testing
>     B[beta]
>   end
> 
>   subgraph Integration
>     D[develop]
>   end
> 
>   subgraph Development
>     F1[feature/xyz]
>     F2[fix/abc]
>     F3[refactor/bla]
>   end
> 
>   F1 --> D
>   F2 --> D
>   F3 --> D
> 
>   D -- Pull Request --> B
>   B -- Pull Request --> M
> ```

#### Commits

Voir [Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/).

**Feature**

```git
feat: add player movement
```

**Fix**

```git
fix: correct typo in player controller
```

**Changement majeur**

```git
fix: player movement

BREAKING CHANGE: change player movement speed from 5 to 10
```

> [!TIP]
> En résumé :
> - Prefixer le message de commit avec un type (`feat`, `fix`, `docs`, `style`, `refactor`, `test`, `chore`).
> - Utiliser le présent et l'impératif.
> - Ajouter une description si besoin.
> - Ajouter des "breaking changes" si besoin.

Dans ce projet, pour faciliter la redaction des messages de commit, nous utilisons [Commitizen](#commitizen).
