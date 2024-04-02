package Classes;

import Motor.GameLoop;

public class SpawnerBehaviour extends Component {

	private GameObject m_Owner;
	private int m_QuantityOfObjectsToSpawn;
	private float m_SpawnTime;
	private GameLoop m_GameLoop;
	private float m_ElapsedTime;
	
	
	public SpawnerBehaviour(GameObject owner, int quantityOfObjectsToSpawn, float spawnTime) {
		super("Spawner");
		this.m_Owner = owner;
		this.m_QuantityOfObjectsToSpawn = quantityOfObjectsToSpawn;
		this.m_SpawnTime = spawnTime;
		this.m_GameLoop = GameLoop.getInstance();
	}

	@Override
	public void update() 
	{
		m_ElapsedTime += m_GameLoop.getDeltaTime();
		System.out.println("Spawner => delta time obtenido " + m_ElapsedTime);
		if(m_ElapsedTime >= m_SpawnTime && m_QuantityOfObjectsToSpawn > 0)
		{
			System.out.println("Spawner: Spawneando un nuevo Dummy.");
			SpawnDummy();
			m_ElapsedTime = 0;
			m_QuantityOfObjectsToSpawn--;
		}
	}
	
	public void SpawnDummy()
	{
		GameObject dummy = new GameObject("Dummy");
		Transform dummyTransform = dummy.getComponent(Transform.class);
		DummyBehaviour dummyComponent = new DummyBehaviour(dummy);
		dummy.addComponent(dummyComponent);
		Health healthComponent = new Health(dummy, 5);
		dummy.addComponent(healthComponent);
		dummyTransform.setPosition(13, 0, 0);
	}

}