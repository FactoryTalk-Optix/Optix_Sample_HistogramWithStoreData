#region Using directives
using System;
using UAManagedCore;
using OpcUa = UAManagedCore.OpcUa;
using FTOptix.HMIProject;
using FTOptix.Retentivity;
using FTOptix.UI;
using FTOptix.NativeUI;
using FTOptix.CoreBase;
using FTOptix.Core;
using FTOptix.NetLogic;
using FTOptix.SQLiteStore;
using FTOptix.Store;
using System.Linq;
using FTOptix.WebUI;
using FTOptix.OPCUAClient;
using FTOptix.OPCUAServer;
#endregion

public class SimulationDataLogic : BaseNetLogic
{
    public override void Start()
    {
        // Insert code to be executed when the user-defined logic is started
    }

    public override void Stop()
    {
        // Insert code to be executed when the user-defined logic is stopped
    }

    [ExportMethod]
    public void FillTableWithData(NodeId store, string tableName, string primaryKeyColumnName, int recordsToCreate)
    {
        Store database = InformationModel.Get<Store>(store);
        if (database != null && !string.IsNullOrEmpty(tableName) && database.Status == StoreStatus.Online)
        {
            string[] headers;
            object[,] dataContainer;
            database.Query($"SELECT * FROM {tableName} ORDER BY {primaryKeyColumnName} DESC", out headers, out dataContainer);
            if (headers != null && dataContainer != null) 
            {
                long lastKey = dataContainer.GetLength(0) > 0 ? (long)dataContainer[0, 0] + 1 : 1;
                dataContainer = new object[recordsToCreate, headers.Length];
                for (long i =lastKey; i < lastKey+recordsToCreate ; i++) 
                {
                    for (int j =0; j < headers.Length; j++)
                    {
                        if (j == 0)
                        {
                            dataContainer[i - lastKey, j] = i;
                        }
                        else
                        {
                            Random random = new Random();
                            dataContainer[i - lastKey, j] = random.Next(5, 90);
                        }
                    }
                }
                database.Insert(tableName, headers, dataContainer);
            }
        }
    }

    [ExportMethod]
    public void DeleteDataFromTable(NodeId store, string tableName) 
    {
        Store database = InformationModel.Get<Store>(store);
        if (database != null && !string.IsNullOrEmpty(tableName) && database.Status == StoreStatus.Online)
        {
            string[] headers;
            object[,] dataContainer;
            database.Query($"DELETE FROM {tableName}", out headers, out dataContainer);
        }
    }
}
