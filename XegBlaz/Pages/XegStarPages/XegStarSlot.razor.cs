using Microsoft.AspNetCore.Components;
using XegStar.Models;
namespace XegBlaz.Pages.XegStarPages
{
	public partial class XegStarSlot
	{
		[Parameter]
		public Slot Slot { get; set; }
		[Parameter]
		public XegStarGame CurrentGame { get; set; }

		protected Player Player1 => CurrentGame.Players[0];
		protected Player Player2 => CurrentGame.Players[1];

		protected string TokenClass => (Slot.CurrentToken != null && Slot.CurrentToken.Owner.Id == Player1.Id) ? "red-token" : "blue-token";

	}
}
