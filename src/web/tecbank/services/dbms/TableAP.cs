using System.Collections.Generic;

namespace tecbank.DBMS{
    /// <summary>
    /// Accesspoint (AP) for a table in a database
    /// </summary>
    public class TableAP{
        // --------------------------------[ Class atributes ]--------------------------------
        private String __table_name;
        private int __table_id;
        private List<int> __related_tables = [];
        // --------------------------------[ Class methods ]--------------------------------
        TableAP(){}
    }
}