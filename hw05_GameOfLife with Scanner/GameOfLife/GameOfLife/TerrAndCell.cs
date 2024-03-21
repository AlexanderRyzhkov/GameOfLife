using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace GameOfLife
{
   

    public enum cellState { Dead, Alive, Empty }

    
   public class TFEventArgs  
   {
       public cellState[,] Field { get; }

       public TFEventArgs(cellState[,] field)
       {
           Field = field;
       }

   }

  
    public class Terrain : AbsClassDecorator
    {


        //public new cellState[,] onlyStatesTerr; //скрываю базовый onlyStatesTerr из 

        

        int sizeField = 10;
        const int N = 50;

        Cell[,] terrain;

        public void InitializeField()
        {
            terrain = new Cell[N, N];

            //чтобы не заниматься рисованием, заполняем рандомно...
            Random rand = new Random();
            int tempforRand;

            for (int i = 0; i < terrain.GetLength(0); i++)
                for (int j = 0; j < terrain.GetLength(1); j++)
                {
                    
                    tempforRand = rand.Next(-3, 2); 
                    if (tempforRand > 0) 
                    {
                        Cell newCelltrue = new Cell(cellState.Alive, i, j);
                        terrain[i, j] = newCelltrue;
                        
                    }

                    else
                    { 
                        Cell newCellfalse = new Cell(cellState.Dead, i, j);
                        terrain[i, j] = newCellfalse; 
                        

                    }
                }


            SetNeigborhoods(); //для каждой клетки по ссылке передаём соседей
  
        }

        public void SetNeigborhoods()
        {
            for (int i = 0; i < terrain.GetLength(0); i++)
                for (int j = 0; j < terrain.GetLength(1); j++)
                {
                    terrain[i, j].SetNeigborhood(terrain, i, j);

                }
        }

        public void MakeChange(int a, int b)
        {
            int x = a; int y = b;

            if (terrain[x,y].isAlive == cellState.Alive)
            {
                    terrain[x, y].isAlive = cellState.Dead;
            }
            else    terrain[x, y].isAlive = cellState.Alive;

        }

        public override void MakeTurn()
        {

            onlyStatesTerr = new cellState[terrain.GetLength(0), terrain.GetLength(1)];
            onlyStatesTerr = NextGeneration(terrain);

            //TurnFinished?.Invoke(this, new TFEventArgs(onlyStatesTerr));

            Thread.Sleep(300);

            
        }



        private cellState[,] NextGeneration(Cell[,] terr) 
        {

            cellState[,] newCellsStates = new cellState[N, N];

                for (int x = 0; x < terr.GetLength(0); x++)
                {
                    for (int y = 0; y < terr.GetLength(1); y++)
                    {
                       newCellsStates[x,y] = terr[x, y].MakeTurn(terrain.GetLength(0), terrain.GetLength(1));
                    
                    }
                }

                for (int x = 0; x < terr.GetLength(0); x++)
                {
                    for (int y = 0; y < terr.GetLength(1); y++)
                    {
                        terr[x, y].isAlive = newCellsStates[x, y];

                    }
                }

            return newCellsStates;
        }

        
        public void MakePaint(PaintEventArgs e)  //здесь поменяли terrain на onlyCellStates
        {
            Graphics g = e.Graphics;
            Brush alive = Brushes.White;
            Brush dead = Brushes.Black;


            for (int x = 0; x < terrain.GetLength(0); x++) //(int dimension): a zero-based dimension of the array whose length needs to be determined
            {
                for (int y = 0; y < terrain.GetLength(1); y++)
                {

                    Brush b;
                    if (onlyStatesTerr != null)
                    {
                        if (onlyStatesTerr[x, y] == cellState.Alive) //terrain[x,y]==true  ссылка на объект не указывает на экземпляр
                            b = alive;
                        else b = dead;
                    }
                    else
                    {
                        if (terrain[x, y].isAlive == cellState.Alive) //terrain[x,y]==true  ссылка на объект не указывает на экземпляр
                            b = alive;
                        else b = dead;
                    }


                    g.FillRectangle(b, x * sizeField, y * sizeField, sizeField, sizeField); //заполняет внутреннюю часть прямоугольника... почитать про этот метод

                }
            }
        }


    }
    
    class Cell
    {
            public cellState isAlive;
            Cell[,] Negbourhoods = new Cell[3,3];

            private int x;
            private int y; 
        
            public Cell()
            {
                isAlive = cellState.Dead;
            }

            public Cell(cellState status) 
            {
                isAlive = status;

            }

            public Cell(cellState status, int x, int y)
            {
                 isAlive = status;
                 this.x = x; this.y = y;
            }

            public void SetNeigborhood(Cell[,] array, int x, int y)
            {
                
                for (int ix = 0; ix < 3; ix++)
                    for (int iy = 0; iy < 3; iy++)
                    {

                         if ((x + (ix - 1)) < 0 || (x + (ix - 1)) > array.GetLength(0) - 1) continue; 
                         if ((y + (iy - 1)) < 0 || (y + (iy - 1)) > array.GetLength(1) - 1) continue; //

                         if (ix == 1 && iy == 1) continue;

                         Negbourhoods[ix, iy] = array[x + (ix - 1), y + (iy - 1)];
                   
                    }

            }

        
            public cellState MakeTurn(int MaxX, int MaxY)
            {
                int count = 0;
                cellState returnedState; 


                for (int i = 0; i < 3; i++)
                     for (int j = 0; j < 3; j++)
                     {
                         if ((x + (i - 1) < 0) || (y + (j - 1) < 0)) continue;
                         if ((x + i > MaxX - 1) || (y + j > MaxY - 1)) continue;

                         if (i == 1 && j == 1) continue;

                         if (Negbourhoods[i, j].isAlive == cellState.Alive) count++; // ссылка на объект не указывает на экземпляр объекта
                     }

                if (this.isAlive == cellState.Alive)
                {
                    if ((count != 3) && (count != 2))
                    {
                        returnedState = cellState.Dead; 
                    }
                    else returnedState = cellState.Alive;
                }
                else if (count == 3)
                {
                    returnedState = cellState.Alive; 
                }
                else returnedState = cellState.Dead;


                return returnedState;
            }


            public int GetNumberOfNeigborhoods(int MaxX, int MaxY)
            {
                int count = 0;
            
                for (int i = 0; i < 3; i++)
                    for (int j = 0; j < 3; j++)
                    {
                        if ((x + i - 1 < 0) || (y + j - 1 < 0)) continue;
                        if ((x + i > MaxX - 1) || (y + j > MaxY - 1)) continue;

                        if (i == 1 && j == 1) continue; 

                        if (Negbourhoods[i, j].isAlive == cellState.Alive) count++; 
                    }

                return count;
            }
  
    }
}