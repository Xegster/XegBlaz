using XegStar;
using XegStar.Models;

namespace XegBlaz.Pages.XegStarPages
{
	public partial class Index
	{
		protected XegStarGame CurrentGame { get; set; }

		protected Player Player1 => CurrentGame.Players[0];
		protected Player Player2 => CurrentGame.Players[1];

		protected string DropClass { get; set; } = "";
		protected Token DragToken { get; set; }
		protected Slot TargetSlot { get; set; }
		protected override void OnInitialized()
		{
			CurrentGame = XegStarFactory.BuildBasicGame();
		}
		private void HandleDragStart(Token selectedToken)
		{
			DragToken = selectedToken;
		}
		protected void HandleDragEnter(Slot slot)
		{
			TargetSlot = slot;

		}
		protected void HandleDragLeave()
		{
			//TargetSlot = null;
		}
		private void HandleDrop()
		{
			TargetSlot.AddToken(DragToken);
		}
	}
}
