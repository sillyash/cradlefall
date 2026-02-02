using System.Collections.Generic;
using UnityEngine;

namespace Constants
{
    public class Roles
    {
        public const string DPS = "DPS";
        public const string TECH = "Technicien";

        public static List<string> roles = new List<string> { DPS, TECH };

        public static Dictionary<int, string> RolesMap = new Dictionary<int, string>()
        {
            { 0, DPS },
            { 1, TECH },
        };
    }
}
