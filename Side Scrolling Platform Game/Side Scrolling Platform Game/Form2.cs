using System;
using System.Windows.Forms;

namespace Side_Scrolling_Platform_Game
{
    public partial class Form2 : Form
    {
        bool goleft = false;// boolean which will control players going left 
        bool goright = false; // boolean which will control players going right 
        bool jumping = false; // boolean to check if player is jumping or not 
        bool hasKey = false; // default value of whether the player has the key 
        int jumpSpeed = 10; // integer to set jump speed 
        int force = 8; // force of the jump in an integer 
        int score = 0; // default score integer set to 0 
        int playSpeed = 18; //this integer will set players speed to 18 
        int backLeft = 8; // this integer will set the background moving speed to 8
        int skyLeft = 4; // this integer will set the background moving speed to 8

        int ReducioSalt = 1; //Variable que almacena el valor que se restara al salto por cada tic del reloj
        int gravetat = 2;//Variale que almacena el valor de la gravedad al saltar
        int gravetatFruita = 7; //variable que almacena el valor que tendra la gravedad si el jugador colsiona con la fruta
        int MaxPosJugadorEsquerra = 50; //Distancia a la que es moura la camara cap a un costat entre el jugador i el costat dret o esquerra
        int MaxPosJugadorDreta = 50; //Distancia a la que es moura la camara cap a un costat entre el jugador i el costat dret o esquerra
        int ampladaJoc = -1353; //Amplada de la pantalla del joc controla fins a on es moura la camera
        int AlturaMort = 60; //valor que almacena la distancia donde el jugador muere si cae
        bool hasKeyMaster = false; // variable que controla cuando el jugador tiene la llave master par salir

        public Form2()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            foreach (Control x in this.Controls)
            {
                System.Reflection.PropertyInfo prop =
                typeof(System.Windows.Forms.Control).GetProperty("DoubleBuffered", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                prop.SetValue(x, true, null);
            }
        }


        private void mainGameTimer1(object sender, EventArgs e)
        {
            player1.Top += jumpSpeed;
            // refresh the player picture box consistently 
            player1.Refresh();
            // if jumping is true and force is less than 0 // then change jumping to false 
            if (jumping && force < 0)
            {
                jumping = false;
            }

            // if jumping is true // then change jump speed to -12 // reduce force by 1
            if (jumping)
            {
                jumpSpeed -= gravetat;
                force -= ReducioSalt;
            }
            else
            { // else change the jump speed to 12 
                jumpSpeed = 12;
            }
            // if go left is true and players left is greater than 100 pixels
            // only then move player towards left of the
            if (goleft && player1.Left > MaxPosJugadorEsquerra)
            {
                player1.Left -= playSpeed;
            }
            // by doing the if statement above, the player picture will stop on the forms left
            // if go right Boolean is true
            // player left plus players width plus 100 is less than the forms width
            // then we move the player towards the right by adding to the players left
            if (goright && player1.Left + (player1.Width + MaxPosJugadorDreta) < this.ClientSize.Width)
            {
                player1.Left += playSpeed;
            }

            // by doing the if statement above, the player picture will stop on the forms right 
            // if go right is true and the background picture left is greater 1352 
            // then we move the background picture towards the left 
            if (goright && background.Left > ampladaJoc)
            {
                background.Left -= backLeft;
                sky.Left -= skyLeft;
                // the for loop below is checking to see the platforms and coins in the level 
                // when they are found it will move them towards the left
                foreach (Control x in this.Controls) 
                {
                    if (x is PictureBox && (x.Tag == "platform" || x.Tag == "coin" || x.Tag == "door" || x.Tag == "key" || x.Tag == "keyMaster" || x.Tag == "Fruita"))
                    {
                        x.Left -= backLeft;
                    }
                }
            }
            // if go left is true and the background pictures left is less than 2 
            // then we move the background picture towards the right 
            if (goleft && background.Left < 2)
            {
                background.Left += backLeft;
                sky.Left += skyLeft;
                // below the is the for loop thats checking to see the platforms and coins in the level
                // when they are found in the level it will move them all towards the right with the background
                foreach (Control x in this.Controls)
                {
                    if (x is PictureBox && (x.Tag == "platform" || x.Tag == "coin" || x.Tag == "door" || x.Tag == "key" || x.Tag == "keyMaster" || x.Tag == "Fruita"))
                    {
                        x.Left += backLeft;
                    }
                }
            }
            // below if the for loop thats checking for all of the controls in this form 
            foreach (Control x in this.Controls)
            {
                // is X is a picture box and it has a tag of platform 
                if (x is PictureBox && x.Tag == "platform")
                {
                    // then we are checking if the player is colliding with the platform
                    // and jumping is set to false
                    if (player1.Bounds.IntersectsWith(x.Bounds) && !jumping)
                    {
                        // then we do the following
                        force = 8; // set the force to 8 
                        player1.Top = x.Top - player1.Height + 1; // also we place the player on top of the picture box 
                        jumpSpeed = 0; // set the jump speed to 0
                    }
                }
                // if the picture box found has a tag of coin 
                if (x is PictureBox && x.Tag == "coin")
                {
                    // now if the player collides with the coin picture box 
                    if (player1.Bounds.IntersectsWith(x.Bounds))
                    {
                        this.Controls.Remove(x); // then we are going to remove the coin image 
                        score++; // add 1 to the score 
                    }
                }

                if (x is PictureBox && x.Tag == "Fruita") //verifico si el jugador colisiona con la fruta para modificar la gravedad de salto
                {
                    // now if the player collides with the coin picture box 
                    if (player1.Bounds.IntersectsWith(x.Bounds)) //si el jugador colisiona con la fruta
                    {
                        this.Controls.Remove(x); // eliimino la fruta
                        gravetat = gravetatFruita; // Modifico la gravedad
                    }
                }
            } // if the player collides with the door and has key boolean is true 
            if (player1.Bounds.IntersectsWith(door.Bounds) && hasKey && hasKeyMaster)
            { // then we change the image of the door to open 
                door.Image = Properties.Resources.door_open;
                gameTimer1.Stop();
                MessageBox.Show("You Completed the level!!"); // show the message box
                this.Close();
            }
            if (player1.Bounds.IntersectsWith(key.Bounds))
            {
                this.Controls.Remove(key);
                hasKey = true;
            }
            if (player1.Bounds.IntersectsWith(KeyMst.Bounds)) //si el jugador colisiona con la nueva llave master
            {
                this.Controls.Remove(KeyMst); //eliminamos la llave
                hasKeyMaster = true; //marcamos variable es igual a true
            }

            // this is where the player dies 
            // if the player goes below the forms height then we will end the game 
            if (player1.Top + player1.Height > this.ClientSize.Height + AlturaMort)
            {
                gameTimer1.Stop(); // stop the timer 
                MessageBox.Show("You Died!!!"); // show the message box 
                this.Close();
            }
        } // linking the jumpspeed i


        private void keyisdown(object sender, KeyEventArgs e)
        {
            // then we set the car left boolean to true 
            if (e.KeyCode == Keys.Left)
            {
                goleft = true;
            }
            // if player pressed the right key and the player left plus player width is less then the panel1 width 
            if (e.KeyCode == Keys.Right)
            { // then we set the player right to true 
                goright = true;
            }
            //if the player pressed the space key and jumping boolean is false 
            if (e.KeyCode == Keys.Space && !jumping)
            { // then we set jumping to true
                jumping = true;
            }

        }

        private void keyisup(object sender, KeyEventArgs e)
        {
            // then we set the car left boolean to true 
            if (e.KeyCode == Keys.Left)
            {
                goleft = false;
            }
            // if player pressed the right key and the player left plus player width is less then the panel1 width 
            if (e.KeyCode == Keys.Right)
            { // then we set the player right to true 
                goright = false;
            }

            if (jumping)
            {
                jumping = false;
            }
        }
    }
}