using System;
using System.Runtime.InteropServices;
using System.Collections;
using UnityEngine;

/*
 * please don't use this code for sell a asset
 * user for free 
 * developed by Poya  @  http://gamesforsoul.com/
 * BLOB support by Jonathan Derrough @ http://jderrough.blogspot.com/
 * Modify and structure by Santiago Bustamante @ busta117@gmail.com
 * Android compatibility by Thomas Olsen @ olsen.thomas@gmail.com
 *
 * */
 
public class SqliteException : Exception
{
	public SqliteException (string message) : base(message)
	{
    
	}
}

public class SqliteDatabase
{
	private bool CanExQuery = true;
	const int SQLITE_OK = 0;
	const int SQLITE_ROW = 100;
	const int SQLITE_DONE = 101;
	const int SQLITE_INTEGER = 1;
	const int SQLITE_FLOAT = 2;
	const int SQLITE_TEXT = 3;
	const int SQLITE_BLOB = 4;
	const int SQLITE_NULL = 5;
        
	[DllImport("sqlite3", EntryPoint = "sqlite3_open")]
	private static extern int sqlite3_open (string filename, out IntPtr db);
 
	[DllImport("sqlite3", EntryPoint = "sqlite3_close")]
	private static extern int sqlite3_close (IntPtr db);
 
	[DllImport("sqlite3", EntryPoint = "sqlite3_prepare_v2")]
	private static extern int sqlite3_prepare_v2 (IntPtr db, string zSql, int nByte, out IntPtr ppStmpt, IntPtr pzTail);
 
	[DllImport("sqlite3", EntryPoint = "sqlite3_step")]
	private static extern int sqlite3_step (IntPtr stmHandle);
 
	[DllImport("sqlite3", EntryPoint = "sqlite3_finalize")]
	private static extern int sqlite3_finalize (IntPtr stmHandle);
 
	[DllImport("sqlite3", EntryPoint = "sqlite3_errmsg")]
	private static extern IntPtr sqlite3_errmsg (IntPtr db);
 
	[DllImport("sqlite3", EntryPoint = "sqlite3_column_count")]
	private static extern int sqlite3_column_count (IntPtr stmHandle);
 
	[DllImport("sqlite3", EntryPoint = "sqlite3_column_name")]
	private static extern IntPtr sqlite3_column_name (IntPtr stmHandle, int iCol);
 
	[DllImport("sqlite3", EntryPoint = "sqlite3_column_type")]
	private static extern int sqlite3_column_type (IntPtr stmHandle, int iCol);
 
	[DllImport("sqlite3", EntryPoint = "sqlite3_column_int")]
	private static extern int sqlite3_column_int (IntPtr stmHandle, int iCol);
 
	[DllImport("sqlite3", EntryPoint = "sqlite3_column_text")]
	private static extern IntPtr sqlite3_column_text (IntPtr stmHandle, int iCol);
 
	[DllImport("sqlite3", EntryPoint = "sqlite3_column_double")]
	private static extern double sqlite3_column_double (IntPtr stmHandle, int iCol);
 
	[DllImport("sqlite3", EntryPoint = "sqlite3_column_blob")]
	private static extern IntPtr sqlite3_column_blob (IntPtr stmHandle, int iCol);

	[DllImport("sqlite3", EntryPoint = "sqlite3_column_bytes")]
	private static extern int sqlite3_column_bytes (IntPtr stmHandle, int iCol);
	
	private IntPtr _connection;

	private bool IsConnectionOpen { get; set; }
	
	private string pathDB;
	
	
    #region Public Methods
    
	/// <summary>
	/// Initializes a new instance of the <see cref="SqliteDatabase"/> class.
	/// </summary>
	/// <param name='dbName'> 
	/// Data Base name. (the file needs exist in the streamingAssets folder)
	/// </param>
	public SqliteDatabase (string dbFilePath)
	{		
		pathDB = dbFilePath;
		//Debug.Log("pathDB========"+pathDB);		
	}
	
	private void Open ()
	{
		this.Open (pathDB);	
	}
	
	private void Open (string path)
	{
		if (IsConnectionOpen) {
			throw new SqliteException ("There is already an open connection");
		}
        
		if (sqlite3_open (path, out _connection) != SQLITE_OK) {
			throw new SqliteException ("Could not open database file: " + path);
		}
        
		IsConnectionOpen = true;
	}
     
	private void Close ()
	{
		if (IsConnectionOpen) {
			sqlite3_close (_connection);
		}
        
		IsConnectionOpen = false;
	}
 
	/// <summary>
	/// Executes a Update, Delete, etc  query.
	/// </summary>
	/// <param name='query'>
	/// Query.
	/// </param>
	/// <exception cref='SqliteException'>
	/// Is thrown when the sqlite exception.
	/// </exception>
	public void ExecuteNonQuery (string query)
	{
		if (!CanExQuery) {
			Debug.Log ("ERROR: Can't execute the query, verify DB origin file");
			return;
		}
			
		this.Open ();
		if (!IsConnectionOpen) {
			throw new SqliteException ("SQLite database is not open.");
		}

		IntPtr stmHandle = Prepare (query);
 
		if (sqlite3_step (stmHandle) != SQLITE_DONE) {
			throw new SqliteException ("Could not execute SQL statement.");
		}
        
		Finalize (stmHandle);
		this.Close ();
	}
	
	/// <summary>
	/// Executes a query that requires a response (SELECT, etc).
	/// </summary>
	/// <returns>
	/// Dictionary with the response data
	/// </returns>
	/// <param name='query'>
	/// Query.
	/// </param>
	/// <exception cref='SqliteException'>
	/// Is thrown when the sqlite exception.
	/// </exception>
	public SqlDataTable ExecuteQuery (string query)
	{
		if (!CanExQuery) {
			Debug.Log ("ERROR: Can't execute the query, verify DB origin file");
			return null;
		}
		
		this.Open ();
		if (!IsConnectionOpen) {
			throw new SqliteException ("SQLite database is not open.");
		}
        
		IntPtr stmHandle = Prepare (query);
 
		int columnCount = sqlite3_column_count (stmHandle);
 
		var dataTable = new SqlDataTable ();
		for (int i = 0; i < columnCount; i++) {
			string columnName = Marshal.PtrToStringAnsi (sqlite3_column_name (stmHandle, i));
			dataTable.Columns.Add (columnName);
		}
        
		
		//populate datatable
		while (sqlite3_step(stmHandle) == SQLITE_ROW) {
			object[] row = new object[columnCount];
			for (int i = 0; i < columnCount; i++) {
				switch (sqlite3_column_type (stmHandle, i)) {
				case SQLITE_INTEGER:
					row [i] = sqlite3_column_int (stmHandle, i);
					break;
                
				case SQLITE_TEXT:
					IntPtr text = sqlite3_column_text (stmHandle, i);
					row [i] = Marshal.PtrToStringAnsi (text);
					break;

				case SQLITE_FLOAT:
					row [i] = sqlite3_column_double (stmHandle, i);
					break;
                    
				case SQLITE_BLOB:
					IntPtr blob = sqlite3_column_blob (stmHandle, i);
					int size = sqlite3_column_bytes (stmHandle, i);
					byte[] data = new byte[size];
					Marshal.Copy (blob, data, 0, size);
					row [i] = data;
					break;
					
				case SQLITE_NULL:
					row [i] = null;
					break;
				}
			}
        
			dataTable.AddRow (row);
		}
        
		Finalize (stmHandle);
		this.Close ();
		return dataTable;
	}
    
	public void ExecuteScript (string script)
	{
		string[] statements = script.Split (';');
        
		foreach (string statement in statements) {
			if (!string.IsNullOrEmpty (statement.Trim ())) {
				ExecuteNonQuery (statement);
			}
		}
	}
    
    #endregion
    
    #region Private Methods
 
	private IntPtr Prepare (string query)
	{
		IntPtr stmHandle;

        byte[] queryByts = System.Text.Encoding.UTF8.GetBytes(query);

        if (sqlite3_prepare_v2 (_connection, query, queryByts.Length, out stmHandle, IntPtr.Zero) != SQLITE_OK) {
			IntPtr errorMsg = sqlite3_errmsg (_connection);
			throw new SqliteException (Marshal.PtrToStringAnsi (errorMsg));
		}
        
		return stmHandle;
	}
 
	private void Finalize (IntPtr stmHandle)
	{
		if (sqlite3_finalize (stmHandle) != SQLITE_OK) {
			throw new SqliteException ("Could not finalize SQL statement.");
		}
	}
    
    #endregion
}
