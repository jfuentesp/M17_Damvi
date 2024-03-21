package Main;

import Classes.GameObject;
import Classes.Transform;
import Motor.GameLoop;

public class Main {
	
	public static void main(String[] args) {
		
		//Initializing the GameLoop. It's a singleton so we initiate it using the method getInstance()
		GameLoop gameLoop = GameLoop.getInstance();
		System.out.println("GameLoop cargado. (Capturada la instancia del Singleton)");
		
		GameObject shooter = new GameObject("Shooter");
		Transform shooterTransform = shooter.getComponent(Transform.class);
		shooterTransform.setPosition(-13, 0, 0);
		
		GameObject dummy = new GameObject("Dummy");
		Transform dummyTransform = dummy.getComponent(Transform.class);
		dummyTransform.setPosition(13, 0, 0);
		
		System.out.println("Iniciando GameLoop...");
		gameLoop.init();
		
		
		
		
	}
}