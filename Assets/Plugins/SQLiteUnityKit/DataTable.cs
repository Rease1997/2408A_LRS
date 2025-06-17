using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;

public class SqlDataRow : Dictionary<string, object>
{
    public new object this[string column]
    {
        get
        {
            if (ContainsKey(column))
            {
                return base[column];
            }
            
            return null;
        }
        set
        {
            if (ContainsKey(column))
            {
                base[column] = value;
            }
            else
            {
                Add(column, value);
            }
        }
    }
}

public class SqlDataTable
{
    public SqlDataTable()
    {
        Columns = new List<string>();
        Rows = new List<SqlDataRow>();
    }
    
    public List<string> Columns { get; set; }
    public List<SqlDataRow> Rows { get; set; }
    
    public SqlDataRow this[int row]
    {
        get
        {
            return Rows[row];
        }
    }
    
    public void AddRow(object[] values)
    {
        if (values.Length != Columns.Count)
        {
            throw new IndexOutOfRangeException("The number of values in the row must match the number of column");
        }
        
        var row = new SqlDataRow();
        for (int i = 0; i < values.Length; i++)
        {
            row[Columns[i]] = values[i];
        }
        
        Rows.Add(row);
    }
}

