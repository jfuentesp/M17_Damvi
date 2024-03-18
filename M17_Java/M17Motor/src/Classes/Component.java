package Classes;

public abstract class Component {
	@SuppressWarnings("unused")
	private String m_Name = "Component";
	
	public Component(String name) {
		this.m_Name = name;
	}
	
	public abstract void update();
}
