using System.Collections.Generic; // For collections like ::List<>::
using System.Threading.Tasks; // For ::SemaphoreSlim::
using System.Text.Json;
using System.IO;

using tecbank.DBMS;


namespace tecbank.services.DBMS{
    /// <summary>
    /// This class allows connecting to a project database and its tables
    /// Project database consists of a relation between .xml files as tables and .json database properties
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

        /// <summary>
        /// Executes a SELECT query on the specified table with the given criteria(if null, selects all)
        /// </summary>
        /// <returns>List of T elements from query</returns>
        /// <exception cref="SystemException"></exception>
        /// <exception cref="KeyNotFoundException"></exception>
        public List<T> SELECT<T>(String table, Func<T, bool>? criteria){
            try{
                __db_traffic.Wait();
                var db_tab = __db_tables.FirstOrDefault(tab => tab.__table_name == table);
                if (db_tab == null) 
                    throw new KeyNotFoundException($"(DBConnect) The table \"{table}\" does not exist in the database {__db_name}: {__db_file}");
                if (criteria == null){
                    return db_tab.extract_all<T>();
                } else {
                    return db_tab.find<T>(criteria);
                }
            } catch (System.Exception e){
                throw new SystemException($"(DBConnect){e.ToString()}");
            } finally {
                __db_traffic.Release();
            }
        }

        /// <summary>
        /// Performs an INSERT query in a specified database table
        /// </summary>
        /// <exception cref="SystemException"></exception>
        /// <exception cref="KeyNotFoundException"></exception>
        public void INSERT<T> (String table,T obj){
            try{
                __db_traffic.Wait();
                var db_tab = __db_tables.FirstOrDefault(tab => tab.__table_name == table);
                if (db_tab == null) 
                    throw new KeyNotFoundException($"(DBConnect) The table \"{table}\" does not exist in the database {__db_name}: {__db_file}");
                db_tab.create<T>(obj);
            } catch (System.Exception e){
                throw new SystemException($"(DBConnect){e.ToString()}");
            } finally {
                __db_traffic.Release();
            }
        }

        /// <summary>
        /// Performs a MODIFY query for a table in the database for all entities that match the criteria
        /// </summary>
        /// <exception cref="SystemException"></exception>
        /// <exception cref="KeyNotFoundException"></exception>
        public void MODIFY<T> (String table,T obj, Func<T,T,bool> criteria){
            try{
                __db_traffic.Wait();
                var db_tab = __db_tables.FirstOrDefault(tab => tab.__table_name == table);
                if (db_tab == null) 
                    throw new KeyNotFoundException($"(DBConnect) The table \"{table}\" does not exist in the database {__db_name}: {__db_file}");
                db_tab.modify<T>(obj, criteria);
            } catch (System.Exception e){
                throw new SystemException($"(DBConnect){e.ToString()}");
            } finally {
                __db_traffic.Release();
            }
        }

        /// <summary>
        /// Executes a REMOVE query in a table of the database for all entities that match the given criteria
        /// </summary>
        /// <exception cref="SystemException"></exception>
        /// <exception cref="KeyNotFoundException"></exception>
        public void REMOVE<T> (String table, Func<T, bool> criteria){
            try{
                __db_traffic.Wait();
                var db_tab = __db_tables.FirstOrDefault(tab => tab.__table_name == table);
                if (db_tab == null) 
                    throw new KeyNotFoundException($"(DBConnect) The table \"{table}\" does not exist in the database {__db_name}: {__db_file}");
                db_tab.delete<T>(criteria);
            } catch (System.Exception e1){
                throw new SystemException($"(DBConnect){e1.ToString()}");
            } finally {
                __db_traffic.Release();
            }
        }
    }
}