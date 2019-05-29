using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SongEditor : MonoBehaviour
{
    public GameObject[] Panels;
    public Text currentRotateIndex;
    private GameObject[,] buttons = new GameObject[2,12];
    private GameObject[] rotateButtons = new GameObject[9];

    private bool[,] checkBoard = new bool[2, 12];
    private bool[] rotateBoard = new bool[9];

    private bool[,] isSeted = new bool[2, 12];
    private Vector2 ensureSet = new Vector2(0,0);
    private Stack<Vector2> redoRecord = new Stack<Vector2>();
    private Stack<int> rotateRecord = new Stack<int>();
    private int index = 0;
    private int rotateIndex;
    private int frame;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 2; i++)
            for(int j = 0 ; j < 12 ; j++){
                checkBoard[i,j] = false;
                isSeted[i,j] = false;
                buttons[i,j] = Panels[i].transform.GetChild(j).gameObject;
            }
        for(int i = 0; i < 9; i++){
            rotateBoard[i] = false;
            rotateButtons[i] = Panels[2].transform.GetChild(i).gameObject;
        }
        
        ensureSet[0] = 0;
        ensureSet[1] = 0;
    }

    public void Initialize(int time)
    {
        frame = time;

        for(int i = 0; i < 2; i++)
            for(int j = 0 ; j < 12 ; j++){
                checkBoard[i,j] = false;
                isSeted[i,j] = false;
                buttons[i,j] = Panels[i].transform.GetChild(j).gameObject;
                buttons[i,j].GetComponent<Image>().color 
                    = Color.white;
                buttons[i,j].transform.GetChild(0)
                    .GetComponent<Text>().text = "";
            }
        for(int i = 0; i < 9; i++){
            rotateBoard[i] = false;
            rotateButtons[i] = Panels[2].transform.GetChild(i).gameObject;
            rotateButtons[i].GetComponent<Image>().color 
                = Color.white;
        }

        ensureSet[0] = 0;
        ensureSet[1] = 0;

        redoRecord.Clear();
        rotateRecord.Clear();
        currentRotateIndex.text = "";
        index = 0;

        if(InfoManager.isExist(time))
        {
            frameInfo info = InfoManager.getInfo(time);
            for(int i = info.relations.Count - 1; i >= 0; i--)
            {
                redoRecord.Push(info.relations[i]);
                rotateRecord.Push(info.rotateIndice[i]);
                checkBoard[0, (int)info.relations[i][0]] = true;
                checkBoard[1, (int)info.relations[i][1]] = true;
                isSeted[0, (int)info.relations[i][0]] = true;
                isSeted[1, (int)info.relations[i][1]] = true;

                buttons[0, (int)info.relations[i][0]].GetComponent<Image>().color 
                    = Color.red;
                buttons[1, (int)info.relations[i][1]].GetComponent<Image>().color 
                    = Color.red;
                buttons[0, (int)info.relations[i][0]].transform.GetChild(0)
                    .GetComponent<Text>().text = index.ToString();
                buttons[1, (int)info.relations[i][1]].transform.GetChild(0)
                    .GetComponent<Text>().text = index.ToString();
                index++;
            }
            if(rotateRecord.Count > 0)
                currentRotateIndex.text = rotateRecord.Peek().ToString();
            else
                currentRotateIndex.text = "";
        }
    }

    public void UpdateCheckBoard(int sort, int id)
    {
        if(isSeted[sort, id])
            return;
        if(!checkBoard[sort, id])
        {
            for(int i = 0 ; i < 12; i++)
            {
                if(i == id){
                    checkBoard[sort,i] = true;
                    buttons[sort,i].GetComponent<Image>().color
                        = Color.black;
                }
                else if(!isSeted[sort,i]){
                    checkBoard[sort, i] = false;
                    buttons[sort,i].GetComponent<Image>().color
                        = Color.white;
                }
            }
        }
        else{
            checkBoard[sort, id] = false;
            buttons[sort, id].GetComponent<Image>().color
                = Color.white;
        }  
    }

    public void UpdateRotateBoard(int id)
    {
        if(!rotateBoard[id])
        {
            for(int i = 0; i  < 9; i++){
                if(i == id)
                {
                    rotateBoard[i] = true;
                    rotateButtons[i].GetComponent<Image>().color
                        = Color.black;
                }
                else
                {
                    rotateBoard[i] = false;
                    rotateButtons[i].GetComponent<Image>().color
                        = Color.white;
                }
            }
        }
        else{
            rotateBoard[id] = false;
            rotateButtons[id].GetComponent<Image>().color
                = Color.white;
        }
    }

    private bool CheckValid()
    {
        int count = 0;
        for(int i = 0; i < 9; i++){
            if(rotateBoard[i]){
                count++;
                rotateIndex = i;
            }
        }
        if(count != 1)
            return false;

        count = 0;
        for(int i = 0; i < 2; i++){
            for(int j = 0 ; j < 12 ; j++){
                if(checkBoard[i,j] && !isSeted[i,j]){
                    count++;
                    ensureSet[i] = j;
                }
            }
            if(count != 1)
                return false;
            count = 0;
        }
    
        return true;
    }
    public void ensure()
    {
        if(CheckValid())
        {
            print("success");
            for(int i = 0; i < 2; i++)
            {
                isSeted[i,(int)ensureSet[i]] = true;
                buttons[i,(int)ensureSet[i]].GetComponent<Image>().color
                    = Color.red;
                buttons[i,(int)ensureSet[i]].transform.GetChild(0)
                    .GetComponent<Text>().text = index.ToString();
            }
            redoRecord.Push(ensureSet);
            rotateRecord.Push(rotateIndex);
            currentRotateIndex.text = rotateRecord.Peek().ToString();
            index++;
        }
        else
        {
            print("error");
        }
    }

    public void redo()
    {
        if(redoRecord.Count > 0)
        {
            print("redo successfully");
            Vector2 tmp = redoRecord.Pop();
            rotateRecord.Pop();
            if(rotateRecord.Count > 0)
                currentRotateIndex.text = rotateRecord.Peek().ToString();
            else
                currentRotateIndex.text = "";
            
            for(int i = 0; i < 2; i++)
            {
                isSeted[i,(int)tmp[i]] = false;
                checkBoard[i,(int)tmp[i]] = false;
                buttons[i,(int)tmp[i]].GetComponent<Image>().color
                    = Color.white;
                buttons[i,(int)tmp[i]].transform.GetChild(0)
                    .GetComponent<Text>().text = "";
            }
            index--;
        }
        else
            print("redo false");
    }

    public void exit()
    {

        List<Vector2> tmprel = new List<Vector2>();
        foreach(Vector2 v in redoRecord)
            tmprel.Add(v);
        List<int> tmprot = new List<int>();
        foreach(int v in rotateRecord)
            tmprot.Add(v);

        InfoManager.insert(frame, tmprel, tmprot);
        
        this.gameObject.SetActive(false);
    }

}
