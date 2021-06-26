using Photon.Pun;

public class Singleton<T> : MonoBehaviourPun where T : MonoBehaviourPun
{
    protected static T _instance;
    
    public static T instance 
    {
        get 
        {
            if (_instance == null) 
            {
                _instance = FindObjectOfType<T>();
            }
            return _instance;
        }
    }
}
