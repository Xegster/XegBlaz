namespace XegStar.Models
{
	public class Slot
	{
		public double Multiplier { get; set; }

		public Token CurrentToken { get; set; }

		public void AddToken(Token dragToken)
		{
			var multipliedValue = CalculateValue(dragToken);
			if (CurrentToken == null)
			{
				dragToken.Value = multipliedValue;
				CurrentToken = dragToken;
			}
			else if (CurrentToken.Owner == dragToken.Owner)
				CurrentToken.Value += multipliedValue;
			else if (CurrentToken.Value > multipliedValue)
				CurrentToken.Value -= multipliedValue;
			else if (CurrentToken.Value == multipliedValue)
				CurrentToken = null;
			else
			{
				dragToken.Value = (multipliedValue - CurrentToken.Value);
				CurrentToken = dragToken;
			}
			dragToken.Owner.Tokens.Remove(dragToken);
		}

		private int CalculateValue(Token dragToken)
		{
			return (int)(dragToken.Value * Multiplier);
		}
	}
}
