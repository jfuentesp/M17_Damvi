package Motor;

public class GameLoop 
{

	public static GameLoop Instance = null;
	
	public static void main(String[] args) 
	{
		// TODO Auto-generated method stub

	}

	public static void init()
	{
		
	}
	
	public static void input()
	{
		//Function where we check the inputs made by the user 
	}
	
	public static void update(float deltaTime)
	{
		//Function where we execute the different actions of the game elements, keeping the timelapse since the last update (deltaTime)
		//Deltatime is an increment, meaning the difference of time between an update and the previous one. 
		/*
		 * There are different ways to handle the deltaTime matter. 
		 * 	- Passing it as update() parameter so every gameobject can have it as reference.
		 *  - Making is as a static class Time (like unity has Time.deltaTime class and as a public variable)
		 *  - Having it set as a read only variable in this own class GameLoop, having a getter to access it whenever a class need it. 
		 */
	}
	
	//Render won't be needed since we are not going to show graphics on screen, just prompts.
	/*public static void render()
	{
		//Function where we show on screen how the different elements of the game behave
	}*/
	
	public GameLoop getInstance()
	{
		if(Instance == null)
			Instance = this;
		return Instance;
	}
}
