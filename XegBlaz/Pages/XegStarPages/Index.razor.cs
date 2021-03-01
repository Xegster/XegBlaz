using XegStar;
using XegStar.Models;

namespace XegBlaz.Pages.XegStarPages
{
	public partial class Index
	{
		protected XegStarGame CurrentGame { get; set; }

		protected override void OnInitialized()
		{
			CurrentGame = XegStarFactory.BuildBasicGame();
		}

	}
}
