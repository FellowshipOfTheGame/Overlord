using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FramingCamera_BHV : MonoBehaviour {

    //Referência para o canvas que contem os frames
	public Canvas framesCanvas;

    //Referência para o Transform alvo a ser seguido
    public Transform targetTransform;

    //Define uma velocidade de interpolação da posição e da escala da camera
    public float interpSpeed = 0.1f;

    //Define se os frames considerados pelo script são dinamicos (atualizam a todo frame)
    public bool dynamicFrames = false;

    //Define o tamanho padrão da camera ortográfica para quando nenhum frame está sendo usado
    public float defaultCameraSize = 3f;


    //Variavel interna de referência para o componente camera desse objeto
    protected Camera myCamera;

    //Variavel interna que guarda a lista dos frames sendo considerados
    private List<RectTransform> frameList;

    //Inicializa as variaveis internas
    void Start() {
        myCamera = this.GetComponent<Camera>();
        GetFrameList();
    }

    //A todo update, acha o quadro e interpola a camera para ele
    void Update() {
        if (dynamicFrames) {
            GetFrameList();
        }
        RectTransform currentFrame = FindCurrentFrame();
        UpdateCamera(currentFrame);
    }

    //Faz a interpolação da posição e do tamanho da camera para o quadro passado (ou para o alvo padrão no caso de valor nulo)
    protected virtual void UpdateCamera(RectTransform frame){
        Vector3 goalCameraPosition;
        float goalCameraSize;
        Vector3 cameraMins = myCamera.ViewportToWorldPoint(Vector3.zero);
        Vector3 cameraMaxs = myCamera.ViewportToWorldPoint(new Vector3(1, 1, 0));
        Rect cameraBox = new Rect(cameraMins.x, cameraMins.y, cameraMaxs.x - cameraMins.x, cameraMaxs.y - cameraMins.y);
        if (frame == null) {
            Rect canvasBox = new Rect(framesCanvas.GetComponent<RectTransform>().rect);
            canvasBox.position += (Vector2)framesCanvas.transform.position;

            goalCameraPosition = targetTransform.position;
            if (goalCameraPosition.x < canvasBox.xMin + (cameraBox.width / 2)) {
                goalCameraPosition.x = canvasBox.xMin + (cameraBox.width / 2);
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

    //Função retorna o quadro mais próximo do alvo em que ele estiver contido
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

    //Função que atualiza a lista de frames do mundo
    private void GetFrameList() {
        frameList = new List<RectTransform>();
        frameList.AddRange(framesCanvas.gameObject.GetComponentsInChildren<RectTransform>());
        frameList.Remove(framesCanvas.GetComponent<RectTransform>());
    }

}
