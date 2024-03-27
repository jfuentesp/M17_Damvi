package Classes;

public class Collider extends Component 
{
	private GameObject m_Owner;
	private Transform m_OwnerTransform;
	
	public Collider(GameObject owner) 
	{
		super("Collider");
		this.m_Owner = owner;
	}

	@Override
	public void update() 
	{

	}

}
