using CommunityCoreLibrary;
using CommunityCoreLibrary.Detour;
using System.Reflection;

namespace CraftingHysteresis
{
    public class DetourInjector : SpecialInjector
    {
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
            
        	/*{
	        	MethodInfo method1 = typeof(RimWorld.Bill).GetMethod("DrawInterface", BindingFlags.Instance | BindingFlags.Public);
	            MethodInfo method2 = typeof(Bill_Detour).GetMethod("DrawInterface", BindingFlags.Static | BindingFlags.Public);
	            if (!Detours.TryDetourFromTo(method1, method2))
	            {
	                return false;
	            }
        	}*/
            
            return true;
        }
    }
}