using CommunityCoreLibrary_CraftingHysteresis;
using System;
using System.Reflection;
using UnityEngine;
using Verse;

namespace CraftingHysteresis
{
    class Bootstrap : Def
    {
        public static FieldInfo ButtonPlus;
        public static FieldInfo ButtonMinus;

        public string ModName;

        static Bootstrap()
        {
            {
                MethodInfo method1 = typeof(RimWorld.BillUtility).GetMethod("MakeNewBill", BindingFlags.Static | BindingFlags.Public);
                MethodInfo method2 = typeof(BillUtility_Detour).GetMethod("MakeNewBill", BindingFlags.Static | BindingFlags.Public);
                if (!Detours.TryDetourFromTo(method1, method2))
                {
                    Debug.LogError("EVERYTHING IS BROKEN");
                    return;
                }
            }

            Assembly Assembly_CSharp = Assembly.Load("Assembly-CSharp.dll");

            Type Verse_TexButton = Assembly_CSharp.GetType("Verse.TexButton");
            ButtonPlus = Verse_TexButton.GetField("Plus", BindingFlags.Static | BindingFlags.Public);
            ButtonMinus = Verse_TexButton.GetField("Minus", BindingFlags.Static | BindingFlags.Public);
        }
    }
}
