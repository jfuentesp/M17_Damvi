package Classes;

import java.util.ArrayList;
import java.util.LinkedList;
import java.util.Queue;

public class Pool extends Component 
{
	private ArrayList<GameObject> m_GameObjects;
	private Queue<GameObject> m_ActiveGameObjects;
	private GameObject m_GameObjectToPool;
	private int m_Quantity;
	
	public Pool(GameObject gameObjectToPool, int quantity) 
	{
		super("Pool");
		this.m_GameObjects = new ArrayList<>();
		this.m_ActiveGameObjects = new LinkedList<>();
		this.m_GameObjectToPool = gameObjectToPool;
		this.m_Quantity = quantity;
		start();
	}
	
	public void start()
	{
		for(int i = 0; i < m_Quantity; i++)
		{
			m_GameObjectToPool.SetActive(false);
			this.m_GameObjects.add(m_GameObjectToPool);
		}
	}
	
	@Override
	public void update() {}
	
	public GameObject getElement()
	{
		for (GameObject gameObject : m_GameObjects) 
		{
			if (!gameObject.GetActive())
			{
				gameObject.SetActive(true);
				m_ActiveGameObjects.add(gameObject);
				return gameObject;
			}
		}	
		return m_ActiveGameObjects.poll();
	}

	public void tryReturnElement(GameObject gameObject)
	{
		for (GameObject gameObjectOnList : m_GameObjects) {
			if(gameObjectOnList.equals(gameObject))
			{
				gameObjectOnList.SetActive(false);
				return;
			}
		}
		System.out.println("Pool => Se ha intentado retornar un objeto a la Pool pero no se ha encontrado el objeto activo correspondiente.");
	}
	
}
