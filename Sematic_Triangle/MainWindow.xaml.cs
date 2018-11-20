using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sematic_Triangle
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        private Recipe recipe = new Recipe();
        private sematic_network network = new sematic_network();
        private Dictionary<string,float> data = new Dictionary<string, float>();
        public MainWindow()
        {
            InitializeComponent();
        }
        
        #region Control
        private void Button_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {

            load_data();

            // calculation
            Sulution();

            // show data
            foreach (dynamic x in recipe.ValueOfElement)
            {
                TextBox tb = FindName(x.Key) as TextBox;
                float temp = x.Value;
                if(temp == -1)
                {
                    tb.Text = "unknown";
                }
                else tb.Text = temp.ToString();
            }
        }
        private void Button_MouseRightButtonDown_2(object sender, MouseButtonEventArgs e)
        {
            foreach (dynamic x in recipe.NameOfElement)
            {
                TextBox tb = FindName(x) as TextBox;
                tb.Text = "";
            }
            data.Clear();
            recipe.SetVaule(data);
            network.clear();
        }
        private void Button_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void Button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            load_data();

            // calculation
            Sulution();

            // show data
            foreach (dynamic x in recipe.ValueOfElement)
            {
                TextBox tb = FindName(x.Key) as TextBox;
                float temp = x.Value;
                if (temp == -1)
                {
                    tb.Text = "unknown";
                }
                else tb.Text = temp.ToString();
            }
        }

        private void Button_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            foreach (dynamic x in recipe.NameOfElement)
            {
                TextBox tb = FindName(x) as TextBox;
                tb.Text = "";
            }
            data.Clear();
            recipe.SetVaule(data);
            network.clear();
        }

        #endregion
        private void load_data()
        {
           
            foreach (TextBox tb in FindVisualChildren<TextBox>(window))
            {
                try
                {
                    if (tb.Text == "" || tb.Text =="unknown")
                    {
                        data.Add(tb.Name, -1);
                     
                    }
                    else data.Add(tb.Name, float.Parse(tb.Text));
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: \n " + ex.Message.ToString(), "Big Bug", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            recipe.SetVaule(data);
            network.set_network(data);
            //recipe.show_DEBUG();
            data.Clear();
        }
        private void Sulution()
        {
            try
            {
                bool flag = true;
                int count = 0;
                int errorLoop = 0;
                int maxLoop = 5;
                while (flag && count != maxLoop && errorLoop != maxLoop)
                {
                    flag = false;
                    //int rowOfMissingElement = network.checkForActive();
                    List<int> rowOfMissingElement = network.rowsActived();
                    // nums of element missing
                    int NumsMissing = recipe.NumsOfMissing();
                    //
                    if(rowOfMissingElement.Count() != 0)
                    {
                        for(int i = 0; i < rowOfMissingElement.Count(); i++)
                        {
                            
                            int indexOfMissingElement = network.MissingElement(rowOfMissingElement[i]);
                            if (indexOfMissingElement == -1) continue;
                            Console.WriteLine(rowOfMissingElement[i] + " " + indexOfMissingElement);
                            recipe.CalculationMissingElement(rowOfMissingElement[i], indexOfMissingElement);
                            // update data
                            network.set_network(recipe.ValueOfElement);
                        }
                        
                        count = 0;
                        flag = true;
                    }
                    if(NumsMissing == recipe.NumsOfMissing())
                    {
                        errorLoop++;
                    }
                    count++;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: \n " + ex.Message.ToString(), "Big Bug", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #region Only Enter Number to textBox (0=99999)
        private static readonly Regex _regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text

        private void edge_c_TextInput_1(object sender, TextCompositionEventArgs e)
        {
            e.Handled = _regex.IsMatch(e.Text);
        }
        private void beta_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsValid(((TextBox)sender).Text + e.Text);
        }
        private void apha_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsValid(((TextBox)sender).Text + e.Text);
        }

        private void omega_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsValid(((TextBox)sender).Text + e.Text);
        }

        private void edge_a_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsValid(((TextBox)sender).Text + e.Text);
        }

        private void edge_c_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsValid(((TextBox)sender).Text + e.Text);
        }

        private void edge_b_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsValid(((TextBox)sender).Text + e.Text);
        }

        private void p_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsValid(((TextBox)sender).Text + e.Text);
        }

        private void s_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsValid(((TextBox)sender).Text + e.Text);
        }

        private void height_a_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsValid(((TextBox)sender).Text + e.Text);
        }

        private void R_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsValid(((TextBox)sender).Text + e.Text);
        }
        #endregion
        #region Tools
        private static bool IsValid(string str)
        {
            int i;
            return int.TryParse(str, out i) && i >= 0 && i <= 99999;
        }
        private static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }




        #endregion

        
    }
    #region recipe class
    public class Recipe
    {
        #region init
        public List<string> NameOfElement = new List<string>() { "edge_a", "edge_b", "edge_c", "apha", "beta", "omega", "height_a", "s", "p", "R" };
        public int Nums_Recipe = 12;
        public Dictionary<string, float> ValueOfElement = new Dictionary<string, float>();
        public List<List<string>> ElementOfRecipes = new List<List<string>>()
        {
            new List<string>() {"apha","beta","omega"}, //1
            new List<string>() {"edge_a","edge_b","edge_c","apha"},//2
            new List<string>() {"edge_a","edge_b","edge_c","beta"},//3
            new List<string>() {"edge_a","edge_b","edge_c","omega"},//4
            new List<string>() {"edge_a","edge_b","apha","beta"},//5
            new List<string>() {"edge_c","edge_b","beta","omega"},//6
            new List<string>() {"s","edge_a", "edge_b", "omega"},//7
            new List<string>() {"s","edge_b", "edge_c", "apha"},//8
            new List<string>() {"s","edge_a", "edge_c", "beta"},//9
            new List<string>() {"p","edge_a", "edge_b", "edge_c"},//10
            new List<string>() {"R","edge_a", "edge_b", "edge_c","s"},//11
            new List<string>() {"height_a", "edge_b", "omega"},//12

        };

        public void SetVaule(Dictionary<string, float> inputs)
        {
            ValueOfElement = new Dictionary<string, float>(inputs);
        }
        public void show_DEBUG()
        {
            foreach (dynamic x in ValueOfElement)
            {
                Console.WriteLine(x);
            }
        }
        #endregion
        public void CalculationMissingElement(int row, int index)// row is ricipe, index is missing element
        {
            switch(row)
            {
                case 0: // recipe 1
                    {
                        switch(NameOfElement[index])
                        {
                            case "apha": //Missing Element
                                {
                                    recipe_1_apha();
                                    //do something
                                    break;
                                }
                            case "beta"://Missing Element
                                {
                                    recipe_1_beta();
                                    //do something
                                    break;
                                }
                            case "omega"://Missing Element
                                {
                                    recipe_1_omega();
                                    //do something
                                    break;
                                }
                        }
                        break;
                    }
                case 1: // recipe 2
                    {
                        switch (NameOfElement[index])
                        {
                            case "apha": //Missing Element
                                {
                                    
                                    //do something
                                    break;
                                }
                            case "edge_a"://Missing Element
                                {
                                    recipe_2_a();
                                    //do something
                                    break;
                                }
                            case "edge_b"://Missing Element
                                {
                                    //do something
                                    break;
                                }
                            case "edge_c"://Missing Element
                                {
                                    //do something
                                    break;
                                }
                        }
                        break;
                    }
                case 2:// recipe 3
                    {
                        switch (NameOfElement[index])
                        {
                            case "beta": //Missing Element
                                {
                                    
                                    //do something
                                    break;
                                }
                            case "edge_a"://Missing Element
                                {
                                    //do something
                                    break;
                                }
                            case "edge_b"://Missing Element
                                {
                                    recipe_3_b();
                                    //do something
                                    break;
                                }
                            case "edge_c"://Missing Element
                                {
                                    //do something
                                    break;
                                }
                        }
                        break;
                    }
                case 3:// recipe 4
                    {
                        switch (NameOfElement[index])
                        {
                            case "omega": //Missing Element
                                {
                                    
                                    //do something
                                    break;
                                }
                            case "edge_a"://Missing Element
                                {
                                    //do something
                                    break;
                                }
                            case "edge_b"://Missing Element
                                {
                                    //do something
                                    break;
                                }
                            case "edge_c"://Missing Element
                                {
                                    recipe_4_c();
                                    //do something
                                    break;
                                }
                        }
                        break;
                    }
                case 4:// recipe 5
                    {
                        switch (NameOfElement[index])
                        {
                            case "edge_a": //Missing Element
                                {
                                    recipe_5_a();
                                    //do something
                                    break;
                                }
                            case "edge_b"://Missing Element
                                {
                                    recipe_5_b();
                                    //do something
                                    break;
                                }
                            case "apha"://Missing Element
                                {
                                    recipe_5_apha();
                                    //do something
                                    break;
                                }
                            case "beta"://Missing Element
                                {
                                    recipe_5_beta();
                                    //do something
                                    break;
                                }
                        }
                        break;
                    }
                case 5:// recipe 6
                    {
                        switch (NameOfElement[index])
                        {
                            case "edge_c": //Missing Element
                                {
                                    recipe_6_c();
                                    //do something
                                    break;
                                }
                            case "edge_b"://Missing Element
                                {

                                    recipe_6_b();
                                    //do something
                                    break;
                                }
                            case "omega"://Missing Element
                                {

                                    recipe_6_omega();
                                    //do something
                                    break;
                                }
                            case "beta"://Missing Element
                                {

                                    recipe_6_beta();
                                    //do something
                                    break;
                                }
                        }
                        break;
                    }
                case 6:// recipe 7
                    {
                        switch (NameOfElement[index])
                        {
                            case "s": //Missing Element
                                {
                                    recipe_7_s();
                                    //do something
                                    break;
                                }
                            case "edge_a"://Missing Element
                                {
                                    recipe_7_a();
                                    //do something
                                    break;
                                }
                            case "edge_b"://Missing Element
                                {
                                    recipe_7_b();
                                    //do something
                                    break;
                                }
                            case "omega"://Missing Element
                                {
                                    recipe_7_omega();
                                    //do something
                                    break;
                                }
                        }
                        break;
                    }
                case 7:// recipe 8
                    {
                        switch (NameOfElement[index])
                        {
                            case "s": //Missing Element
                                {
                                    recipe_8_s();
                                    //do something
                                    break;
                                }
                            case "apha"://Missing Element
                                {

                                    recipe_8_apha();
                                    //do something
                                    break;
                                }
                            case "edge_b"://Missing Element
                                {

                                    recipe_8_b();
                                    //do something
                                    break;
                                }
                            case "edge_c"://Missing Element
                                {
                                    recipe_8_c();
                                    //do something
                                    break;
                                }
                        }
                        break;
                    }
                case 8:// recipe 9
                    {
                        switch (NameOfElement[index])
                        {
                            case "s": //Missing Element
                                {
                                    recipe_9_S();
                                    //do something
                                    break;
                                }
                            case "edge_a"://Missing Element
                                {
                                    recipe_9_a();
                                    //do something
                                    break;
                                }
                            case "beta"://Missing Element
                                {
                                    recipe_9_beta();
                                    //do something
                                    break;
                                }
                            case "edge_c"://Missing Element
                                {
                                    recipe_9_c();
                                    //do something
                                    break;
                                }
                        }
                        break;
                    }
                case 9:// recipe 10
                    {
                        switch (NameOfElement[index])
                        {
                            case "p": //Missing Element
                                {
                                    recipe_10_p();
                                    //do something
                                    break;
                                }
                            case "edge_a"://Missing Element
                                {
                                    recipe_10_a();
                                    //do something
                                    break;
                                }
                            case "edge_b"://Missing Element
                                {
                                    recipe_10_b();
                                    //do something
                                    break;
                                }
                            case "edge_c"://Missing Element
                                {
                                    recipe_10_c();
                                    //do something
                                    break;
                                }
                        }
                        break;
                    }
                case 10:// recipe 11
                    {
                        switch (NameOfElement[index])
                        {
                            case "R": //Missing Element
                                {
                                    //do something
                                    recipe_11_R();
                                    break;
                                }
                            case "edge_a"://Missing Element
                                {
                                    recipe_11_a();
                                    //do something
                                    break;
                                }
                            case "edge_b"://Missing Element
                                {
                                    recipe_11_b();
                                    //do something
                                    break;
                                }
                            case "edge_c"://Missing Element
                                {
                                    recipe_11_c();
                                    //do something
                                    break;
                                }
                            case "s"://Missing Element
                                {
                                    recipe_11_S();
                                    //do something
                                    break;
                                }
                        }
                        break;
                    }
                case 11:// recipe 12
                    {
                        switch (NameOfElement[index])
                        {
                            case "height_a": //Missing Element
                                {
                                    recipe_12_ha();
                                    //do something
                                    break;
                                }
                            case "edge_b"://Missing Element
                                {
                                    recipe_12_b();
                                    //do something
                                    break;
                                }
                            case "omega"://Missing Element
                                {
                                    recipe_12_gamma();
                                    //do something
                                    break;
                                }
                      
                        }
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        } 
        public int NumsOfMissing()
        {
            int count = 0;
            foreach(dynamic x in ValueOfElement)
            {
                if(x.Value == -1)
                {
                    count++;
                }
            }
            return count;
        }
        #region apha + beta + omega  = 180
        private void recipe_1_apha()
        {
            float beta = ValueOfElement["beta"];
            float omega = ValueOfElement["omega"];
            float result;
            try
            {
                if(180 - beta - omega > 0)
                {
                    result = 180 - beta - omega;
                    ValueOfElement["apha"] = result;
                }
                else
                {
                    ValueOfElement["apha"] = float.Parse("error");
                }
            }
            catch 
            {
                MessageBox.Show("Tong 3 goc lon hon 180 ", "Big Bug", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void recipe_1_beta()
        {
            float apha = ValueOfElement["apha"];
            float omega = ValueOfElement["omega"];
            float result;
            try
            {
                if (180 - apha - omega > 0)
                {
                    result = 180 - apha - omega;
                    ValueOfElement["beta"] = result;
                }
                else
                {
                    ValueOfElement["beta"] = float.Parse("error");
                }
            }
            catch
            {
                MessageBox.Show("Tong 3 goc lon hon 180 ", "Big Bug", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void recipe_1_omega()
        {
            float apha = ValueOfElement["apha"];
            float beta = ValueOfElement["beta"];
            float result;
            try
            {
                if (180 - beta - apha > 0)
                {
                    result = 180 - beta - apha;
                    ValueOfElement["omega"] = result;
                }
                else
                {
                    ValueOfElement["omega"] = float.Parse("error");
                }
            }
            catch
            {
                MessageBox.Show("Tong 3 goc lon hon 180 ", "Big Bug", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion
        #region a2 = b2 + c2 - 2.b.c.cos(apha)
        void recipe_2_a()
        {
            double b = ValueOfElement["edge_b"];
            double c = ValueOfElement["edge_c"];
            double apha = Math.PI * ValueOfElement["apha"] / 180.0;
            double result = Math.Sqrt(Math.Pow(b, 2) + Math.Pow(c, 2) - 2 * b * c * Math.Cos(apha));
            ValueOfElement["edge_a"] = (float)result;
        }
        #endregion
        #region b2 = a2 + c2 - 2.a.c.cos(beta)
        void recipe_3_b()
        {
            double a = ValueOfElement["edge_a"];
            double c = ValueOfElement["edge_c"];
            double beta = Math.PI * ValueOfElement["beta"] / 180.0;
            double result = Math.Sqrt(Math.Pow(a, 2) + Math.Pow(c, 2) - 2 * a * c * Math.Cos(beta));
            Console.WriteLine(result);
            ValueOfElement["edge_b"] = (float)result;
        }
        #endregion
        #region c2 = a2 + b2 - 2.a.b.cos(gamma)
        void recipe_4_c()
        {
            double a = ValueOfElement["edge_a"];
            double b = ValueOfElement["edge_b"];
            double gamma = Math.PI * ValueOfElement["omega"] / 180.0;
            double result = Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2) - 2 * a * b * Math.Cos(gamma));
            ValueOfElement["edge_c"] = (float)result;
        }
        #endregion
        #region a/sin(apha) = b/sin(beta)
        void recipe_5_a()
        {
            double apha = Math.PI * ValueOfElement["apha"] / 180.0;
            double beta = Math.PI * ValueOfElement["beta"] / 180.0;
            double edge_b = ValueOfElement["edge_b"];
            double result = Math.Sin(apha) * edge_b / Math.Sin(beta);
            ValueOfElement["edge_a"] = (float)result;

        }
        void recipe_5_b()
        {
            double apha = Math.PI * ValueOfElement["apha"] / 180.0;
            double beta = Math.PI * ValueOfElement["beta"] / 180.0;
            double edge_a = ValueOfElement["edge_a"];
            double result = Math.Sin(beta) * edge_a / Math.Sin(apha);
            ValueOfElement["edge_b"] = (float)result;
        }
        void recipe_5_apha()
        {
            double beta = Math.PI * ValueOfElement["beta"] / 180.0;
            double edge_a = ValueOfElement["edge_a"];
            double edge_b = ValueOfElement["edge_b"];
            double result = Math.Asin(Math.Sin(beta) * edge_a / edge_b) * (180.0 / Math.PI);
            Console.WriteLine(Math.Sin(beta) * edge_a / edge_b);
            ValueOfElement["apha"] = (float)result;
        }
        void recipe_5_beta()
        {
            double apha = Math.PI * ValueOfElement["apha"] / 180.0;
            double edge_a = ValueOfElement["edge_a"];
            double edge_b = ValueOfElement["edge_b"];
            double result = Math.Asin(Math.Sin(apha) * edge_b / edge_a) * (180.0 / Math.PI);
            Console.WriteLine(result);
            ValueOfElement["beta"] = (float)result;
        }


        #endregion
        #region c/sin(omega) = b/sin(beta)
        void recipe_6_c()
        {
            double omega = Math.PI * ValueOfElement["omega"] / 180.0;
            double beta = Math.PI * ValueOfElement["beta"] / 180.0;
            double edge_b = ValueOfElement["edge_b"];
            double result = Math.Sin(omega) * edge_b / Math.Sin(beta);
            ValueOfElement["edge_c"] = (float)result;

        }
        void recipe_6_b()
        {
            double omega = Math.PI * ValueOfElement["omega"] / 180.0;
            double beta = Math.PI * ValueOfElement["beta"] / 180.0;
            double edge_c = ValueOfElement["edge_c"];
            double result = Math.Sin(beta) * edge_c / Math.Sin(omega);
            ValueOfElement["edge_b"] = (float)result;
        }
        void recipe_6_omega()
        {
            double beta = Math.PI * ValueOfElement["beta"] / 180.0;
            double edge_c = ValueOfElement["edge_c"];
            double edge_b = ValueOfElement["edge_b"];
            double result = Math.Asin(Math.Sin(beta) * edge_c / edge_b) * (180.0 / Math.PI);
            Console.WriteLine(Math.Sin(beta) * edge_c / edge_b);
            ValueOfElement["omega"] = (float)result;
        }
        void recipe_6_beta()
        {
            double omega = Math.PI * ValueOfElement["omega"] / 180.0;
            double edge_c = ValueOfElement["edge_c"];
            double edge_b = ValueOfElement["edge_b"];
            double result = Math.Asin(Math.Sin(omega) * edge_b / edge_c) * (180.0 / Math.PI);
            Console.WriteLine(result);
            ValueOfElement["beta"] = (float)result;
        }
        #endregion
        #region S = a.b.sin(omega) / 2
        void recipe_7_s()
        {
            double omega = Math.PI * ValueOfElement["omega"] / 180.0;
            double edge_b = ValueOfElement["edge_b"];
            double edge_a = ValueOfElement["edge_a"];
            double result = edge_a * edge_b * Math.Sin(omega) / 2;
            ValueOfElement["s"] = (float)result;
        }
        void recipe_7_a()
        {
            double omega = Math.PI * ValueOfElement["omega"] / 180.0;
            double edge_b = ValueOfElement["edge_b"];
            double s = ValueOfElement["s"];
            double result =   (2 * s) / (edge_b * Math.Sin(omega)) ;
            ValueOfElement["edge_a"] = (float)result;
        }
        void recipe_7_b()
        {
            double omega = Math.PI * ValueOfElement["omega"] / 180.0;
            double edge_a = ValueOfElement["edge_a"];
            double s = ValueOfElement["s"];
            double result = (2 * s) / (edge_a * Math.Sin(omega));
            ValueOfElement["edge_b"] = (float)result;
        }
        void recipe_7_omega()
        {
            double edge_b = ValueOfElement["edge_b"];
            double edge_a = ValueOfElement["edge_a"];
            double s = ValueOfElement["s"];
            double result = Math.Asin((2 * s) / (edge_a * edge_b)) * (180.0 / Math.PI);
            ValueOfElement["omega"] = (float)result;
        }


        #endregion
        #region S = b.c.sin(apha) / 2
        void recipe_8_s()
        {
            double apha = Math.PI * ValueOfElement["apha"] / 180.0;
            double edge_b = ValueOfElement["edge_b"];
            double edge_c = ValueOfElement["edge_c"];
            double result = edge_c * edge_b * Math.Sin(apha) / 2;
            ValueOfElement["s"] = (float)result;
        }
        void recipe_8_b()
        {
            double apha = Math.PI * ValueOfElement["apha"] / 180.0;
            double edge_c = ValueOfElement["edge_c"];
            double s = ValueOfElement["s"];
            double result = (2 * s) / (edge_c * Math.Sin(apha));
            ValueOfElement["edge_b"] = (float)result;
        }
        void recipe_8_c()
        {
            double apha = Math.PI * ValueOfElement["apha"] / 180.0;
            double edge_b = ValueOfElement["edge_b"];
            double s = ValueOfElement["s"];
            double result = (2 * s) / (edge_b * Math.Sin(apha));
            ValueOfElement["edge_c"] = (float)result;
        }
        void recipe_8_apha()
        {
            double edge_b = ValueOfElement["edge_b"];
            double edge_c = ValueOfElement["edge_c"];
            double s = ValueOfElement["s"];
            double result = Math.Asin((2 * s) / (edge_c * edge_b )) * (180.0 / Math.PI);
            ValueOfElement["apha"] = (float)result;
        }
        #endregion
        #region S=a.c.sin(beta)/2
        void recipe_9_S()
        {
            double edge_a = ValueOfElement["edge_a"];
            double edge_c = ValueOfElement["edge_c"];
            double beta = Math.PI * ValueOfElement["beta"] / 180.0;
            double result = edge_a * edge_c * Math.Sin(beta) / 2;
            ValueOfElement["s"] = (float)result;
        }
        void recipe_9_a()
        {
            double S = ValueOfElement["s"];
            double edge_c = ValueOfElement["edge_c"];
            double beta = Math.PI * ValueOfElement["beta"] / 180.0;
            double result = (double)(2 * S) / (edge_c * Math.Sin(beta));
            ValueOfElement["edge_a"] = (float)result;
        }
        void recipe_9_c()
        {
            double S = ValueOfElement["s"];
            double edge_a = ValueOfElement["edge_a"];
            double beta = Math.PI * ValueOfElement["beta"] / 180.0;
            double result = (double)(2 * S) / (edge_a * Math.Sin(beta));
            ValueOfElement["edge_c"] = (float)result;
        }
        void recipe_9_beta()
        {
            double S = ValueOfElement["s"];
            double edge_a = ValueOfElement["adge_a"];
            double edge_c = ValueOfElement["edge_c"];
            double result = Math.Asin((double)(2 * S) / (edge_a * edge_c)) * (180.0 / Math.PI);
            ValueOfElement["beta"] = (float)result;
        }
        #endregion
        #region 2p=a+b+c
        void recipe_10_p()
        {
            double edge_a = ValueOfElement["edge_a"];
            double edge_b = ValueOfElement["edge_b"];
            double edge_c = ValueOfElement["edge_c"];
            double result = (double)(edge_a + edge_b + edge_c) / 2;
            ValueOfElement["p"] = (float)result;
        }
        void recipe_10_a()
        {
            double edge_b = ValueOfElement["edge_b"];
            double edge_c = ValueOfElement["edge_c"];
            double p = ValueOfElement["p"];
            double result = 2 * p - (edge_b + edge_c);
            ValueOfElement["edge_a"] = (float)result;
        }
        void recipe_10_b()
        {
            double edge_a = ValueOfElement["edge_a"];
            double edge_c = ValueOfElement["edge_c"];
            double p = ValueOfElement["p"];
            double result = 2 * p - (edge_a + edge_c);
            ValueOfElement["edge_b"] = (float)result;
        }
        void recipe_10_c()
        {
            double edge_a = ValueOfElement["edge_a"];
            double edge_b = ValueOfElement["edge_b"];
            double p = ValueOfElement["p"];
            double result = 2 * p - (edge_a + edge_b);
            ValueOfElement["edge_c"] = (float)result;
        }
        #endregion
        #region R=a.b.c/(4.s)
        void recipe_11_R()
        {
            double edge_a = ValueOfElement["edge_a"];
            double edge_b = ValueOfElement["edge_b"];
            double edge_c = ValueOfElement["edge_c"];
            double S = ValueOfElement["s"];
            double result = (double)(edge_a * edge_b * edge_c) / (4 * S);
            ValueOfElement["R"] = (float)result;
        }
        void recipe_11_a()
        {
            double edge_b = ValueOfElement["edge_b"];
            double edge_c = ValueOfElement["edge_c"];
            double S = ValueOfElement["s"];
            double R = ValueOfElement["R"];
            double result = (double)(R * 4 * S) / (edge_b * edge_c);
            ValueOfElement["edge_a"] = (float)result;
        }
        void recipe_11_b()
        {
            double edge_a = ValueOfElement["edge_a"];
            double edge_c = ValueOfElement["edge_c"];
            double S = ValueOfElement["s"];
            double R = ValueOfElement["R"];
            double result = (double)(R * 4 * S) / (edge_a * edge_c);
            ValueOfElement["edge_b"] = (float)result;
        }
        void recipe_11_c()
        {
            double edge_b = ValueOfElement["edge_b"];
            double edge_a = ValueOfElement["edge_a"];
            double S = ValueOfElement["s"];
            double R = ValueOfElement["R"];
            double result = (double)(R * 4 * S) / (edge_b * edge_a);
            ValueOfElement["edge_c"] = (float)result;
        }
        void recipe_11_S()
        {
            double edge_a = ValueOfElement["edge_a"];
            double edge_b = ValueOfElement["edge_b"];
            double edge_c = ValueOfElement["edge_c"];
            double R = ValueOfElement["R"];
            double result = (double)(edge_a * edge_b * edge_c) / (4 * R);
            ValueOfElement["s"] = (float)result;
        }
        #endregion
        #region ha=b.sin(gamma)
        void recipe_12_ha()
        {
            double edge_b = ValueOfElement["edge_b"];
            double gamma = Math.PI*ValueOfElement["omega"]/180.0;
            double result = edge_b * Math.Sin(gamma);
            ValueOfElement["height_a"] = (float)result;
        }
        void recipe_12_b()
        {
            double ha = ValueOfElement["height_a"];
            double gamma = Math.PI*ValueOfElement["omega"]/180.0;
            double result = (double)ha / Math.Sin(gamma);
            ValueOfElement["edge_b"] = (float)result;
        }
        void recipe_12_gamma()
        {
            double edge_b = ValueOfElement["edge_b"];
            double ha = ValueOfElement["height_a"];
            double result = Math.Asin(ha / edge_b) * (180.0 / Math.PI);
            ValueOfElement["omega"] = (float)result;
        }
        #endregion

    }
    #endregion
    #region Sematic_network
    public class sematic_network: Recipe
    {
        List<List<int>> matrix = new List<List<int>>();
        public sematic_network()
        {
            for(int i = 0; i < Nums_Recipe; i++)
            {
                List<int> temp = new List<int>(new int[NameOfElement.Count()]); // gia tri ban dau bang 0
                matrix.Add(temp);
            }
            int row = 0;
            foreach (dynamic x in ElementOfRecipes)
            {
                foreach (dynamic y in x)
                {
                    int index = NameOfElement.IndexOf(y);
                    matrix[row][index] = -1;
                }
                row++;
            }
        }
        public void clear()
        {
            sematic_network temp = new sematic_network();
            this.matrix = temp.matrix;
        }
        public void set_network(Dictionary<string, float> data)
        {
            foreach( dynamic x in data)
            {
                if(x.Value != -1)
                {
                    string name = x.Key;
                    int index = NameOfElement.IndexOf(name);
                    //update matrix
                    for(int i = 0; i < Nums_Recipe; i++)
                    {
                        if(matrix[i][index] == -1)
                        {
                            matrix[i][index] = 1;
                        }
                    }
                }
            }
        }
        public int checkForActive()
        {
            int row = -1;
            foreach(dynamic x in matrix)
            {
                int sum = 0;
                foreach(dynamic y in x)
                {
                    sum += y;
                }
                int location = matrix.IndexOf(x);
                if (sum == ElementOfRecipes[location].Count() - 2)
                {
                    row = location;
                    break;
                }
            }
            return row;
        }

        public List<int> rowsActived()
        {
            List<int> row = new List<int>();
            foreach (dynamic x in matrix)
            {
                int sum = 0;
                foreach (dynamic y in x)
                {
                    sum += y;
                }
                int location = matrix.IndexOf(x);
                if (sum == ElementOfRecipes[location].Count() - 2)
                {
                    row.Add(location);
                    
                }
            }
            return row;
        }
        public int MissingElement(int row)
        {
            int index = -1;
            for(int i = 0; i < NameOfElement.Count(); i++)
            {
                if(matrix[row][i] == -1)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }

        public void show_data_DEBUG()
        {
            foreach(dynamic x in matrix)
            {
                foreach(dynamic y in x)
                {
                    Console.Write(y + " ");
                }
                Console.WriteLine();
            }
        }
    }
    #endregion
}
