using RimWorld;
using Verse;
using Verse.Sound;
using UnityEngine;

namespace CraftingHysteresis
{
	public class Bill_Production_Hysteresis : Bill_Production
	{
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
					return "";
				}
			}
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
			if (pauseOnCompletion && this.recipe.WorkerCounter.CountProducts(this) >= targetCount)
			{
				paused = true;
			}
			
			if (unpauseOnExhaustion && this.recipe.WorkerCounter.CountProducts(this) < unpauseThreshold)
			{
				paused = false;
			}
			
			if (paused)
			{
				return false;
			}
			
			return base.ShouldDoNow();
		}
		
		public static readonly Texture2D Plus = ContentFinder<Texture2D>.Get("UI/Buttons/Plus", true);
		public static readonly Texture2D Minus = ContentFinder<Texture2D>.Get("UI/Buttons/Minus", true);
		
		protected override void DrawConfigInterface(Rect baseRect, Color baseColor)
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
			if (widgetRow.ButtonText(this.repeatMode.GetLabel().PadRight(20), null, true, false))
			{
				BillRepeatModeUtility.MakeConfigFloatMenu(this);
			}
			if (widgetRow.ButtonIcon(Plus, null))
			{
				if (this.repeatMode == BillRepeatMode.Forever)
				{
					this.repeatMode = BillRepeatMode.RepeatCount;
					this.repeatCount = 1;
				}
				else if (this.repeatMode == BillRepeatMode.TargetCount)
				{
					this.targetCount += this.recipe.targetCountAdjustment;
				}
				else if (this.repeatMode == BillRepeatMode.RepeatCount)
				{
					this.repeatCount++;
				}
				SoundDefOf.AmountIncrement.PlayOneShotOnCamera();
			}
			if (widgetRow.ButtonIcon(Minus, null))
			{
				if (this.repeatMode == BillRepeatMode.Forever)
				{
					this.repeatMode = BillRepeatMode.RepeatCount;
					this.repeatCount = 1;
				}
				else if (this.repeatMode == BillRepeatMode.TargetCount)
				{
					this.targetCount = Mathf.Max(0, this.targetCount - this.recipe.targetCountAdjustment);
				}
				else if (this.repeatMode == BillRepeatMode.RepeatCount)
				{
					this.repeatCount = Mathf.Max(0, this.repeatCount - 1);
				}
				SoundDefOf.AmountDecrement.PlayOneShotOnCamera();
			}
		}
	}
}

