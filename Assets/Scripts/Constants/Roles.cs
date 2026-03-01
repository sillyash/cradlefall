using System.Collections.Generic;
using UnityEngine;

namespace Constants
{
    public class Roles
    {
        public enum Role
        {
            DPS,
            Technicien,
            Soigneur,
            Tank
        }

        public const string DPS = "DPS";
        public const string TECH = "Technicien";
        public const string HEAL = "Soigneur";
        public const string TANK = "Tank";

        public static List<string> roles = new List<string> { DPS, TECH, HEAL, TANK };

        public static Dictionary<int, string> RolesMap = new Dictionary<int, string>()
        {
            { 0, DPS },
            { 1, TECH },
            { 2, HEAL },
            { 3, TANK },
        };
    }
}
