package Motor;

public abstract class Character implements GameObject {
	
	private float velocity;
	private int hp;
	private int coins;
	private int lives;
	
	public Character() {
		this.velocity = 0;
		this.hp = 5;
		this.coins = 0;
		this.lives = 1;
	}	
}
