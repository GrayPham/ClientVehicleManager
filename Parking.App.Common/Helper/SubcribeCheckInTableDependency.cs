namespace Parking.App.Common.Helper
{
    public class SubcribeCheckInTableDependency
    {
        //SqlTableDependency<tblUserHistory> tableDependency;
        //CheckInHub checkInHub;

        //public SubcribeCheckInTableDependency(CheckInHub checkIn)
        //{
        //    this.checkInHub = checkIn;
        //}

        //public void SubcribeTableDependency()
        //{
        //    string connectionString = "Data Source=mssql.thlsoft.com,1433;Initial Catalog=KIOSKDB;User ID=thlsoft;Password=258369147Aa;Persist Security Info=True;";
        //    tableDependency = new SqlTableDependency<tblUserHistory>(connectionString);
        //    tableDependency.OnChanged += TableDependency_OnChanged;
        //    tableDependency.OnError += TableDependency_OnError;
        //    tableDependency.Start();
        //}

        //private void TableDependency_OnError(object sender, ErrorEventArgs e)
        //{
        //    Console.WriteLine($"{nameof(tblUserHistory)} SQLTableDependency error: {e.Error.Message}");
        //}

        //private void TableDependency_OnChanged(object sender, RecordChangedEventArgs<tblUserHistory> e)
        //{
        //    if (e.ChangeType != TableDependency.SqlClient.Base.Enums.ChangeType.Insert)
        //    {
        //        checkInHub.SendCheckIns();

        //    }
        //}
    }
}
