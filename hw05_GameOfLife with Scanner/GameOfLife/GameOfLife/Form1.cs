using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameOfLife
{
    

    public partial class Form1 : Form
    {
        int sizeField = 10;


        Terrain field = new Terrain();

        AbstractDecorator smth;

        AbstractDecorator smth2;


        public Form1()
        {

            InitializeComponent();
                        
            field.InitializeField();

        }
       
        private void DisplayField(object sender, TFEventArgs e)
        {
 //!!!!!!    здесь должна быть отрисовка поля...
            pictureBox2.Invalidate();
        }

        private void button5_Click(object sender, EventArgs e)  //STEP
        {
           
            field.MakeTurn();
          
           

            pictureBox2.Invalidate();
        }

       

        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
            

            field.MakePaint(e);
            
        }

        

        
        //terr - игровое поле
        //x,y - координаты

        

        private void pictureBox2_MouseDown(object sender, MouseEventArgs e) //можно щелкать по полю и рисовать...
        {
            int x = e.X / sizeField;
            int y = e.Y / sizeField;

            field.MakeChange(x, y);
               

            pictureBox2.Invalidate(); 
        }
       
        private void button6_Click(object sender, EventArgs e) //PLAY
        {
            if (btnPlay.Text == "PLAY")
            {
                timer1.Start();
                btnPlay.Text = "PAUSE";
            }
            else
            {
                timer1.Stop();
                btnPlay.Text = "PLAY";
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            if (checkBox1.CheckState == CheckState.Checked)
            {
                smth = new ScannerTerrainDecorator(field); //ПОМЕНЯЛ НА тип ABSTRACTdECORATOR И ЗАРАБОТАЛО...
                smth.MakeTurn();

            }
            else field.MakeTurn(); 

            pictureBox2.Invalidate();
        }

        //private void checkBox1_CheckedChanged(object sender, EventArgs e)
        //{
        //   //когда пользователь щелкает - вознивает событие... но как сделать так, чтобы пока...

        //    if (checkBox1.Checked == true)
        //    {
        //        AbstractDecorator smth = new ScannerTerrainDecorator(field); //ПОМЕНЯЛ НА тип ABSTRACTdECORATOR И ЗАРАБОТАЛО...
        //        smth.MakeTurn();
                
        //    }
           
        //}

        private void checkBox1_CheckStateChanged(object sender, EventArgs e)
        {
            if (checkBox1.CheckState == CheckState.Checked)
            {
                smth = new ScannerTerrainDecorator(field); //ПОМЕНЯЛ НА тип ABSTRACTdECORATOR И ЗАРАБОТАЛО... попробовать поменять там наверху...
                smth.MakeTurn();

            }



            label4.Text = smth2.elapsedTime; 

        }
    }

}





