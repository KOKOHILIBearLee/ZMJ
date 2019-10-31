using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class OperateBoard : MonoBehaviour
{
    public static OperateBoard singleton;
    [SerializeField]
    public List<Board> boards;

    GameObject current;

    private void Awake()
    {
        singleton = this;
        SetVisible(false);
    }

    Board FindBoard(GameObject game)
    {
        if (game == null)
            return null;

        return boards.Find(b =>
         {
             return b.father == game || b.child.Contains(game);
         });
    }

    public void OnRight()
    {
        SetDirection(Direction.Right);
    }

    public void OnLeft()
    {
        SetDirection(Direction.Left);
    }

    public void OnUp()
    {
        SetDirection(Direction.UP);
    }

    public void OnDown()
    {
        SetDirection(Direction.Down);
    }

    void SetDirection(Direction dir)
    {
        if (current == null)
            RefreshCurrentState();
        else
        {
            Board b = FindBoard(current);
            if (b == null)
                current = null;
            else
            {
                switch (dir)
                {
                    case Direction.UP:
                        if (b.father == current)
                        {
                            if (b.child.Count != 0)
                                current = b.child[0];
                        }
                        else
                        {
                            int index = b.child.IndexOf(current);
                            if (index >= 0 && index < b.child.Count - 1)
                            {
                                current = b.child[index + 1];
                            }
                        }
                        break;
                    case Direction.Down:
                        if (true)
                        {
                            int index = b.child.IndexOf(current);
                            if (index >= 0 && index <= b.child.Count - 1)
                            {
                                if (index != 0)
                                    current = b.child[index - 1];
                            }
                        }
                        break;
                    case Direction.Left:
                        if (true)
                        {
                            int index = boards.IndexOf(b);
                            if (index > 0)
                            {
                                if (boards[index - 1].child.Count == 0)
                                    current = boards[index - 1].father;
                                else
                                    current = boards[index - 1].child[0];
                            }
                        }
                        break;
                    case Direction.Right:
                        if (true)
                        {
                            int index = boards.IndexOf(b);
                            if (index < boards.Count - 1)
                            {
                                if (boards[index + 1].child.Count == 0)
                                    current = boards[index + 1].father;
                                else
                                    current = boards[index + 1].child[0];
                            }
                        }
                        break;
                }
            }
            RefreshCurrentState();
        }
    }

    void RefreshCurrentState()
    {
        if (current == null)
        {
            //   current = boards[0].father;
            if (boards[0].child.Count != 0)
                current = boards[0].child[0];
            else
                current = boards[0].father;
        }
        Board board = FindBoard(current);



        if (board == null)
            return;

        boards.ForEach(b =>
        {
            if (b != board)
            {
                Highlight(b.father, false);
                b.child.ForEach(C => { Highlight(C, false); });
            }
            else
            {
                Highlight(b.father, true);
                b.child.ForEach(c =>
                {
                    if (current == c)
                        Highlight(c, true);
                    else
                        Highlight(c, false);
                });
            }
        });
    }

    void Highlight(GameObject game, bool val)
    {
        if (game == null)
            return;
        foreach (Transform tran in game.transform.parent)
            if (tran.gameObject != game)
                tran.gameObject.SetActive(val);
            else
                tran.gameObject.SetActive(!val);
    }

    public void SetVisible(bool val)
    {
        boards.ForEach(b =>
        {
            b.father.SetActive(val);

            foreach (Transform tran in b.father.transform.parent)
            {
                if (tran.gameObject != b.father)
                    tran.gameObject.SetActive(false);
            }

            b.child.ForEach(c =>
            {
                c.SetActive(val);
                foreach (Transform tran in c.transform.parent)
                {
                    if (tran.gameObject != c)
                        tran.gameObject.SetActive(false);
                }
            });
        });
        current = null;
    }

    public bool GetVisible()
    {
        if (boards.Count != 0)
            return boards[0].father.activeSelf;
        return false;
    }

    public void OnEvent()
    {
        if (current == null)
            return;
        Board b = FindBoard(current);
        if (b == null)
            return;

        int row = boards.IndexOf(b);
        int colum = b.child.IndexOf(current);
        BoardEvent(row, colum);
    }

    /// <summary>
    /// 菜单事件触发
    /// </summary>
    /// <param name="row"></param>
    /// <param name="colum"></param>
    void BoardEvent(int row, int colum)
    {
        print(row + ":" + colum);
        if (row == 0)
        {
            OnJXMY(); //井下漫游
        }

        if (row == 1)
        {
            OnZNCM(); //智能采煤
        }

        if (row == 2)
        {
            OnSBCK(); //设备操控
        }

        if (row == 3 && colum == 0)
        {
            OnYJQD(); //一键启动
        }

        if (row == 3 && colum == 1)
        {
            OnRYDW(); //人员定位
        }

        if (row == 3 && colum == 2)
        {
            OnYDGJ(); //移动跟机
        }

        if (row == 3 && colum == 3)
        {
            OnZDTZ(); //自动调直
        }

        if (row == 3 && colum == 4)
        {
            OnJYGM();  //记忆割煤
        }

        if (row == 4)
        {
            mainUIEventHandle.sin.ONFHDT();
            //Application.Quit();  //退出程序
        }

        SetVisible(false);
    }

    #region 菜单事件函数

    public void OnJXMY()
    {
        print("触发菜单：" + "井下漫游");
        mainUIEventHandle.sin.OnJXMY();
    }

    public void OnZNCM()
    {
        print("触发菜单：" + "智能采煤");

        //GameObject.FindObjectOfType<mainUIEventHandle>().OnZNCM();

        mainUIEventHandle.sin.OnZNCM();
    }

    public void OnSBCK()
    {
        print("触发菜单：" + "设备操控");
        mainUIEventHandle.sin.OnSBCK();
    }

    public void OnYJQD()
    {
        print("触发菜单：" + "一键启动");
        secUIEventHandle.sin.OnYJQD();
    }

    public void OnRYDW()
    {
        print("触发菜单：" + "人员定位");
        secUIEventHandle.sin.OnRYDW();
    }

    public void OnYDGJ()
    {
        print("触发菜单：" + "移动跟机");

        secUIEventHandle.sin.OnYDGJ();
    }

    public void OnZDTZ()
    {
        print("触发菜单：" + "自动调直");
        secUIEventHandle.sin.OnZDTZ();
    }

    public void OnJYGM()
    {
        print("触发菜单：" + "记忆割煤");
        secUIEventHandle.sin.OnJYGM();
    }
    #endregion
}

public enum Direction
{
    UP,
    Down,
    Left,
    Right,
}

[Serializable]
public class Board
{
    public GameObject father;
    public List<GameObject> child = new List<GameObject>();
}