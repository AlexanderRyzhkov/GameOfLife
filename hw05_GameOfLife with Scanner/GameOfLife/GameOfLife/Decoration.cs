using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace GameOfLife
{
    public abstract class AbsClassDecorator
    {
        public cellState[,] onlyStatesTerr;

        

        public abstract void MakeTurn();
    }


    public abstract class AbstractDecorator : AbsClassDecorator
    {
        protected AbsClassDecorator component;

        public AbstractDecorator(AbsClassDecorator smth)
        {
            component = smth;
            
        }

        public override void MakeTurn()
        {
            if (component != null)
            {
                component.MakeTurn();
                
            }
        }

        public void SetTerrain(Terrain t)
        {

        }
    }

    public class ScannerTerrainDecorator : AbstractDecorator
    {
        Scanner scan = new Scanner();

        public ScannerTerrainDecorator (Terrain smth) : base(smth)
        {

        }

        public override void MakeTurn()
        {
                component.MakeTurn();

                component.onlyStatesTerr = scan.FindPattern(component.onlyStatesTerr);
            
        }

    }

    public class StatisticTerrainDecorator : AbstractDecorator
    {
        int prevStats;
        int currStats;
        int Time;

        

        public StatisticTerrainDecorator(Terrain smth) : base(smth)
        {

        }



        public override void MakeTurn()
        {


            //https://docs.microsoft.com/ru-ru/dotnet/api/system.diagnostics.stopwatch?view=netframework-4.8

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();



            component.MakeTurn();


            //посчитали статистику...


            stopWatch.Stop();

            TimeSpan ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
        }
    }


   

        





        

    

    public class Statistic
    {
        //1. принять поле значений, записать (не по ссылке!) в своё поле
        //2. скопировать (не по ссылке!) в другое поле и получить только паттерны
        //3. "наложить" паттерны на поле
        //4. посчитать там и там
        //5. вернуть интовое числоы
    }
}
        
    

