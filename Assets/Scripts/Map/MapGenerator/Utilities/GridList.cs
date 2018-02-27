
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class GridList<E> : List<E>
{

    public override string ToString()
{
    StringBuilder builder = new StringBuilder();
    builder.Append("GridList [");
    foreach (List<E> row in rows)
    {
        builder.Append("[");
        bool first = true;
        foreach (E e in row)
        {
            if (!first)
                builder.Append(", ");
            builder.Append(e.ToString());
            first = false;
        }
        builder.Append("]\n");
    }
    builder.Append("]");
    return builder.ToString();
}

private List<List<E>> rows;
private int numberOfColumns = 0;

/**
 * Create an empty grid.
 */
public GridList()
{
    rows = new List<List<E>>();
}

/**
 * Create a grid of the given size, populated with empty objects.
 * 
 * @param numberOfRows
 *            The number of rows in the new grid.
 * @param numberOfColumns
 *            The number of columns in the new grid.
 * @param emptyCell
 *            The default value to enter into the grid.
 */
public GridList(int numberOfRows, int numberOfColumns, E emptyCell)
{
    // System.out.println("Creating grid of " + numberOfRows + " rows and "
    // + numberOfColumns + " columns");
    rows = new List<List<E>>();
    List<E> emptyRow = new List<E>();
    for (int i = 0; i < numberOfColumns; i++)
    {
        emptyRow.Add(emptyCell);
    }
    // System.out.println("Empty Row has " + emptyRow.Count + " entries.");
    this.numberOfColumns = numberOfColumns;
    for (int i = 0; i < numberOfRows; i++)
    {
        rows.Add(emptyRow);
    }
    // System.out.println("There are " + rows.Count + " rows.");

}

/**
 * Get the number of columsn that currently exist in the grid.
 * 
 * @return The size of the grid in columns.
 */
public int getNumberOfColumns()
{
    return this.numberOfColumns;
}

/**
 * Get the number of rows that currently exist in the grid.
 * 
 * @return The size of the grid in rows.
 */
public int getNumberOfRows()
{
    return this.rows.Count;
}

/**
 * Adds a row to the current grid. If the grid is empty, sets the width of
 * the grid.
 * 
 * @param _row
 *            The row to add.
 * @return If the given row does not match the current width, returns false.
 *         Otherwise, returns the value of the call to add(), which should
 *         be true.
 */
public bool addRow(List<E> _row)
{
    // TODO System.out.println("Number of columns: " + numberOfColumns);
    if (numberOfColumns != 0)
    {
        // TODO System.out.println("Adding row with " + _row.Count +
        // " columns");
        if (_row.Count != numberOfColumns)
        {
            // TODO System.out.println("...but they don't match");
            return false;
        }
    }

        // System.out.println("Setting number of columns to " + _row.Count);
        numberOfColumns = _row.Count;
        try
        {
            rows.Add(_row);
        }
        catch (Exception e)
        {
            Debug.Log(e);
            return false;
        }
        return true;
}

/**
 * Add a column to the current grid. If the grid is empty, creates enough
 * rows to ensure the entire column fits. If the grid is not empty, ensures
 * the column size matches the size of the existing columns.
 * 
 * @param _column
 *            The column to add.
 * @return true if the column was added successfully, or false if the sizes
 *         do not match.
 */
public bool addColumn(List<E> _column)
{
    // First column!
    if (rows.Count == 0)
    {
        foreach (E e in _column)
        {
            List<E> newRow = new List<E>();
            newRow.Add(e);
            rows.Add(newRow);
        }
        return true;
    }
    // The grid already has data. Make sure the new column matches
    // the existing size.
    else if (rows.Count == _column.Count)
    {
        foreach (E e in _column)
        {
            // Add each element of the column in the same position
            rows[_column.IndexOf(e)].Add(e);
            //rows.Get(_column.IndexOf(e)).add(e);
        }
        return true;
    }
    else
        return false;
}

/**
 * Get the values of a single row of the grid.
 * 
 * @param index
 *            Which row to retrieve the values of.
 * @return A row of the grid.
 */
public List<E> getRow(int index)
{
    return rows[index];
}

/**
 * Get the values of a single column of the grid.
 * 
 * @param index
 *            Which column to retrieve the values of.
 * @return A column of the grid.
 */
public List<E> getColumn(int index)
{
    List<E> returnValue = new List<E>();
    foreach (List<E> row in rows)
    {
        returnValue.Add(row[index]);
    }
    return returnValue;
}

/**
 * Returns the value of a single cell.
 * 
 * @param rowIndex
 *            The row to retrieve.
 * @param columnIndex
 *            The column to retrieve.
 * @return The value at rowIndex,columnIndex
 */
public E getCell(int rowIndex, int columnIndex)
{
    return rows[rowIndex][columnIndex];
}

public void setCell(int rowIndex, int columnIndex, E data)
{
    rows[rowIndex][columnIndex] = data;
}


    public bool add(E arg0)
{
    // Cannot add a single value to the grid.
    return false;
}

    public bool addAll(List<E> arg0)
{
    // Must use the specific addRow() or addColumn() calls.
    return false;
}

    public void clear()
{
    rows.Clear();
}

    public bool contains(E arg0)
{
    // Matches if object matches a single entry in the grid...
    foreach (List<E> row in rows)
    {
        if (row.Contains(arg0))
            return true;
    }
        // ...or an entire row.
        return false; // rows.Contains(arg0);
}

    public bool ContainsAll(List<E> arg0)
{
    return false;
}

    public bool isEmpty()
{
    return rows.Count == 0;
}


/**
 * Returns the row index of the first occurrence of the specified element in
 * this grid, or -1 if this grid does not contain the element. More
 * formally, returns the lowest index i such that
 * <code>(o==null ? get(i)==null :
 * o.equals(get(i)))</code>, or -1 if there is no such index. Search is
 * row-first, then column.
 * 
 * @param o
 *            element to search for
 * @return the index of the first occurrence of the specified element in
 *         this list, or -1 if this list does not contain the element
 */
public int rowIndexOf(E o)
{
    if (this.Contains(o))
    {
        int rowIndex = 0;
        foreach (List<E> al in rows)
        {
            if (al.Contains(o))
            {
                return (rowIndex);
            }
            else
            {
                rowIndex++;
            }
        }
    }
    return -1;
}

/**
 * Returns the index of the first occurrence of the specified element in
 * this grid, or -1 if this grid does not contain the element. More
 * formally, returns the lowest index i such that
 * <code>(o==null ? get(i)==null :
 * o.equals(get(i)))</code>, or -1 if there is no such index. This method
 * treats the grid as a flat array, left to right, top to bottom.
 * 
 * @param o
 *            element to search for
 * @return the index of the first occurrence of the specified element in
 *         this list, or -1 if this list does not contain the element.
 */
public int indexOf(E o)
{
    if (this.contains(o))
    {
        int rowIndex = 0;
        foreach (List<E> al in rows)
        {
            if (al.Contains(o))
            {
                return (rowIndex + al.IndexOf(o));
            }
            else
            {
                rowIndex += numberOfColumns;
            }
        }
    }
    return -1;
}

/**
 * Creates a new instance of the GridList that copies this one, except that
 * it switches the rows and columns. For instance, if you ran this method
 * on a grid of 4 rows by 8 columns, this method would return a grid of 8
 * rows by 4 columns.
 * 
 * @return A grid that matches this one, except the rows and columns are
 *         swapped.
 */
public GridList<E> switchColumnsRows()
{
    GridList<E> rv = new GridList<E>();
    foreach (List<E> row in rows)
    {
        rv.addColumn(row);
    }
    return rv;
}

/**
 * Returns the row index of the first occurrence of the specified element in
 * this grid, or -1 if this grid does not contain the element. More
 * formally, returns the lowest index i such that
 * <code>(o==null ? get(i)==null :
 * o.equals(get(i)))</code>, or -1 if there is no such index. Search is
 * column-first, then row.
 * 
 * @param o
 *            element to search for
 * @return the index of the first occurrence of the specified element in
 *         this list, or -1 if this list does not contain the element
 */
public int columnIndexOf(E o)
{
    if (this.contains(o))
    {
        GridList<E> temp = this.switchColumnsRows();
        return temp.rowIndexOf(o);
    }
    return -1;
}

/**
 * Returns the index of the first occurrence of the specified element in the
 * row given, or -1 if that row does not contain the element.
 * 
 * @param o
 *            element to search for
 * @param rowNumber
 *            row of the grid to search
 * @return the index of the first occurrence of the specified element in the
 *         given row, or -1 if that row does not contain the element.
 */
public int columnIndexOf(E o, int rowNumber)
{
    List<E> row = this.getRow(rowNumber);
    return row.IndexOf(o);
}

    
    public bool remove(E arg0)
{
    // Cannot remove a single object.
    return false;
}


    public bool removeAll(List<E> arg0)
{
    // Cannot remove randomly.
    return false;
}


    public bool retainAll(List<E> arg0)
{
    // Cannot remove randomly.
    return false;
}

/*
 * (non-Javadoc)
 * Returns the total number of cells.
 * @see java.util.Collection#Count
 */

    public int size()
{
    if (rows.Count == 0)
        return 0;
    else
        return (rows.Count * rows[0].Count);
}


    /*public E[] toArray()
{
    E[] rv = new E[rows.Count];
    int index = 0;
    foreach (List<E> row in rows)
    {
        rv[index] = row.ToArray();
        index++;
    }
    return rv;
}*/

    // TODO If needed: Insert Row at index
    // TODO If needed: Insert Column at index
    // TODO If needed: Remove Row
    // TODO If needed: Remove Column
}
