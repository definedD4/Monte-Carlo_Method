using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Monte_Carlo_Method_3D.Controls
{
    /// <summary>
    /// Логика взаимодействия для PrevNextTabControl.xaml
    /// </summary>
    public partial class PrevNextTabControl : UserControl
    {
        private int m_SelectedItemIdx = 0;

        public PrevNextTabControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ItemSourceProperty = DependencyProperty.Register(
            "ItemSource", typeof(Items), typeof(PrevNextTabControl), new PropertyMetadata(default(IEnumerable<object>), ItemsSourceChanged));

        private static void ItemsSourceChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var c = dependencyObject as PrevNextTabControl;

            if (c != null)
            {
                c.SelectedContent.Content = c.ItemSource?.FirstOrDefault();
                c.m_SelectedItemIdx = 0;
            }
        }

        public Items ItemSource
        {
            get { return (Items) GetValue(ItemSourceProperty); }
            set { SetValue(ItemSourceProperty, value); }
        }


        private void Next(object sender, RoutedEventArgs e)
        {
            if (m_SelectedItemIdx + 1 >= ItemSource.Count()) return;
            m_SelectedItemIdx++;
            SelectedContent.Content = ItemSource.ToList()[m_SelectedItemIdx];
        }

        private void Prev(object sender, RoutedEventArgs e)
        {
            if (m_SelectedItemIdx - 1 < 0) return;
            m_SelectedItemIdx--;
            SelectedContent.Content = ItemSource.ToList()[m_SelectedItemIdx];
        }
    }

    public class Items: List<object> { }
}
