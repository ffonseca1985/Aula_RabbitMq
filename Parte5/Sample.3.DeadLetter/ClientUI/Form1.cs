using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientUI
{
    public partial class Form1 : Form
    {
        private const string HostName = "localhost";
        private const string UserName = "guest";
        private const string Password = "guest";
        private const string QueueName = "Module3.Sample9.Normal";

        public Form1()
        {
            InitializeComponent();
        }

        private void sendButton_Click(object sender, EventArgs e)
        {
            var connectionFactory = new ConnectionFactory
            {
                HostName = HostName,
                UserName = UserName,
                Password = Password
            };
            

            var connection = connectionFactory.CreateConnection();
            var model = connection.CreateModel();

            //Setup properties
            var properties = model.CreateBasicProperties();
            properties.SetPersistent(true);

            //Serialize
            byte[] messageBuffer = Encoding.Default.GetBytes(messageTextBox.Text);

            //Send message
            model.BasicPublish("", QueueName, properties, messageBuffer);
        }
    }
}
