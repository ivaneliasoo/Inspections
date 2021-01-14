using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ReportsApp.Droid.Persistence;
using ReportsApp.Persistence;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLiteDb))]
namespace ReportsApp.Droid.Persistence
{
    public class SQLiteDb : ISQLiteDb
    {
        public SQLiteAsyncConnection GetConnection()
        {
            var commonAppDataPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData);
            var path = Path.Combine(commonAppDataPath, "cseReports.db3");
            return new SQLiteAsyncConnection(path);
        }
    }
}