package Classes;

import Enum.RigidbodyType;

public class Rigidbody extends Component {

	private GameObject m_Owner;
	private RigidbodyType m_Type = RigidbodyType.DYNAMIC;
	private float m_Velocity;
	private float m_Drag = 0.1f;
	private float m_GravityForce = 10;
	
	public Rigidbody(GameObject owner, Transform transform) 
	{
		super("Rigidbody");
		this.m_Owner = owner;
		System.out.println("Añadido un componente Rigidbody al objeto " + owner.getName());
	}

	public void SetBodyType(RigidbodyType type)
	{
		this.m_Type = type;
		System.out.println("El Rigidbody del objeto " + m_Owner.getName() + " ha cambiado el tipo de cuerpo a " + type);
	}
	
	public void SetVelocity(float velocity)
	{
		this.m_Velocity = velocity;
		System.out.println("La velocidad del objeto " + m_Owner.getName() + " ha cambiado a " + this.m_Velocity);
	}
	
	public void SetDrag(float drag)
	{
		this.m_Drag = drag;
		System.out.println("El rozamiento del objeto " + m_Owner.getName() + " ha cambiado a " + this.m_Drag);
	}
	
	public void SetGravity(float gravity)
	{
		this.m_GravityForce = gravity;
		System.out.println("La fuerza de gravedad que afecta al objeto " + m_Owner.getName() + " ha cambiado a " + this.m_GravityForce);
	}
	
	@Override
	public void update() {}

}