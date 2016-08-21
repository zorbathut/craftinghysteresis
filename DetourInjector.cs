using CommunityCoreLibrary;
using CommunityCoreLibrary.Controller;
using CommunityCoreLibrary.Detour;
using System;
using System.Reflection;

namespace CraftingHysteresis
{
    public class DetourInjector : SpecialInjector
    {
    	public static FieldInfo    ButtonPlus;
    	public static FieldInfo    ButtonMinus;
    	
        public override bool Inject()
        {
        	{
	            MethodInfo method1 = typeof(RimWorld.BillUtility).GetMethod("MakeNewBill", BindingFlags.Static | BindingFlags.Public);
	            MethodInfo method2 = typeof(BillUtility_Detour).GetMethod("MakeNewBill", BindingFlags.Static | BindingFlags.Public);
	            if (!Detours.TryDetourFromTo(method1, method2))
	            {
	                return false;
	            }
        	}
            
        	Type Verse_TexButton = Data.Assembly_CSharp.GetType("Verse.TexButton");
        	ButtonPlus = Verse_TexButton.GetField("Plus", BindingFlags.Static | BindingFlags.Public);
        	ButtonMinus = Verse_TexButton.GetField("Minus", BindingFlags.Static | BindingFlags.Public);
            
            return true;
        }
    }
}