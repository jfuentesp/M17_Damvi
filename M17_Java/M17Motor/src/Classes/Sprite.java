package Classes;

public class Sprite extends Component {

	private GameObject m_Owner;
	private String m_Path;
	private String m_Description;
	
	public Sprite(GameObject owner, String path, String description) 
	{
		super("Sprite");
		this.m_Owner = owner;
		this.m_Path = path;
		this.m_Description = description;
		System.out.println(String.format("Añadido un componente Sprite para el objeto {0}, tomando el sprite <<{1}>> y con descripción {2}.", 
				m_Owner.getName(), path, description));
	}

	public void ChangePath(String path)
	{
		this.m_Path = path;
		System.out.println("Cambiada la imagen del componente Sprite del objeto " + m_Owner.getName() + " a " + path);
	}
	
	public void ChangeDescription(String description)
	{
		this.m_Description = description;
		System.out.println("Cambiada la descripción del componente Sprite del objeto " + m_Owner.getName() + " a " + description);
	}
	
	@Override
	public void update() {}

	public String getPath() {
		return m_Path;
	}

	public String getDescription() {
		return m_Description;
	}	
}