using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace tecbank.DBMS{
    /// <summary>
    /// Accesspoint (AP) for a table in a database
    /// </summary>
    public class TableAP{
        // --------------------------------[ Class atributes ]--------------------------------
        private String __table_file;
        public int? __table_id;
        public String? __table_name;
        private List<String> primary_keys = [];
        private List<String> foreign_keys = [];
        // --------------------------------[ Class methods ]--------------------------------
        /*public TableAP(String table){
            this.__table_file = table;
            Console.WriteLine($"Leyendo tabla: {__table_file}");

            // >> Cargar las propiedades de la tabla
            var xml_doc = XDocument.Load(__table_file);
            if (xml_doc == null) throw new SystemException($"El archivo de tabla {__table_file} no existe");

            this.__table_name = xml_doc.Element("table").Element("name")?.Value;
            this.__table_id = int.Parse(xml_doc.Element("table").Element("id")?.Value);
            foreach (var pk in xml_doc.Elements("PKs").Descendants("PK")){
                primary_keys.Add(pk.Value);
            }
            foreach (var fk in xml_doc.Elements("FKs").Descendants("FK")){
                foreign_keys.Add(fk.Value);
            }
        }*/

        public TableAP(string table)
        {
            if (string.IsNullOrWhiteSpace(table))
                throw new ArgumentNullException(nameof(table));

            this.__table_file = table;
            Console.WriteLine($"Leyendo tabla: {__table_file}");

            // Cargar el documento XML con verificación
            var xmlDoc = XDocument.Load(__table_file) ?? 
                throw new SystemException($"El archivo de tabla {__table_file} no existe o está corrupto");

            var tableElement = xmlDoc.Element("table") ?? 
                throw new SystemException($"El archivo {__table_file} no contiene un elemento 'table' válido");

            // Manejo seguro del nombre de tabla
            this.__table_name = tableElement.Element("name")?.Value?.Trim();
            if (string.IsNullOrWhiteSpace(__table_name))
                throw new SystemException($"Nombre de tabla no válido en {__table_file}");

            // Manejo seguro del ID de tabla
            var idElement = tableElement.Element("id");
            if (idElement == null || !int.TryParse(idElement.Value, out var tableId))
                throw new SystemException($"ID de tabla no válido en {__table_file}");
            this.__table_id = tableId;

            // Carga segura de PKs
            var pksElement = tableElement.Element("PKs");
            if (pksElement != null)
            {
                foreach (var pk in pksElement.Descendants("PK"))
                {
                    if (!string.IsNullOrWhiteSpace(pk.Value))
                        primary_keys.Add(pk.Value.Trim());
                }
            }

            // Carga segura de FKs
            var fksElement = tableElement.Element("FKs");
            if (fksElement != null)
            {
                foreach (var fk in fksElement.Descendants("FK"))
                {
                    if (!string.IsNullOrWhiteSpace(fk.Value))
                        foreign_keys.Add(fk.Value.Trim());
                }
            }
        }

        /*public List<T> find<T> (Func<T,bool> criteria){
            XDocument xml_doc = XDocument.Load(__table_file);
            var values = xml_doc.Descendants("value");
            List<T> tuples = [];
            if (values != null){
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                foreach (var value in values){
                    Console.WriteLine(value.ToString());
                    StringReader reader = new StringReader(value.ToString());
                    T tup = (T) serializer.Deserialize(reader);
                    reader.Close();
                    tuples.Add(tup);
                }
            } else {
                return [];
            }
            return tuples.Where(criteria).ToList();
        }*/
        public List<T> find<T>(Func<T,bool> criteria)
        {
            XDocument xml_doc = XDocument.Load(__table_file);
            var values = xml_doc.Descendants("value");
            List<T> tuples = new List<T>();
            
            if (values != null)
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                foreach (var value in values)
                {
                    if (value == null) continue;
                    
                    Console.WriteLine(value.ToString());
                    using (StringReader reader = new StringReader(value.ToString()))
                    {
                        object? deserialized = serializer.Deserialize(reader);
                        if (deserialized is T tup)
                        {
                            tuples.Add(tup);
                        }
                    }
                }
            }
            
            return tuples.Where(criteria).ToList();
        }
        

        //
        /* public List<T> extract_all<T>()
        {
            try
            {
                XDocument xmlDoc = XDocument.Load(__table_file);
                List<T> results = new List<T>();
                XmlSerializer serializer = new XmlSerializer(typeof(T));

                // Buscar todos los nodos value, independientemente de su profundidad
                var valueNodes = xmlDoc.Descendants("value").ToList();

                foreach (var valueNode in valueNodes)
                {
                    try
                    {
                        XElement nodeToProcess = valueNode;

                        // Si el nodo value contiene otro nodo value, usamos el interno
                        if (valueNode.Elements("value").Any())
                        {
                            nodeToProcess = valueNode.Element("value");
                        }

                        using (StringReader reader = new StringReader(nodeToProcess.ToString()))
                        {
                            T obj = (T)serializer.Deserialize(reader);
                            if (obj != null)
                            {
                                results.Add(obj);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Loggear el error y continuar
                        Console.WriteLine($"Error deserializando nodo: {ex.Message}");
                        continue;
                    }
                }

                return results;
            }
            catch (Exception ex)
            {
                throw new SystemException($"Error al leer registros: {ex.Message}");
            }
        } */

       public List<T> extract_all<T>()
        {
            try
            {
                XDocument xmlDoc = XDocument.Load(__table_file);
                List<T> results = new List<T>();
                XmlSerializer serializer = new XmlSerializer(typeof(T));

                foreach (var valueNode in xmlDoc.Descendants("value").ToList())
                {
                    try
                    {
                        XElement? nodeToProcess = valueNode;  // Cambiado a nullable

                        if (valueNode.Elements("value").Any())
                        {
                            nodeToProcess = valueNode.Element("value") ?? valueNode;  // Usamos valueNode como fallback
                        }

                        if (nodeToProcess == null) continue;  // Verificación adicional

                        using (StringReader reader = new StringReader(nodeToProcess.ToString()))
                        {
                            if (serializer.Deserialize(reader) is T obj)
                            {
                                results.Add(obj);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error deserializando nodo: {ex.Message}");
                        continue;
                    }
                }

                return results;
            }
            catch (Exception ex)
            {
                throw new SystemException($"Error al leer registros: {ex.Message}");
            }
        }

        public void modify<T>(T obj, Func<T,bool> criteria){

        }

        /* public void create<T>(T obj)
        {
            try
            {
                XDocument xmlDoc = XDocument.Load(__table_file);
                XElement root = xmlDoc.Element("table");
                
                // Asegurar que existe el elemento tuples
                var tuples = root.Element("tuples") ?? new XElement("tuples");
                if (tuples.Parent == null)
                {
                    root.Add(tuples);
                }
                
                // Serializar directamente al formato deseado
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", ""); // Eliminar namespaces
                
                // Crear el elemento value directamente con el contenido serializado
                XElement newValue;
                using (StringWriter writer = new StringWriter())
                {
                    serializer.Serialize(writer, obj, ns);
                    // Convertir el XML serializado a XElement
                    newValue = XElement.Parse(writer.ToString());
                }
                
                // Renombrar el elemento raíz del objeto a "value"
                newValue.Name = "value";
                
                tuples.Add(newValue);
                xmlDoc.Save(__table_file);
            }
            catch (Exception ex)
            {
                throw new SystemException($"Error al crear registro: {ex.Message}");
            }
        } */

        public void create<T>(T obj)
        {
            try
            {
                XDocument xmlDoc = XDocument.Load(__table_file);
                XElement? root = xmlDoc.Element("table");  // Añadido ? para nullable
                
                if (root == null)  // Verificación añadida
                {
                    throw new SystemException("El documento XML no contiene un elemento 'table' válido");
                }
                
                // Asegurar que existe el elemento tuples
                var tuples = root.Element("tuples") ?? new XElement("tuples");
                if (tuples.Parent == null)
                {
                    root.Add(tuples);
                }
                
                // Serializar directamente al formato deseado
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                
                // Crear el elemento value directamente con el contenido serializado
                XElement newValue;
                using (StringWriter writer = new StringWriter())
                {
                    serializer.Serialize(writer, obj, ns);
                    // Convertir el XML serializado a XElement
                    string xmlContent = writer.ToString();
                    if (string.IsNullOrEmpty(xmlContent))  // Verificación añadida
                    {
                        throw new SystemException("Error al serializar el objeto - contenido XML vacío");
                    }
                    newValue = XElement.Parse(xmlContent);
                }
                
                // Renombrar el elemento raíz del objeto a "value"
                newValue.Name = "value";
                
                tuples.Add(newValue);
                xmlDoc.Save(__table_file);
            }
            catch (Exception ex)
            {
                throw new SystemException($"Error al crear registro: {ex.Message}");
            }
        }

        public void delete<T>(T obj, Func<T, bool> criteria){

        }
    }
}