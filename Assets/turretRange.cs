using UnityEngine;

//reference: https://discussions.unity.com/t/linerenderer-to-create-an-ellipse/433603
public class turretRange : MonoBehaviour
{
    [SerializeField] private GameObject rangeIndicator;
    [SerializeField] private float range = 15f;
    [SerializeField] private Color rangeColor = new Color(0, 1, 0, 0.3f); //start with valid color first

    void Start()
    {
        rangeIndicator.transform.localScale = Vector3.one * (range * 2); // Diameter = range * 2
        rangeIndicator.SetActive(false);
    }

    public void SetVisible(bool isVisible) 
    {
        rangeIndicator.SetActive(isVisible);
    }

    /*
    public int segments;
	public float xradius;
	public float yradius;
	LineRenderer line;

    void Start()
    {
        line = gameObject.GetComponent<LineRenderer>();
		
		line.SetVertexCount(segments + 1);
		line.useWorldSpace = false;
		CreatePoints();
        SetVisible(true);
    }

    void CreatePoints ()
	{
		float x;
		float y;
		float z = 0f;
		
		float angle = 20f;
		
		for (int i = 0; i < (segments + 1); i++)
		{
			x = Mathf.Sin (Mathf.Deg2Rad * angle) * xradius;
			y = Mathf.Cos (Mathf.Deg2Rad * angle) * yradius;
      		        
			line.SetPosition (i,new Vector3(x,y,z) );
      		        
			angle += (360f / segments);
		}
	}

    public void SetVisible(bool isVisible){
        line.enabled = isVisible;
    }
    */
}
