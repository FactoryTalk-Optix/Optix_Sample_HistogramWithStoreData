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
using System.Diagnostics;
using System.Threading;
using FTOptix.WebUI;
using FTOptix.OPCUAClient;
using FTOptix.OPCUAServer;
#endregion

public class HistrogramDatabaseLogic : BaseNetLogic
{
    
    public override void Start()
    {
        RefreshDataGrid();
    }

    public override void Stop()
    {
        // Insert code to be executed when the user-defined logic is stopped
    }

    [ExportMethod]
    public void RefreshDataGrid()
    {
        IUAObject variablesFromDatabase = Owner.GetObject("VariablesFromDatabase");
        Store sourceDatabase = InformationModel.Get<Store>(Owner.GetVariable("Database").Value);
        IUAVariable queryVariable = Owner.GetVariable("QueryString");
        if (sourceDatabase != null && variablesFromDatabase != null && queryVariable != null)
        {
            if (sourceDatabase.Status == StoreStatus.Online)
            {
                string[] headers;
                object[,] dataFromDatabase;
                sourceDatabase.Query(queryVariable.Value, out headers, out dataFromDatabase);
                if (headers != null && dataFromDatabase != null && dataFromDatabase.GetLength(0) > 0) 
                {
                    foreach(IUANode variable in variablesFromDatabase.Children) 
                    {
                        variablesFromDatabase.Remove(variable);
                    }
                    for (int i = 0; i < headers.Length; i++) 
                    {
                        NodeId datatype = DataTypesHelper.GetDataTypeIdByNetType(dataFromDatabase[0, i].GetType());
                        if (datatype == null)
                        {
                            datatype = OpcUa.DataTypes.Int32;
                        }
                        IUAVariable newVariable = InformationModel.MakeVariable(headers[i], datatype);
                        newVariable.Value = new UAValue(dataFromDatabase[0, i]);
                        variablesFromDatabase.Add(newVariable);
                    }                    
                }
                else
                {
                    foreach (IUANode variable in variablesFromDatabase.Children)
                    {
                        if (variable.NodeClass == NodeClass.Variable) 
                        {
                            (variable as IUAVariable).Value = 0;
                        }
                    }
                }
                (Owner as HistogramChart).Refresh();
            }
        }
    }
}
