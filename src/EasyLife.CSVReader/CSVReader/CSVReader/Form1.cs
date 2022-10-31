using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace CSVReader
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtFilePath.Text = @"D:\Work\Projects\E\EasyLife\Db and related stuff\PocketExpense_data_20220408.csv";
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                string[] Fields;
                try
                {
                    //@"D:\Work\Projects\E\EasyLife\Db and related stuff\PocketExpense_data_20220408.csv"
                    string[] Lines = File.ReadAllLines(txtFilePath.Text);
                    
                    double sumCount = 0;
                    Fields = Lines[10].Split(new char[] { ',' });
                    int Cols = Fields.GetLength(0);
                    
                    //1st row must be column names; force lower case to ensure matching later on.
                    for (int i = 0; i < Cols; i++)
                        dt.Columns.Add(Fields[i].ToLower(), typeof(string));
                    DataRow Row;
                    for (int i = 11; i < Lines.GetLength(0); i++)
                    {
                        Fields = Lines[i].Split(new char[] { ',' });
                        Row = dt.NewRow();
                        for (int f = 0; f < Fields.Count(); f++)
                        {
                            if (f >= 6)
                            {
                                if (f == Fields.Count())
                                    Row[6] += Fields[f];
                                else
                                    Row[6] += Fields[f] + ", ";
                            }
                            else
                            {
                                if (f == 4)
                                    sumCount += Convert.ToDouble(Fields[f]);
                                Row[f] = Fields[f];
                            }
                        }
                        dt.Rows.Add(Row);
                    }
                    dataGridView1.DataSource = dt;
                }
                catch (Exception excp)
                {
                    throw excp;
                }

                string connectionString;
                SqlConnection cnn;

                connectionString = @"Server=SQL5109.site4now.net;Database=db_a7e733_easylife;Trusted_Connection=True;MultipleActiveResultSets=true;User ID=db_a7e733_easylife_admin;Password=wGq9@xpHZehQq5W;Integrated Security=False";

                cnn = new SqlConnection(connectionString);
                cnn.Open();

                foreach (DataRow item in dt.Rows)
                {
                    CultureInfo cultureInfo = new CultureInfo("en-US");
                    var expenseDate = Convert.ToDateTime(item["date&Time"].ToString(), cultureInfo);
                    var categoryName = item["category"].ToString();
                    var payee = item["payee/Place"].ToString();
                    var amount = item["amount"].ToString();
                    var note = item["note"].ToString().TrimEnd(' ');
                    note = note.TrimEnd(',');
                    Guid categoryExpenseId;
                    string[] categorySplit = categoryName.Split(':');

                    if (Convert.ToDouble(amount) > -1)
                    {
                        #region GetExpenseCategoryId
                        SqlCommand command = new SqlCommand("Select id from EarningCategory where CategoryName like @categoryName", cnn);
                        if (categorySplit.Count() > 1)
                        {
                            //need to put condition in case of new import to ignore those values which are not considered in earnings.
                            if (!categorySplit.Contains("SIP"))
                            {
                                SqlParameter p;
                                if (categorySplit.Count() > 1)
                                {
                                    p = new SqlParameter("@categoryName", SqlDbType.NVarChar)
                                    {
                                        Value = "%" + categorySplit[categorySplit.Count() - 1] + "%"
                                    };
                                }
                                else
                                {
                                    p = new SqlParameter("@categoryName", SqlDbType.NVarChar)
                                    {
                                        Value = "%" + categorySplit[0] + "%"
                                    };
                                }
                                
                                command.Parameters.Add(p);
                                //command.Parameters.AddWithValue("@categoryName", "'%" + categorySplit[categorySplit.Count() - 1] + "%'");
                            }
                            else
                            {
                                //ignore this entry and continue next
                            }
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@categoryName", categoryName);
                        }

                        var earningCategoryId = command.ExecuteScalar();

                        #endregion GetExpenseCategoryId

                        if (earningCategoryId != null)
                        {
                            try
                            {
                                SqlCommand cmd = new SqlCommand();
                                cmd.Connection = cnn;
                                cmd.CommandText = "INSERT INTO EarningsBackup(Id, Payee, Note, EarningDate, DoNotConsiderInTotal, Money, EarningCategoryId, UserId, CreationTime, CreatorUserId, IsDeleted, ConsiderInTotal) VALUES(@Id, @Payee, @Note, @EarningDate, @DoNotConsiderInTotal, @Money, @EarningCategoryId, @UserId, @CreationTime, @CreatorUserId, @IsDeleted, @ConsiderInTotal)";

                                cmd.Parameters.AddWithValue("@Id", Guid.NewGuid());
                                cmd.Parameters.AddWithValue("@Payee", payee);
                                cmd.Parameters.AddWithValue("@Note", note);
                                cmd.Parameters.AddWithValue("@EarningDate", expenseDate);
                                cmd.Parameters.AddWithValue("@DoNotConsiderInTotal", false);
                                cmd.Parameters.AddWithValue("@Money", amount);
                                cmd.Parameters.AddWithValue("@EarningCategoryId", Guid.Parse(earningCategoryId.ToString()));
                                cmd.Parameters.AddWithValue("@UserId", 3);
                                cmd.Parameters.AddWithValue("@CreationTime", DateTime.Now);
                                cmd.Parameters.AddWithValue("@CreatorUserId", 3);
                                cmd.Parameters.AddWithValue("@IsDeleted", false);
                                cmd.Parameters.AddWithValue("@ConsiderInTotal", false);

                                cmd.ExecuteNonQuery();
                            }
                            catch (Exception innerExc)
                            {
                                throw innerExc;
                            }
                        }
                    }
                    else
                    {
                        #region GetExpenseCategoryId
                        SqlCommand command = new SqlCommand("Select id from ExpenseCategory where CategoryName like @categoryName", cnn);
                        if (categorySplit.Count() > 1)
                        {
                            //conditions for SIP, Shares, Share Earn(ignore these entries), 
                            if (categorySplit[categorySplit.Count() - 2] != "SIP" && categorySplit[categorySplit.Count() - 1] != "SIP" && categorySplit[categorySplit.Count() - 1] != "Shares" && categorySplit[categorySplit.Count() - 1] != "Share")
                            {
                                SqlParameter p = new SqlParameter("@categoryName", SqlDbType.NVarChar)
                                {
                                    Value = "%" + categorySplit[categorySplit.Count() - 1] + "%"
                                };
                                command.Parameters.Add(p);
                                //command.Parameters.AddWithValue("@categoryName", "'%" + categorySplit[categorySplit.Count() - 1] + "%'");
                            }
                            else
                            {
                                //ignore this entry and continue next
                            }
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@categoryName", categoryName);
                        }

                        var expenseCategoryId = command.ExecuteScalar();

                        //using (SqlDataReader reader = command.ExecuteReader())
                        //{
                        //    if (reader.Read())
                        //    {
                        //        Console.WriteLine(String.Format("{0}", reader["id"]));
                        //    }
                        //}
                        #endregion GetExpenseCategoryId

                        try
                        {
                            SqlCommand cmd = new SqlCommand();
                            cmd.Connection = cnn;
                            cmd.CommandText = "INSERT INTO ExpensesBackup(Id, Payee, Note, ExpenseDate, DoNotConsiderInTotal, Money, ExpenseCategoryId, UserId, CreationTime, CreatorUserId, IsDeleted) VALUES(@Id, @Payee, @Note, @ExpenseDate, @DoNotConsiderInTotal, @Money, @ExpenseCategoryId, @UserId, @CreationTime, @CreatorUserId, @IsDeleted)";

                            cmd.Parameters.AddWithValue("@Id", Guid.NewGuid());
                            cmd.Parameters.AddWithValue("@Payee", payee);
                            cmd.Parameters.AddWithValue("@Note", note);
                            cmd.Parameters.AddWithValue("@ExpenseDate", expenseDate);
                            cmd.Parameters.AddWithValue("@DoNotConsiderInTotal", false);
                            cmd.Parameters.AddWithValue("@Money", amount);
                            cmd.Parameters.AddWithValue("@ExpenseCategoryId", Guid.Parse(expenseCategoryId.ToString()));
                            cmd.Parameters.AddWithValue("@UserId", 3);
                            cmd.Parameters.AddWithValue("@CreationTime", DateTime.Now);
                            cmd.Parameters.AddWithValue("@CreatorUserId", 3);
                            cmd.Parameters.AddWithValue("@IsDeleted", false);

                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception innerExc)
                        {
                            throw innerExc;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}