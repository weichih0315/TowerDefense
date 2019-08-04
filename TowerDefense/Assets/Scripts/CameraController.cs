using UnityEngine;

public class CameraController : MonoBehaviour {

	public float panSpeed = 30f;
	public float panBorderThickness = 10f;

	public float scrollSpeed = 5f;
    public Vector3 maxBorder;
    public Vector3 minBorder;

    // Update is called once per frame
    void Update () {

		/*if (GameManager.GameIsOver)
		{
			this.enabled = false;
			return;
		}*/

		if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - panBorderThickness)
		{
            transform.Translate(Vector3.forward * panSpeed * Time.deltaTime, Space.World);
		}
		if (Input.GetKey("s") || Input.mousePosition.y <= panBorderThickness)
		{
			transform.Translate(Vector3.back * panSpeed * Time.deltaTime, Space.World);
		}
		if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBorderThickness)
		{
			transform.Translate(Vector3.right * panSpeed * Time.deltaTime, Space.World);
		}
		if (Input.GetKey("a") || Input.mousePosition.x <= panBorderThickness)
		{
			transform.Translate(Vector3.left * panSpeed * Time.deltaTime, Space.World);
		}

		float scroll = Input.GetAxis("Mouse ScrollWheel");

		Vector3 pos = transform.position;
        
		pos.y -= scroll * 1000 * scrollSpeed * Time.deltaTime;

        pos.x = Mathf.Clamp(pos.x, minBorder.x, maxBorder.x);
        pos.y = Mathf.Clamp(pos.y, minBorder.y, maxBorder.y);
        pos.z = Mathf.Clamp(pos.z, minBorder.z, maxBorder.z);

        transform.position = pos;

	}
}
