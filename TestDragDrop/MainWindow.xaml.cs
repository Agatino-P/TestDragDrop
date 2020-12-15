using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace TestDragDrop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<DragableObj> Elenco = new ObservableCollection<DragableObj>();
        ObservableCollection<DragableObj> Elenco2 = new ObservableCollection<DragableObj>();
        string previousTbText = "";
        ListBox lbDragSource;

        public MainWindow()
        {
            FillElenco();
            InitializeComponent();
            this.DataContext = this;
            lbElenco.ItemsSource = Elenco;
            lbElenco2.ItemsSource = Elenco2;
        }

        private void FillElenco()
        {
            for (int i = 0; i< 10; i++)
            {
                Elenco.Add(
                    new DragableObj("Object #" + i)
                    );
            }
        }

        private void BtnTest_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var obj in Elenco)
            {
                sb.Append(obj.Name+".");
            }
            MessageBox.Show(sb.ToString());
        }

        private void LbElenco_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ListBox lb = sender as ListBox;
            if ( (lb != null) && (e.LeftButton == MouseButtonState.Pressed) && (lb.SelectedItem !=null) )
            {
                lbDragSource = lb;
                DragableObj sourceObj = lb.SelectedItem as DragableObj;
                DragDrop.DoDragDrop(lb, sourceObj, DragDropEffects.All);
            }
        }

        private void TxtTarget_DragEnter(object sender, DragEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb != null)
            {
                string previousTbText = tb.Text;
                if (e.Data.GetDataPresent(DataFormats.StringFormat))
                {
                    tb.Text = (string)e.Data.GetData(DataFormats.StringFormat);
                }
            }
        }

        private void TxtTarget_DragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.None;

            if (e.Data.GetDataPresent(DataFormats.StringFormat))
            {
                string dataString = (string)e.Data.GetData(DataFormats.StringFormat);
            }
        }

        private void TxtTarget_DragLeave(object sender, DragEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb != null)
            {
                tb.Text = previousTbText;
            }
        }

        private void TxtTarget_Drop(object sender, DragEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb != null)
            {
                if (e.Data.GetDataPresent(DataFormats.StringFormat))
                {
                    tb.Text = (string)e.Data.GetData(DataFormats.StringFormat);
                }
            }
        }

        private void LbElenco_DragEnter(object sender, DragEventArgs e)
        {
            
        }

        private void LbElenco_DragLeave(object sender, DragEventArgs e)
        {
        }

        private void LbElenco_DragOver(object sender, DragEventArgs e)
        {
        }

        private void LbElenco_Drop(object sender, DragEventArgs e)
        {
        }

        private void LbElenco2_DragEnter(object sender, DragEventArgs e)
        {
            ListBox lb = sender as ListBox;
            if (lb != null)
            {
                //Needed?
            }
        }

        private void LbElenco2_DragLeave(object sender, DragEventArgs e)
        {
            ListBox lb = sender as ListBox;
            if (lb != null)
            {
                //Needed?
            }
        }

        private void LbElenco2_DragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.None;

            if (e.Data.GetDataPresent(DataFormats.StringFormat))
            {
                e.Effects = DragDropEffects.Copy | DragDropEffects.Move;
            }

        }

        private void LbElenco2_Drop(object sender, DragEventArgs e)
        {
            ListBox lb = sender as ListBox;
            if (lb != null)
            {
                if (e.Data.GetDataPresent(typeof(DragableObj)))
                {
                    DragableObj draggedObj = (DragableObj)e.Data.GetData(typeof(DragableObj));

                    int index = -1;
                    for (int i = 0; i < Elenco2.Count; i++)
                    {
                        var lbi = lbElenco2.ItemContainerGenerator.ContainerFromIndex(i) as ListBoxItem;
                        if (lbi == null) continue;
                        if (IsMouseOverTarget(lbi, e.GetPosition((IInputElement)lbi)))
                        {
                            index = i;
                            break;
                        }
                    }
                    if (index >= 0)
                    {
                        Elenco2.Insert(index, draggedObj);
                    }
                    else
                        Elenco2.Add(draggedObj);

                    ((ObservableCollection<DragableObj>)lbDragSource.ItemsSource).Remove(draggedObj);
                }
            }
        }

        private static bool IsMouseOverTarget(Visual target, Point point)
        {
            var bounds = VisualTreeHelper.GetDescendantBounds(target);
            return bounds.Contains(point);
        }
    }
}
