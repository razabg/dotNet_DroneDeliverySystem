﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BO;
using BlApi;

namespace PL
{
    /// <summary>
    /// Interaction logic for ParceList.xaml
    /// </summary>
    public partial class ParceList : Window
    {
        private IBL BLAccess;
        public ParceList(IBL BLAccess)
        {
            InitializeComponent();
            this.BLAccess = BLAccess;
            ParcelListView.DataContext = BLAccess.GetParcelToLists();

        }

        private void ParcelListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
