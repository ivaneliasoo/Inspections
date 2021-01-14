using Foundation;
using ReportsApp.iOS;
using ReportsApp.Persistence;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLiteDb))]
namespace ReportsApp.iOS
{
    public class SQLiteDb : ISQLiteDb
    {
        public SQLiteAsyncConnection GetConnection()
        {
            var commonAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            var path = Path.Combine(commonAppDataPath, "cseReports.db3");
            return new SQLiteAsyncConnection(path);
        }
    }
}