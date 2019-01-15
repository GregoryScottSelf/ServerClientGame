using System;
using System.Windows.Forms;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace ChatClient
{
    public partial class Form1 : Form
    {
        //creates an instance where the client is able to send information to our server
        System.Net.Sockets.TcpClient clientSocket = new System.Net.Sockets.TcpClient();
        NetworkStream serverStream = default(NetworkStream);
        string readData = null;


        //display the game form
        public Form1()
        {
            InitializeComponent();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            string[] text = {"0", "0", "0", "0","0", "0","0", "0","0" };
            
            //if ((textBox10.Text=="x"||textBox10.Text=="o")&& textBox12.Text==")

            text[0] = textBox10.Text;//determines what is sent in the array to the server for each box
            text[1] = textBox11.Text;
            text[2] = textBox12.Text;
            text[3] = textBox13.Text;
            text[4] = textBox14.Text;
            text[5] = textBox15.Text;
            text[6] = textBox16.Text;
            text[7] = textBox17.Text;
            text[8] = textBox18.Text;
            //reads array and sets blank spots to '0'
            for (int i = 0; i < 9; i++)
            {
                if (text[i] == "")
                {
                    text[i] = "0";
                }
               
                
            }

            //sends each textbox to the server with the contents that are included in the array
            byte[] outStream = System.Text.Encoding.ASCII.GetBytes(text[0]+text[1] + text[2] + text[3] + text[4] + text[5] + text[6] + text[7] + text[8] + "$");
            
            serverStream = clientSocket.GetStream();
            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //readData = "Conected to Chat Server ...";
            //msg();
            //initialize the instance
            clientSocket.Connect("127.0.0.1", 8888);
            serverStream = clientSocket.GetStream();
            // sends out the users names to the server
            byte[] outStream = System.Text.Encoding.ASCII.GetBytes(textBox3.Text + "$");
            //writes out the users name in the server console 
            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();

            Thread ctThread = new Thread(getMessage);
            ctThread.Start();
        }

        private void getMessage()
        {
            while (true)
            {


                serverStream = clientSocket.GetStream();
                int buffSize = 0;
                byte[] inStream = new byte[(int)clientSocket.ReceiveBufferSize];
                buffSize = clientSocket.ReceiveBufferSize;
                serverStream.Read(inStream, 0, buffSize);
                string returndata = System.Text.Encoding.ASCII.GetString(inStream);
                string board = returndata;
                //this displays a new window box for who wins 
                if ((board[0] == 'X' || board[0] == 'O') && board[2] == 'w' && board[3] == 'i' && board[4] == 'n' && board[5] == 's')
                {
                    MessageBox.Show(board, "Tic-Tac-Toe", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    readData = "" + returndata;
                    
                }
                //tie game displayed on new window 
                else if (board[0]=='C'&&board[1]=='a'&&board[2]=='t')
                {
                    MessageBox.Show(board, "Tic-Tac-Toe", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    readData = "" + returndata;
                }
                //no win or loss yet game keeps going 
                else
                {
                    readData = "" + returndata;
                    msg();
                }
            }
        }

        private void msg()
        {
            string board = "0";
            //string board10 = "0", board11 = "0", board12 = "0", board13 = "0", board14 = "0", board15 = "0", board16 = "0", board17 = "0", board18 = "0";
            //char boardarr;
            if (this.InvokeRequired)
                this.Invoke(new MethodInvoker(msg));
            else
            {
                board = readData;
                // the box is left empty if the array has the prestored value of '0'
                //if the user enters in either X or O then it leaves it the same and updates the array that is send to the server
                if (board[0] != '0')
                {
                    textBox10.Text = board[0].ToString();
                }
                else
                {
                    textBox10.Text = "";
                }

                if (board[1] != '0')
                {
                    textBox11.Text = board[1].ToString();
                }
                else
                {
                    textBox11.Text = "";
                }

                if (board[2] != '0')
                {
                    textBox12.Text = board[2].ToString();
                }
                else
                {
                    textBox12.Text = "";
                }

                if (board[3] != '0')
                {
                    textBox13.Text = board[3].ToString();
                }
                else
                {
                    textBox13.Text = "";
                }

                if (board[4] != '0')
                {
                    textBox14.Text = board[4].ToString();
                }
                else
                {
                    textBox14.Text = "";
                }

                if (board[5] != '0')
                {
                    textBox15.Text = board[5].ToString();
                }
                else
                {
                    textBox15.Text = "";
                }

                if (board[6] != '0')
                {
                    textBox16.Text = board[6].ToString();
                }
                else
                {
                    textBox16.Text = "";
                }

                if (board[7] != '0')
                {
                    textBox17.Text = board[7].ToString();
                }
                else
                {
                    textBox17.Text = "";
                }
                if (board[8] != '0')
                {
                    textBox18.Text = board[8].ToString();
                }
                else
                {
                    textBox18.Text = "";
                }

            }
            
            


        }
        //all the next lines are each individual button and how it sends the array to the server
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            button2_Click(sender, e);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBoxLoc_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox14_TextChanged(object sender, EventArgs e)
        {
            button1_Click(sender, e);
        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            button1_Click(sender, e);
        }
        private void textBox11_TextChanged(object sender, EventArgs e)
        {
            button1_Click(sender, e);
        }
        private void textBox12_TextChanged(object sender, EventArgs e)
        {
            button1_Click(sender, e);
        }
        private void textBox13_TextChanged(object sender, EventArgs e)
        {
            button1_Click(sender, e);
        }
        private void textBox15_TextChanged(object sender, EventArgs e)
        {
            button1_Click(sender, e);
        }
        private void textBox16_TextChanged(object sender, EventArgs e)
        {
            button1_Click(sender, e);
        }
        private void textBox17_TextChanged(object sender, EventArgs e)
        {
            button1_Click(sender, e);
        }
        private void textBox18_TextChanged(object sender, EventArgs e)
        {
            button1_Click(sender, e);
        }
       
    }
}