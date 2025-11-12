** Generation Procedurale **
--------------------------------------------------

Ce projet possède trois algorithme de génération procédurale pour Unity 6 en C#. Chaque algorithmes génère différents types de terrains et d'environnements adaptés au développement de jeux.

* BSP : génère une architecture de donjon avec des salles et des couloirs.

* Cellular Automata : produit des environnements pouvant être utiliser pour donner des aspects de formations naturelles d'îles ou de grottes.

* Génération d'un fractale noise : permet de créer des textures, paysages , des biomes ou des effets réalistes, sans avoir besoin de tout modéliser à la main.


Chaque algorithme utilise l'architecture ScriptableObject d'Unity et utilisent UniTask pour sa géneration.
La géneration est réaliser sur une grille dont les paramètres peuvent être modifier depuis l'inspecteur d'Unity.





## 1. BSP (`BSP.cs`)

Crée des pièces et des couloirs interconnectés en divisant récursivement l'espace de la grille.

* Utilise la subdivision récursive  pour le placement des pièces
* La génération des couloirs entre les pièces s'effectue en liant les leafs et le sisters enssemble
* Dimensions des pièces configurables

<center>
    <img src="generation procedural\Assets\Screenshot/BSP.png">
</center>

### fonctionnement de l'algorithme

Division récursive de la grille en rectangles plus petits puis créer des pièces dans les nœuds terminaux et réalise la connexion des pièces par des couloirs en L


Le BSP peut servir à gérer de grandes cartes divisées en zones logiques.
Cela nous permet un chargement dynamique pour la géneration d'un donjon par exemple




## 2. Cellular Automata (`CellularAutomata.cs`)

Simule la formation organique de grottes ou d'une île à l'aide de règles d'automates cellulaires sur plusieurs itérations.



Processus de simulation itératif
Formations de grottes d'aspect naturel
Génération de montagnes dans les zones de sol dense
Génération visuelle étape par étape avec délais
Densité et seuils configurables





### fonctionnement de l'algorithme

*Initialise de la grille avec un noise aléatoire

*Application des règles du Cellular Automata:
    4 voisins sol → devient du sol
    moins de 4 voisins sol→ devient de l'eau

*Génération du terrain 

*Visualisation de chaque itération avec délai


Le Cellular Automata nous est utile pour modéliser la propagation, la diffusion ou l'évolution d’écosystèmes, comme par exemples un réseaux de grottes.





## 3. Génération d'un fractale noise (`NoiseGenerator.cs`)

Génère un terrains à l'aide de FastNoiseLite avec des motifs d'un fractale noise. Cette méthode peut nous permetre de crée  différents biomes, paysage en fonction des valeurs attribuer. 
Le résultat est modifiable via les parametres du Script ProceduralGridGenerator.cs .

Un fractal noise peut donc nous être utile pour générer des textures de roches, nuages, marbre, bois, eau, etc.
Le bruit fractal crée des motifs, des textures, des biomes sans répétition visible, très naturels ou des effet visuel.
Et en combinant plusieurs bruits, on peut même générer un monde complet.





## Comment créer mon algorithme

```csharp


using System.Threading;
using Components.ProceduralGeneration;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;


[CreateAssetMenu(menuName = "Procedural Generation Method/TON NOM D'ALGO")]

public class MyAlgo : ProceduralGenerationMethod
{
    protected override async UniTask ApplyGeneration(CancellationToken cancellationToken)
    {
        // Declare variables here
        // ........
        int nbRoom = 0;
        for (int i = 0; i < _maxSteps; i++)
        {

            // Check for cancellation
            cancellationToken.ThrowIfCancellationRequested();

            // Your algorithm here
            // .......







            // Waiting between steps to see the result.
            await UniTask.Delay(GridGenerator.StepDelay, cancellationToken: cancellationToken);
        }



    }
}

```

