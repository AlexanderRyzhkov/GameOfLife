using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameOfLife
{
    
    public class Scanner
    {
        PatternGraphCollection pGraph = new PatternGraphCollection();

      

        cellState[,] patternField; //поле паттернов для отрисовки

        bool patternDetected;
   

        public cellState[,] FindPattern(cellState[,] Field)
        {
            bool tempFlag;
            patternDetected = false;         

            cellState[,] commonField = Field;

            patternField = new cellState[commonField.GetLength(0), commonField.GetLength(1)];

            for (int x = 0; x < patternField.GetLength(0); x++)
                for (int y = 0; y < patternField.GetLength(1); y++)
                {
                    patternField[x,y] = cellState.Dead;
                }


            for (int x = 0; x < commonField.GetLength(0); x++)
                for (int y = 0; y < commonField.GetLength(1); y++)
                {
                    if (commonField[x, y] == cellState.Alive)
                    {

                        tempFlag = pGraph.CheckPattern(commonField, x, y, patternField);
                        
                        if (tempFlag) patternDetected = true;
                    }
                }

                                           
                return patternField;
                        
        }      
    }
     

    #region 

    public class Node
    {
        public string patternName;
        public cellState[,] value;

        public int xForBoard;
        public int yForBoard;

        public int yForPattern;

        public string nameOfBoard;

        public Node(string name, cellState[,] pattern, int x, int y, string nameB, int tempered)
        {
            value = new cellState[pattern.GetLength(0), pattern.GetLength(1)];
            value = pattern;
            patternName = name;
            xForBoard = x;
            yForBoard = y;
            nameOfBoard = nameB;
            yForPattern = tempered;
        }
    }

    #region PatternGraphCollection- коллекция паттернов для сверки
    public class PatternGraphCollection
    {
        //Dictionary<string, cellState[,]> patternGraph = new Dictionary<string, cellState[,]>();
        LinkedList<Node> patternGraph = new LinkedList<Node>();

        public PatternGraphCollection() 
        {
                this.InicializePatternCollection();
        }

        //инициализация коллекции шаблонов
        // https://en.wikipedia.org/wiki/Conway%27s_Game_of_Life

      
        

        public bool CheckPattern(cellState[,] field, int x, int y, cellState[,] patternField)
        {
            bool flag = true;
            int tempY;


            foreach (var item in patternGraph)
            {
                flag = true;
                
                if ((x + item.value.GetLength(0) >= field.GetLength(0)) || (y + item.value.GetLength(1) >= field.GetLength(1))) return false; // throw new ArgumentOutOfRangeException("за границей массива");


                for (int i = 0; i < item.value.GetLength(0); i++)
                {
                   
                    for (int j = 0; j < item.value.GetLength(1); j++)
                    {

                        tempY = y + item.yForPattern;
                        


                        if (((tempY + j) >= 0) && ((tempY + j) < field.GetLength(1))) //не выпадаем за границы массива
                        {
                            if (!item.value[i, j].Equals(field[x + i, tempY + j])) flag = false;

                        }

                    }
                }

                
               

                if (flag)
                {

                    switch (item.nameOfBoard) //из-за неправильной работы switch не ищет некоторые паттерны, причем при удалённом Check55 продолжает находить отвечающие ему
                    {
                        case "Check35":
                            flag = Test35(field, x + item.xForBoard, y + item.yForBoard);
                            break;
                        case "Check53":
                            flag = Test53(field, x + item.xForBoard, y + item.yForBoard);
                            break;
                        case "Check44":
                            flag = Test44(field, x + item.xForBoard, y + item.yForBoard);
                            break;
                        case "Check55":
                            flag = Test55(field, x + item.xForBoard, y + item.yForBoard);
                            break;
                        case "Check65":
                            flag = Test65(field, x + item.xForBoard, y + item.yForBoard);
                            break;
                        case "Check56":
                            flag = Test56(field, x + item.xForBoard, y + item.yForBoard);
                            break;
                        case "Check66":
                            flag = Test66(field, x + item.xForBoard, y + item.yForBoard);
                            break;

                    }
                }

                if (flag)
                {
                        for (int i = 0; i < item.value.GetLength(0); i++)
                            for (int j = 0; j < item.value.GetLength(1); j++)
                            {
                                cellState test = item.value[i, j];
                                patternField[x + i, y + j] = test;
                            }
                     
                }

                

            }

            return flag;
          
        }


        #region тесты на рамки

        public bool Test66(cellState[,] array, int x, int y)
        {
            int tx = x; int ty = y;
            int constX = tx; int constY = ty; //константные переменные

            for (int i = 0; i < 5; i++)
            {
                //рамка может выйти за границу массива, но только на один шаг, иначе отрисовка невозможна
                if ((constX + i > array.GetLength(0) - 1) || (constY + i > array.GetLength(1) - 1) || (constX < -1) || (constY < -1))
                {
                    return false;
                }




                if ((constY + i <= array.GetLength(1) - 1) && (constY + i >= 0) && (constX >= 0)) if (array[constX, constY + i] != cellState.Dead) return false;
                if ((constX + i + 1 >= 0) && (constY >= 0) && (constX + i + 1 <= array.GetLength(0) - 1)) if (array[constX + i + 1, constY] != cellState.Dead) return false;



                if ((constX + 5 > array.GetLength(0) - 1) || (constY + 5 > array.GetLength(1) - 1))
                {
                    if ((constX + 5 > array.GetLength(0) - 1) && (constY + 5 > array.GetLength(1) - 1))
                    {
                        continue;
                    }
                    else if ((constX + 5 > array.GetLength(0) - 1) && (constX + i >= 0) && (constX + i <= array.GetLength(0) - 1))
                    {
                        if (array[constX + i, constY + 5] != cellState.Dead) return false;
                        continue;
                    }
                    else if ((constY + 5 > array.GetLength(1) - 1) && (constY + i + 1 <= array.GetLength(1) - 1) && (constY + i >= 0))
                    {
                        if (array[constX + 5, constY + i + 1] != cellState.Dead) return false;
                        continue;
                    }
                }
                else
                {
                    if (constX + i >= 0) if (array[constX + i, constY + 5] != cellState.Dead) return false;
                    if (constY + i + 1 >= 0) if (array[constX + 5, constY + i + 1] != cellState.Dead) return false;
                }

            }
            return true;
        }

        public bool Test65(cellState[,] array, int x, int y)
        {
            int tx = x; int ty = y;
            int constX = tx; int constY = ty; //константные переменные

            for (int i = 0; i < 5; i++)
            {
                //рамка может выйти за границу массива, но только на один шаг, иначе отрисовка невозможна
                if ((constX + i > array.GetLength(0) - 1) || (constY + i > array.GetLength(1)) || (constX < -1) || (constY < -1))
                {
                    return false; //построение границы невозможно
                }



                //дополнительная прверка на границу необходима, потому что у рамки сторона Y на единицу меньше, чем Х
                if ((constY + i <= array.GetLength(1) - 1) && (constY + i >= 0) && (constX >= 0)) if (array[constX, constY + i] != cellState.Dead) return false;
                if ((constX + i >= 0) && (constY >= 0)) if (array[constX + i, constY] != cellState.Dead) return false;



                if ((constX + 5 > array.GetLength(0) - 1) || (constY + 4 > array.GetLength(1) - 1))
                {
                    if ((constX + 5 > array.GetLength(0) - 1) && (constY + 4 > array.GetLength(1) - 1))
                    {
                        continue;
                    }
                    else if ((constX + 5 > array.GetLength(0) - 1) && (constX + i >= 0)) //не добавить ли (constX + i <= array.GetLength(1) - 1)?
                    {
                        if (array[constX + i, constY + 4] != cellState.Dead) return false;
                        continue;
                    }
                    else if ((constY + 4 > array.GetLength(1) - 1) && (constY + i <= array.GetLength(1) - 1) && (constY + i >= 0))
                    {
                        if (array[constX + 5, constY + i] != cellState.Dead) return false;
                        continue;
                    }
                }
                else
                {
                    if (constX + i >= 0) if (array[constX + i, constY + 4] != cellState.Dead) return false;
                    if (constY + i >= 0) if (array[constX + 5, constY + i] != cellState.Dead) return false;
                }
            }
            return true;
        }


        public bool Test56(cellState[,] array, int x, int y)
        {
            int tx = x; int ty = y;
            int constX = x - 1; int constY = y - 1; //константные переменные

            for (int i = 0; i < 5; i++)
            {
                //рамка может выйти за границу массива, но только на один шаг, иначе отрисовка невозможна
                if ((constX + i > array.GetLength(0)) || (constY + i > array.GetLength(1) - 1) || (constX < -1) || (constY < -1))
                {
                    return false; //построение границы невозможно
                }




                if ((constY + i <= array.GetLength(1) - 1) && (constY + i >= 0) && (constX >= 0)) if (array[constX, constY + i] != cellState.Dead) return false;
                if ((constX + i >= 0) && (constY >= 0) && (constX + i <= array.GetLength(0) - 1)) if (array[constX + i, constY] != cellState.Dead) return false;



                if ((constX + 4 > array.GetLength(0) - 1) || (constY + 5 > array.GetLength(1) - 1))
                {
                    if ((constX + 4 > array.GetLength(0) - 1) && (constY + 5 > array.GetLength(1) - 1))
                    {
                        continue;
                    }
                    else if ((constX + 4 > array.GetLength(0) - 1) && (constX + i >= 0) && (constX + i <= array.GetLength(0) - 1))
                    {
                        if (array[constX + i, constY + 5] != cellState.Dead) return false;
                        continue;
                    }
                    else if ((constY + 5 > array.GetLength(1) - 1) && (constY + i <= array.GetLength(1) - 1) && (constY + i >= 0))
                    {
                        if (array[constX + 4, constY + i] != cellState.Dead) return false;
                        continue;
                    }
                }
                else
                {
                    if (constX + i >= 0) if (array[constX + i, constY + 5] != cellState.Dead) return false;
                    if (constY + i >= 0) if (array[constX + 4, constY + i] != cellState.Dead) return false;
                }

            }

            return true;
        }


        public bool Test53(cellState[,] array, int x, int y)
        {



            int tx = x; int ty = y;

            if ((tx < -1) || (ty < -1) || ((tx + 4) > array.GetLength(0)) || (ty + 2 > array.GetLength(1))) return false;
            for (int j = 0; j < 5; j++)
            {

                if ((tx + j > -1) && (ty > -1) && (ty < array.GetLength(1)) && (tx + j < array.GetLength(0))) if (array[tx + j, ty] != cellState.Dead) return false;

                if ((tx + j > -1) && (tx + j < array.GetLength(0)) && (ty + 2 < array.GetLength(1))) if (array[tx + j, ty + 2] != cellState.Dead) return false;

            }
            if (tx > -1) if (array[tx, ty + 1] != cellState.Dead) return false;
            if ((tx + 4) < array.GetLength(0)) if (array[tx + 4, ty + 1] != cellState.Dead) return false;

            return true;
        }

        public bool Test35(cellState[,] array, int x, int y)
        {



            int tx = x; int ty = y;

            if ((tx < -1) || (ty < -1) || ((ty + 4) > array.GetLength(1)) || (tx + 2 > array.GetLength(0))) return false;
            for (int j = 0; j < 5; j++)
            {

                if ((ty + j > -1) && (tx > -1) && (tx < array.GetLength(0)) && (ty + j < array.GetLength(1))) if (array[tx, ty + j] != cellState.Dead) return false;

                if ((ty + j > -1) && (ty + j < array.GetLength(1)) && (tx + 2 < array.GetLength(0))) if (array[tx + 2, ty + j] != cellState.Dead) return false;

            }
            if (ty > -1) if (array[tx + 1, ty] != cellState.Dead) return false;
            if ((ty + 4) < array.GetLength(1)) if (array[tx + 1, ty + 4] != cellState.Dead) return false;

            return true;
        }


        public bool Test55(cellState[,] array, int x, int y)
        {
            int tx = x; int ty = y;

            int constX = tx; int constY = ty; //константные переменные

            for (int i = 0; i < 4; i++)
            {
                //рамка может выйти за границу массива, но только на один шаг, иначе отрисовка невозможна
                if ((constX + i > array.GetLength(0) - 1) || (constY + i > array.GetLength(1) - 1) || (constX < -1) || (constY < -1))
                {
                    return false; //построение границы невозможно
                }




                if ((constY + i <= array.GetLength(1) - 1) && (constY + i >= 0) && (constX >= 0)) if (array[constX, constY + i] != cellState.Dead) return false;
                if ((constX + i + 1 >= 0) && (constY >= 0) && (constX + i + 1 <= array.GetLength(0) - 1)) if (array[constX + i + 1, constY] != cellState.Dead) return false;



                if ((constX + 4 > array.GetLength(0) - 1) || (constY + 4 > array.GetLength(1) - 1))
                {
                    if ((constX + 4 > array.GetLength(0) - 1) && (constY + 4 > array.GetLength(1) - 1))
                    {
                        continue;
                    }
                    else if ((constX + 4 > array.GetLength(0) - 1) && (constX + i >= 0) && (constX + i <= array.GetLength(0) - 1))
                    {
                        if (array[constX + i, constY + 4] != cellState.Dead) return false;
                        continue;
                    }
                    else if ((constY + 4 > array.GetLength(1) - 1) && (constY + i + 1 <= array.GetLength(1) - 1) && (constY + i >= 0))
                    {
                        if (array[constX + 4, constY + i + 1] != cellState.Dead) return false;
                        continue;
                    }
                }
                else
                {
                    if (constX + i >= 0) if (array[constX + i, constY + 4] != cellState.Dead) return false;
                    if (constY + i + 1 >= 0) if (array[constX + 4, constY + i + 1] != cellState.Dead) return false;
                }


            }
            return true;
        }


        public bool Test44(cellState[,] array, int x, int y)
        {

            int constX = x; int constY = y;


            for (int i = 0; i < 3; i++)
            {
                //рамка может выйти за границу массива, но только на один шаг, иначе отрисовка невозможна
                if ((constX + i > array.GetLength(0) - 1) || (constY + i > array.GetLength(1) - 1) || (constX < -1) || (constY < -1))
                {
                    return false; //построение границы невозможно
                }




                if ((constY + i <= array.GetLength(1) - 1) && (constY + i >= 0) && (constX >= 0)) if (array[constX, constY + i] != cellState.Dead) return false;
                if ((constX + i + 1 >= 0) && (constY >= 0) && (constX + i + 1 <= array.GetLength(0) - 1)) if (array[constX + i + 1, constY] != cellState.Dead) return false;



                if ((constX + 3 > array.GetLength(0) - 1) || (constY + 3 > array.GetLength(1) - 1))
                {
                    if ((constX + 3 > array.GetLength(0) - 1) && (constY + 3 > array.GetLength(1) - 1))
                    {
                        continue;
                    }
                    else if ((constX + 3 > array.GetLength(0) - 1) && (constX + i >= 0) && (constX + i <= array.GetLength(0) - 1))
                    {
                        if (array[constX + i, constY + 3] != cellState.Dead) return false;
                        continue;
                    }
                    else if ((constY + 3 > array.GetLength(1) - 1) && (constY + i + 1 <= array.GetLength(1) - 1) && (constY + i >= 0))
                    {
                        if (array[constX + 3, constY + i + 1] != cellState.Dead) return false;
                        continue;
                    }
                }
                else
                {
                    if (constX + i >= 0) if (array[constX + i, constY + 3] != cellState.Dead) return false;
                    if (constY + i + 1 >= 0) if (array[constX + 3, constY + i + 1] != cellState.Dead) return false;
                }


            }

            return true;
        }

        #endregion
        public void InicializePatternCollection()
        {
            #region Block
            cellState[,] Block = new cellState[2, 2]; //верно+

            for(int i=0; i<2; i++)
                for(int j=0; j<2; j++)
                {
                    Block[i, j] = cellState.Alive;
                    
                }
            #endregion

            #region Boat

            cellState[,] Boat = new cellState[3, 3]; //верно+

            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                {

                    if ((i == 0 && j == 0) || (i == 0 && j == 1) || (i == 1 && j == 0) || (i == 1 && j == 2) || (i == 2 && j == 1))
                    {
                        Boat[i, j] = cellState.Alive;
                    }
                    else Boat[i, j] = cellState.Dead;

                }

            cellState[,] or1Boat = new cellState[3, 3]; //верно

            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                {

                    if ((i == 0 && j == 2) || (i == 0 && j == 1) || (i == 2 && j == 1) || (i == 1 && j == 2) || (i == 1 && j == 0))
                    {
                        or1Boat[i, j] = cellState.Alive;
                    }
                    else or1Boat[i, j] = cellState.Dead;

                }


            cellState[,] or2Boat = new cellState[3, 3]; //верно

            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                {

                    if ((i == 0 && j == 1) || (i == 2 && j == 0) || (i == 2 && j == 1) || (i == 1 && j == 2) || (i == 1 && j == 0))
                    {
                        or2Boat[i, j] = cellState.Alive;
                    }
                    else or2Boat[i, j] = cellState.Dead;

                }

            cellState[,] or3Boat = new cellState[3, 3]; //верно

            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                {

                    if ((i == 1 && j == 0) || (i == 0 && j == 1) || (i == 2 && j == 1) || (i == 1 && j == 2) || (i == 2 && j == 2))
                    {
                        or3Boat[i, j] = cellState.Alive;
                    }
                    else or3Boat[i, j] = cellState.Dead;

                }

            /*
             * то же, но с рамкой
             * 
            cellState[,] Boat = new cellState[5, 5]; //верно

            for (int i = 1; i < 4; i++)
                for (int j = 1; j < 4; j++)
                {
                   
                    if ((i == 1 && j == 1)  ||(i == 1 && j == 2) || (i == 2 && j == 1) || (i == 2 && j == 3) || (i == 3 && j == 2))
                    {
                        Boat[i, j] = cellState.Alive;
                    }
                    else  Boat[i, j] = cellState.Dead;
                    
                }


            cellState[,] or1Boat = new cellState[5, 5];

            for (int i = 1; i < 4; i++)
                for (int j = 1; j < 4; j++)
                {

                    if ((i == 1 && j == 3) || (i == 1 && j == 2) || (i == 3 && j == 2) || (i == 2 && j == 3) || (i == 2 && j == 1))
                    {
                        or1Boat[i, j] = cellState.Alive;
                    }
                    else or1Boat[i, j] = cellState.Dead;

                }

            cellState[,] or2Boat = new cellState[5, 5];

            for (int i = 1; i < 4; i++)
                for (int j = 1; j < 4; j++)
                {

                    if ((i == 1 && j == 2) || (i == 3 && j == 1) || (i == 3 && j == 2) || (i == 2 && j == 3) || (i == 2 && j == 1))
                    {
                        or2Boat[i, j] = cellState.Alive;
                    }
                    else or2Boat[i, j] = cellState.Dead;

                }

            cellState[,] or3Boat = new cellState[5, 5];

            for (int i = 1; i < 4; i++)
                for (int j = 1; j < 4; j++)
                {

                    if ((i == 2 && j == 1) || (i == 1 && j == 2) || (i == 3 && j == 2) || (i == 2 && j == 3) || (i == 3 && j == 3))
                    {
                        or3Boat[i, j] = cellState.Alive;
                    }
                    else or3Boat[i, j] = cellState.Dead;

                }
                */
            #endregion

            #region Tub
            cellState[,] Tub = new cellState[3, 3]; //верно

            
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                {
                    if ((i == 0 && j == 1) || (i == 1 && j == 0) || (i == 1 && j == 2) || (i == 2 && j == 1))
                    {
                        Tub[i, j] = cellState.Alive;
                    }
                    else Tub[i, j] = cellState.Dead;
                }
            #endregion
            
            #region Blinker

            cellState[,] Blinker1 = new cellState[1, 3];

            for(int i = 0; i<3; i++)
            {
                Blinker1[0,i] = cellState.Alive;
            }

            cellState[,] Blinker2 = new cellState[3, 1];

            for (int i = 0; i < 3; i++)
            {
                Blinker2[i, 0] = cellState.Alive;
            }

            /*
            cellState[,] Blinker1 = new cellState[3, 5]; //верно 3 на 5


            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 5; j++)
                {
                    if (i == 1 && j != 0 && j != 4) Blinker1[i, j] = cellState.Alive;
                    else Blinker1[i, j] = cellState.Dead;
                }
            

            cellState[,] Blinker2 = new cellState[5, 3]; //верно 5 на 3

            for (int i = 0; i < 5; i++)
                for (int j = 0; j < 3; j++)
                {
                    if (j == 1 && i != 0 && i != 4) Blinker2[i, j] = cellState.Alive;
                    else Blinker2[i, j] = cellState.Dead;
                }

    */
            #endregion

            #region Loaf
            cellState[,] Loaf = new cellState[4, 4]; //верно

            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                {
                    if (i == 0 && (j == 1 || j == 2)) { Loaf[i, j] = cellState.Alive; continue; }
                    if (i == 1 && (j == 0 || j == 3)) { Loaf[i, j] = cellState.Alive; continue; }
                    if (i == 2 && (j == 1 || j == 3)) { Loaf[i, j] = cellState.Alive; continue; }
                    if (i == 3 && (j == 2))           { Loaf[i, j] = cellState.Alive; continue; }
                    Loaf[i, j] = cellState.Dead;
                }

            cellState[,] or1Loaf = new cellState[4, 4]; //верно

            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                {
                    if (i == 0 && (j == 1 || j == 2)) { or1Loaf[i, j] = cellState.Alive; continue; }
                    if (i == 1 && (j == 0 || j == 3)) { or1Loaf[i, j] = cellState.Alive; continue; }
                    if (i == 2 && (j == 0 || j == 2)) { or1Loaf[i, j] = cellState.Alive; continue; }
                    if (i == 3 && (j == 1)) { or1Loaf[i, j] = cellState.Alive; continue; }
                    or1Loaf[i, j] = cellState.Dead;
                }


            cellState[,] or2Loaf = new cellState[4, 4]; //верно

            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                {
                    if (i == 3 && (j == 1 || j == 2)) { or2Loaf[i, j] = cellState.Alive; continue; }
                    if (i == 1 && (j == 0 || j == 2)) { or2Loaf[i, j] = cellState.Alive; continue; }
                    if (i == 2 && (j == 0 || j == 3)) { or2Loaf[i, j] = cellState.Alive; continue; }
                    if (i == 0 && (j == 1)) { or2Loaf[i, j] = cellState.Alive; continue; }
                    or2Loaf[i, j] = cellState.Dead;
                }

            cellState[,] or3Loaf = new cellState[4, 4]; //верно

            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                {
                    if (i == 3 && (j == 1 || j == 2)) { or3Loaf[i, j] = cellState.Alive; continue; }
                    if (i == 1 && (j == 3 || j == 1)) { or3Loaf[i, j] = cellState.Alive; continue; }
                    if (i == 2 && (j == 3 || j == 0)) { or3Loaf[i, j] = cellState.Alive; continue; }
                    if (i == 0 && (j == 2)) { or3Loaf[i, j] = cellState.Alive; continue; }
                    or3Loaf[i, j] = cellState.Dead;
                }

            /*
             cellState[,] Loaf = new cellState[6, 6]; //верно

            for (int i = 1; i < 5; i++)
                for (int j = 1; j < 5; j++)
                {
                    if (i == 1 && (j == 2 || j == 3)) { Loaf[i, j] = cellState.Alive; continue; }
                    if (i == 2 && (j == 1 || j == 4)) { Loaf[i, j] = cellState.Alive; continue; }
                    if (i == 3 && (j == 2 || j == 4)) { Loaf[i, j] = cellState.Alive; continue; }
                    if (i == 4 && (j == 3))           { Loaf[i, j] = cellState.Alive; continue; }
                    Loaf[i, j] = cellState.Dead;
                }

            cellState[,] or1Loaf = new cellState[6, 6];

            for (int i = 1; i < 5; i++)
                for (int j = 1; j < 5; j++)
                {
                    if (i == 1 && (j == 2 || j == 3)) { or1Loaf[i, j] = cellState.Alive; continue; }
                    if (i == 2 && (j == 1 || j == 4)) { or1Loaf[i, j] = cellState.Alive; continue; }
                    if (i == 3 && (j == 1 || j == 3)) { or1Loaf[i, j] = cellState.Alive; continue; }
                    if (i == 4 && (j == 2)) { or1Loaf[i, j] = cellState.Alive; continue; }
                    or1Loaf[i, j] = cellState.Dead;
                }


            cellState[,] or2Loaf = new cellState[6, 6];

            for (int i = 1; i < 5; i++)
                for (int j = 1; j < 5; j++)
                {
                    if (i == 4 && (j == 2 || j == 3)) { or2Loaf[i, j] = cellState.Alive; continue; }
                    if (i == 2 && (j == 1 || j == 3)) { or2Loaf[i, j] = cellState.Alive; continue; }
                    if (i == 3 && (j == 1 || j == 4)) { or2Loaf[i, j] = cellState.Alive; continue; }
                    if (i == 1 && (j == 2)) { or2Loaf[i, j] = cellState.Alive; continue; }
                    or2Loaf[i, j] = cellState.Dead;
                }

            cellState[,] or3Loaf = new cellState[6, 6];

            for (int i = 1; i < 5; i++)
                for (int j = 1; j < 5; j++)
                {
                    if (i == 4 && (j == 2 || j == 3)) { or3Loaf[i, j] = cellState.Alive; continue; }
                    if (i == 2 && (j == 4 || j == 2)) { or3Loaf[i, j] = cellState.Alive; continue; }
                    if (i == 3 && (j == 4 || j == 1)) { or3Loaf[i, j] = cellState.Alive; continue; }
                    if (i == 1 && (j == 3)) { or3Loaf[i, j] = cellState.Alive; continue; }
                    or3Loaf[i, j] = cellState.Dead;
                }
             
             */

            #endregion

            #region Beehive

            cellState[,] Beehive = new cellState[3, 4]; //верно

            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 4; j++)
                {
                    if (i == 0 && (j == 1 || j == 2)) { Beehive[i, j] = cellState.Alive; continue; }
                    if (i == 1 && (j == 0 || j == 3)) { Beehive[i, j] = cellState.Alive; continue; }
                    if (i == 2 && (j == 1 || j == 2)) { Beehive[i, j] = cellState.Alive; continue; }
                    else Beehive[i, j] = cellState.Dead;
                }

            cellState[,] orBeehive = new cellState[4, 3]; //верно

            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 3; j++)
                {
                    if ((i == 0 || i == 3) && j == 1) { orBeehive[i, j] = cellState.Alive; continue; }
                    if (i == 1 && (j == 0 || j == 2)) { orBeehive[i, j] = cellState.Alive; continue; }
                    if (i == 2 && (j == 0 || j == 2)) { orBeehive[i, j] = cellState.Alive; continue; }

                    orBeehive[i, j] = cellState.Dead;

                }

            /*
              cellState[,] Beehive = new cellState[5, 6]; //верно

            for (int i = 1; i < 4; i++)
                for (int j = 1; j < 5; j++)
                {
                    if (i == 1 && (j == 2 || j == 3)) { Beehive[i, j] = cellState.Alive; continue; }
                    if (i == 2 && (j == 1 || j == 4)) { Beehive[i, j] = cellState.Alive; continue; }
                    if (i == 3 && (j == 2 || j == 3)) { Beehive[i, j] = cellState.Alive; continue; }
                   
                    Beehive[i, j] = cellState.Dead;
                }

            cellState[,] orBeehive = new cellState[6, 5];

            for (int i = 1; i < 5; i++)
                for (int j = 1; j < 4; j++)
                {
                    if ((i == 1 || i == 4) && j == 2) { orBeehive[i, j] = cellState.Alive; continue; }
                    if (i == 2 && (j == 1 || j == 3)) { orBeehive[i, j] = cellState.Alive; continue; }
                    if (i == 3 && (j == 1 || j == 3)) { orBeehive[i, j] = cellState.Alive; continue; }

                    orBeehive[i, j] = cellState.Dead;

                }
             
             */
            #endregion

            #region Toad
            cellState[,] Toad1 = new cellState[2, 4]; //верно

            for (int i = 0; i < 2; i++)
                for (int j = 0; j < 4; j++)
                {

                    if ((i == 0 && j == 0) || (i == 1 && j == 3)) Toad1[i, j] = cellState.Dead;
                    else Toad1[i, j] = cellState.Alive;
                }

            cellState[,] Toad2 = new cellState[4, 4]; //верно

            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                {
                    if ((i == 0 && j == 2) || (i == 3 && j == 1)) { Toad2[i, j] = cellState.Alive;  }
                    else if (i == 1 && (j == 0 || j == 3)) { Toad2[i, j] = cellState.Alive;  }
                    else if (i == 2 && (j == 0 || j == 3)) { Toad2[i, j] = cellState.Alive;}
                    else Toad2[i, j] = cellState.Dead;
                }

            /*
             cellState[,] Toad1 = new cellState[6, 6]; //верно

            for (int i = 1; i < 5; i++)
                for (int j = 1; j < 5; j++)
                {
                    
                    if (i == 2 && (j == 2 || j == 3 || j == 4)) { Toad1[i, j] = cellState.Alive;  }
                    else if (i == 3 && (j == 1 || j == 2 || j == 3)) { Toad1[i, j] = cellState.Alive;  }
                    else Toad1[i, j] = cellState.Dead;
                }

            cellState[,] Toad2 = new cellState[6, 6]; //верно

            for (int i = 1; i < 5; i++)
                for (int j = 1; j < 5; j++)
                {
                    if ((i == 1 && j == 3) || (i == 4 && j == 2)) { Toad2[i, j] = cellState.Alive;  }
                    else if (i == 2 && (j == 1 || j == 4)) { Toad2[i, j] = cellState.Alive;  }
                    else if (i == 3 && (j == 1 || j == 4)) { Toad2[i, j] = cellState.Alive;}
                    else Toad2[i, j] = cellState.Dead;
                }
             */
            #endregion

            #region Beacon

            cellState[,] Beacon1 = new cellState[4, 4]; //верно

            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                {
                    if (i == 0 && (j == 0 || j == 1)) { Beacon1[i, j] = cellState.Alive; continue; }
                    if (i == 1 && (j == 0 || j == 1)) { Beacon1[i, j] = cellState.Alive; continue; }
                    if (i == 2 && (j == 2 || j == 3)) { Beacon1[i, j] = cellState.Alive; continue; }
                    if (i == 3 && (j == 2 || j == 3)) { Beacon1[i, j] = cellState.Alive; continue; }

                    Beacon1[i, j] = cellState.Dead;
                }

            cellState[,] Beacon2 = new cellState[4, 4]; //верно

            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                {
                    if (i == 0 && (j == 0 || j == 1)) { Beacon2[i, j] = cellState.Alive; continue; }
                    if (i == 1 && (j == 0)) { Beacon2[i, j] = cellState.Alive; continue; }
                    if (i == 2 && (j == 3)) { Beacon2[i, j] = cellState.Alive; continue; }
                    if (i == 3 && (j == 2 || j == 3)) { Beacon2[i, j] = cellState.Alive; continue; }

                    Beacon2[i, j] = cellState.Dead;
                }

            cellState[,] orBeacon2 = new cellState[4, 4]; //верно

            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                {
                    if (i == 0 && (j == 2 || j == 3)) { orBeacon2[i, j] = cellState.Alive; continue; }
                    if (i == 2 && (j == 0)) { orBeacon2[i, j] = cellState.Alive; continue; }
                    if (i == 1 && (j == 3)) { orBeacon2[i, j] = cellState.Alive; continue; }
                    if (i == 3 && (j == 0 || j == 1)) { orBeacon2[i, j] = cellState.Alive; continue; }

                    orBeacon2[i, j] = cellState.Dead;
                }

            cellState[,] orBeacon1 = new cellState[4, 4]; //верно

            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                {
                    if (i == 0 && (j == 2 || j == 3)) { orBeacon1[i, j] = cellState.Alive; continue; }
                    if (i == 2 && (j == 0 || j == 1)) { orBeacon1[i, j] = cellState.Alive; continue; }
                    if (i == 1 && (j == 3 || j == 2)) { orBeacon1[i, j] = cellState.Alive; continue; }
                    if (i == 3 && (j == 0 || j == 1)) { orBeacon1[i, j] = cellState.Alive; continue; }

                    orBeacon1[i, j] = cellState.Dead;
                }

            #endregion

            #region добавление шаблонов в коллекцию
            var tempNode = new Node("Block", Block, -1, -1, "Check44", 0);
            patternGraph.AddLast(tempNode);
            var tempNode1 = new Node("Loaf", Loaf, -1, -2, "Check66", -1);
            patternGraph.AddLast(tempNode1);
            var tempNode2 = new Node("Loaf", or1Loaf, -1, -2, "Check66", -1);
            patternGraph.AddLast(tempNode2);
            var tempNode3 = new Node("Loaf", or2Loaf, -1, -2, "Check66", -1);
            patternGraph.AddLast(tempNode3);
            var tempNode4 = new Node("Loaf", or3Loaf, -1, -3, "Check66", -2);
            patternGraph.AddLast(tempNode4);
            var tempNode5 = new Node("Tub", Tub, -1, -2, "Check55", -1);
            patternGraph.AddLast(tempNode5);
            var tempNode6 = new Node("Boat", Boat, -1, -1, "Check55", 0);
            patternGraph.AddLast(tempNode6);
            var tempNode7 = new Node("Boat", or1Boat, -1, -2, "Check55", -1); //почему не работает именно этот?
            patternGraph.AddLast(tempNode7);
            var tempNode8 = new Node("Boat", or2Boat, -1, -2, "Check55", -1);
            patternGraph.AddLast(tempNode8);
            var tempNode9 = new Node("Boat", or3Boat, -1, -2, "Check55", -1);
            patternGraph.AddLast(tempNode9);
            var tempNode10 = new Node("Toad", Toad1, -2, -2, "Check66", -1);
            patternGraph.AddLast(tempNode10);
            var tempNode11 = new Node("Toad", Toad2, -1, -3, "Check66", -2);
            patternGraph.AddLast(tempNode11);
            var tempNode12 = new Node("Beacon", Beacon1, -1, -1, "Check66", 0);
            patternGraph.AddLast(tempNode12);
            var tempNode13 = new Node("Beacon", Beacon2, -1, -1, "Check66", 0);
            patternGraph.AddLast(tempNode13);
            var tempNode14 = new Node("Beacon", orBeacon1, -1, -3, "Check66", -2);
            patternGraph.AddLast(tempNode14);
            var tempNode15 = new Node("Beacon", orBeacon2, -1, -3, "Check66", -2);
            patternGraph.AddLast(tempNode15);
            var tempNode16 = new Node("Beehive", Beehive, -2, -1, "Check56", -1);
            patternGraph.AddLast(tempNode16);
            var tempNode17 = new Node("Beehive", orBeehive, -1, -2, "Check65", -1);
            patternGraph.AddLast(tempNode17);
            var tempNode18 = new Node("Blinker1", Blinker1, -1, -1, "Check35", 0);
            patternGraph.AddLast(tempNode18);
            var tempNode19 = new Node("Blinker2", Blinker2, -1, -1, "Check53", 0);
            patternGraph.AddLast(tempNode19);
            #endregion
            //почему-то не ищет orBoat1, Toad, Beehive2
        }

        
    }

    #endregion
   
    #endregion
}



