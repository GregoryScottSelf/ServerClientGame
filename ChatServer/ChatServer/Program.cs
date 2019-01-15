using System;
using System.Threading;
using System.Net.Sockets;
using System.Text;
using System.Collections;

namespace ChatServer
{
    class Program
    {
        public static Hashtable clientsList = new Hashtable();
        static void Main(string[] args)
        {
            //listens to the client for information to be sent 
            TcpListener serverSocket = new TcpListener(8888);
            TcpClient clientSocket = default(TcpClient);
            int counter = 0;

            serverSocket.Start();
            //server starts 
            Console.WriteLine("Chat Server Started ....");
            counter = 0;
            while ((true))
            {
                counter += 1;
                clientSocket = serverSocket.AcceptTcpClient();

                byte[] bytesFrom = new byte[(int)clientSocket.ReceiveBufferSize];
                string dataFromClient = null;
                //string board = "000000000";
                
                

                NetworkStream networkStream = clientSocket.GetStream();
                networkStream.Read(bytesFrom, 0, (int)clientSocket.ReceiveBufferSize);
                dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom);
                dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("$"));

                clientsList.Add(dataFromClient, clientSocket);

                Console.WriteLine(dataFromClient + " Joined chat room ");
                handleClinet client = new handleClinet();
                client.startClient(clientSocket, dataFromClient, clientsList);
            }

            clientSocket.Close();
            serverSocket.Stop();
            Console.WriteLine("exit");
            Console.ReadLine();
        }
        //brodcast messages to the client 
        public static void broadcast(string board, string msg, bool flag)//what is broadcasted to the clients //flag=true game keeps going flag=false someone won
        {
            foreach (DictionaryEntry Item in clientsList)
            {
                TcpClient broadcastSocket;
                broadcastSocket = (TcpClient)Item.Value;
                NetworkStream broadcastStream = broadcastSocket.GetStream();
                Byte[] broadcastBytes = null;
                

                if (flag == true)
                {

                    
                    //logic for determing if X or O wins or if there is a tie game for the client side of the game 
                    if (board[0] != '0' && board[0] == board[1] && board[0] == board[2])
                    {
                        if (board[0] == 'x')
                        {
                            msg="X winsssssss";//+ board;
                        }
                        else
                        {
                            msg="O wins";//+ board;
                            
                        }
                    }
                    else if (board[3] != '0' && board[3] == board[4] && board[3] == board[5])
                    {
                        if (board[3] == 'x')
                        {
                            msg="X wins";//+ board;
                        }
                        else
                        {
                            msg="O wins";//+ board;
                        }
                    }
                    else if (board[6] != '0' && board[6] == board[7] && board[6] == board[8])
                    {
                        if (board[6] == 'x')
                        {
                            msg="X wins";//+ board;
                        }
                        else
                        {
                            msg="O wins";//+ board;
                        }
                    }
                    else if (board[0] != '0' && board[0] == board[3] && board[0] == board[6])
                    {
                        if (board[0] == 'x')
                        {
                            msg="X wins";//+ board;
                        }
                        else
                        {
                            msg = "O wins";// + board;
                        }
                    }
                    else if (board[1] != '0' && board[1] == board[4] && board[1] == board[7])
                    {
                        if (board[1] == 'x')
                        {
                            msg = "X wins";// + board;
                        }
                        else
                        {
                            msg = "O wins";//+ board;
                        }
                    }
                    else if (board[2] != '0' && board[2] == board[5] && board[2] == board[8])
                    {
                        if (board[2] == 'x')
                        {
                            msg = "X wins";// + board;
                        }
                        else
                        {
                            msg = "O wins";// + board;
                        }
                    }
                    else if (board[0] != '0' && board[0] == board[4] && board[0] == board[8])
                    {
                        if (board[0] == 'x')
                        {
                            msg = "X wins";//+ board;
                        }
                        else
                        {
                            msg = "O wins";// + board;
                        }
                    }
                    else if (board[2] != '0' && board[2] == board[4] && board[2] == board[6])
                    {
                        if (board[2] == 'x')
                        {
                            msg = "X wins";// + board;
                        }
                        else
                        {
                            msg = "O wins";// +board;
                        }
                    }
                    else
                    {
                        msg=board;
                    }
                    if (board[0] != '0' && board[1] != '0' && board[2] != '0' && board[3] != '0' && board[4] != '0' && board[5] != '0' && board[6] != '0' && board[7] != '0' && board[8] != '0')
                    {
                        msg = "Cats game";
                    }
                    broadcastBytes = Encoding.ASCII.GetBytes(msg);
                   
                }
                //if there is no winner than the game keeps going 
                else
                {
                    broadcastBytes = Encoding.ASCII.GetBytes(msg);
                }

                broadcastStream.Write(broadcastBytes, 0, broadcastBytes.Length);
                broadcastStream.Flush();
            }
        }  //end broadcast function
    }//end Main class


    public class handleClinet
    {
        TcpClient clientSocket;
        string clNo;
        Hashtable clientsList;

        public void startClient(TcpClient inClientSocket, string clineNo, Hashtable cList)
        {
            this.clientSocket = inClientSocket;
            this.clNo = clineNo;
            this.clientsList = cList;
            Thread ctThread = new Thread(doChat);
            ctThread.Start();
        }

        private void doChat()//////////this is what the console displays
        {
            int requestCount = 0;
            byte[] bytesFrom = new byte[(int)clientSocket.ReceiveBufferSize];
            string dataFromClient = null;
            Byte[] sendBytes = null;
            string serverResponse = null;
            string rCount = null;
            requestCount = 0;
            string board = "000000000";

            while ((true))
            {
                try
                {
                    requestCount = requestCount + 1;
                    NetworkStream networkStream = clientSocket.GetStream();
                    networkStream.Read(bytesFrom, 0, (int)clientSocket.ReceiveBufferSize);
                    dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom);
                    dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("$"));
                    //this is where the board is sent to the console
                    Console.WriteLine("Here "+dataFromClient);
                    board = dataFromClient;
                    //this is used for the server to determine who win and losses and is displayed in the console 
                    if (board[0] != '0' && board[0] == board[1] && board[0] == board[2])
                    {
                        if (board[0] == 'x')
                        {
                            Console.WriteLine("X wins");
                            
                        }
                        else
                        {
                            Console.WriteLine("O wins");
                        }
                    }
                    else if (board[3] != '0' && board[3] == board[4] && board[3] == board[5])
                    {
                        if (board[3] == 'x')
                        {
                            Console.WriteLine("X wins");
                        }
                        else
                        {
                            Console.WriteLine("O wins");
                        }
                    }
                    else if (board[6] != '0' && board[6] == board[7] && board[6] == board[8])
                    {
                        if (board[6] == 'x')
                        {
                            Console.WriteLine("X wins");
                        }
                        else
                        {
                            Console.WriteLine("O wins");
                        }
                    }
                    else if (board[0] != '0' && board[0] == board[3] && board[0] == board[6])
                    {
                        if (board[0] == 'x')
                        {
                            Console.WriteLine("X wins");
                        }
                        else
                        {
                            Console.WriteLine("O wins");
                        }
                    }
                    else if (board[1] != '0' && board[1] == board[4] && board[1] == board[7])
                    {
                        if (board[1] == 'x')
                        {
                            Console.WriteLine("X wins");
                        }
                        else
                        {
                            Console.WriteLine("O wins");
                        }
                    }
                    else if (board[2] != '0' && board[2] == board[5] && board[2] == board[8])
                    {
                        if (board[2] == 'x')
                        {
                            Console.WriteLine("X wins");
                        }
                        else
                        {
                            Console.WriteLine("O wins");
                        }
                    }
                    else if (board[0] != '0' && board[0] == board[4] && board[0] == board[8])
                    {
                        if (board[0] == 'x')
                        {
                            Console.WriteLine("X wins");
                        }
                        else
                        {
                            Console.WriteLine("O wins");
                        }
                    }
                    else if (board[2] != '0' && board[2] == board[4] && board[2] == board[6])
                    {
                        if (board[2] == 'x')
                        {
                            Console.WriteLine("X wins");
                        }
                        else
                        {
                            Console.WriteLine("O wins");
                        }
                    }
                    //no winner or loser the game keep playing
                    //the array is still displayed in the console 
                    else
                    {
                        Console.WriteLine("Keep going");
                    }
                    rCount = Convert.ToString(requestCount);

                    Program.broadcast(dataFromClient, clNo, true);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }//end while
        }//end doChat
    }
}
