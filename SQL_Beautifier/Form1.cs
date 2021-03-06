﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SQL_Beautifier
{
  public partial class FormTab : Form
  {
    #region Constants
    private const string BEAUTIFY = "Beautify";
    private const string COMMAFY = "Commafy";
    private const string UNQUOTEIFY = "Unquoteify";
    private const string QUOTEIFY = "Quoteify";
    #endregion

    #region Private Variables
    private string[] keywords = { "SELECT", "FROM", "WHERE", "LEFT OUTER JOIN", "LEFT JOIN", "ORDER BY", "INNER JOIN", "DESC", "INSERT INTO" };
    #endregion

    #region Constructor
    public FormTab()
    {
      InitializeComponent();
      comboBoxChoices.SelectedIndex = 0;
      listViewKeywords.View = View.Details;
      listViewKeywords.Columns.Add("Keywords", 200);
      foreach (string keyword in keywords)
        listViewKeywords.Items.Add(keyword);
    }

    #endregion

    #region Events
    private void buttonExecuteChoice_Click(object sender, EventArgs e)
    {
      string[] lines = textBoxSQL.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
      string result = string.Empty;
      
      switch (comboBoxChoices.Text)
      {
        case BEAUTIFY:
          result = Beautify(lines);
          break;
        case QUOTEIFY:
          result = Quoteify(lines);
          break;
        case UNQUOTEIFY:
          result = Unquoteify(lines);
          break;
        case COMMAFY:
          result = Commafy(lines);
          break;
        default:
          result = "You haven't made a combobox choice. PANIC!!!";
          break;
      }

      textBoxSQL.Text = result;
    }

    private void comboBoxChoices_SelectedIndexChanged(object sender, EventArgs e)
    {
      button1.Text = comboBoxChoices.Text;
    }

    private void buttonAddListviewItem_Click(object sender, EventArgs e)
    {
      if (!string.IsNullOrEmpty(textBoxListviewItem.Text))
      {
        listViewKeywords.Items.Add(textBoxListviewItem.Text.ToUpper());
      }
    }

    private void buttonRemoveListviewItem_Click(object sender, EventArgs e)
    {
      foreach (ListViewItem itm in listViewKeywords.SelectedItems)
        listViewKeywords.Items.Remove(itm);
    }
    #endregion

    #region Private Functions

    #region Choices
    private string Beautify(string[] lines)
    {
      string result = string.Empty;

      foreach (string line in lines)
      {
        string temp = line.Replace("\n", String.Empty);
        temp = temp.Replace(" ", String.Empty);

        result += temp;
      }

      string[] keywordsWithoutSpaces = RemoveSpacesFromList(keywords);


      foreach (string keyword in keywordsWithoutSpaces)
      {
        if (keyword.Equals("SELECT") || keyword.Equals("INSERTINTO")) result = result.Replace(keyword, keyword + " ");
        else result = result.Replace(keyword, "\r\n" + keyword + " ");
      }

      result = result.Replace(",", ", ");

      result = Revert(result);
      result = result.Replace("AND", " AND ");
      result = result.Replace("ON", " ON ");

      return result;
    }

    private string Commafy(string[] lines)
    {
      string comma_part = string.Empty;
      for (int i = 1; i < lines.Length - 2; i++)
        comma_part += "'" + lines[i] + "', ";

      comma_part += "'" + lines[lines.Length - 1] + "'";

      return "WHERE " + lines[0] + " IN (" + comma_part + ")";
    }

    private string Quoteify(string[] lines)
    {
      string result = string.Empty;

      for (int i = 0; i < lines.Length - 1; i++)
        result += "\"" + lines[i] + " \" & _ \r\n";

      result += "\"" + lines[lines.Length - 1] + "\"";

      return result;
    }

    private string Unquoteify(string[] lines)
    {
      string result = string.Empty;

      foreach (string line in lines)
      {
        string temp = line.Replace("\"", String.Empty);
        temp = temp.Replace("&", String.Empty);
        temp = temp.Replace("_", String.Empty);

        result += temp;
      }

      return result;
    }
    #endregion

    #region Helper Methods
    private string Revert(string givenResult)
    {
      foreach (string keyword in keywords)
      {
        givenResult = givenResult.Replace(keyword.Replace(" ", string.Empty), keyword);
      }

      return givenResult;
    }

    private string[] RemoveSpacesFromList(string[] givenLis)
    {
      string[] newLis = new string[givenLis.Length];

      for (int i = 0; i < givenLis.Length; i++)
        newLis[i] = givenLis[i].Replace(" ", string.Empty);

      return newLis;
    }
    #endregion

    #endregion
  }
}
