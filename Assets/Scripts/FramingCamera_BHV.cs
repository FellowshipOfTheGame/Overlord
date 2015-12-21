using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FramingCamera_BHV : MonoBehaviour {

	public Canvas framesCanvas;
    public Transform targetTransform;
    public float interpSpeed = 0.1f;
    public bool dynamicFrames = false;

    private List<RectTransform> frameList;
    public float defaultCameraSize = 3f;
    protected Camera myCamera;

    void Start() {
        myCamera = this.GetComponent<Camera>();
        GetFrameList();
    }

    void Update() {
        if (dynamicFrames) {
            GetFrameList();
        }
        RectTransform currentFrame = FindCurrentFrame();
        UpdateCamera(currentFrame);
    }

    protected virtual void UpdateCamera(RectTransform frame){
        Vector3 goalCameraPosition;
        float goalCameraSize;
        Vector3 cameraMins = myCamera.ViewportToWorldPoint(Vector3.zero);
        Vector3 cameraMaxs = myCamera.ViewportToWorldPoint(new Vector3(1, 1, 0));
        Rect cameraBox = new Rect(cameraMins.x, cameraMins.y, cameraMaxs.x - cameraMins.x, cameraMaxs.y - cameraMins.y);
        if (frame == null) {
            Rect canvasBox = new Rect(framesCanvas.GetComponent<RectTransform>().rect);
            canvasBox.position = canvasBox.position + (Vector2)framesCanvas.transform.position;

            goalCameraPosition = targetTransform.position;
            if (goalCameraPosition.x < canvasBox.xMin + (cameraBox.width / 2)) {
                goalCameraPosition.x = canvasBox.xMin + (cameraBox.width / 2);
                Debug.Log(myCamera.rect.width);
                Debug.Log(canvasBox.xMin);
                Debug.Log(goalCameraPosition.x);
            }
            else if (goalCameraPosition.x > canvasBox.xMax - (cameraBox.width / 2)) {
                goalCameraPosition.x = canvasBox.xMax - (cameraBox.width / 2);
            }
            if (goalCameraPosition.y < canvasBox.yMin + (cameraBox.height / 2)) {
                goalCameraPosition.y = canvasBox.yMin + (cameraBox.height / 2);
            }
            else if (goalCameraPosition.y > canvasBox.yMax - (cameraBox.height / 2)) {
                goalCameraPosition.y = canvasBox.yMax - (cameraBox.height / 2);
            }

            goalCameraSize = defaultCameraSize;
        }
        else {
            goalCameraPosition = (Vector2)frame.transform.position;
            float cameraDims = cameraBox.width / cameraBox.height;
            float frameDims = frame.rect.width / frame.rect.height;
            if (cameraDims >= frameDims) {//camera is wider than frame -> fit camera height
                goalCameraSize = myCamera.orthographicSize * (frame.rect.height / cameraBox.height);
            }
            else {//frame is wider than camera -> fit camera width
                goalCameraSize = myCamera.orthographicSize * (frame.rect.width / cameraBox.width);
            }
        }

        Vector2 newPos = Vector2.Lerp(this.transform.position, goalCameraPosition, interpSpeed);
        this.transform.position = new Vector3(newPos.x, newPos.y, this.transform.position.z);
        myCamera.orthographicSize = Mathf.Lerp(myCamera.orthographicSize, goalCameraSize, interpSpeed);

    }

    protected virtual RectTransform FindCurrentFrame(){
        if (frameList != null) {
            float minDist = float.MaxValue;
            RectTransform minFrame = null;
            foreach (RectTransform curRT in frameList) {
                Rect frame = new Rect(curRT.rect);
                frame.position = frame.position + (Vector2)curRT.transform.position;
                float curDist = ((Vector2)this.transform.position - frame.position).magnitude;
                if(frame.Contains((Vector2)targetTransform.position)){
                    if(curDist < minDist){
                        minDist = curDist;
                        minFrame = curRT;
                    }
                }
            }
            return minFrame;
        }
        return null;
    }

    private void GetFrameList() {
        frameList = new List<RectTransform>();
        frameList.AddRange(framesCanvas.gameObject.GetComponentsInChildren<RectTransform>());
        frameList.Remove(framesCanvas.GetComponent<RectTransform>());
        Debug.Log(frameList.Count);
        for (int i = 0; i < frameList.Count; i++) {
            Debug.Log("Elemento: " + i);
            Debug.Log(frameList[i]);
            Debug.Log(frameList[i].rect);
            Debug.Log(frameList[i].rect.position);
        }

    }

}
