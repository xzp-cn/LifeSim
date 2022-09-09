using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameFramework.Event;
using UnityEngine;

public class PuzzleMgr : IUIModule
{
    private Transform gridPar;
    private GameObject gridItem;

    private Transform piecePar;
    private GameObject pieceItem;

    private EdgeStruct m_EdgeStruct;

    Grid m_Grid;
    private GridView gridView;
    private GridData gridData;
    private GridPiece gridPiece;
    private int width=2, height=2;
    public void Init(Transform _puzzleTr, GameObject _gridItem, GameObject _pieceItem, EdgeStruct _edgeStruct)
    {
        gridPar = _puzzleTr.Find("Grid");
        piecePar = _puzzleTr.Find("GridPiece");

        //
        gridItem = _gridItem;
        pieceItem = _pieceItem;

        m_EdgeStruct = _edgeStruct;

        m_Grid = new Grid() { gridArray = new GridItem[width, height], width = width, height = height };
        gridData = new GridData();
        //生成格子
        gridView = new GridView();
        gridView.OnInit(gridPar, gridItem);

        gridPiece = new GridPiece();
        gridPiece.OnInit(piecePar, pieceItem, m_EdgeStruct);

    }

    public async void OnOpen()
    {
        await gridData.GetData(m_Grid);

        gridView.OnOpen(m_Grid.gridArray);
        //生成碎片
        gridPiece.OnOpen(m_Grid.gridArray, m_Grid.width);

    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            gridPiece.GetAnswer(m_Grid.gridArray, m_Grid.width);
        }
    }

    public void OnClose(bool isShutdown, object userData)
    {
        gridView.OnClose();
        gridPiece.OnClose();
    }

    public bool Settle()
    {
        bool isfinish= gridView.Settle();
        return isfinish;
    }

    public void OnRecycle()
    {

    }

}
