using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Azure; // Namespace for CloudConfigurationManager
using Microsoft.Azure.Storage; // Namespace for CloudStorageAccount
using Microsoft.Azure.Storage.Queue; // Namespace for Queue storage types

namespace AzureQueueExemple1
{
    class Program
    {

        static void Main(string[] args)
        {
            var filaName = "minha-fila1";

            //====================================//
            //[====== Obter dados de conexao ======]
            //====================================//

            // Parse the connection string and return a reference to the storage account.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                    CloudConfigurationManager.GetSetting("StorageConnectionString"));

            //====================================//
            //[========= Criando a fila ===========]
            //====================================//

            // Create the queue client.
            CloudQueueClient filaClient = storageAccount.CreateCloudQueueClient();

            // Retrieve a reference to a container.
            CloudQueue fila = filaClient.GetQueueReference(filaName);

            // Create the queue if it doesn't already exist
            fila.CreateIfNotExistsAsync();

            Console.WriteLine("Pressione: \n'C' para criar a Fila! \n'L' para listar o primeiro da Fila! \n'R' para obter e retirar da fila todos!");

            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.C:
                    //===================================================//
                    //[========= Insert a message into a queue ===========]
                    //===================================================//
                    // Create a message  
                    CloudQueueMessage message = new CloudQueueMessage("Hello, World [primeiro item] of Azure Queue!");
                    // add it to the queue.
                    fila.AddMessageAsync(message);

                    for (int i = 0; i < 4; i++)
                    {
                        fila.AddMessageAsync(new CloudQueueMessage("Hello, World [" + i + "] of Azure Queue!"));
                    }

                    Console.WriteLine("Dados inseridos na fila=" + filaName + " com sucesso!");
                    break;
                case ConsoleKey.L:
                    Console.WriteLine("Mostrando os dados da fila=" + filaName + "\n");

                    // Peek at the next message
                    CloudQueueMessage peekedMessage = fila.PeekMessage();
                    // Display message.
                    Console.WriteLine(peekedMessage.AsString);
                    break;

                case ConsoleKey.R:
                    var existeItem = true;
                    while (existeItem)
                    {
                        var itemDaFila = fila.GetMessage();// GetMessageAsync();

                        if (itemDaFila != null)
                        {
                            Console.WriteLine("Mensagem obtida: " + itemDaFila.AsString + "\n");
                        }
                        else
                        {
                            existeItem = false;
                        }
                    } 
                    break;
                default:
                    Console.WriteLine("Nenhuma opcao valida");
                    break;
            }

            Console.ReadKey();
        }
    }
}
