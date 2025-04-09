using System.Collections.Generic; // For collections like ::List<>::
using System.Threading.Tasks; // For ::SemaphoreSlim::
using System.Text.Json;
using System.IO;

using tecbank.DBMS;


namespace tecbank.services.DBMS{
    /// <summary>
    /// This class allows connecting to a project database and its tables
    /// Project database consists of a relation between .csv tables and .json database properties
    /// </summary>
    public class DBConnect{
        // --------------------------------[ Class atributes ]--------------------------------
        private static String database_dir = Path.Combine(".","..","..","..","database");
        private String __db_file = "";
        private String __db_name = "";
        private List<TableAP> __db_tables = [];
        private SemaphoreSlim __db_traffic = new SemaphoreSlim(1);
        // --------------------------------[ Class methods ]--------------------------------
        public DBConnect(String db_name){
            this.__db_file = Path.Combine(database_dir,$"{db_name}_db.json");
            // >> Cargar propiedades de la base de datos
            JsonDocument json = JsonDocument.Parse(File.ReadAllText(__db_file));
            if (json != null){
                this.__db_name = json.RootElement.GetProperty("name").ToString();
                String ext = json.RootElement.GetProperty("ext").ToString();
                JsonElement tables = json.RootElement.GetProperty("tables");
                foreach (JsonElement table in tables.EnumerateArray()){
                    try{
                        __db_tables.Add(new(Path.Combine(database_dir, $"{__db_name}_tables", $"{table.ToString()}.{ext}")));
                    } catch (System.Exception e){
                        throw new SystemException($"(DBConnect){e.ToString}");
                    }
                }
            } else {
                throw new SystemException($"(DBConnect) The database({db_name}) file doesnt exist: {__db_file}");
            }
        }

        public List<T> SELECT<T>(String table, Func<T, bool> criteria){
            try{
                __db_traffic.Wait();
                var db_tab = __db_tables.FirstOrDefault(tab => tab.__table_name == table);
                if (db_tab == null) throw new KeyNotFoundException($"La tabla {table} no existe en la base de datos {__db_name}");
                return db_tab.find<T>(criteria);
            } catch (System.Exception e){
                // TODO: Catch exceptions and log
                throw new SystemException($"TABLEAP.FIND failed: {e}");
            } finally {
                __db_traffic.Release();
            }
        }

        public void INSERT<T> (String table,T obj){
            try{
                __db_traffic.Wait();
            } catch (System.Exception){
                // TODO: Catch exceptions and log
                throw;
            } finally {
                __db_traffic.Release();
            }
        }

        public void MODIFY<T> (String table,T obj, Func<T,bool> criteria){
            try{
                __db_traffic.Wait();
            } catch (System.Exception){
                // TODO: Catch exceptions and log
                throw;
            } finally {
                __db_traffic.Release();
            }
        }

        public void REMOVE<T> (String table, Func<T, bool> criteria){
            try{
                __db_traffic.Wait();
            } catch (System.Exception){
                // TODO: Catch exceptions and log
                throw;
            } finally {
                __db_traffic.Release();
            }
        }
    }
}