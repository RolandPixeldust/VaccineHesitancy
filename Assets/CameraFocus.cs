using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Rendering;
using Sirenix.OdinInspector;

public class CameraFocus : MonoBehaviour
{
	public Transform focusObj;
	public Transform camera;

	public float cameraDist;
	[Space]
	public float offset=0;
	public float buffer=0;
	public float nearRange = 1;
	public float farRange=1;
	public bool showGizmo=true;
	[Space]
	public Volume volume;
	public VolumeProfile volumeProfile;
	public DepthOfField depthOfField;

	private void Awake() => Initialize();
	
	[Button]
	void Initialize()
	{
		if (!depthOfField && volume)
		{
			volumeProfile = volume.profile;
			volumeProfile.TryGet<DepthOfField>(out depthOfField);		
		}
	}

    void Update()
    {
		if(!camera || !focusObj) return;
        cameraDist = Vector3.Distance(focusObj.position,camera.position);

		if (depthOfField)
		{
			depthOfField.nearFocusStart.value = cameraDist- nearRange - buffer + offset;
			depthOfField.nearFocusEnd.value = cameraDist - buffer + offset;

			depthOfField.farFocusStart.value = cameraDist+ buffer + offset;
			depthOfField.farFocusEnd.value = cameraDist+farRange+ buffer + offset;
		}
    }

	private void OnDrawGizmos()
	{
		if (!camera || !focusObj || !showGizmo) return;
		var dir = (focusObj.position- camera.position).normalized;
	
		Debug.DrawLine(camera.position, camera.position + dir * (cameraDist - buffer + offset), Color.yellow);
		Debug.DrawLine(camera.position + dir * (cameraDist - nearRange - buffer + offset - .02f) - camera.right * .5f, camera.position + dir * (cameraDist - nearRange - buffer + offset - .02f) + camera.right*.5f, Color.yellow);

		Debug.DrawLine(camera.position + dir * (cameraDist - buffer + offset - .02f) - camera.right, camera.position + dir * (cameraDist - nearRange - buffer + offset - .02f) - camera.right * .5f, Color.yellow);
		Debug.DrawLine(camera.position + dir * (cameraDist - buffer + offset - .02f) + camera.right, camera.position + dir * (cameraDist - nearRange - buffer + offset - .02f) + camera.right * .5f, Color.yellow);

		Debug.DrawLine( camera.position + dir * (cameraDist - buffer + offset -.02f) -camera.right, camera.position + dir * (cameraDist - buffer + offset - .02f) + camera.right, Color.yellow);
		Debug.DrawLine(camera.position + dir * (cameraDist - buffer + offset + .02f) - camera.right, camera.position + dir * (cameraDist - buffer + offset + .02f) + camera.right, Color.red);
		
		Debug.DrawLine(camera.position + dir * (cameraDist - buffer + offset), camera.position + dir * (cameraDist + buffer + offset), Color.red);

		Debug.DrawLine(camera.position + dir * (cameraDist + buffer + offset - .02f) - camera.right, camera.position + dir * (cameraDist + buffer + offset - .02f) + camera.right, Color.red);
		Debug.DrawLine(camera.position + dir * (cameraDist + buffer + offset + .02f) - camera.right, camera.position + dir * (cameraDist + buffer + offset + .02f) + camera.right, Color.blue);

		Debug.DrawLine(camera.position + dir * (cameraDist + farRange + buffer + offset) - camera.right * 1.5f, camera.position + dir * (cameraDist + buffer + offset + .02f) - camera.right, Color.blue);
		Debug.DrawLine(camera.position + dir * (cameraDist + farRange + buffer + offset) + camera.right * 1.5f, camera.position + dir * (cameraDist + buffer + offset + .02f) + camera.right, Color.blue);

		Debug.DrawLine(camera.position + dir * (cameraDist + buffer + offset), camera.position + dir * (cameraDist + farRange + buffer + offset),Color.blue);
		Debug.DrawLine(camera.position + dir * (cameraDist + farRange + buffer + offset) - camera.right * 1.5f, camera.position + dir * (cameraDist + farRange + buffer + offset) + camera.right * 1.5f, Color.blue);
	}
}
