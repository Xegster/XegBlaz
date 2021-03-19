namespace XegDoKu.Utilities
{
	public enum Difficulty
	{
		Easy,
		Medium,
		Hard,
		Impossible
	}

	public enum Quadrant
	{
		UpperLeft = 0,      //0 0
		UpperMiddle = 1,    //0 3
		UpperRight = 2,     //0 6
		MiddleLeft = 3,     //4 0
		MiddleMiddle = 4,   //4 3 
		MiddleRight = 5,    //4 6
		LowerLeft = 6,      //6 0
		LowerMiddle = 7,    //6 3
		LowerRight = 8      //6 6
	}

	public enum SolutionState
	{
		Processing,
		Impossible,
		Solved
	}

	public enum SudokuSize
	{
		ShiDoku = 4,
		GoDoku = 5,
		RokuDoku = 6,
		Standard = 9,
		Maxi = 12,
		Giant = 25,
		SudokuZilla = 100

	}
}
