using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NevAgent : MonoBehaviour
{
    private PriorityQueue<Node> _openList;
    private List<Node> _closeList;

    private List<Vector3Int> _routePath;

    public float speed = 5f;
    public bool corenrCheck = false;

    private Vector3Int _currentPosition; // 현재 타일 위치
    private Vector3Int _destination; //목표 타일 위치

    [SerializeField]
    private Tilemap _tilemap;

    private LineRenderer _lineRenderer;

    private void Awake()
    {
        _openList = new PriorityQueue<Node>();
        _closeList = new List<Node>();
        _routePath = new List<Vector3Int>();
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        Vector3Int cellPos = _tilemap.WorldToCell(transform.position);
        _currentPosition = cellPos;
        transform.position = _tilemap.GetCellCenterWorld(cellPos);
        
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mPos = Input.mousePosition;
            mPos.z = 0;
            Vector3 world = Camera.main.ScreenToWorldPoint(mPos);
            Vector3Int cellPos = MapManager.Instance.GetTilePos(world);

            // cellPos가 갈 수 있는 곳인지 확인

            _destination = cellPos;
            CalcRoute();
            PrintRoute();

        }
    }
    private void PrintRoute() //계산 경로 Debug찍기
    {
        _lineRenderer.positionCount = _routePath.Count;
        _lineRenderer.SetPositions(_routePath.Select(p => MapManager.Instance.GetWorldPos(p)).ToArray());
        
        
    }

    private bool CalcRoute()
    {
        _openList.Clear();
        _closeList.Clear();

        _openList.Push(new Node { 
            pos = _currentPosition, 
            _parent = null, 
            G = 0, 
            F = CalcH(_currentPosition) 
        });

        bool result = false;
        int cnt = 0; // 안전 코드
        while(_openList.Count > 0)
        {
            Node n = _openList.Pop();
            FindOpenList(n);
            _closeList.Add(n);
            if(n.pos == _destination)
            {
                result = true;
                break;
            }

            //안전코드
            cnt++;
            if (cnt > 10000)
            {
                Debug.Log("While루프 너무 많이 돌아서 빠갬");
                break;
            }
        }
        if (result) // 길을 찾았다
        {
            _routePath.Clear();
            Node last = _closeList[_closeList.Count - 1];

            while(last._parent != null)
            {
                _routePath.Add(last.pos);
                last = last._parent;
            }
            _routePath.Reverse(); // 역순 리스트 뒤집기
        }

        return result;
    }
    //넌 Node n과 연결된 오픈 리스트를 다 찾아서 _openList에 넣어줄꺼야
    private void FindOpenList(Node n)
    {
        for(int y = -1; y <= 1; y++)
        {
            for(int x = -1; x <= 1; x++)
            {
                if (x == y) continue;
                Vector3Int nextPos = n.pos + new Vector3Int(x, y, 0);

                Node temp = _closeList.Find(x => x.pos == nextPos); // 이미 방문한 장소
                if (temp != null) continue;

                if(MapManager.Instance.CanMove(nextPos))
                {
                    float g = (n.pos - nextPos).magnitude + n.G;

                    Node nextOpenNode = new Node
                    {
                        pos = nextPos,
                        _parent = n,
                        G = g,
                        F = g + CalcH(nextPos)
                    };
                    //넣기 전에 검사필요
                    Node exist = _openList.Contains(nextOpenNode);
                    if(exist != null)
                    {
                        //검증 한 번
                        if(nextOpenNode.G < exist.G)
                        {
                            exist.G = nextOpenNode.G;
                            exist.F = nextOpenNode.F;
                            exist._parent = nextOpenNode._parent;
                        }
                    }
                    else
                    {
                        _openList.Push(nextOpenNode);
                    }
                }
            }
        }
    }
    private float CalcH(Vector3Int pos)
    {
        Vector3 distance = _destination - pos;
        return distance.magnitude;
    }
}
