using System.Collections.Generic;
using UnityEngine;

namespace Constants
{
    public class Scenes
    {
        public static readonly string MainMenu = "MainMenu";
        public static readonly string MainGame = "MainGame";
        public static readonly string MainGameMulti = "MainGame (MULTI)";
        public static readonly string LoseMenu = "LoseMenu";
        public static readonly string VictoryMenu = "VictoryMenu";
        public static readonly string MultiScreen = "MultiScreen";

        public static List<string> sceneList = new List<string> { MainMenu, MainGame, MainGameMulti, LoseMenu, VictoryMenu, MultiScreen };

        public static bool isValidScene(string scene) { return sceneList.Contains(scene); }
    }
}
