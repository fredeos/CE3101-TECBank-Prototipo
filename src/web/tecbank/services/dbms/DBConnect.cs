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
            Console.WriteLine($"Ruta de la DB: {__db_file}");
            // >> Cargar propiedades de la base de datos
            JsonDocument json = JsonDocument.Parse(File.ReadAllText(__db_file));
            if (json != null){
                this.__db_name = json.RootElement.GetProperty("name").ToString();
                String ext = json.RootElement.GetProperty("ext").ToString();
                JsonElement tables = json.RootElement.GetProperty("tables");
                foreach (JsonElement table in tables.EnumerateArray()){
                    __db_tables.Add(new(Path.Combine(database_dir, $"{__db_name}_tables", $"{table.ToString()}.{ext}")));
                }
            } else {
                throw new SystemException($"El archivo de JSON de la base de datos({db_name}) no existe");
            }
        }

        /// <summary>
        /// Executes a SELECT query on the specified table with the given criteria
        /// </summary>
        /// <typeparam name="T">The type of objects to return</typeparam>
        /// <param name="table">Name of the table to query</param>
        /// <param name="criteria">Filter condition for the records</param>
        /// <returns>List of matching records of type T</returns>
        /// <exception cref="KeyNotFoundException">Thrown when the specified table doesn't exist</exception>
        /// <exception cref="SystemException">Thrown when any database operation fails</exception>
        /// <remarks>
        public List<T> SELECT<T>(String table, Func<T, bool> criteria){
            try{
                // Acquire lock for thread-safe database access
                __db_traffic.Wait();
                // Find the requested table
                var db_tab = __db_tables.FirstOrDefault(tab => tab.__table_name == table);
                if (db_tab == null) throw new KeyNotFoundException($"Table {table} doesn't exist in database {__db_name}");
                // Execute the query with criteria
                return db_tab.find<T>(criteria);
            } catch (System.Exception e){
                // TODO: Catch exceptions and log
                throw new SystemException($"TABLEAP.FIND failed: {e}");
            } finally {
                // Ensure lock is always released
                __db_traffic.Release();
            }
        }

        /// <summary>
        /// Retrieves all records from specified table
        /// </summary>
        /// <typeparam name="T">Return type</typeparam>
        /// <param name="table">Source table name</param>
        /// <exception cref="KeyNotFoundException">Table doesn't exist</exception>
        /// <exception cref="SystemException">Extraction failed</exception>
        public List<T> extract_all<T>(String table)
        {
            try
            {
                __db_traffic.Wait();
                var db_tab = __db_tables.FirstOrDefault(tab => tab.__table_name == table);
                if (db_tab == null) 
                    throw new KeyNotFoundException($"La tabla {table} no existe en la base de datos {__db_name}");
                
                return db_tab.extract_all<T>();
            }
            catch (System.Exception e)
            {
                // TODO: Implementar logging aquí
                throw new SystemException($"DBConnect.extract_all failed: {e.Message}");
            }
            finally
            {
                __db_traffic.Release();
            }
        }
        
        /// <summary>
        /// Inserts an object into the specified database table.
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="table">Target table name</param>
        /// <param name="obj">Object to insert</param>
        /// <exception cref="KeyNotFoundException">Table doesn't exist</exception>
        /// <exception cref="SystemException">Insert operation failed</exception>
        public void INSERT<T>(String table, T obj)
        {
            try
            {
                __db_traffic.Wait();
                var db_tab = __db_tables.FirstOrDefault(tab => tab.__table_name == table);
                if (db_tab == null) 
                    throw new KeyNotFoundException($"La tabla {table} no existe en la base de datos {__db_name}");
                
                db_tab.create<T>(obj);
            }
            catch (System.Exception e)
            {
                // TODO: Implementar logging aquí
                throw new SystemException($"DBConnect.INSERT failed: {e.Message}");
            }
            finally
            {
                __db_traffic.Release();
            }
        }

        // Falta
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

        // Falta
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