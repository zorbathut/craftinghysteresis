using RimWorld;
using Verse;
using Verse.Sound;
using UnityEngine;

namespace CraftingHysteresis
{
	public class Bill_Production_Hysteresis : Bill_Production
	{
		protected override string StatusString
		{
			get
			{
				if (paused)
				{
					return "Paused";
				}
				else
				{
					return null;
				}
			}
		}
		
		public bool paused = false;
		
		public bool pauseOnCompletion = false;
		public bool unpauseOnExhaustion = false;
		public int unpauseThreshold = 5;
		
		public Bill_Production_Hysteresis()
		{
			
		}
		
		public Bill_Production_Hysteresis(RecipeDef recipe) : base(recipe)
		{
			
		}
		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.LookValue<bool>(ref paused, "paused", false, false);
			Scribe_Values.LookValue<bool>(ref pauseOnCompletion, "pauseOnCompletion", false, false);
			Scribe_Values.LookValue<bool>(ref unpauseOnExhaustion, "unpauseOnExhaustion", false, false);
			Scribe_Values.LookValue<int>(ref unpauseThreshold, "unpauseThreshold", 0, false);
		}
		
		public override bool ShouldDoNow()
		{
			bool baseShouldDoNow = base.ShouldDoNow();
			
			if (pauseOnCompletion && !suspended && !baseShouldDoNow)
			{
				paused = true;
			}
			
			if (unpauseOnExhaustion && recipe.WorkerCounter.CanCountProducts(this) && recipe.WorkerCounter.CountProducts(this) < unpauseThreshold)
			{
				paused = false;
			}
			
			if (paused)
			{
				return false;
			}
			
			return baseShouldDoNow;
		}
		
		protected override void DoConfigInterface(Rect baseRect, Color baseColor)
		{
			Rect rect = new Rect(28f, 32f, 100f, 30f);
			GUI.color = new Color(1f, 1f, 1f, 0.65f);
			Widgets.Label(rect, this.RepeatInfoText);
			GUI.color = baseColor;
			WidgetRow widgetRow = new WidgetRow(baseRect.xMax, baseRect.y + 29f, UIDirection.LeftThenUp, 99999f, 4f);
			if (widgetRow.ButtonText("Details".Translate() + "...", null, true, false))
			{
				Find.WindowStack.Add(new Dialog_BillConfig_Hysteresis(this, ((Thing)this.billStack.billGiver).Position));
			}
            DrawStandardControls(this, widgetRow);
        }

        public static void DrawStandardControls(Bill_Production bill, WidgetRow widgetRow)
        {
            if (widgetRow.ButtonText(bill.repeatMode.GetLabel().PadRight(20), null, true, false))
            {
                BillRepeatModeUtility.MakeConfigFloatMenu(bill);
            }

            int amount = Event.current.shift ? 5 : 1;
            if (widgetRow.ButtonIcon((Texture2D)CraftingHysteresis.Bootstrap.ButtonPlus.GetValue(null), null))
            {
                if (bill.repeatMode == BillRepeatMode.Forever)
                {
                    bill.repeatMode = BillRepeatMode.RepeatCount;
                    bill.repeatCount = 1;
                }
                else if (bill.repeatMode == BillRepeatMode.TargetCount)
                {
                    bill.targetCount += bill.recipe.targetCountAdjustment * amount;
                }
                else if (bill.repeatMode == BillRepeatMode.RepeatCount)
                {
                    bill.repeatCount += amount;
                }
                SoundDefOf.AmountIncrement.PlayOneShotOnCamera();
                if (TutorSystem.TutorialMode && bill.repeatMode == BillRepeatMode.RepeatCount)
                {
                    TutorSystem.Notify_Event(bill.recipe.defName + "-RepeatCountSetTo-" + bill.repeatCount);
                }
            }
            if (widgetRow.ButtonIcon((Texture2D)CraftingHysteresis.Bootstrap.ButtonMinus.GetValue(null), null))
            {
                if (bill.repeatMode == BillRepeatMode.Forever)
                {
                    bill.repeatMode = BillRepeatMode.RepeatCount;
                    bill.repeatCount = 1;
                }
                else if (bill.repeatMode == BillRepeatMode.TargetCount)
                {
                    bill.targetCount = Mathf.Max(0, bill.targetCount - bill.recipe.targetCountAdjustment * amount);
                }
                else if (bill.repeatMode == BillRepeatMode.RepeatCount)
                {
                    bill.repeatCount = Mathf.Max(0, bill.repeatCount - amount);
                }
                SoundDefOf.AmountDecrement.PlayOneShotOnCamera();
                if (TutorSystem.TutorialMode && bill.repeatMode == BillRepeatMode.RepeatCount)
                {
                    TutorSystem.Notify_Event(bill.recipe.defName + "-RepeatCountSetTo-" + bill.repeatCount);
                }
            }
        }
    }
}

