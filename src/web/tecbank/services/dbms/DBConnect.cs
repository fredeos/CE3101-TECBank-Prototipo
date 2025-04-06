using System.Collections.Generic; // For collections like ::List<>::
using tecbank.models;
using System.Threading.Tasks; // For ::SemaphoreSlim::


namespace tecbank.services.DBMS{
    /// <summary>
    /// This class allows connecting to a project database and its tables
    /// Project database consists of a relation between .csv tables and .json database properties
    /// </summary>
    public class DBConnect{
        // --------------------------------[ Class atributes ]--------------------------------
        private String __db_file = "";
        private List<Tuple<String,int>> __db_tables = [];
        private SemaphoreSlim __db_traffic = new SemaphoreSlim(1);
        // --------------------------------[ Class methods ]--------------------------------
        public DBConnect(String db_name){
            this.__db_file = db_name;
        }
    }
}