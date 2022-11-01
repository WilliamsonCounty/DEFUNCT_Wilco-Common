namespace Wilco;

public interface IDiscoverable<in T>
{
	bool Exists(T type);
}